using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerlinMap {
	class MT19937 {
		/* Period parameters */
		private const int N = 624;
		private const int M = 397;
		private const uint MATRIX_A = 0x9908b0df; /* constant vector a */
		private const uint UPPER_MASK = 0x80000000; /* most significant w-r bits */
		private const uint LOWER_MASK = 0x7fffffff; /* least significant r bits */

		/* Tempering parameters */
		private const uint TEMPERING_MASK_B = 0x9d2c5680;
		private const uint TEMPERING_MASK_C = 0xefc60000;

		private static uint TEMPERING_SHIFT_U(uint y) { return (y >> 11); }
		private static uint TEMPERING_SHIFT_S(uint y) { return (y << 7); }
		private static uint TEMPERING_SHIFT_T(uint y) { return (y << 15); }
		private static uint TEMPERING_SHIFT_L(uint y) { return (y >> 18); }

		//static unsigned long mt[N]; /* the array for the state vector  */
		private static uint[] mt = new uint[N];

		// static int mti=N+1; /* mti==N+1 means mt[N] is not initialized */
		private static short mti = N + 1; /* mti==N+1 means mt[N] is not initialized */

		/* initializing the array with a NONZERO seed */
		public static void Seed(uint seed) {
			/* setting initial seeds to mt[N] using         */
			/* the generator Line 25 of Table 1 in          */
			/* [KNUTH 1981, The Art of Computer Programming */
			/*    Vol. 2 (2nd Ed.), pp102]                  */

			mt[0] = seed & 0xffffffffU;
			for (mti = 1; mti < N; mti++) {
				mt[mti] = (69069 * mt[mti - 1]) & 0xffffffffU;
			}
		}

		private static uint[/* 2 */] mag01 = { 0x0, MATRIX_A };
		/* generating reals */
		/* unsigned long */
		/* for integer generation */
		public static double Random() {
			uint y;

			/* mag01[x] = x * MATRIX_A  for x=0,1 */
			if (mti >= N) /* generate N words at one time */ {
				short kk;

				if (mti == N + 1) /* if sgenrand() has not been called, */ {
					Seed(4357); /* a default initial seed is used   */
				}

				for (kk = 0; kk < N - M; kk++) {
					y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
					mt[kk] = mt[kk + M] ^ (y >> 1) ^ mag01[y & 0x1];
				}

				for (; kk < N - 1; kk++) {
					y = (mt[kk] & UPPER_MASK) | (mt[kk + 1] & LOWER_MASK);
					mt[kk] = mt[kk + (M - N)] ^ (y >> 1) ^ mag01[y & 0x1];
				}

				y = (mt[N - 1] & UPPER_MASK) | (mt[0] & LOWER_MASK);
				mt[N - 1] = mt[M - 1] ^ (y >> 1) ^ mag01[y & 0x1];

				mti = 0;
			}

			y = mt[mti++];
			y ^= TEMPERING_SHIFT_U(y);
			y ^= TEMPERING_SHIFT_S(y) & TEMPERING_MASK_B;
			y ^= TEMPERING_SHIFT_T(y) & TEMPERING_MASK_C;
			y ^= TEMPERING_SHIFT_L(y);

			return ((double)y / 0xffffffffU); /* reals */
			/* return y; */
			/* for integer generation */
		}

		public static int Random(int upper) {
			return Convert.ToInt32(Random() * upper);
		}

		public static int Random(int lower, int upper) {
			return Random(upper - lower) + lower;
		}

	}
}
