using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest
{
    public partial struct Fraction
    {
/*Known Issue:
 * 
 * The object built with default parameterless constructor cannot be used correctly.
 * The detection for this error is NOT deployed based on performance consideration.
 * 
*/

        public Fraction(uint numerator, uint denominator)
        {
            if (denominator == 0)
                throw new ArgumentOutOfRangeException("denominator", "The denominator cannot be zero.");
            this.sign = 1;
            this.integer = 0; this.numerator = 0; this.denominator = 0; numeratorHigh = 0; denominatorHigh = 0; //Useless. Kept for fulfilling C# standard.
            numeratorLow = numerator;
            denominatorLow = denominator;
            Simplify();
        }

        public Fraction(bool positive, uint numerator, uint denominator)
        {
            if (denominator == 0)
                throw new ArgumentOutOfRangeException("denominator", "The denominator cannot be zero.");
            if (positive || numerator == 0)
                this.sign = 1;
            else
                this.sign = -1;
            this.integer = 0; this.numerator = 0; this.denominator = 0; numeratorHigh = 0; denominatorHigh = 0; //Useless. Kept for fulfilling C# standard.
            numeratorLow = numerator;
            denominatorLow = denominator;
            Simplify();
        }

        public Fraction(long integer, uint numerator, uint denominator)
        {
            if (denominator == 0)
                throw new ArgumentOutOfRangeException("denominator", "The denominator cannot be zero.");

            if (integer >= 0)
            {
                sign = 1;
                this.integer = (ulong)integer;
            }
            else
            {
                sign = -1;
                this.integer = (ulong)(-integer);
            }
            this.numerator = 0; this.denominator = 0; numeratorHigh = 0; denominatorHigh = 0; //Useless. Kept for fulfilling C# standard.
            numeratorLow = numerator;
            denominatorLow = denominator;
            Simplify();
        }

        public Fraction(ulong integer, uint numerator, uint denominator)
        {
            if (denominator == 0)
                throw new ArgumentOutOfRangeException("denominator", "The denominator cannot be zero.");

            sign = 1;
            this.integer = integer;
            this.numerator = 0; this.denominator = 0; numeratorHigh = 0; denominatorHigh = 0; //Useless. Kept for fulfilling C# standard.
            numeratorLow = numerator;
            denominatorLow = denominator;
            Simplify();
        }

        public Fraction(bool positive, ulong integer, uint numerator, uint denominator)
        {
            if (denominator == 0)
                throw new ArgumentOutOfRangeException("denominator", "The denominator cannot be zero.");

            if (positive || (integer == 0 && numerator == 0))
            {
                sign = 1;
                this.integer = integer;
            }
            else
            {
                sign = -1;
                this.integer = integer;
            }
            this.numerator = 0; this.denominator = 0; numeratorHigh = 0; denominatorHigh = 0; //Useless. Kept for fulfilling C# standard.
            numeratorLow = numerator;
            denominatorLow = denominator;
            Simplify();
        }

        public Fraction(long dividend, long divisor)
        {
            if (divisor == 0)
                throw new ArgumentOutOfRangeException("divisor", "The divisor cannot be zero.");
            if (dividend == 0)
            {
                sign = 1;
                integer = 0;
                numeratorHigh = 0;
                numeratorLow = 0;
                numerator = 0;
                denominatorHigh = 0;
                denominatorLow = 1;
                denominator = 1;
            }
            else
            {
                numeratorHigh = 0; denominatorHigh = 0; numeratorLow = 0; denominatorLow = 0; integer = 0;//Useless. Kept for fulfilling C# standard.
                if (dividend > 0) //Zero is filtered
                {
                    sign = 1;
                    numerator = (ulong)dividend;
                }
                else
                {
                    sign = -1;
                    numerator = (ulong)(-dividend);
                }
                if (divisor > 0) //Zero is filtered
                {
                    denominator = (ulong)divisor;
                }
                else
                {
                    sign = -sign;
                    denominator = (ulong)(-divisor);
                }
                Simplify();
                OverflowCheck();
            }
        }

    }
}
