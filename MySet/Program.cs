using System;
using System.Collections.Generic;

namespace MySet
{
    class Program
    {
        static void Main(string[] args)
        {
            var set1 = new MySet(HelperMethods.MyRangesFromString("1..10,22,28,30..31"));
            var set2 = new MySet(HelperMethods.MyRangesFromString("5..8,22,23,24,32..35"));
            
            var set3 = new MySet(HelperMethods.MyRangesFromString("5..10,15..25,26,27,28,30..33"));
            var set4 = new MySet(HelperMethods.MyRangesFromString("1,2,6..7,9..13,15..17,26,28,31"));
            
            var set5 = new MySet(HelperMethods.MyRangesFromString("5..10,15..25,26,27,28,30..33"));
            var set6 = new MySet(HelperMethods.MyRangesFromString("1,2,8..17,26,28,31"));

            foreach (var x in set1 + set2)
                Console.Write($"{x} ");
            Console.WriteLine();
            
            foreach (var x in set3 - set4)
                Console.Write($"{x} ");
            Console.WriteLine();            
            
            foreach (var x in set5 * set6)
                Console.Write($"{x} ");
            
        }
    }
}