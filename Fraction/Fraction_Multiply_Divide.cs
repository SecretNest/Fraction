using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SecretNest
{
    public partial struct Fraction
    {
        public static Fraction operator *(Fraction value1, Fraction value2)
        {
            if (value1.IsZero || value2.IsZero)
                return value1;
            if (value2.sign == -1)
                value1.sign = -value1.sign;
            try
            {
                //Fast mode
                //(a+b/c)(d+e/f) = (ac+b)*(df+e)/cf
                ulong numerator = checked((value1.integer * value1.denominator + value1.numerator) * (value2.integer * value2.denominator + value2.numerator)); //overflow possibility
                value1.numerator = numerator;
                value1.denominator *= value2.denominator;
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
                    //(a+b/c)(d+e/f) = ad+ae/f+bd/c+be/cf = ad+(aec+bdf+be)/cf
                    ulong numerator = checked(value1.integer * value2.numerator * value1.denominator + value1.numerator * value2.integer * value2.denominator + value1.numerator * value2.numerator); //overflow possibility
                    value1.integer *= value2.integer; //ad
                    value1.denominator *= value2.denominator; //cf
                    value1.numerator = numerator;
                    value1.Simplify();
                    value1.OverflowCheck();
                    return value1;
                }
                catch (OverflowException)
                {
#endif
                    //Safe mode by using BigInteger
                    //(a+b/c)(d+e/f) = (ac+b)*(df+e)/cf
                    BigInteger bigA = new BigInteger(value1.integer);
                    BigInteger bigB = new BigInteger(value1.numerator);
                    BigInteger bigC = new BigInteger(value1.denominator);
                    BigInteger bigD = new BigInteger(value2.integer);
                    BigInteger bigE = new BigInteger(value2.numerator);
                    BigInteger bigF = new BigInteger(value2.denominator);
                    BigInteger numerator = (bigA * bigC + bigB) * (bigD * bigE + bigF);
                    BigInteger denominator = bigC * bigF;
                    BigInteger greatestCommonDivisor = GreatestCommonDivisor(numerator, denominator);
                    try
                    {
                        value1.integer = (ulong)(long)(numerator / denominator);
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

        public static Fraction operator *(int value1, Fraction value2)
        {
            return (long)value1 * value2;
        }

        public static Fraction operator *(long value1, Fraction value2)
        {
            if (value1 == 0)
                return 0;
            if (value2.IsZero)
                return value2;
            if (value1 > 0)
            {
                value2.integer *= (ulong)value1;
                value2.numerator *= (ulong)value1;
            }
            else
            {
                value2.integer *= (ulong)(-value1);
                value2.numerator *= (ulong)(-value1);
                value2.sign = -value2.sign;
            }
            value2.Simplify();
            value2.OverflowCheck();
            return value2;
        }

        public static Fraction operator *(Fraction value1, int value2)
        {
            return (long)value2 * value1;
        }

        public static Fraction operator *(Fraction value1, long value2)
        {
            return value2 * value1;
        }

        public static Fraction operator /(Fraction value1, Fraction value2)
        {
            if (value1.IsZero)
                return value1;
            if (value2.IsZero)
                throw new DivideByZeroException("The value2 cannot be zero.");
            if (value2.sign == -1)
                value1.sign = -value1.sign;
            //(a+b/c)/(d+e/f) = (ac+b)f/(df+e)c
            try
            {
                //Fast mode
                ulong numerator = checked((value1.integer * value1.denominator + value1.numerator) * value2.denominator);
                //overflow possibility
                ulong denominator = checked((value2.integer * value2.denominator + value2.numerator) * value1.denominator);
                //overflow possibility
                value1.numerator = numerator;
                value1.denominator = denominator;
                value1.Simplify();
                value1.OverflowCheck();
                return value1;
            }
            catch (OverflowException)
            {
                //Safe mode by using BigInteger
                BigInteger bigNumerator = new BigInteger(value1.integer);
                bigNumerator *= value1.denominator;
                bigNumerator += value1.numerator;
                bigNumerator *= value2.denominator;
                BigInteger bigDenominator = new BigInteger(value2.integer);
                bigDenominator *= value2.denominator;
                bigDenominator += value2.numerator;
                bigDenominator *= value1.denominator;
                BigInteger greatestCommonDivisor = GreatestCommonDivisor(bigNumerator, bigDenominator);
                try
                {
                    value1.integer = (ulong)(long)(bigNumerator / bigDenominator);
                    bigNumerator = bigNumerator % bigDenominator;
                    if (greatestCommonDivisor != 1)
                    {
                        bigNumerator /= greatestCommonDivisor;
                        bigDenominator /= greatestCommonDivisor;
                    }
                    value1.numeratorHigh = 0;
                    value1.denominatorHigh = 0;
                    value1.numeratorLow = (uint)bigNumerator;
                    value1.denominatorLow = (uint)bigDenominator;
                    return value1;
                }
                catch (OverflowException)
                {
                    throw new OverflowException("Data is too big to be fill into data structure");
                }
            }
        }

        public static Fraction operator /(int value1, Fraction value2)
        {
            return (long)value1 / value2;
        }

        public static Fraction operator /(long value1, Fraction value2)
        {
            if (value1 == 0)
                return 0;
            if (value2.IsZero)
                throw new DivideByZeroException("The value2 cannot be zero.");
            // x / (a+b/c) = xc / ac+b
            ulong newDenominator = value2.integer * value2.denominator + value2.numerator;
            try
            {
                //Fast mode
                if (value1 > 0) //Zero is filtered
                {
                    value2.numerator = checked((ulong)value1 * value2.denominator); //overflow possibility
                }
                else
                {
                    value2.numerator = checked((ulong)(-value1) * value2.denominator); //overflow possibility
                    value2.sign = -value2.sign;
                }
                value2.denominator = newDenominator;
                value2.integer = 0;
                value2.Simplify();
                value2.OverflowCheck();
                return value2;
            }
            catch (OverflowException)
            {
                //Safe mode by using BigInteger
                BigInteger bigNumerator = new BigInteger(value2.denominator);
                if (value1 > 0) //Zero is filtered
                {
                    bigNumerator *= value1;
                }
                else
                {
                    bigNumerator *= -value1;
                    value2.sign = -value2.sign;
                }
                BigInteger bigDenominator = new BigInteger(newDenominator);
                BigInteger greatestCommonDivisor = GreatestCommonDivisor(bigNumerator, bigDenominator);
                try
                {
                    value2.integer = (ulong)(long)(bigNumerator / bigDenominator);
                    bigNumerator = bigNumerator % bigDenominator;
                    if (greatestCommonDivisor != 1)
                    {
                        bigNumerator /= greatestCommonDivisor;
                        bigDenominator /= greatestCommonDivisor;
                    }
                    value2.numeratorHigh = 0;
                    value2.denominatorHigh = 0;
                    value2.numeratorLow = (uint)bigNumerator;
                    value2.denominatorLow = (uint)bigDenominator;
                    return value2;
                }
                catch (OverflowException)
                {
                    throw new OverflowException("Data is too big to be fill into data structure");
                }
            }
        }

        public static Fraction operator /(Fraction value1, int value2)
        {
            return value1 / (long)value2;
        }

        public static Fraction operator /(Fraction value1, long value2)
        {
            if (value1.IsZero)
                return value1;
            if (value2 == 0)
                throw new DivideByZeroException("The value2 cannot be zero.");
            value1.numerator += value1.integer * value1.denominator;
            value1.integer = 0;
            try
            {
                //Fast mode
                if (value2 > 0) //Zero is filtered
                {
                    value1.denominator = checked((ulong)value2 * value1.denominator); //overflow possibility
                }
                else
                {
                    value1.denominator = checked((ulong)(-value2) * value1.denominator); //overflow possibility
                    value1.sign = -value1.sign;
                }
                value1.Simplify();
                value1.OverflowCheck();
                return value1;
            }
            catch (OverflowException)
            {
                //Safe mode by using BigInteger
                BigInteger bigDenominator = new BigInteger(value1.denominator);
                if (value2 > 0) //Zero is filtered
                {
                    bigDenominator *= value2;
                }
                else
                {
                    bigDenominator *= -value2;
                    value1.sign = -value1.sign;
                }
                BigInteger bigNumerator = new BigInteger(value1.numerator);
                BigInteger greatestCommonDivisor = GreatestCommonDivisor(bigNumerator, bigDenominator);
                try
                {
                    value1.integer = (ulong)(long)(bigNumerator / bigDenominator);
                    bigNumerator = bigNumerator % bigDenominator;
                    if (greatestCommonDivisor != 1)
                    {
                        bigNumerator /= greatestCommonDivisor;
                        bigDenominator /= greatestCommonDivisor;
                    }
                    value1.numeratorHigh = 0;
                    value1.denominatorHigh = 0;
                    value1.numeratorLow = (uint)bigNumerator;
                    value1.denominatorLow = (uint)bigDenominator;
                    return value1;
                }
                catch (OverflowException)
                {
                    throw new OverflowException("Data is too big to be fill into data structure");
                }
            }
        }
    }
}
