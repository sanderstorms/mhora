using System;

namespace Mhora.Util
{
	public static class Calc
	{
        public static int NumberOfSetBits(this int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        public static double Floor(this double nr)
        {
	        if (nr < 0)
	        {
		        return -Math.Floor(-nr);
	        }

	        return (Math.Floor(nr));
        }

        public static double Round(this double nr)
        {
	        if (nr < 0)
	        {
		        return -Math.Round(-nr);
	        }

	        return (Math.Round(nr));
        }

        public static double Ceil(this double nr)
        {
	        if (nr < 0)
	        {
		        return -Math.Ceiling(-nr);
	        }

	        return (Math.Ceiling(nr));
        }

        public static double Trunc(this double nr)
        {
	        if (nr < 0)
	        {
		        return -Math.Truncate(-nr);
	        }

	        return (Math.Truncate(nr));
        }

        public static decimal Floor(this decimal nr) => (decimal) ((double) nr).Floor();
        public static decimal Ceil(this decimal nr) => (decimal) ((double) nr).Ceil();
        public static decimal Round(this decimal nr) => (decimal) ((double) nr).Round();
        public static decimal Trunc(this decimal nr) => (decimal) ((double) nr).Trunc();

	}
}
