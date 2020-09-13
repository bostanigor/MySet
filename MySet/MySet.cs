using System;
using System.Collections;
using System.Collections.Generic;

namespace MySet
{
    public class MySet : IEnumerable
    {
        private readonly int[] _lefts;
        private readonly int[] _rights;        
        public MySet(string desc)
        {
            var lefts = new List<int>{int.MinValue};
            var rights = new List<int>{int.MinValue};
            
            var pairs = desc.Split(',');
            foreach (var pairStr in pairs)
            {
                // TODO: Change Parse to TryParse or error handling
                var borders = pairStr.Split("..");
                var left = int.Parse(borders[0]);
                var right = int.Parse(borders.Length == 2 ? borders[1] : borders[0]);
                
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
            int i1 = 1, i2 = 1;
            var lastLeft = int.MinValue;
            var lastRight = int.MinValue;
            MySet lastObject;
            while (i1 < this._lefts.Length && i2 < other._lefts.Length)
            {
                var left1 = this._lefts[i1];
                var left2 = other._lefts[i2];
                var right1 = this._rights[i1];
                var right2 = other._rights[i2];
                
                if (left1 <= lastRight)
                {
                    if (right1 > lastRight)
                        lastRight = right1;
                    i1++;
                }
                else if (left2 <= lastRight)
                {
                    if (right2 > lastRight)
                        lastRight = right2;
                    i2++;
                }
                else
                {
                    newLefts.Add(lastLeft);
                    newRights.Add(lastRight);
                    if (left1 < left2)
                    {
                        lastLeft = left1;
                        lastRight = right1;
                        i1++;
                    }
                    else
                    {
                        lastLeft = left2;
                        lastRight = right2;
                        i2++;
                    }
                }
            }
            newLefts.Add(lastLeft);
            newRights.Add(lastRight);
            var unfinishedSet = i1 == this._lefts.Length ? other : this;
            var i = i1 == this._lefts.Length ? i2 : i1;
            for (; i < unfinishedSet._lefts.Length; i++)
            {
                newLefts.Add(unfinishedSet._lefts[i]);
                newRights.Add(unfinishedSet._rights[i]);
            }
            return new MySet(newLefts.ToArray(), newRights.ToArray());
        }

        public MySet Difference(MySet other)
        {
            var newLefts = new List<int>() { int.MinValue };
            var newRights = new List<int>() { int.MinValue };
            int i1 = 1, i2 = 1;
            var lastLeft = this._lefts[1];
            var lastRight = this._rights[1];
            while (true)
            {
                if (i2 == other._lefts.Length)
                {
                    newLefts.Add(lastLeft);
                    newRights.Add(lastRight);
                    for (i1++; i1 < this._lefts.Length; i1++)
                    {
                        newLefts.Add(this._lefts[i1]);
                        newRights.Add(this._rights[i1]);
                    }
                    break;
                }
                
                var left1 = this._lefts[i1];
                var left2 = other._lefts[i2];
                var right1 = this._rights[i1];
                var right2 = other._rights[i2];

                if (lastLeft > lastRight)
                {
                    i1++;
                    if (i1 == this._lefts.Length)
                        break;
                    lastLeft = this._lefts[i1];
                    lastRight = this._rights[i1];
                    continue;
                }

                if (left2 > lastRight)
                {
                    newLefts.Add(lastLeft);
                    newRights.Add(lastRight);
                    i1++;
                    if (i1 == this._lefts.Length)
                        break;
                    lastLeft = this._lefts[i1];
                    lastRight = this._rights[i1];
                    continue;
                }
                if (right2 < lastLeft)
                    i2++;
                else
                {
                    if (left2 <= left1)
                        lastLeft = right2 + 1;
                    else {
                        newLefts.Add(lastLeft);
                        newRights.Add(left2 - 1);
                        if (right2 < lastRight)
                            i2++;
                        lastLeft = right2 + 1;
                    }
                }
            }
            return new MySet(newLefts.ToArray(), newRights.ToArray());
        }
        
        public MySet Intersection(MySet other) {
            var newLefts = new List<int>() { int.MinValue };
            var newRights = new List<int>() { int.MinValue };
            int i1 = 1, i2 = 1;
            while (i1 < this._lefts.Length && i2 < other._lefts.Length)
            {
                var left1 = this._lefts[i1];
                var left2 = other._lefts[i2];
                var right1 = this._rights[i1];
                var right2 = other._rights[i2];

                if (right2 < left1)
                {
                    i2++;
                    continue;
                }
                if (right1 < left2)
                {
                    i1++;
                    continue;
                }
                
                newLefts.Add(left2 >= left1 ? left2 : left1);
                if (right2 <= right1)
                {
                    newRights.Add(right2);
                    i2++;
                }
                else
                {
                    newRights.Add(right1);
                    i1++;
                }
            }
            return new MySet(newLefts.ToArray(), newRights.ToArray());
        }

        public static MySet operator +(MySet left, MySet right)
            => left.Union(right);
        
        public static MySet operator -(MySet left, MySet right)
            => left.Difference(right);
    
        public static MySet operator *(MySet left, MySet right)
            => left.Intersection(right);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }

        public MySetEnum GetEnumerator()
        {
            return new MySetEnum(_lefts, _rights);
        }
    }
    
    
}