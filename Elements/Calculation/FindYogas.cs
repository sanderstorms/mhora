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

	private readonly Horoscope   h;
	private readonly ZodiacHouse zhLagna;
	public           Node        rootNode;


	private XmlYogaNode xmlNode;


	public FindYogas(Horoscope _h, Division __dtype)
	{
		h       = _h;
		_dtype  = __dtype;
		zhLagna = h.GetPosition(Body.BodyType.Lagna).ToDivisionPosition(_dtype).ZodiacHouse;
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

	public string getRuleName()
	{
		if (xmlNode == null || xmlNode.mhoraRule == null)
		{
			return string.Empty;
		}

		return xmlNode.mhoraRule;
	}

	public bool evaluateYoga(XmlYogaNode n)
	{
		xmlNode = n;
		return evaluateYoga(n.mhoraRule);
	}

	public bool evaluateYoga(string rule)
	{
		rootNode = new Node(null, rule, _dtype);

		//mhora.Log.Debug ("");
		//mhora.Log.Debug ("Evaluating yoga .{0}.", rule);
		generateSimpleParseTree();
		expandSimpleNodes();
		var bRet = reduceTree();

		//mhora.Log.Debug ("Final: {0} = {1}", bRet, rule);
		//mhora.Log.Debug ("");
		return bRet;
	}


	public string trimWhitespace(string sCurr)
	{
		// remove leading & trailing whitespaces
		sCurr = Regex.Replace(sCurr, @"^\s*(.*?)\s*$", "$1");

		// remove contiguous whitespace
		sCurr = Regex.Replace(sCurr, @"(\s+)", " ");

		return sCurr;
	}

	public string peelOuterBrackets(string sCurr)
	{
		// remove leading "(" and whitespace
		sCurr = Regex.Replace(sCurr, @"^\s*\(\s*", string.Empty);
		// remove trailing ")" and whitespace
		sCurr = Regex.Replace(sCurr, @"\s*\)\s*$", string.Empty);
		return sCurr;
	}

	public string[] getComplexTerms(string sInit)
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

	public bool checkBirthTime(string sTime)
	{
		switch (sTime)
		{
			case "day":   return h.IsDayBirth();
			case "night": return !h.IsDayBirth();
			default:
				MessageBox.Show("Unknown birth time: " + sTime + getRuleName());
				return false;
		}
	}

	public bool evaluateNode(Node n)
	{
		Debug.Assert(n.type == Node.EType.Single);

		var cats        = string.Empty;
		var simpleTerms = n.term.Split(' ');
		var simpleVals  = new string[simpleTerms.Length];
		for (var i = 0; i < simpleTerms.Length; i++)
		{
			cats          += " " + getCategory(simpleTerms[i]);
			simpleVals[i] =  (string) getValues(simpleTerms[i])[0];
		}

		cats = trimWhitespace(cats);

		Body.BodyType        b1,   b2, b3;
		ZodiacHouse.Rasi zh1,  zh2;
		int              hse1, hse2;

		var evalDiv = n.dtype;
		switch (cats)
		{
			case "gr: in rasi:":
			case "gr: in house:":
				b1  = stringToBody(simpleVals[0]);
				zh1 = stringToRasi(simpleVals[2]);
				if (h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == zh1)
				{
					return true;
				}

				return false;
			case "gr: in mt":
			case "gr: in moolatrikona":
				b1 = stringToBody(simpleVals[0]);
				return h.GetPosition(b1).ToDivisionPosition(evalDiv).IsInMoolaTrikona();
			case "gr: in exlt":
			case "gr: in exaltation":
				b1 = stringToBody(simpleVals[0]);
				return h.GetPosition(b1).ToDivisionPosition(evalDiv).IsExaltedPhalita();
			case "gr: in deb":
			case "gr: in debilitation":
				b1 = stringToBody(simpleVals[0]);
				return h.GetPosition(b1).ToDivisionPosition(evalDiv).IsDebilitatedPhalita();
			case "gr: in own":
			case "gr: in ownhouse":
			case "gr: in own house":
				b1 = stringToBody(simpleVals[0]);
				return h.GetPosition(b1).ToDivisionPosition(evalDiv).IsInOwnHouse();
			case "gr: is gr:":
				b1 = stringToBody(simpleVals[0]);
				b2 = stringToBody(simpleVals[2]);
				if (b1 == b2)
				{
					return true;
				}

				return false;
			case "gr: with gr:":
				b1 = stringToBody(simpleVals[0]);
				b2 = stringToBody(simpleVals[2]);
				if (h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == h.GetPosition(b2).ToDivisionPosition(evalDiv).ZodiacHouse.Sign)
				{
					return true;
				}

				return false;
			case "gr: asp gr:":
				b1 = stringToBody(simpleVals[0]);
				b2 = stringToBody(simpleVals[2]);
				if (h.GetPosition(b1).ToDivisionPosition(evalDiv).GrahaDristi(h.GetPosition(b2).ToDivisionPosition(evalDiv).ZodiacHouse))
				{
					return true;
				}

				return false;
			case "gr: in house: from rasi:":
				b1   = stringToBody(simpleVals[0]);
				hse1 = stringToHouse(simpleVals[2]);
				zh1  = stringToRasi(simpleVals[4]);
				if (h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == new ZodiacHouse(zh1).Add(hse1).Sign)
				{
					return true;
				}

				return false;
			case "gr: in house: from gr:":
				b1   = stringToBody(simpleVals[0]);
				hse1 = stringToHouse(simpleVals[2]);
				b2   = stringToBody(simpleVals[4]);
				return h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == h.GetPosition(b2).ToDivisionPosition(evalDiv).ZodiacHouse.Add(hse1).Sign;
			case "Graha in house: from gr: except gr:":
				hse1 = stringToHouse(simpleVals[2]);
				b1   = stringToBody(simpleVals[4]);
				b2   = stringToBody(simpleVals[6]);
				zh1  = h.GetPosition(b1).ToDivisionPosition(evalDiv).ZodiacHouse.Add(hse1).Sign;
				for (var i = (int) Body.BodyType.Sun; i <= (int) Body.BodyType.Lagna; i++)
				{
					var bExc = (Body.BodyType) i;
					if (bExc != b2 && h.GetPosition(bExc).ToDivisionPosition(evalDiv).ZodiacHouse.Sign == zh1)
					{
						return true;
					}
				}

				return false;
			case "rasi: in house: from rasi:":
				zh1  = stringToRasi(simpleVals[0]);
				hse1 = stringToHouse(simpleVals[2]);
				zh2  = stringToRasi(simpleVals[4]);
				if (new ZodiacHouse(zh1).Add(hse1).Sign == zh2)
				{
					return true;
				}

				return false;
			case "birth in time:": return checkBirthTime(simpleVals[2]);
			default:
				MessageBox.Show("Unknown rule: " + cats + getRuleName());
				return false;
		}
	}

	public bool reduceTree(Node n)
	{
		//mhora.Log.Debug ("Enter ReduceTree {0} {1}", n.type, n.term);
		var bRet = false;
		switch (n.type)
		{
			case Node.EType.Not:
				Debug.Assert(n.children.Length == 1);
				bRet = !reduceTree(n.children[0]);
				goto reduceTreeDone;
			case Node.EType.Or:
				for (var i = 0; i < n.children.Length; i++)
				{
					if (reduceTree(n.children[i]))
					{
						bRet = true;
						goto reduceTreeDone;
					}
				}

				bRet = false;
				goto reduceTreeDone;
			case Node.EType.And:
				for (var i = 0; i < n.children.Length; i++)
				{
					if (reduceTree(n.children[i]) == false)
					{
						bRet = false;
						goto reduceTreeDone;
					}
				}

				bRet = true;
				goto reduceTreeDone;
			default:
			case Node.EType.Single:
				bRet = evaluateNode(n);
				goto reduceTreeDone;
		}

	reduceTreeDone:
		//mhora.Log.Debug ("Exit ReduceTree {0} {1} {2}", n.type, n.term, bRet);
		return bRet;
	}

	public bool reduceTree()
	{
		return reduceTree(rootNode);
	}

	public void generateSimpleParseTreeForNode(Queue q, Node n)
	{
		var text = n.term;

		// remove general whitespace
		text = trimWhitespace(text);

		var bOpen  = Regex.IsMatch(text, @"\(");
		var bClose = Regex.IsMatch(text, @"\)");

		var mDiv = Regex.Match(text, @"^([^&!<\(]*@)");
		if (mDiv.Success)
		{
			n.dtype = stringToDivision(mDiv.Groups[1].Value);
			text    = text.Replace(mDiv.Groups[1].Value, string.Empty);
			//				mhora.Log.Debug ("Match. Replaced {0}. Text now {1}", 
			//					mDiv.Groups[1].Value, text);
		}

		// already in simple format
		if (false == bOpen && false == bClose)
		{
			n.type = Node.EType.Single;
			n.term = text;
			//mhora.Log.Debug ("Need to evaluate simple node {0}", text);
			return;
		}

		// Find operator. One of !, &&, ||
		if (text[0] == '!')
		{
			var notChild = new Node(n, text.Substring(1, text.Length - 1), n.dtype);
			q.Enqueue(notChild);

			n.type = Node.EType.Not;
			n.addChild(notChild);
			return;
		}

		if (text[0] == '&' && text[1] == '&')
		{
			n.type = Node.EType.And;
		}

		else if (text[0] == '|' && text[1] == '|')
		{
			n.type = Node.EType.Or;
		}

		// non-binary term with brackets. Peel & reparse
		else
		{
			n.term = peelOuterBrackets(text);
			q.Enqueue(n);
		}


		// Parse terms with more than one subterm
		if (n.type == Node.EType.And || n.type == Node.EType.Or)
		{
			var subTerms = getComplexTerms(text);
			foreach (var subTerm in subTerms)
			{
				var subChild = new Node(n, subTerm, n.dtype);
				q.Enqueue(subChild);
				n.addChild(subChild);
			}
		}

		//mhora.Log.Debug ("Need to evaluate complex node {0}", text);
	}

	public void generateSimpleParseTree()
	{
		var q = new Queue();
		q.Enqueue(rootNode);

		while (q.Count > 0)
		{
			var n = (Node) q.Dequeue();
			if (n == null)
			{
				throw new Exception("FindYogas::generateSimpleParseTree. Dequeued null");
			}

			generateSimpleParseTreeForNode(q, n);
		}
	}


	public Body.BodyType stringToBody(string s)
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
				MessageBox.Show("Unknown body: " + s + getRuleName());
				return Body.BodyType.Other;
		}
	}

	public Division stringToDivision(string s)
	{
		// trim trailing @
		s = s.Substring(0, s.Length - 1);

		Vargas.DivisionType _dtype;
		switch (s)
		{
			case "rasi":
			case "d-1":
			case "d1":
				_dtype = Vargas.DivisionType.Rasi;
				break;
			case "navamsa":
			case "d-9":
			case "d9":
				_dtype = Vargas.DivisionType.Navamsa;
				break;
			default:
				MessageBox.Show("Unknown division: " + s + getRuleName());
				_dtype = Vargas.DivisionType.Rasi;
				break;
		}

		return new Division(_dtype);
	}

	public ZodiacHouse.Rasi stringToRasi(string s)
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
				MessageBox.Show("Unknown rasi: " + s + getRuleName());
				return ZodiacHouse.Rasi.Ari;
		}
	}

	public int stringToHouse(string s)
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

	public string replaceBasicNodeCat(string cat)
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

	public string replaceBasicNodeTermHelper(Division d, string cat, string val)
	{
		var              tempVal = 0;
		ZodiacHouse.Rasi zh;
		Body.BodyType        b;
		switch (cat)
		{
			case "rasi:":
			case "house:":
			case "hse:":
				tempVal = stringToHouse(val);
				if (tempVal > 0)
				{
					return zhLagna.Add(tempVal).ToString().ToLower();
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
				b = stringToBody(val);
				return h.GetPosition(b).ToDivisionPosition(d).ZodiacHouse.Sign.ToString().ToLower();
			case "lordof:":
				tempVal = stringToHouse(val);
				if (tempVal > 0)
				{
					return h.LordOfZodiacHouse(zhLagna.Add(tempVal), d).ToString().ToLower();
				}

				zh = stringToRasi(val);
				return h.LordOfZodiacHouse(zh, d).ToString().ToLower();
			case "simplelordof:":
				tempVal = stringToHouse(val);
				if (tempVal > 0)
				{
					return h.LordOfZodiacHouse(zhLagna.Add(tempVal), d).ToString().ToLower();
				}

				zh = stringToRasi(val);
				return zh.SimpleLordOfZodiacHouse().ToString().ToLower();
			case "dispof:":
				b = stringToBody(val);
				return h.LordOfZodiacHouse(h.GetPosition(b).ToDivisionPosition(d).ZodiacHouse, d).ToString().ToLower();
		}

		return val;
	}

	public string getDivision(string sTerm)
	{
		var mDiv = Regex.Match(sTerm, "<(.*)@");
		if (mDiv.Success)
		{
			return mDiv.Groups[1].Value.ToLower();
		}

		return string.Empty;
	}

	public string getCategory(string sTerm)
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

	public ArrayList getValues(string sTerm)
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

	public string replaceBasicNodeTerm(Division d, string sTerm)
	{
		var sDiv   = getDivision(sTerm);
		var sCat   = getCategory(sTerm);
		var alVals = getValues(sTerm);

		var hash = new Hashtable();
		foreach (string s in alVals)
		{
			var sRep = replaceBasicNodeTermHelper(d, sCat, s);
			if (!hash.ContainsKey(sRep))
			{
				hash.Add(sRep, null);
			}
		}

		var bStart       = false;
		var sNew         = replaceBasicNodeCat(sCat);
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

	public string simplifyBasicNodeTerm(Node n, string sTerm)
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

			var d      = n.dtype;
			var sInner = m.Value;

			// see if a varga was explicitly specified
			var mDiv = Regex.Match(sInner, "<([^:<>]*@)");
			if (mDiv.Success)
			{
				d = stringToDivision(mDiv.Groups[1].Value);
				sInner.Replace(mDiv.Groups[1].Value, string.Empty);
			}

			// Found a term, evaluated it. Nothing happened. Done.
			var newInner = replaceBasicNodeTerm(d, sInner);

			//mhora.Log.Debug ("{0} && {1}", newInner.Length, m.Value.Length);

			if (newInner == m.Value.ToLower())
			{
				return sTerm;
			}

			// Replace the current term and continue along merrily
			sTerm = sTerm.Replace(m.Value, newInner);
		}
	}

	public void simplifyBasicNode(Queue q, Node n)
	{
		// A simple wrapper that takes each individual whitespace
		// separated term, and tries to simplify it down to bare
		// bones single stuff ready for true / false evaluation
		//string cats = "";

		var sNew        = string.Empty;
		var simpleTerms = n.term.Split(' ');
		for (var i = 0; i < simpleTerms.Length; i++)
		{
			simpleTerms[i] =  simplifyBasicNodeTerm(n, simpleTerms[i]);
			sNew           += " " + simpleTerms[i];
			//cats += " " + this.getCategory(simpleTerms[i]);
		}

		n.term = trimWhitespace(sNew);

		//cats = this.trimWhitespace(cats);
		//mhora.Log.Debug ("Cats = {0}", cats);
	}

	public void expandSimpleNode(Queue q, Node n)
	{
		// <a,b,> op <d,e> 
		// becomes
		// ||(<a> op <e>)(<a> op <e>)(<b> op <d>)(<b> op <e>)

		var eLogic = Node.EType.Or;

		//mhora.Log.Debug ("Inner logic: n.term is {0}", n.term);
		if (n.term[0] == '&' && n.term[1] == '&')
		{
			eLogic = Node.EType.And;
			n.term = trimWhitespace(n.term.Substring(2, n.term.Length - 2));
		}
		else if (n.term[0] == '|' && n.term[1] == '|')
		{
			n.term = trimWhitespace(n.term.Substring(2, n.term.Length - 2));
		}
		//mhora.Log.Debug ("Inner logic: n.term is now {0}", n.term);

		// find num Vals etc
		var simpleTerms         = n.term.Split(' ');
		var catTerms            = new string[simpleTerms.Length];
		var simpleTermsValues   = new int[simpleTerms.Length];
		var simpleTermsRealVals = new ArrayList[simpleTerms.Length];

		var numExps = 1;

		// determine total # exps
		for (var i = 0; i < simpleTerms.Length; i++)
		{
			catTerms[i]            = getCategory(simpleTerms[i]);
			simpleTermsRealVals[i] = getValues(simpleTerms[i]);
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
		var _numConc = numExps;
		for (var i = 0; i < simpleTerms.Length; i++)
		{
			// if more than one value, n-ary reduction
			if (simpleTermsValues[i] > 1)
			{
				_numConc /= simpleTermsValues[i];
			}

			// determine repeat count. with one value, assign to 1
			var numConc = _numConc;
			if (simpleTermsValues[i] == 1)
			{
				numConc = 1;
			}

			// baseIndex increments to numConc after each iteration
			// continue till we fill the list
			var baseIndex = 0;
			var valIndex  = 0;
			while (baseIndex < numExps)
			{
				for (var j = 0; j < numConc; j++)
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

				baseIndex += numConc;
				valIndex++;
				if (valIndex == simpleTermsValues[i])
				{
					valIndex = 0;
				}
			}
		}

		n.type = eLogic;
		for (var i = 0; i < sNew.Length; i++)
		{
			var nChild = new Node(n, trimWhitespace(sNew[i]), n.dtype);
			n.addChild(nChild);
			//mhora.Log.Debug ("sNew[{0}]: {1}", i, sNew[i]);
		}
	}

	public void expandSimpleNodes()
	{
		var q = new Queue();
		q.Enqueue(rootNode);

		while (q.Count > 0)
		{
			var n = (Node) q.Dequeue();
			if (n == null)
			{
				throw new Exception("FindYogas::expandSimpleNodes. Dequeued null");
			}

			if (n.type == Node.EType.Single)
			{
				simplifyBasicNode(q, n);
			}
			else
			{
				foreach (var nChild in n.children)
				{
					q.Enqueue(nChild);
				}
			}
		}


		q.Enqueue(rootNode);
		while (q.Count > 0)
		{
			var n = (Node) q.Dequeue();
			if (n.type == Node.EType.Single)
			{
				expandSimpleNode(q, n);
			}
			else
			{
				foreach (var nChild in n.children)
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

		public Node[]   children;
		public Division dtype;
		public Node     parent;
		public string   term;
		public EType    type;

		public Node(Node _parent, string _term, Division _dtype)
		{
			parent   = _parent;
			term     = _term;
			dtype    = _dtype;
			type     = EType.Single;
			children = new Node[0];
		}

		public bool hasChildren()
		{
			if (children != null)
			{
				return true;
			}

			return false;
		}

		public bool isRoot()
		{
			if (parent == null)
			{
				return true;
			}

			return false;
		}

		public void addChild(Node nChild)
		{
			ArrayList al = null;

			if (children != null)
			{
				al = new ArrayList(children);
			}
			else
			{
				al = new ArrayList();
			}

			al.Add(nChild);
			children = (Node[]) al.ToArray(typeof(Node));
		}
	}
}