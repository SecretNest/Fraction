using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest
{
    public partial struct Fraction
    {
        public static bool operator >(Fraction value1, Fraction value2)
        {
            if (value1.sign != value2.sign)
                return value1.sign > value2.sign;
            if (value1.integer == value2.integer)
                return (value1.numerator * value2.denominator > value2.numerator * value1.denominator) ^ (value1.sign < 0);
            return (value1.integer > value2.integer) ^ (value1.sign < 0);
        }

        public static bool operator >(int value1, Fraction value2)
        {
            return (long)value1 > value2;
        }

        public static bool operator >(long value1, Fraction value2)
        {
            bool sign = value1 >= 0;
            ulong uvalue1;
            if (value2.sign > 0 && sign)
                uvalue1 = (ulong)value1;
            else if (value2.sign < 0 && !sign)
                uvalue1 = (ulong)(-value1);
            else
                return sign;
            if (uvalue1 > value2.integer)
                return sign;
            if (uvalue1 == value2.integer && value2.numerator == 0)
                return false;
            return !sign;
        }

        public static bool operator >(float value1, Fraction value2)
        {
            return (double)value1 > value2;
        }

        public static bool operator >(double value1, Fraction value2)
        {
            bool sign = value1 >= 0;
            ulong uvalue1;
            if (value2.sign > 0 && sign)
                uvalue1 = (ulong)Math.Truncate(value1);
            else if (value2.sign < 0 && !sign)
                uvalue1 = (ulong)(-Math.Truncate(value1));
            else
                return sign;
            if (uvalue1 > value2.integer)
                return sign;
            if (uvalue1 == value2.integer)
                if (sign)
                    return value1 - uvalue1 > (double)value2.numerator / value2.denominator;
                else
                    return -value1 - uvalue1 < (double)value2.numerator / value2.denominator;
            return !sign;
        }

        public static bool operator >(decimal value1, Fraction value2)
        {
            bool sign = value1 >= 0;
            ulong uvalue1;
            if (value2.sign > 0 && sign)
                uvalue1 = (ulong)Math.Truncate(value1);
            else if (value2.sign < 0 && !sign)
                uvalue1 = (ulong)(-Math.Truncate(value1));
            else
                return sign;
            if (uvalue1 > value2.integer)
                return sign;
            if (uvalue1 == value2.integer)
                if (sign)
                    return value1 - uvalue1 > (decimal)value2.numerator / value2.denominator;
                else
                    return -value1 - uvalue1 < (decimal)value2.numerator / value2.denominator;
            return !sign;
        }

        public static bool operator >(Fraction value1, int value2)
        {
            return value1 > (long)value2;
        }

        public static bool operator >(Fraction value1, long value2)
        {
            bool sign = value2 >= 0;
            ulong uvalue2;
            if (value1.sign > 0 && sign)
                uvalue2 = (ulong)value2;
            else if (value1.sign < 0 && !sign)
                uvalue2 = (ulong)(-value2);
            else
                return !sign;
            if (value1.integer < uvalue2)
                return !sign;
            if (value1.integer == uvalue2 && value1.numerator == 0)
                return false;
            return sign;
        }

        public static bool operator >(Fraction value1, float value2)
        {
            return value1 > (double)value2;
        }

        public static bool operator >(Fraction value1, double value2)
        {
            bool sign = value2 >= 0;
            ulong uvalue2;
            if (value1.sign > 0 && sign)
                uvalue2 = (ulong)Math.Truncate(value2);
            else if (value1.sign < 0 && !sign)
                uvalue2 = (ulong)(-Math.Truncate(value2));
            else
                return !sign;
            if (value1.integer < uvalue2)
                return !sign;
            if (uvalue2 == value1.integer)
                if (sign)
                    return (double)value1.numerator / value1.denominator > value2 - uvalue2;
                else
                    return (double)value1.numerator / value1.denominator < -value2 - uvalue2;
            return sign;
        }

        public static bool operator >(Fraction value1, decimal value2)
        {
            bool sign = value2 >= 0;
            ulong uvalue2;
            if (value1.sign > 0 && sign)
                uvalue2 = (ulong)Math.Truncate(value2);
            else if (value1.sign < 0 && !sign)
                uvalue2 = (ulong)(-Math.Truncate(value2));
            else
                return !sign;
            if (value1.integer < uvalue2)
                return !sign;
            if (uvalue2 == value1.integer)
                if (sign)
                    return (decimal)value1.numerator / value1.denominator > value2 - uvalue2;
                else
                    return (decimal)value1.numerator / value1.denominator < -value2 - uvalue2;
            return sign;
        }

        public static bool operator <(Fraction value1, Fraction value2)
        {
            return value2 > value1;
        }

        public static bool operator <(int value1, Fraction value2)
        {
            return value2 > (long)value1;
        }

        public static bool operator <(long value1, Fraction value2)
        {
            return value2 > value1;
        }

        public static bool operator <(float value1, Fraction value2)
        {
            return value2 > (double)value1;
        }

        public static bool operator <(double value1, Fraction value2)
        {
            return value2 > value1;
        }

        public static bool operator <(decimal value1, Fraction value2)
        {
            return value2 > value1;
        }

        public static bool operator <(Fraction value1, int value2)
        {
            return (long)value2 > value1;
        }

        public static bool operator <(Fraction value1, long value2)
        {
            return value2 > value1;
        }

        public static bool operator <(Fraction value1, float value2)
        {
            return (double)value2 > value1;
        }

        public static bool operator <(Fraction value1, double value2)
        {
            return value2 > value1;
        }

        public static bool operator <(Fraction value1, decimal value2)
        {
            return value2 > value1;
        }

        public static bool operator <=(Fraction value1, Fraction value2)
        {
            return !(value1 > value2);
        }

        public static bool operator <=(int value1, Fraction value2)
        {
            return !((long)value1 > value2);
        }

        public static bool operator <=(long value1, Fraction value2)
        {
            return !(value1 > value2);
        }

        public static bool operator <=(float value1, Fraction value2)
        {
            return !((double)value1 > value2);
        }

        public static bool operator <=(double value1, Fraction value2)
        {
            return !(value1 > value2);
        }

        public static bool operator <=(decimal value1, Fraction value2)
        {
            return !(value1 > value2);
        }

        public static bool operator <=(Fraction value1, int value2)
        {
            return !(value1 > (long)value2);
        }

        public static bool operator <=(Fraction value1, long value2)
        {
            return !(value1 > value2);
        }

        public static bool operator <=(Fraction value1, float value2)
        {
            return !(value1 > (double)value2);
        }

        public static bool operator <=(Fraction value1, double value2)
        {
            return !(value1 > value2);
        }

        public static bool operator <=(Fraction value1, decimal value2)
        {
            return !(value1 > value2);
        }

        public static bool operator >=(Fraction value1, Fraction value2)
        {
            return !(value2 > value1);
        }

        public static bool operator >=(int value1, Fraction value2)
        {
            return !(value2 > (long)value1);
        }

        public static bool operator >=(long value1, Fraction value2)
        {
            return !(value2 > value1);
        }

        public static bool operator >=(float value1, Fraction value2)
        {
            return !(value2 > (double)value1);
        }

        public static bool operator >=(double value1, Fraction value2)
        {
            return !(value2 > value1);
        }

        public static bool operator >=(decimal value1, Fraction value2)
        {
            return !(value2 > value1);
        }

        public static bool operator >=(Fraction value1, int value2)
        {
            return !((long)value2 > value1);
        }

        public static bool operator >=(Fraction value1, long value2)
        {
            return !(value2 > value1);
        }

        public static bool operator >=(Fraction value1, float value2)
        {
            return !((double)value2 > value1);
        }

        public static bool operator >=(Fraction value1, double value2)
        {
            return !(value2 > value1);
        }

        public static bool operator >=(Fraction value1, decimal value2)
        {
            return !(value2 > value1);
        }
    }
}
