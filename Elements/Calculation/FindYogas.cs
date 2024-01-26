/******
Copyright (C) 2005 Ajit Krishnan (http://www.mudgala.com)

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
******/

using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Mhora.Components.Property;
using Mhora.Tables;

namespace Mhora.Elements.Calculation;
/*
 * Here's the syntax that we take
 */

/* Here's a call-stack type walkthrough of this class
 This is particularly useful because some of the functions are poorly names
    evaluate yoga ( full user-specified rule )
    Phase 1: Generate basic parse tree. i.e. each bracketed portion forms one node
        - FindYogas.generateSimpleParseTree (wrapper)
            - FindYogas.generateSimpleParseTreeForNode (worker function)

    Phase 2: Expand each of these nodes. This involves taking each leaf node, which
        may contain implicit if-blocks ex <Graha:sun,moon> in <rasi:ari,2nd>,
        evaluating some values (2nd=gem), and expanding this into its 4 node equivalent
        - FindYogas.expandSimpleNodes (wrapper)
            - FindYogas.simplifyBasicNode (simplify these <lordof:<rasi:blah>>) exps
                - FindYogas.simplifyBasicNodeTerm (simplify each single term)
                    - FindYogas.replaceBasicNodeTerm (replacement parser)
            - FindYogas.expandSimpleNode (implicit binary expansion)

    Phase 3: The real evaluation
        Here we simply walk the parse tree, calling ourserves recursively and evaluating
        our &&, ||, ! and (true, false) semantics
        - ReduceTree
            - Recursively call ReduceTree as needed
            - evaluateNode (take simple node, and authoritatively return trueor false
*/

public class FindYogas
{
	private readonly Division _dtype;

	private readonly Horoscope   _h;
	private readonly ZodiacHouse _zhLagna;
	public           Node        RootNode;


	private XmlYogaNode _xmlNode;


	public FindYogas(Horoscope h, Division dtype)
	{
		this._h       = h;
		_dtype  = dtype;
		_zhLagna = this._h.GetPosition(Body.BodyType.Lagna).ToDivisionPosition(_dtype).ZodiacHouse;
	}

	public static void Test(Horoscope h, Division dtype)
	{
		var fy = new FindYogas(h, dtype);
		//fy.evaluateYoga ("gr<sun> in hse <1st>");
		//fy.evaluateYoga ("  gr<sun> in hse <1st>  ");
		//fy.evaluateYoga ("( gr<sun> in   hse <1st> )");
		//fy.evaluateYoga ("(gr<sun> in hse <1st>)");
		//fy.evaluateYoga ("(gr<sun> in hse <1st> )  ");

		//fy.evaluateYoga ("<gr:sun,moon,mars,ketu> in <rasi:1st,2nd,3rd,4th,5th,6th,7th,8th>");
		//fy.evaluateYoga ("<gr:mer> with <gr:<lordof:ari>>");
		//fy.evaluateYoga ("&&(<gr:mer> with <gr:<lordof:ari>>)(birth in <time:day>)");
		//fy.evaluateYoga ("&&(<gr:mer> with <gr:<lordof:ari>>)(birth in <time:night>)");

		//fy.evaluateYoga ("<gr:mer> in <rasi:leo>");
		//fy.evaluateYoga ("rasi@(<gr:mer> in <rasi:leo>)");
		//fy.evaluateYoga ("navamsa@(<gr:mer> in <rasi:leo>)");
		//fy.evaluateYoga ("rasi@(<gr:mer> in <rasi:can>)");
		//fy.evaluateYoga ("navamsa@(<gr:mer> in <rasi:can>)");
		//fy.evaluateYoga ("&&(rasi@(<gr:mer> in <rasi:leo>))(d9@(<gr:mer> in <rasi:can>)))");


		//fy.evaluateYoga ("<gr:<dispof:mer>> is <gr:moon>");
		//fy.evaluateYoga ("d9@(<gr:<dispof:<dispof:mer>>> is <gr:moon>)");
		//fy.evaluateYoga ("<gr:<d9@dispof:merc>> with <gr:sun>");
		//fy.evaluateYoga ("&&(<gr:sun,moon,mars> in <rasi:1st,1st,ari> with <gr:moon> and <gr:jup,pis>)(<gr:moon> in <rasi:2nd>)");
		//fy.evaluateYoga ("(&& (gr<sun> in hse<1st>) (mid term) (gr<moon> in  hse<2nd> ) )");
	}

	public string GetRuleName()
	{
		if (_xmlNode == null || _xmlNode.mhoraRule == null)
		{
			return string.Empty;
		}

		return _xmlNode.mhoraRule;
	}

	public bool EvaluateYoga(XmlYogaNode n)
	{
		_xmlNode = n;
		return EvaluateYoga(n.mhoraRule);
	}

	public bool EvaluateYoga(string rule)
	{
		RootNode = new Node(null, rule, _dtype);

		//mhora.Log.Debug ("");
		//mhora.Log.Debug ("Evaluating yoga .{0}.", rule);
		GenerateSimpleParseTree();
		ExpandSimpleNodes();
		var bRet = ReduceTree();

		//mhora.Log.Debug ("Final: {0} = {1}", bRet, rule);
		//mhora.Log.Debug ("");
		return bRet;
	}


	public string TrimWhitespace(string sCurr)
	{
		// remove leading & trailing whitespaces
		sCurr = Regex.Replace(sCurr, @"^\s*(.*?)\s*$", "$1");

		// remove contiguous whitespace
		sCurr = Regex.Replace(sCurr, @"(\s+)", " ");

		return sCurr;
	}

	public string PeelOuterBrackets(string sCurr)
	{
		// remove leading "(" and whitespace
		sCurr = Regex.Replace(sCurr, @"^\s*\(\s*", string.Empty);
		// remove trailing ")" and whitespace
		sCurr = Regex.Replace(sCurr, @"\s*\)\s*$", string.Empty);
		return sCurr;
	}

	public string[] GetComplexTerms(string sInit)
	{
		var al = new ArrayList();

		var level = 0;
		var start = 0;
		var end   = 0;

		for (var i = 0; i < sInit.Length; i++)
		{
			var curr = sInit[i];

			// we're only concerned about the grouping
			if (curr != '(' && curr != ')')
			{
				continue;
			}

			if (curr == '(')
			{
				if (++level == 1)
				{
					start = i;
				}
			}

			if (curr == ')')
			{
				if (level-- == 1)
				{
					end = i;
					var sInner = sInit.Substring(start, end - start + 1);
					al.Add(sInner);
				}
			}

			if (level == 0 && curr != '(' && curr != ')')
			{
				throw new YogasParseException("Found unexpected char outside parantheses");
			}
		}

		if (level > 0)
		{
			throw new YogasParseException("Unmatched parantheses");
		}

		return (string[]) al.ToArray(typeof(string));
	}

	public bool CheckBirthTime(string sTime)
	{
		switch (sTime)
		{
			case "day":   return _h.IsDayBirth();
			case "night": return !_h.IsDayBirth();
			default:
				MessageBox.Show("Unknown birth time: " + sTime + GetRuleName());
				return false;
		}
	}

	public bool EvaluateNode(Node n)
	{
		Debug.Assert(n.Type == Node.EType.Single);

		var cats        = string.Empty;
		var simpleTerms = n.Term.Split(' ');
		var simpleVals  = new string[simpleTerms.Length];
		for (var i = 0; i < simpleTerms.Length; i++)
		{
			cats          += " " + GetCategory(simpleTerms[i]);
			simpleVals[i] =  (string) GetValues(simpleTerms[i])[0];
		}

		cats = TrimWhitespace(cats);

		Body.BodyType        b1,   b2, b3;
		ZodiacHouse.Rasi zh1,  zh2;
		int              hse1, hse2;

		var evalDiv = n.Dtype;
		switch (cats)
		{
			case "gr: in rasi:":
			case "gr: in house:":
				b1  = StringToBody(simpleVals[0]);
				zh1 = StringToRasi(simpleVals[2]);
				if (_h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == zh1)
				{
					return true;
				}

				return false;
			case "gr: in mt":
			case "gr: in moolatrikona":
				b1 = StringToBody(simpleVals[0]);
				return _h.GetPosition(b1).ToDivisionPosition(evalDiv).IsInMoolaTrikona();
			case "gr: in exlt":
			case "gr: in exaltation":
				b1 = StringToBody(simpleVals[0]);
				return _h.GetPosition(b1).ToDivisionPosition(evalDiv).IsExaltedPhalita();
			case "gr: in deb":
			case "gr: in debilitation":
				b1 = StringToBody(simpleVals[0]);
				return _h.GetPosition(b1).ToDivisionPosition(evalDiv).IsDebilitatedPhalita();
			case "gr: in own":
			case "gr: in ownhouse":
			case "gr: in own house":
				b1 = StringToBody(simpleVals[0]);
				return _h.GetPosition(b1).ToDivisionPosition(evalDiv).IsInOwnHouse();
			case "gr: is gr:":
				b1 = StringToBody(simpleVals[0]);
				b2 = StringToBody(simpleVals[2]);
				if (b1 == b2)
				{
					return true;
				}

				return false;
			case "gr: with gr:":
				b1 = StringToBody(simpleVals[0]);
				b2 = StringToBody(simpleVals[2]);
				if (_h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == _h.GetPosition(b2).ToDivisionPosition(evalDiv).ZodiacHouse.Sign)
				{
					return true;
				}

				return false;
			case "gr: asp gr:":
				b1 = StringToBody(simpleVals[0]);
				b2 = StringToBody(simpleVals[2]);
				if (_h.GetPosition(b1).ToDivisionPosition(evalDiv).GrahaDristi(_h.GetPosition(b2).ToDivisionPosition(evalDiv).ZodiacHouse))
				{
					return true;
				}

				return false;
			case "gr: in house: from rasi:":
				b1   = StringToBody(simpleVals[0]);
				hse1 = StringToHouse(simpleVals[2]);
				zh1  = StringToRasi(simpleVals[4]);
				if (_h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == new ZodiacHouse(zh1).Add(hse1).Sign)
				{
					return true;
				}

				return false;
			case "gr: in house: from gr:":
				b1   = StringToBody(simpleVals[0]);
				hse1 = StringToHouse(simpleVals[2]);
				b2   = StringToBody(simpleVals[4]);
				return _h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == _h.GetPosition(b2).ToDivisionPosition(evalDiv).ZodiacHouse.Add(hse1).Sign;
			case "Graha in house: from gr: except gr:":
				hse1 = StringToHouse(simpleVals[2]);
				b1   = StringToBody(simpleVals[4]);
				b2   = StringToBody(simpleVals[6]);
				zh1  = _h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Add(hse1).Sign;
				for (var i = (int) Body.BodyType.Sun; i <= (int) Body.BodyType.Lagna; i++)
				{
					var bExc = (Body.BodyType) i;
					if (bExc != b2 && _h.GetPosition(bExc).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == zh1)
					{
						return true;
					}
				}

				return false;
			case "rasi: in house: from rasi:":
				zh1  = StringToRasi(simpleVals[0]);
				hse1 = StringToHouse(simpleVals[2]);
				zh2  = StringToRasi(simpleVals[4]);
				if (new ZodiacHouse(zh1).Add(hse1).Sign == zh2)
				{
					return true;
				}

				return false;
			case "birth in time:": return CheckBirthTime(simpleVals[2]);
			default:
				MessageBox.Show("Unknown rule: " + cats + GetRuleName());
				return false;
		}
	}

	public bool ReduceTree(Node n)
	{
		//mhora.Log.Debug ("Enter ReduceTree {0} {1}", n.type, n.term);
		var bRet = false;
		switch (n.Type)
		{
			case Node.EType.Not:
				Debug.Assert(n.Children.Length == 1);
				bRet = !ReduceTree(n.Children[0]);
				goto reduceTreeDone;
			case Node.EType.Or:
				for (var i = 0; i < n.Children.Length; i++)
				{
					if (ReduceTree(n.Children[i]))
					{
						bRet = true;
						goto reduceTreeDone;
					}
				}

				bRet = false;
				goto reduceTreeDone;
			case Node.EType.And:
				for (var i = 0; i < n.Children.Length; i++)
				{
					if (ReduceTree(n.Children[i]) == false)
					{
						bRet = false;
						goto reduceTreeDone;
					}
				}

				bRet = true;
				goto reduceTreeDone;
			default:
			case Node.EType.Single:
				bRet = EvaluateNode(n);
				goto reduceTreeDone;
		}

	reduceTreeDone:
		//mhora.Log.Debug ("Exit ReduceTree {0} {1} {2}", n.type, n.term, bRet);
		return bRet;
	}

	public bool ReduceTree()
	{
		return ReduceTree(RootNode);
	}

	public void GenerateSimpleParseTreeForNode(Queue q, Node n)
	{
		var text = n.Term;

		// remove general whitespace
		text = TrimWhitespace(text);

		var bOpen  = Regex.IsMatch(text, @"\(");
		var bClose = Regex.IsMatch(text, @"\)");

		var mDiv = Regex.Match(text, @"^([^&!<\(]*@)");
		if (mDiv.Success)
		{
			n.Dtype = StringToDivision(mDiv.Groups[1].Value);
			text    = text.Replace(mDiv.Groups[1].Value, string.Empty);
			//				mhora.Log.Debug ("Match. Replaced {0}. Text now {1}", 
			//					mDiv.Groups[1].Value, text);
		}

		// already in simple format
		if (false == bOpen && false == bClose)
		{
			n.Type = Node.EType.Single;
			n.Term = text;
			//mhora.Log.Debug ("Need to evaluate simple node {0}", text);
			return;
		}

		// Find operator. One of !, &&, ||
		if (text[0] == '!')
		{
			var notChild = new Node(n, text.Substring(1, text.Length - 1), n.Dtype);
			q.Enqueue(notChild);

			n.Type = Node.EType.Not;
			n.AddChild(notChild);
			return;
		}

		if (text[0] == '&' && text[1] == '&')
		{
			n.Type = Node.EType.And;
		}

		else if (text[0] == '|' && text[1] == '|')
		{
			n.Type = Node.EType.Or;
		}

		// non-binary term with brackets. Peel & reparse
		else
		{
			n.Term = PeelOuterBrackets(text);
			q.Enqueue(n);
		}


		// Parse terms with more than one subterm
		if (n.Type == Node.EType.And || n.Type == Node.EType.Or)
		{
			var subTerms = GetComplexTerms(text);
			foreach (var subTerm in subTerms)
			{
				var subChild = new Node(n, subTerm, n.Dtype);
				q.Enqueue(subChild);
				n.AddChild(subChild);
			}
		}

		//mhora.Log.Debug ("Need to evaluate complex node {0}", text);
	}

	public void GenerateSimpleParseTree()
	{
		var q = new Queue();
		q.Enqueue(RootNode);

		while (q.Count > 0)
		{
			var n = (Node) q.Dequeue();
			if (n == null)
			{
				throw new Exception("FindYogas::generateSimpleParseTree. Dequeued null");
			}

			GenerateSimpleParseTreeForNode(q, n);
		}
	}


	public Body.BodyType StringToBody(string s)
	{
		switch (s)
		{
			case "su":
			case "sun": return Body.BodyType.Sun;
			case "mo":
			case "moo":
			case "moon": return Body.BodyType.Moon;
			case "ma":
			case "mar":
			case "mars": return Body.BodyType.Mars;
			case "me":
			case "mer":
			case "mercury": return Body.BodyType.Mercury;
			case "ju":
			case "jup":
			case "jupiter": return Body.BodyType.Jupiter;
			case "ve":
			case "ven":
			case "venus": return Body.BodyType.Venus;
			case "sa":
			case "sat":
			case "saturn": return Body.BodyType.Saturn;
			case "ra":
			case "rah":
			case "rahu": return Body.BodyType.Rahu;
			case "ke":
			case "ket":
			case "ketu": return Body.BodyType.Ketu;
			case "la":
			case "lag":
			case "lagna":
			case "asc": return Body.BodyType.Lagna;
			default:
				MessageBox.Show("Unknown body: " + s + GetRuleName());
				return Body.BodyType.Other;
		}
	}

	public Division StringToDivision(string s)
	{
		// trim trailing @
		s = s.Substring(0, s.Length - 1);

		Vargas.DivisionType dtype;
		switch (s)
		{
			case "rasi":
			case "d-1":
			case "d1":
				dtype = Vargas.DivisionType.Rasi;
				break;
			case "navamsa":
			case "d-9":
			case "d9":
				dtype = Vargas.DivisionType.Navamsa;
				break;
			default:
				MessageBox.Show("Unknown division: " + s + GetRuleName());
				dtype = Vargas.DivisionType.Rasi;
				break;
		}

		return new Division(dtype);
	}

	public ZodiacHouse.Rasi StringToRasi(string s)
	{
		switch (s)
		{
			case "ari": return ZodiacHouse.Rasi.Ari;
			case "tau": return ZodiacHouse.Rasi.Tau;
			case "gem": return ZodiacHouse.Rasi.Gem;
			case "can": return ZodiacHouse.Rasi.Can;
			case "leo": return ZodiacHouse.Rasi.Leo;
			case "vir": return ZodiacHouse.Rasi.Vir;
			case "lib": return ZodiacHouse.Rasi.Lib;
			case "sco": return ZodiacHouse.Rasi.Sco;
			case "sag": return ZodiacHouse.Rasi.Sag;
			case "cap": return ZodiacHouse.Rasi.Cap;
			case "aqu": return ZodiacHouse.Rasi.Aqu;
			case "pis": return ZodiacHouse.Rasi.Pis;
			default:
				MessageBox.Show("Unknown rasi: " + s + GetRuleName());
				return ZodiacHouse.Rasi.Ari;
		}
	}

	public int StringToHouse(string s)
	{
		var tempVal = 0;

		switch (s)
		{
			case "1":
			case "1st":
				tempVal = 1;
				break;
			case "2":
			case "2nd":
				tempVal = 2;
				break;
			case "3":
			case "3rd":
				tempVal = 3;
				break;
			case "4":
			case "4th":
				tempVal = 4;
				break;
			case "5":
			case "5th":
				tempVal = 5;
				break;
			case "6":
			case "6th":
				tempVal = 6;
				break;
			case "7":
			case "7th":
				tempVal = 7;
				break;
			case "8":
			case "8th":
				tempVal = 8;
				break;
			case "9":
			case "9th":
				tempVal = 9;
				break;
			case "10":
			case "10th":
				tempVal = 10;
				break;
			case "11":
			case "11th":
				tempVal = 11;
				break;
			case "12":
			case "12th":
				tempVal = 12;
				break;
		}

		return tempVal;
	}

	public string ReplaceBasicNodeCat(string cat)
	{
		switch (cat)
		{
			case "simplelordof:":
			case "lordof:":
			case "dispof:":
				//case "grahasin":
				return string.Empty;
			default: return cat;
		}
	}

	public string ReplaceBasicNodeTermHelper(Division d, string cat, string val)
	{
		var              tempVal = 0;
		ZodiacHouse.Rasi zh;
		Body.BodyType        b;
		switch (cat)
		{
			case "rasi:":
			case "house:":
			case "hse:":
				tempVal = StringToHouse(val);
				if (tempVal > 0)
				{
					return _zhLagna.Add(tempVal).ToString().ToLower();
				}

				switch (val)
				{
					case "kendra": return "1st,4th,7th,10th";
				}

				break;
			case "gr:":
			case "Graha:":
				switch (val)
				{
					case "ben": return "mer,jup,ven,moo";
				}

				break;
			case "rasiof:":
				b = StringToBody(val);
				return _h.GetPosition(b).ToDivisionPosition(d).ZodiacHouse.Sign.ToString().ToLower();
			case "lordof:":
				tempVal = StringToHouse(val);
				if (tempVal > 0)
				{
					return _h.LordOfZodiacHouse(_zhLagna.Add(tempVal), d).ToString().ToLower();
				}

				zh = StringToRasi(val);
				return _h.LordOfZodiacHouse(zh, d).ToString().ToLower();
			case "simplelordof:":
				tempVal = StringToHouse(val);
				if (tempVal > 0)
				{
					return _h.LordOfZodiacHouse(_zhLagna.Add(tempVal), d).ToString().ToLower();
				}

				zh = StringToRasi(val);
				return zh.SimpleLordOfZodiacHouse().ToString().ToLower();
			case "dispof:":
				b = StringToBody(val);
				return _h.LordOfZodiacHouse(_h.GetPosition(b).ToDivisionPosition(d).ZodiacHouse, d).ToString().ToLower();
		}

		return val;
	}

	public string GetDivision(string sTerm)
	{
		var mDiv = Regex.Match(sTerm, "<(.*)@");
		if (mDiv.Success)
		{
			return mDiv.Groups[1].Value.ToLower();
		}

		return string.Empty;
	}

	public string GetCategory(string sTerm)
	{
		// Find categofy
		var mCat = Regex.Match(sTerm, "<.*@(.*:)");
		if (mCat.Success == false)
		{
			mCat = Regex.Match(sTerm, "<(.*:)");
		}

		if (mCat.Success)
		{
			return mCat.Groups[1].Value.ToLower();
		}

		return sTerm;
	}

	public ArrayList GetValues(string sTerm)
	{
		// Find values. Find : or , on the left
		var alVals = new ArrayList();
		var mVals  = Regex.Matches(sTerm, "[:,]([^<:,>]*)");
		if (mVals.Count >= 1)
		{
			foreach (Match m in mVals)
			{
				alVals.Add(m.Groups[1].Value.ToLower());
			}
		}
		else
		{
			alVals.Add(sTerm);
		}

		return alVals;
	}

	public string ReplaceBasicNodeTerm(Division d, string sTerm)
	{
		var sDiv   = GetDivision(sTerm);
		var sCat   = GetCategory(sTerm);
		var alVals = GetValues(sTerm);

		var hash = new Hashtable();
		foreach (string s in alVals)
		{
			var sRep = ReplaceBasicNodeTermHelper(d, sCat, s);
			if (!hash.ContainsKey(sRep))
			{
				hash.Add(sRep, null);
			}
		}

		var bStart       = false;
		var sNew         = ReplaceBasicNodeCat(sCat);
		var sPreserveCat = sNew.Length == 0;

		if (false == sPreserveCat)
		{
			sNew = "<" + sNew;
		}

		var alSort = new ArrayList();
		foreach (string s in hash.Keys)
		{
			alSort.Add(s);
		}

		alSort.Sort();

		foreach (string s in alSort)
		{
			if (bStart)
			{
				sNew += "," + s;
			}
			else
			{
				sNew += s;
			}

			bStart = true;
		}

		if (false == sPreserveCat)
		{
			sNew += ">";
		}

		//mhora.Log.Debug ("{0} evals to {1}", sTerm, sNew);
		return sNew;
	}

	public string SimplifyBasicNodeTerm(Node n, string sTerm)
	{
		while (true)
		{
			//mhora.Log.Debug ("Simplifying basic term: .{0}.", sTerm);		
			var m = Regex.Match(sTerm, "<[^<>]*>");

			// No terms found. Nothing to do.
			if (m.Success == false)
			{
				return sTerm;
			}

			var d      = n.Dtype;
			var sInner = m.Value;

			// see if a varga was explicitly specified
			var mDiv = Regex.Match(sInner, "<([^:<>]*@)");
			if (mDiv.Success)
			{
				d = StringToDivision(mDiv.Groups[1].Value);
				sInner.Replace(mDiv.Groups[1].Value, string.Empty);
			}

			// Found a term, evaluated it. Nothing happened. Done.
			var newInner = ReplaceBasicNodeTerm(d, sInner);

			//mhora.Log.Debug ("{0} && {1}", newInner.Length, m.Value.Length);

			if (newInner == m.Value.ToLower())
			{
				return sTerm;
			}

			// Replace the current term and continue along merrily
			sTerm = sTerm.Replace(m.Value, newInner);
		}
	}

	public void SimplifyBasicNode(Queue q, Node n)
	{
		// A simple wrapper that takes each individual whitespace
		// separated term, and tries to simplify it down to bare
		// bones single stuff ready for true / false evaluation
		//string cats = "";

		var sNew        = string.Empty;
		var simpleTerms = n.Term.Split(' ');
		for (var i = 0; i < simpleTerms.Length; i++)
		{
			simpleTerms[i] =  SimplifyBasicNodeTerm(n, simpleTerms[i]);
			sNew           += " " + simpleTerms[i];
			//cats += " " + this.getCategory(simpleTerms[i]);
		}

		n.Term = TrimWhitespace(sNew);

		//cats = this.trimWhitespace(cats);
		//mhora.Log.Debug ("Cats = {0}", cats);
	}

	public void ExpandSimpleNode(Queue q, Node n)
	{
		// <a,b,> op <d,e> 
		// becomes
		// ||(<a> op <e>)(<a> op <e>)(<b> op <d>)(<b> op <e>)

		var eLogic = Node.EType.Or;

		//mhora.Log.Debug ("Inner logic: n.term is {0}", n.term);
		if (n.Term[0] == '&' && n.Term[1] == '&')
		{
			eLogic = Node.EType.And;
			n.Term = TrimWhitespace(n.Term.Substring(2, n.Term.Length - 2));
		}
		else if (n.Term[0] == '|' && n.Term[1] == '|')
		{
			n.Term = TrimWhitespace(n.Term.Substring(2, n.Term.Length - 2));
		}
		//mhora.Log.Debug ("Inner logic: n.term is now {0}", n.term);

		// find num Vals etc
		var simpleTerms         = n.Term.Split(' ');
		var catTerms            = new string[simpleTerms.Length];
		var simpleTermsValues   = new int[simpleTerms.Length];
		var simpleTermsRealVals = new ArrayList[simpleTerms.Length];

		var numExps = 1;

		// determine total # exps
		for (var i = 0; i < simpleTerms.Length; i++)
		{
			catTerms[i]            = GetCategory(simpleTerms[i]);
			simpleTermsRealVals[i] = GetValues(simpleTerms[i]);
			simpleTermsValues[i]   = simpleTermsRealVals[i].Count;
			if (simpleTermsValues[i] > 1)
			{
				numExps *= simpleTermsValues[i];
			}
		}

		//mhora.Log.Debug ("Exp: {0} requires {1} exps", n.term, numExps);

		// done
		if (numExps <= 1)
		{
			return;
		}

		var sNew = new string[numExps];

		// use binary reduction. first term repeats n times, then n/2 etc.
		// "binary" actualy n-ary on number of possible values
		var numConc = numExps;
		for (var i = 0; i < simpleTerms.Length; i++)
		{
			// if more than one value, n-ary reduction
			if (simpleTermsValues[i] > 1)
			{
				numConc /= simpleTermsValues[i];
			}

			// determine repeat count. with one value, assign to 1
			var num = numConc;
			if (simpleTermsValues[i] == 1)
			{
				num = 1;
			}

			// baseIndex increments to numConc after each iteration
			// continue till we fill the list
			var baseIndex = 0;
			var valIndex  = 0;
			while (baseIndex < numExps)
			{
				for (var j = 0; j < num; j++)
				{
					var ix = valIndex;
					if (simpleTermsValues[i] == 1)
					{
						ix = 0;
					}

					sNew[baseIndex + j] += " ";
					if (catTerms[i][catTerms[i].Length - 1] == ':')
					{
						sNew[baseIndex + j] += "<" + catTerms[i] + simpleTermsRealVals[i][ix] + ">";
					}
					else
					{
						sNew[baseIndex + j] += simpleTermsRealVals[i][ix];
					}
				}

				baseIndex += num;
				valIndex++;
				if (valIndex == simpleTermsValues[i])
				{
					valIndex = 0;
				}
			}
		}

		n.Type = eLogic;
		for (var i = 0; i < sNew.Length; i++)
		{
			var nChild = new Node(n, TrimWhitespace(sNew[i]), n.Dtype);
			n.AddChild(nChild);
			//mhora.Log.Debug ("sNew[{0}]: {1}", i, sNew[i]);
		}
	}

	public void ExpandSimpleNodes()
	{
		var q = new Queue();
		q.Enqueue(RootNode);

		while (q.Count > 0)
		{
			var n = (Node) q.Dequeue();
			if (n == null)
			{
				throw new Exception("FindYogas::expandSimpleNodes. Dequeued null");
			}

			if (n.Type == Node.EType.Single)
			{
				SimplifyBasicNode(q, n);
			}
			else
			{
				foreach (var nChild in n.Children)
				{
					q.Enqueue(nChild);
				}
			}
		}


		q.Enqueue(RootNode);
		while (q.Count > 0)
		{
			var n = (Node) q.Dequeue();
			if (n.Type == Node.EType.Single)
			{
				ExpandSimpleNode(q, n);
			}
			else
			{
				foreach (var nChild in n.Children)
				{
					q.Enqueue(nChild);
				}
			}
		}
	}

	public class Node
	{
		public enum EType
		{
			And,
			Or,
			Not,
			Single
		}

		public Node[]   Children;
		public Division Dtype;
		public Node     Parent;
		public string   Term;
		public EType    Type;

		public Node(Node parent, string term, Division dtype)
		{
			Parent   = parent;
			Term     = term;
			Dtype    = dtype;
			Type     = EType.Single;
			Children = new Node[0];
		}

		public bool HasChildren()
		{
			if (Children != null)
			{
				return true;
			}

			return false;
		}

		public bool IsRoot()
		{
			if (Parent == null)
			{
				return true;
			}

			return false;
		}

		public void AddChild(Node nChild)
		{
			ArrayList al = null;

			if (Children != null)
			{
				al = new ArrayList(Children);
			}
			else
			{
				al = new ArrayList();
			}

			al.Add(nChild);
			Children = (Node[]) al.ToArray(typeof(Node));
		}
	}
}