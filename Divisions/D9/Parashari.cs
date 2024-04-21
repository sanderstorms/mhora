using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mhora.Divisions.D9
{
	//D9 - Parashari Navamsha
	//
	//Method1:
	//Triad	Signs	Sequence of Navamsha
	//Fiery		Ari, Leo, Sag	Ari, Tau, Gem, Can, Leo, Vir, Lib, Sco, Sag
	//Earthy	Tau, Vir, Cap	Cap, Aqu, Pis, Ari, Tau, Gem, Can, Leo, Vir
	//Ariy		Gem, Lib, Aqu	Lib, Sco, Sag, Cap, Aqu, Pis, Ari, Tau, Gem
	//Watery	Can, Sco, Pis	Can, Leo, Vir, Lib, Sco, Sag, Cap, Aqu, Pis
	//
	//Method2:
	//For movable signs, the first navamsha occupy the sign itself. For fixed signs, the first navamsha
	//occupy the ninth sign from itself. For dual signs, the first navamsha occupy the fifth sign from itself.
	//The other navamsha follows in direct order.
	//
	//Method3:
	//Multiply the longitude of the planet (including sign) by nine. Expunge multiples of 12 signs.
	//The resultant is the longitude of the planet in navamsha.
	//
	//Method4:
	//Multiply the the number of completed signs by 9. Add the number of navamsha occupied in the sign.
	//Expunge multiples of twelve from the sum. The reminder will be the sign occupied in navamsha.
	//
	//Part	Degrees	Ari, Leo, Sag	Tau, Vir, Cap	Gem, Lib, Aqu	Can, Sco, Pis
	//1		3⁰ - 20'	Ari				Cap				Lib				Can
	//2		6⁰ - 40'	Tau				Aqu				Sco				Leo
	//3		10⁰ - 00'	Gem				Pis				Sag				Vir
	//4		13⁰ - 20'	Can				Ari				Cap				Lib
	//5		16⁰ - 40'	Leo				Tau				Aqu				Sco
	//6		20⁰ - 00'	Vir				Gem				Pis				Sag
	//7		23⁰ - 20'	Lib				Can				Ari				Cap
	//8		26⁰ - 40'	Sco				Leo				Tau				Aqu
	//9		30⁰ - 00'	Sag				Vir				Gem				Pis
	internal class Parashari
	{
	}
}
