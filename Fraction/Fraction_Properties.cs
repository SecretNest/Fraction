using System;
using System.Text;

namespace SecretNest
{
    public partial struct Fraction
    {

        public bool Sign { get { return sign > 0; } }

        public long Integer 
        {
            get { return sign * (long)integer; }
            set
            {
                if (value >= 0)
                {
                    sign = 1;
                    integer = (ulong)value;
                }
                else
                {
                    sign = -1;
                    integer = (ulong)(-value);
                }
            } 
        }

        public uint Numerator
        { 
            get { return numeratorLow; }
            set { numeratorLow = value; numeratorHigh = 0; Simplify(); } //Not thread-safe
        } 

        public uint Denominator 
        { 
            get { return denominatorLow; } 
            set //Not thread-safe
            {
                if (value == 0)
                    throw new ArgumentOutOfRangeException("Denominator", "The Denominator cannot be zero.");
                denominatorLow = value; denominatorHigh = 0;
                Simplify();
            }
        }

        public bool IsZero { get { return integer == 0 && numerator == 0; } }
    }
}
