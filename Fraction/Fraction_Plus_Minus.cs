using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SecretNest
{
    public partial struct Fraction
    {
        static Fraction OperatorPlusWithSameSign(Fraction value1, Fraction value2)  //sign of value2 is ignored
        {
            if (value1.numerator == 0)
            {
                value2.sign = value1.sign;
                return OperatorPlusWithSameSign(value1.sign * (long)value1.integer, value2);
            }
            if (value2.numerator == 0)
                return OperatorPlusWithSameSign(value1.sign * (long)value2.integer, value1);
            value1.integer += value2.integer;
            try
            {
                //Fast mode
                ulong numerator = checked(value1.numerator * value2.denominator + value2.numerator * value1.denominator); //overflow possibility
                value1.denominator = value1.denominator * value2.denominator;
                value1.numerator = numerator;
                value1.Simplify();
                value1.OverflowCheck();
                return value1;
            }
            catch (OverflowException)
            {
#if EnableMoreOptimizing
                try
                {
                    //Mid mode
                    var greatestCommonDivisor = GreatestCommonDivisor(value1.denominator, value2.denominator);
                    var multiplier1 = value1.denominator / greatestCommonDivisor;
                    var multiplier2 = value2.denominator / greatestCommonDivisor;
                    ulong numerator = checked(value1.numerator * multiplier2 + value2.numerator * multiplier1); //overflow possibility
                    value1.denominator = greatestCommonDivisor * multiplier1 * multiplier2;
                    value1.numerator = numerator;
                    value1.Simplify();
                    value1.OverflowCheck();
                    return value1;
                }
                catch (OverflowException)
                {
#endif
                    //Safe mode by using BigInteger
                    BigInteger numerator =
                        new BigInteger(value1.numerator) * new BigInteger(value2.denominator) +
                        new BigInteger(value2.numerator) * new BigInteger(value1.denominator);
                    BigInteger denominator = value1.denominator * value2.denominator;
                    BigInteger greatestCommonDivisor = GreatestCommonDivisor(numerator, denominator);
                    try
                    {
                        value1.integer += (ulong)(long)(numerator / denominator);
                        numerator = numerator % denominator;
                        if (greatestCommonDivisor != 1)
                        {
                            numerator /= greatestCommonDivisor;
                            denominator /= greatestCommonDivisor;
                        }
                        value1.numeratorHigh = 0;
                        value1.denominatorHigh = 0;
                        value1.numeratorLow = (uint)numerator;
                        value1.denominatorLow = (uint)denominator;
                        return value1;
                    }
                    catch (OverflowException)
                    {
                        throw new OverflowException("Data is too big to be fill into data structure");
#if EnableMoreOptimizing
                    }
#endif
                }
            }
        }

        static Fraction OperatorPlusWithSameSign(long value1, Fraction value2)  //sign of value1 is ignored
        {
            value2.integer += (ulong)Math.Abs(value1);
            value2.OverflowCheckIntegerOnly();
            return value2;
        }

        static Fraction OperatorMinusWithPositive(Fraction value1, Fraction value2)  //signs of value1 and value2 are ignored
        {
            if (value1.numerator == 0)
            {
                value2.sign = 1;
                return OperatorMinusWithSameSign(value1.sign * (long)value1.integer, value2);
            }
            if (value2.numerator == 0)
                return OperatorMinusWithSameSign(value1, value1.sign * (long)value2.integer);
            long newInteger = (long)(value1.integer) - (long)(value2.integer);
            value1.numerator = value1.numerator * value2.denominator;
            value2.numerator = value2.numerator * value1.denominator;
            value1.denominator *= value2.denominator;
            if (newInteger == 0)
            {
                value1.integer = 0;
                if (value2.numerator > value1.numerator)
                {
                    // 1/5 - 4/5 = -3/5
                    value1.sign = -1;
                    value1.numerator = value2.numerator - value1.numerator;
                }
                else
                {
                    // 4/5 - 1/5 = 3/5
                    value1.sign = 1;
                    value1.numerator -= value2.numerator;
                }
            }
            else if (newInteger > 0)
            {
                value1.sign = 1;
                if (value2.numerator > value1.numerator)
                {
                    // 1 1/5 - 4/5 = 2/5
                    value1.integer = (ulong)newInteger - 1;
                    value1.numerator += value1.denominator - value2.numerator;
                }
                else
                {
                    // 1 4/5 - 1/5 = 1 3/5
                    value1.integer = (ulong)newInteger;
                    value1.numerator -= value2.numerator;
                }
            }
            else //newInteger < 0
            {
                value1.sign = -1;
                if (value2.numerator > value1.numerator)
                {
                    // 1/5 - 1 4/5 = -1 3/5
                    value1.integer = (ulong)(-newInteger);
                    value1.numerator = value2.numerator - value1.numerator;
                }
                else
                {
                    // 4/5 - 1 1/5 = -2/5
                    value1.integer = (ulong)(-newInteger) - 1;
                    value1.numerator = value2.numerator + value1.denominator - value1.numerator;
                }
            }
            value1.Simplify();
            value1.OverflowCheck();
            if (value1.integer == 0 && value1.numerator == 0)
                value1.sign = 1;
            return value1;
        }

        static Fraction OperatorMinusWithSameSign(long value1, Fraction value2)  //sign of value1 is ignored
        {
            ulong value = (ulong)Math.Abs(value1);
            if (value2.numerator == 0)
            {
                if (value > value2.integer)
                    value2.integer = value - value2.integer;
                else if (value == value2.integer)
                {
                    value2.integer = 0;
                    value2.sign = 1;
                }
                else
                {
                    value2.integer = value2.integer - value;
                    value2.sign = -value2.sign;
                }
            }
            else
            {
                if (value > value2.integer)
                {
                    value2.integer = value - value2.integer - 1;
                    value2.numerator = value2.denominator - value2.numerator;
                }
                else
                {
                    value2.integer = value2.integer - value;
                    value2.sign = -value2.sign;
                }
            }
            return value2;
        }

        static Fraction OperatorMinusWithSameSign(Fraction value1, long value2)  //sign of value2 is ignored
        {
            ulong value = (ulong)Math.Abs(value2);
            if (value1.integer > value)
            {
                value1.integer -= value;
            }
            else if (value1.integer == value)
            {
                value1.integer = 0;
                if (value1.numerator == 0) value1.sign = 1;
            }
            else if (value1.numerator == 0)
            {
                value1.integer = value - value1.integer;
                value1.sign = -value1.sign;
            }
            else
            {
                value1.integer = value - value1.integer - 1;
                value1.sign = -value1.sign;
                value1.numerator = value1.denominator - value1.numerator;
            }
            return value1;
        }

        public static Fraction operator +(Fraction value1, Fraction value2)
        {
            if (value1.IsZero) return value2;
            if (value2.IsZero) return value1;
            if (value1.sign == value2.sign)
                return OperatorPlusWithSameSign(value1, value2);
            if (value1.Sign)
                return OperatorMinusWithPositive(value1, value2); //In this function, the signs are ignored.
            return OperatorMinusWithPositive(value2, value1); //In this function, the signs are ignored.
        }

        public static Fraction operator +(int value1, Fraction value2)
        {
            return (long)value1 + value2;
        }

        public static Fraction operator +(long value1, Fraction value2)
        {
            if (value1 == 0) return value2;
            if (value2.IsZero) return value1;
            if ((value1 > 0) == value2.Sign)
                return OperatorPlusWithSameSign(value1, value2);
            if (value1 > 0)
            {
                value2.sign = 1;
                return OperatorMinusWithSameSign(value1, value2);
            }
            return OperatorMinusWithSameSign(value2, value1); //dont need to replace value1 to -value1, due to sign of the 2nd parameter is ignored.
        }

        public static Fraction operator +(Fraction value1, int value2)
        {
            return (long)value2 + value1;
        }

        public static Fraction operator +(Fraction value1, long value2)
        {
            return value2 + value1;
        }

        public static Fraction operator -(Fraction value1, Fraction value2)
        {
            if (value2.IsZero) return value1;
            if (value1.IsZero)
            {
                value2.sign = -value2.sign;
                return value2;
            }
            if (value1.sign != value2.sign)
                return OperatorPlusWithSameSign(value1, value2);  //sign of value2 is ignored
            if (value1.sign == 1) //value2.sign == 1
                return OperatorMinusWithPositive(value1, value2); //In this function, the signs are ignored.
            //value1.sign == -1 value2.sign == -1
            return OperatorMinusWithPositive(value2, value1); //In this function, the signs are ignored.
        }

        public static Fraction operator -(int value1, Fraction value2)
        {
            return (long)value1 - value2;
        }

        public static Fraction operator -(long value1, Fraction value2)
        {
            if (value1 == 0)
            {
                value2.sign = -value2.sign;
                return value2;
            }
            if (value2.IsZero) return value1;
            if ((value1 > 0) == value2.Sign)
                return OperatorMinusWithSameSign(value1, value2);
            value2.sign = -value2.sign;
            return OperatorPlusWithSameSign(value1, value2); //sign of value1 is ignored.
        }

        public static Fraction operator -(Fraction value1, int value2)
        {
            return value1 - (long)value2;
        }

        public static Fraction operator -(Fraction value1, long value2)
        {
            if (value2 == 0) return value1;
            if (value1.IsZero) return -value2;
            if ((value2 > 0) == value1.Sign)
                return OperatorMinusWithSameSign(value1, value2);
            return OperatorPlusWithSameSign(value2, value1); //sign of value2 is ignored.
        }

        public static Fraction operator -(Fraction value1)
        {
            if (value1.IsZero) return value1;
            value1.sign = -value1.sign;
            return value1;
        }
    }
}