using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest
{
    public partial struct Fraction
    {
        public override bool Equals(object obj)
        {
            if (obj is Fraction)
            {
                return this == (Fraction)obj;
            }
            else
                return false;
        }
        public override int GetHashCode()
        {
            return (int)(((long)integer + (long)(numerator / denominator)) * sign);
        }

        public static bool operator ==(Fraction value1, Fraction value2)
        {
            return value1.sign == value2.sign &&
                   value1.integer == value2.integer &&
                   value1.numerator == value2.numerator &&
                   value1.denominator == value2.denominator;
        }

        public static bool operator ==(int value1, Fraction value2)
        {
            return (long)value1 == value2;
        }

        public static bool operator ==(long value1, Fraction value2)
        {
            if (value1 == 0)
                return value2.IsZero;
            if (value1 > 0)
                return value2.numerator == 0 && value2.sign == 1 && value2.integer == (ulong)value1;
            return value2.numerator == 0 && value2.sign == -1 && value2.integer == (ulong)(-value1);
        }

        public static bool operator ==(float value1, Fraction value2)
        {
            return (double)value1 == value2;
        }

        public static bool operator ==(double value1, Fraction value2)
        {
            if (value1 == 0)
                return value2.IsZero;
            if ((long)Math.Sign(value1) != value2.sign)
                return false;
            else if (value2.sign == -1)
                value1 = -value1;
            var truncated = Math.Truncate(value1);
            return truncated == value2.integer && value1 - truncated == (double)value2.numerator / value2.denominator;
        }

        public static bool operator ==(decimal value1, Fraction value2)
        {
            if (value1 == 0)
                return value2.IsZero;
            if ((long)Math.Sign(value1) != value2.sign)
                return false;
            else if (value2.sign == -1)
                value1 = -value1;
            var truncated = Math.Truncate(value1);
            return truncated == value2.integer && value1 - truncated == (decimal)value2.numerator / value2.denominator;
        }

        public static bool operator ==(Fraction value1, int value2)
        {
            return (long)value2 == value1;
        }

        public static bool operator ==(Fraction value1, long value2)
        {
            return value2 == value1;
        }

        public static bool operator ==(Fraction value1, float value2)
        {
            return (double)value2 == value1;
        }

        public static bool operator ==(Fraction value1, double value2)
        {
            return value2 == value1;
        }

        public static bool operator ==(Fraction value1, decimal value2)
        {
            return value2 == value1;
        }

        public static bool operator !=(Fraction value1, Fraction value2)
        {
            return value1.sign != value2.sign ||
                   value1.integer != value2.integer ||
                   value1.numerator != value2.numerator ||
                   value1.denominator != value2.denominator;
        }

        public static bool operator !=(int value1, Fraction value2)
        {
            return (long)value1 != value2;
        }

        public static bool operator !=(long value1, Fraction value2)
        {
            if (value1 == 0)
                return !value2.IsZero;
            if (value1 > 0)
                return value2.numerator != 0 || value2.sign != 1 || value2.integer != (ulong)value1;
            return value2.numerator != 0 || value2.sign != -1 || value2.integer != (ulong)(-value1);
        }

        public static bool operator !=(float value1, Fraction value2)
        {
            return (double)value1 != value2;
        }

        public static bool operator !=(double value1, Fraction value2)
        {
            if (value1 == 0)
                return !value2.IsZero;
            if ((long)Math.Sign(value1) != value2.sign)
                return true;
            else if (value2.sign == -1)
                value1 = -value1;
            var truncated = Math.Truncate(value1);
            return truncated != value2.integer || value1 - truncated != (double)value2.numerator / value2.denominator;
        }

        public static bool operator !=(decimal value1, Fraction value2)
        {
            if (value1 == 0)
                return !value2.IsZero;
            if ((long)Math.Sign(value1) != value2.sign)
                return true;
            else if (value2.sign == -1)
                value1 = -value1;
            var truncated = Math.Truncate(value1);
            return truncated != value2.integer || value1 - truncated != (decimal)value2.numerator / value2.denominator;
        }

        public static bool operator !=(Fraction value1, int value2)
        {
            return (long)value2 != value1;
        }

        public static bool operator !=(Fraction value1, long value2)
        {
            return value2 != value1;
        }

        public static bool operator !=(Fraction value1, float value2)
        {
            return (double)value2 != value1;
        }

        public static bool operator !=(Fraction value1, double value2)
        {
            return value2 != value1;
        }

        public static bool operator !=(Fraction value1, decimal value2)
        {
            return value2 != value1;
        }
    }
}
