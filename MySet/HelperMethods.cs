using System;
using System.Collections.Generic;

namespace MySet
{
    public static class HelperMethods
    {
        public static MyRange[] MyRangesFromString(string desc)
        {
            var newRanges = new List<MyRange>();
            foreach (var range in desc.Split(','))
            {
                var temp = range.Split("..");
                if (temp.Length == 1)
                {
                    var val = Int32.Parse(temp[0]);
                    newRanges.Add(new MyRange(val, val));
                }
                else
                    newRanges.Add(new MyRange(
                        Int32.Parse(temp[0]), Int32.Parse(temp[1]))
                    );
            }

            return newRanges.ToArray();
        }
    }
}