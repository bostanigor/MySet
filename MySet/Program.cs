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
            var newLefts = new List<int>{int.MinValue};
            var newRights = new List<int>{int.MinValue};
            int i_1 = 1;
            int i_2 = 1;
            MySet lastObject;
            while (true)
            {
                var left_1 = this._lefts[i_1];
                var left_2 = other._lefts[i_2];
                var right_1 = this._rights[i_1];
                var right_2 = other._rights[i_2];
                int leftNew;
                int rightNew;
                if (left_1 < left_2)
                {
                    leftNew = left_1;
                    if (right_1 < left_2)
                        rightNew = right_1;
                    else {
                        rightNew = right_1 <= right_2 ? right_2 : right_1;
                        i_2++;
                    }
                    i_1++;
                }
                else
                {
                    leftNew = left_2;
                    if (right_2 < left_1)
                        rightNew = right_2;
                    else {
                        rightNew = right_2 <= right_1 ? right_1 : right_2;
                        i_1++;
                    }
                    i_2++;
                }
                newLefts.Add(leftNew);
                newRights.Add(rightNew);
                
                if (i_1 == this._lefts.Length)
                {
                    for (; i_2 < other._lefts.Length; i_2++)
                    {
                        newLefts.Add(other._lefts[i_2]);
                        newRights.Add(other._rights[i_2]);
                    }
                    break;
                }
                if (i_2 == other._lefts.Length)
                {
                    for (; i_1 < this._lefts.Length; i_1++)
                    {
                        newLefts.Add(this._lefts[i_1]);
                        newRights.Add(this._rights[i_1]);
                    }
                    break;
                }
            }
            return new MySet(newLefts.ToArray(), newRights.ToArray());
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
            var set1 = new MySet("1..10,22,28,30..31");
            var set2 = new MySet("5..8,22,23,24,32..35");
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