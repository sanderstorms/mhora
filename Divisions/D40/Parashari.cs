using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mhora.Divisions.D40
{
	//Parashara Khavedamsha (D40)	
	//Part	Deg			Ari	Tau	Gem	Can	Leo	Vir	Lib	Sco	Sag	Cap	Aqu	Pis
	//1		00⁰-45'		Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib
	//2		01⁰-30'		Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco
	//3		02⁰-15'		Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag
	//4		03⁰-00'		Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap
	//5		03⁰-45'		Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu
	//6		04⁰-30'		Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis
	//7		05⁰-15'		Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari
	//8		06⁰-00'		Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau
	//9		06⁰-45'		Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem
	//10	07⁰-30'		Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can
	//11	08⁰-15'		Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo
	//12	09⁰-00'		Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir
	//13	09⁰-45'		Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib
	//14	10⁰-30'		Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco
	//15	11⁰-15'		Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag
	//16	12⁰-00'		Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap
	//17	12⁰-45'		Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu
	//18	13⁰-30'		Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis
	//19	14⁰-15'		Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari
	//20	15⁰-00'		Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau
	//21	15⁰-45'		Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem
	//22	16⁰-30'		Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can
	//23	17⁰-15'		Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo
	//24	18⁰-00'		Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir
	//25	18⁰-45'		Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib
	//26	19⁰-30'		Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco
	//27	20⁰-15'		Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag
	//28	21⁰-00'		Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap
	//29	21⁰-45'		Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu
	//30	22⁰-30'		Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis
	//31	23⁰-15'		Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari
	//32	24⁰-00'		Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau
	//33	24⁰-45'		Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem
	//34	25⁰-30'		Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can
	//35	26⁰-15'		Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo	Aqu	Leo
	//36	27⁰-00'		Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir	Pis	Vir
	//37	27⁰-45'		Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib	Ari	Lib
	//38	28⁰-30'		Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco	Tau	Sco
	//39	29⁰-15'		Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag	Gem	Sag
	//40	30⁰-00'		Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap	Can	Cap
													
	internal class Parashari
	{
	}
}
