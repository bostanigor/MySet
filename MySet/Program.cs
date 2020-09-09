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

        public bool Contains(int value)
        {
            for (var i = 1; i < _lefts.Length; i++)
                if (value >= _lefts[i] && value <= _rights[i])
                    return true;
            return false;
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
            var set = new MySet("1..10,22,30..31");
            foreach (var x in set)
            {
                Console.Write(x);
                Console.Write(" ");
            }
            Console.WriteLine("Hello World!");
        }
    }
}