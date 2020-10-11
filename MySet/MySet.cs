using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MySet
{
    public class MySet : IEnumerable<int>
    {
        private IntervalTree tree;
        public MySet(params MyRange[] ranges)
        {
            tree = new IntervalTree(ranges);
        }

        private MySet(IntervalTree tree)
        {
            this.tree = tree;
        }

        public bool Contains(int value)
        {
            return tree.Contains(value);
        }

        public MySet Union(MySet other)
        {
            return new MySet(tree.Union(other.tree));
        }

        public MySet Difference(MySet other)
        {
            return new MySet(tree.Difference(other.tree));
        }
        
        public MySet Intersection(MySet other)
        {            
            return new MySet(tree.Intersection(other.tree));
        }

        public static MySet operator +(MySet left, MySet right)
            => left.Union(right);
        
        public static MySet operator -(MySet left, MySet right)
            => left.Difference(right);
    
        public static MySet operator *(MySet left, MySet right)
            => left.Intersection(right);
        
        public IEnumerator<int> GetEnumerator()
        {
            return tree.GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    
}