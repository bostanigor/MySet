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

        public MySet Union(MySet other)
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

        public MySet Substraction(MySet other)
        {
            var newLefts = new List<int>() { int.MinValue };
            var newRights = new List<int>() { int.MinValue };
            var i_1 = 1;
            var i_2 = 1;
            var lastLeft = this._lefts[1];
            var lastRight = this._rights[1];
            while (true)
            {
                if (i_2 == other._lefts.Length)
                {
                    newLefts.Add(lastLeft);
                    newRights.Add(lastRight);
                    for (i_1++; i_1 < this._lefts.Length; i_1++)
                    {
                        newLefts.Add(this._lefts[i_1]);
                        newRights.Add(this._rights[i_1]);
                    }
                    break;
                }
                
                var left_1 = this._lefts[i_1];
                var left_2 = other._lefts[i_2];
                var right_1 = this._rights[i_1];
                var right_2 = other._rights[i_2];

                if (lastLeft > lastRight)
                {
                    i_1++;
                    if (i_1 == this._lefts.Length)
                        break;
                    lastLeft = this._lefts[i_1];
                    lastRight = this._rights[i_1];
                    continue;
                }

                if (left_2 > lastRight)
                {
                    newLefts.Add(lastLeft);
                    newRights.Add(lastRight);
                    i_1++;
                    if (i_1 == this._lefts.Length)
                        break;
                    lastLeft = this._lefts[i_1];
                    lastRight = this._rights[i_1];
                    continue;
                }
                if (right_2 < lastLeft)
                {
                    i_2++;
                    if (i_2 == other._lefts.Length)
                    {
                        newLefts.Add(lastLeft);
                        newRights.Add(lastRight);
                        for (i_1++; i_1 < this._lefts.Length; i_1++)
                        {
                            newLefts.Add(this._lefts[i_1]);
                            newRights.Add(this._rights[i_1]);
                        }
                        break;
                    }
                }
                else
                {
                    if (left_2 <= left_1)
                        lastLeft = right_2 + 1;
                    else
                    {
                        newLefts.Add(lastLeft);
                        newRights.Add(left_2 - 1);
                        if (right_2 < lastRight)
                            i_2++;
                        lastLeft = right_2 + 1;
                    }
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
            // var set1 = new MySet("1..10,22,28,30..31");
            // var set2 = new MySet("5..8,22,23,24,32..35");
            
            var set1 = new MySet("1..10,22,28,30..31");
            var set2 = new MySet("5..15,22,23,24,32..35");
            
            // var set1 = new MySet("1..22,23");
            // var set2 = new MySet("5..8,22,23,32..35");
            
            // var set1 = new MySet("5,6,7,22");
            // var set2 = new MySet("1..22,23");
            
            var set3 = new MySet("5..10,15..25,26,27,28,30..33");
            var set4 = new MySet("1,2,6..7,9..13,15..17,26,28,31");
            // 5 8 18-25 27 30 32 33
            
            foreach (var x in set1.Union(set2))
                Console.Write($"{x} ");
            Console.WriteLine();
            foreach (var x in set3.Substraction(set4))
                Console.Write($"{x} ");

            Console.WriteLine("Hello World!");
        }
    }
}