using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace SecretNest
{
    [StructLayout(LayoutKind.Explicit)]
    public partial struct Fraction
    {
        [FieldOffset(0)]
        ulong integer;

        [FieldOffset(8)]
        ulong numerator;
        [FieldOffset(16)]
        ulong denominator;

        [FieldOffset(8)]
        uint numeratorLow;
        [FieldOffset(16)]
        uint denominatorLow;
        [FieldOffset(12)]
        uint numeratorHigh;
        [FieldOffset(20)]
        uint denominatorHigh;

        [FieldOffset(24)]
        long sign;

        void Simplify()
        {
            if (numerator == 0)
            {
                denominator = 1;
            }
            else if (numerator == denominator)
            {
                integer++;
                numerator = 0;
                denominator = 1;
            }
            else
            {
                if (numerator > denominator)
                {
                    ulong newNumerator = numerator % denominator;
                    integer += numerator / denominator;
                    numerator = newNumerator;
                }

                ulong greatestCommonDivisor = GreatestCommonDivisor(numerator, denominator);
                if (greatestCommonDivisor != 1)
                {
                    numerator /= greatestCommonDivisor;
                    denominator /= greatestCommonDivisor;
                }
            }
        }

        static ulong GreatestCommonDivisor(ulong x, ulong y) { return (y != 0) ? GreatestCommonDivisor(y, x % y) : x; }

        static BigInteger GreatestCommonDivisor(BigInteger x, BigInteger y) { return (y != 0) ? GreatestCommonDivisor(y, x % y) : x; }

        void OverflowCheck()
        {
            if (integer >= long.MaxValue || denominator >= uint.MaxValue || numerator >= uint.MaxValue)
                throw new OverflowException("Data is too big to be fill into data structure");
        }

        void OverflowCheckIntegerOnly()
        {
            if (integer >= long.MaxValue)
                throw new OverflowException("Data is too big to be fill into data structure");
        }
    }
}
