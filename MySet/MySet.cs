using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MySet
{
    public class MySet : IEnumerable<int>
    {
        private readonly IntervalTree _tree;

        public int Count
        {
            get { return _tree.Count; }
        }
        public MySet(params MyRange[] ranges)
        {
            _tree = new IntervalTree(ranges);
        }

        private MySet(IntervalTree tree)
        {
            this._tree = tree;
        }

        public bool Contains(int value)
        {
            return _tree.Contains(value);
        }

        public MySet Union(MySet other)
        {
            return new MySet(_tree.Union(other._tree));
        }

        public MySet Difference(MySet other)
        {
            return new MySet(_tree.Difference(other._tree));
        }
        
        public MySet Intersection(MySet other)
        {            
            return new MySet(_tree.Intersection(other._tree));
        }

        public static MySet operator +(MySet left, MySet right)
            => left.Union(right);
        
        public static MySet operator -(MySet left, MySet right)
            => left.Difference(right);
    
        public static MySet operator *(MySet left, MySet right)
            => left.Intersection(right);
        
        public IEnumerator<int> GetEnumerator()
        {
            return _tree.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    
}