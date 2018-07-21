using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest
{
    public partial struct Fraction
    {
        public static explicit operator long(Fraction value)
        {
            return value.sign * (long)(value.integer);
        }

        public static explicit operator int(Fraction value)
        {
            return (int)value.sign * (int)(value.integer);
        }

        public static explicit operator float(Fraction value)
        {
            return value.sign * ((float)value.integer + (float)value.numerator / (float)value.denominator);
        }

        public static explicit operator double(Fraction value)
        {
            return value.sign * ((double)value.integer + (double)value.numerator / (double)value.denominator);
        }

        public static explicit operator decimal(Fraction value)
        {
            return value.sign * ((decimal)value.integer + (decimal)value.numerator / (decimal)value.denominator);
        }

        public static implicit operator Fraction(long value)
        {
            ulong integer;
            long sign;
            if (value >= 0)
            {
                integer = (ulong)value;
                sign = 1;
            }
            else
            {
                integer = (ulong)(-value);
                sign = -1;
            }

            return new Fraction()
            {
                sign = sign,
                integer = integer,
                numeratorHigh = 0,
                numeratorLow = 0,
                numerator = 0,
                denominatorHigh = 0,
                denominatorLow = 1,
                denominator = 1
            };
        }

        public static implicit operator Fraction(int value)
        {
            ulong integer;
            long sign;
            if (value >= 0)
            {
                integer = (ulong)value;
                sign = 1;
            }
            else
            {
                integer = (ulong)(-value);
                sign = -1;
            }

            return new Fraction()
            {
                sign = sign,
                integer = integer,
                numeratorHigh = 0,
                numeratorLow = 0,
                numerator = 0,
                denominatorHigh = 0,
                denominatorLow = 1,
                denominator = 1
            };
        }
    }
}
