using System.Collections;
using System.Collections.Generic;

namespace IntervalSet
{
    public class IntervalSet : IEnumerable<int>
    {
        private readonly IntervalTree _tree;

        public int Count
        {
            get { return _tree.Count; }
        }
        public IntervalSet(params MyRange[] ranges)
        {
            _tree = new IntervalTree(ranges);
        }

        private IntervalSet(IntervalTree tree)
        {
            this._tree = tree;
        }

        public bool Contains(int value)
        {
            return _tree.Contains(value);
        }

        public IntervalSet Union(IntervalSet other)
        {
            return new IntervalSet(_tree.Union(other._tree));
        }

        public IntervalSet Difference(IntervalSet other)
        {
            return new IntervalSet(_tree.Difference(other._tree));
        }
        
        public IntervalSet Intersection(IntervalSet other)
        {            
            return new IntervalSet(_tree.Intersection(other._tree));
        }

        public static IntervalSet operator +(IntervalSet left, IntervalSet right)
            => left.Union(right);
        
        public static IntervalSet operator -(IntervalSet left, IntervalSet right)
            => left.Difference(right);
    
        public static IntervalSet operator *(IntervalSet left, IntervalSet right)
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