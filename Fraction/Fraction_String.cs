using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest
{
    public partial struct Fraction
    {
        public override string ToString()
        {
            if (integer == 0 && numerator == 0) return "0";
            StringBuilder builder = new StringBuilder();
            if (sign < 0) builder.Append("-");
            if (integer != 0)
            {
                builder.Append(integer);
                if (numerator != 0)
                {
                    builder.Append(" ");
                    builder.Append(numerator);
                    builder.Append("/");
                    builder.Append(denominator);
                }
            }
            else
            {
                builder.Append(numerator);
                builder.Append("/");
                builder.Append(denominator);
            }
            return builder.ToString();
        }

        public Fraction(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException(text, "The text cannot be null, empty, or white space.");
            text = text.Trim();
            if (text.StartsWith("-"))
            {
                text = text.Substring(1);
                sign = -1;
            }
            else
            {
                sign = 1;
            }
            if (ulong.TryParse(text, out integer))  // x
            {
                numeratorHigh = 0;
                numeratorLow = 0;
                numerator = 0;
                denominatorHigh = 0;
                denominatorLow = 1;
                denominator = 1;
            }
            else  // x y/z   or   y/z
            {
                numeratorHigh = 0;
                numeratorLow = 0;
                denominatorHigh = 0;
                denominatorLow = 0;
                var texts = text.Split(' ');
                if (texts.Length <= 2) //  x y/z   or   y/z
                {
                    if (texts.Length == 2)  // x y/z
                    {
                        if (!ulong.TryParse(texts[0], out integer))
                            throw new ArgumentException(text, "The text format cannot be recognized.");
                        text = texts[1];
                    }
                    else
                    {
                        integer = 0;
                    }
                    texts = text.Split('/');  //  y/z
                    if (texts.Length != 2)
                        throw new ArgumentException(text, "The text format cannot be recognized.");
                    if (!ulong.TryParse(texts[0], out numerator))
                        throw new ArgumentException(text, "The text format cannot be recognized.");
                    if (!ulong.TryParse(texts[1], out denominator))
                        throw new ArgumentException(text, "The text format cannot be recognized.");
                }
                else
                    throw new ArgumentException(text, "The text format cannot be recognized.");
            }
            Simplify();
            if (IsZero) sign = 1;
        }
    }
}
