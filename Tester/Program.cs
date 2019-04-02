using SecretNest;
using System;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //Fraction zero = 0;
            //Fraction one = 1;
            //Fraction minusOne = -1;
            //Fraction oneFifth = new Fraction(1, 5);
            //Fraction twoThirds = new Fraction(2, 3);
            //Fraction fourFifths = new Fraction(4, 5);
            //Fraction minusOneFifth = new Fraction(-1, 5);
            //Fraction minusFourFifths = new Fraction(-4, 5);
            //Fraction oneAndOneFifth = new Fraction(6, 5);
            //Fraction oneAndFourFifths = new Fraction(9, 5);
            //Fraction minusOneAndOneFifth = new Fraction(-6, 5);
            //Fraction minusOneAndFourFifths = new Fraction(-9, 5);

            //var x = fourFifths != 0.8;
            //var y = minusOneAndFourFifths != -1.8;

            var result = new Fraction("2") * new Fraction("1/2");
            Console.Write(result);

            result = new Fraction("2") / new Fraction("1/2");
            Console.Write(result);

            Console.Read();
        }
    }
}
