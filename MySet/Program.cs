using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;

namespace MySet
{
    class MySet : IEnumerable
    {
        private int[] _lefts;
        private int[] _rights;        
        public MySet(string desc)
        {
            var lefts = new List<int>{int.MinValue};
            var rights = new List<int>{int.MinValue};
            
            var pairs = desc.Split(',');
            foreach (var pairStr in pairs)
            {
                // TODO: Change Parse to TryParse or error handling
                var borders = pairStr.Split("..");
                var left = Int32.Parse(borders[0]);
                var right = Int32.Parse(borders.Length == 2 ? borders[1] : borders[0]);
                
                if (left > right)
                    throw new ArgumentException("Invalid range limits");

                lefts.Add(left);
                rights.Add(right);
            }

            _lefts = lefts.ToArray();
            _rights = rights.ToArray();
        }

        public MySet(int[] lefts, int[] rights)
        {
            // TODO: Add optimization like merging close values in one interval (ex. 1, 2, 3 to 1..3)  
            _lefts = lefts;
            _rights = rights;
        }

        public bool Contains(int value)
        {
            for (var i = 1; i < _lefts.Length; i++)
                if (value >= _lefts[i] && value <= _rights[i])
                    return true;
            return false;
        }

        public MySet Merge(MySet other)
        {
            var newLefts = new List<int>();
            var newRights = new List<int>();
            int i_1 = 1;
            int i_2 = 1;
            int lastLeft = int.MinValue;
            int lastRight = int.MinValue;
            MySet lastObject;
            while (i_1 < this._lefts.Length && i_2 < other._lefts.Length)
            {
                var left_1 = this._lefts[i_1];
                var left_2 = other._lefts[i_2];
                var right_1 = this._rights[i_1];
                var right_2 = other._rights[i_2];
                
                if (left_1 <= lastRight)
                {
                    if (right_1 > lastRight)
                        lastRight = right_1;
                    i_1++;
                }
                else if (left_2 <= lastRight)
                {
                    if (right_2 > lastRight)
                        lastRight = right_2;
                    i_2++;
                }
                else
                {
                    newLefts.Add(lastLeft);
                    newRights.Add(lastRight);
                    if (left_1 < left_2)
                    {
                        lastLeft = left_1;
                        lastRight = right_1;
                        i_1++;
                    }
                    else
                    {
                        lastLeft = left_2;
                        lastRight = right_2;
                        i_2++;
                    }
                }
            }
            newLefts.Add(lastLeft);
            newRights.Add(lastRight);
            var unfinishedSet = i_1 == this._lefts.Length ? other : this;
            var i = i_1 == this._lefts.Length ? i_2 : i_1;
            for (; i < unfinishedSet._lefts.Length; i++)
            {
                newLefts.Add(unfinishedSet._lefts[i]);
                newRights.Add(unfinishedSet._rights[i]);
            }
            return new MySet(newLefts.ToArray(), newRights.ToArray());
        }

        public MySet Substract(MySet set)
        {
            var newLefts = new List<int>();
            var newRights = new List<int>();
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }

        public MySetEnum GetEnumerator()
        {
            return new MySetEnum(_lefts, _rights);
        }
    }

    class MySetEnum : IEnumerator
    {
        private int[] _lefts;
        private int[] _rights;
        private int _curIntervalPos;
        private int _curVal;
        public MySetEnum(int[] lefts, int[] rights)
        {
            _lefts = lefts;
            _rights = rights;
            _curIntervalPos = 0;
            _curVal = _lefts[0];
        }

        public bool MoveNext()
        {
            if (_curVal == _rights[_curIntervalPos])
            {
                _curIntervalPos++;
                if (_curIntervalPos == _rights.Length)
                    return false;
                _curVal = _lefts[_curIntervalPos];
            }
            else
                _curVal++;
            return true;
        }

        public void Reset()
        {
            _curIntervalPos = 0;
            _curVal = _lefts[0];
        }

        object IEnumerator.Current => Current;

        public int Current => _curVal;
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            // var set1 = new MySet("1..10,22,28,30..31");
            // var set2 = new MySet("5..8,22,23,24,32..35");
            
            var set1 = new MySet("1..10,22,28,30..31");
            var set2 = new MySet("5..15,22,23,24,32..35");
            
            // var set1 = new MySet("1..22,23");
            // var set2 = new MySet("5..8,22,23,32..35");
            
            // var set1 = new MySet("5,6,7,22");
            // var set2 = new MySet("1..22,23");
            var set3 = set1.Merge(set2);
            foreach (var x in set3)
            {
                Console.Write(x);
                Console.Write(" ");
            }
            Console.WriteLine("Hello World!");
        }
    }
}