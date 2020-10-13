using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IntervalSet
{
    public struct MyRange
    {
        public int Min { get; private set; }
        public int Max { get; private set; }
        public int Count { get; }

        public MyRange(int min, int max)
        {
            this.Min = min;
            this.Max = max;
            this.Count = max - min + 1;
        }
    }

    public class IntervalTree : IEnumerable<int>
    {
        private readonly TreeNode _root;
        private int _intervalsCount;
        public int Count { get; private set; }
        public IntervalTree(params MyRange[] intervals)
        {
            // Sort intervals by Min value
            if (intervals.Length == 0)
            {
                _root = null;
                return;
            }
            Array.Sort(intervals, (x, y) => x.Min.CompareTo(y.Min));
            var mergedIntervals = MergeIntervals(intervals).ToArray();
            _root = new TreeNode(MergeIntervals(intervals).ToArray());
            _intervalsCount = mergedIntervals.Length;
            Count = mergedIntervals.Aggregate(0, (sum, range) => sum + range.Count);
        }
        public IntervalTree(IntervalTree other)
        {
            this._root = new TreeNode(other._root);
            this._intervalsCount = other._intervalsCount;
            this.Count = other.Count;
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            if (_root == null)
                yield break;
            foreach (var x in RecursiveIEnumInt(_root))
                yield return x;
        }

        public bool Contains(int x)
        {
            var node = _root;
            while (node != null)
            {
                if (x <= node.xMid)
                    if (x >= node.intMin)
                        return true;
                    else
                        node = node.left;
                else if (x <= node.intMax)
                    return true;
                else
                    node = node.right;
            }

            return false;
        }
        
        public IntervalTree Union(IntervalTree other)
        {
            if (this._intervalsCount == 0)
                return new IntervalTree(other);
            if (other._intervalsCount == 0)
                return new IntervalTree(this);
            var intervals1 = this.RecursiveIEnumRange(this._root).GetEnumerator();
            var intervals2 = other.RecursiveIEnumRange(other._root).GetEnumerator();
            var newRanges = new List<MyRange>();
            bool notEmpty1 = intervals1.MoveNext();
            bool notEmpty2 = intervals2.MoveNext();
            int lastMin, lastMax;
            if (intervals1.Current.Min < intervals2.Current.Min)
            {
                lastMin = intervals1.Current.Min;
                lastMax = intervals1.Current.Max;
                notEmpty1 = intervals1.MoveNext();
            }
            else
            {
                lastMin = intervals2.Current.Min;
                lastMax = intervals2.Current.Max;
                notEmpty2 = intervals2.MoveNext();
            }
            while (notEmpty1 && notEmpty2)
            {
                var min1 = intervals1.Current.Min;
                var min2 = intervals2.Current.Min;
                var max1 = intervals1.Current.Max;
                var max2 = intervals2.Current.Max;
                if (min1 <= lastMax)
                {
                    lastMax = max1 > lastMax ? max1 : lastMax;
                    notEmpty1 = intervals1.MoveNext();
                }
                else if (min2 <= lastMax)
                {
                    lastMax = max2 > lastMax ? max2 : lastMax;
                    notEmpty2 = intervals2.MoveNext();
                }
                else
                {
                    newRanges.Add(new MyRange(lastMin, lastMax));
                    if (min1 < min2)
                    {
                        lastMin = min1;
                        lastMax = max1;
                        notEmpty1 = intervals1.MoveNext();
                    }
                    else
                    {
                        lastMin = min2;
                        lastMax = max2;
                        notEmpty2 = intervals2.MoveNext();
                    }
                }
            }
            newRanges.Add(new MyRange(lastMin, lastMax));
            var unfinishedSet = notEmpty1 ? intervals1 : intervals2;
            do {
                newRanges.Add(unfinishedSet.Current);
            } while (unfinishedSet.MoveNext());

            return new IntervalTree(newRanges.ToArray());
        }

        public IntervalTree Intersection(IntervalTree other)
        {
            if (this._intervalsCount == 0 || other._intervalsCount == 0)
                return new IntervalTree();
            var newRanges = new List<MyRange>();
            var intervals1 = this.RecursiveIEnumRange(this._root).GetEnumerator();
            var intervals2 = other.RecursiveIEnumRange(other._root).GetEnumerator();
            bool notEmpty1 = intervals1.MoveNext();
            bool notEmpty2 = intervals2.MoveNext();
            while (notEmpty1 && notEmpty2)
            {
                var min1 = intervals1.Current.Min;
                var min2 = intervals2.Current.Min;
                var max1 = intervals1.Current.Max;
                var max2 = intervals2.Current.Max;

                if (max2 < min1)
                {
                    notEmpty2 = intervals2.MoveNext();
                    continue;
                }
                if (max1 < min2)
                {
                    notEmpty1 = intervals1.MoveNext();
                    continue;
                }
                
                var newMin = Math.Max(min1, min2);
                if (max2 <= max1)
                {
                    newRanges.Add(new MyRange(newMin, max2));
                    notEmpty2 = intervals2.MoveNext();
                }
                else
                {
                    newRanges.Add(new MyRange(newMin, max1));
                    notEmpty1 = intervals1.MoveNext();
                }
            }
            return new IntervalTree(newRanges.ToArray());
        }

        public IntervalTree Difference(IntervalTree other)
        {
            if (this._intervalsCount == 0)
                return new IntervalTree();
            if (other._intervalsCount == 0)
                return new IntervalTree(this);
            var newRanges = new List<MyRange>();
            var intervals1 = this.RecursiveIEnumRange(this._root).GetEnumerator();
            var intervals2 = other.RecursiveIEnumRange(other._root).GetEnumerator();
            bool notEmpty1 = intervals1.MoveNext();
            bool notEmpty2 = intervals2.MoveNext();
            if (!notEmpty1) // If first tree is empty
                return new IntervalTree();
            var lastMin = intervals1.Current.Min;
            var lastMax = intervals1.Current.Max;

            while (notEmpty1 && intervals2.Current.Min > lastMax)
            {
                newRanges.Add(new MyRange(lastMin, lastMax));
                notEmpty1 = intervals1.MoveNext();
                lastMin = intervals1.Current.Min;
                lastMax = intervals1.Current.Max;
            }

            while (notEmpty1 && notEmpty2)
            {
                var min1 = intervals1.Current.Min;
                var min2 = intervals2.Current.Min;
                var max1 = intervals1.Current.Max;
                var max2 = intervals2.Current.Max;

                if (lastMin > lastMax)
                {
                    notEmpty1 = intervals1.MoveNext();
                    lastMin = intervals1.Current.Min;
                    lastMax = intervals1.Current.Max;
                    continue;
                }

                if (max2 < lastMin)
                    notEmpty2 = intervals2.MoveNext();
                else
                {
                    if (min2 <= min1)
                        lastMin = max2 + 1;
                    else
                    {
                        newRanges.Add(new MyRange(lastMin, min2 - 1));
                        if (max2 < lastMax)
                            notEmpty2 = intervals2.MoveNext();
                        lastMin = max2 + 1;
                    }
                }
            }
            if (notEmpty1)
            {
                newRanges.Add(new MyRange(lastMin, lastMax)); 
                while (intervals1.MoveNext())
                    newRanges.Add(intervals1.Current);
            }

            return new IntervalTree(newRanges.ToArray());
        }
        
        /// Merge overlapping intervals (ex. 0..10, 7..15 -> 0..15) 
        private List<MyRange> MergeIntervals(MyRange[] intervals)
        {
            var result = new List<MyRange>();
            var intMin = intervals[0].Min;
            var intMax = intervals[0].Max;
            
            for (int i = 1; i < intervals.Length; i++)
            {
                if (intervals[i].Min <= intMax)
                    intMax = Math.Max(intMax, intervals[i].Max);
                else
                {
                    result.Add(new MyRange(intMin, intMax));
                    intMin = intervals[i].Min;
                    intMax = intervals[i].Max;
                }
            }
            result.Add(new MyRange(intMin, intMax));
            
            return result;
        }
        
        private IEnumerable<int> RecursiveIEnumInt(TreeNode node)
        {
            if (node.left != null)
                foreach (int x in RecursiveIEnumInt(node.left))
                    yield return x;
                    
            for (int x = node.intMin; x <= node.intMax; x++)
                yield return x;
            
            if (node.right != null)
                foreach (int x in RecursiveIEnumInt(node.right))
                    yield return x;
        }
        
        private IEnumerable<MyRange> RecursiveIEnumRange(TreeNode node)
        {
            if (node.left != null)
                foreach (var range in RecursiveIEnumRange(node.left))
                    yield return range;
            
            yield return new MyRange(node.intMin, node.intMax);
            
            if (node.right != null)
                foreach (var range in RecursiveIEnumRange(node.right))
                    yield return range;
        }
        
        
        private class TreeNode
        {
            public int xMid;
            public int intMin, intMax; // Interval borders
        
            public TreeNode parent;
            public TreeNode left, right;
        
            public TreeNode(MyRange[] intervals, TreeNode parent = null)
            {
                if (intervals.Length == 0)
                    throw new Exception("intervals count is zero");
            
                int totalCount = intervals.Aggregate(0,
                    (count, interval) => count + interval.Count);
                int middle = totalCount / 2;
            
                for (int i = 0; i < intervals.Length; i++)
                {
                    if (intervals[i].Count > middle)
                    {
                        this.xMid = intervals[i].Min + middle;
                        this.intMin = intervals[i].Min;
                        this.intMax = intervals[i].Max;
                        this.parent = parent;

                        var temp = intervals.Length - (i + 1);
                        var leftIntervals = new MyRange[i];
                        var rightIntervals = new MyRange[temp];
                        Array.Copy(intervals, 0, leftIntervals, 0, i);
                        Array.Copy(intervals, i + 1, rightIntervals, 0, temp);
                        left = leftIntervals.Length > 0 ? new TreeNode(leftIntervals, this) : null;
                        right = rightIntervals.Length > 0 ? new TreeNode(rightIntervals, this) : null;
                        return;
                    }
                    middle -= intervals[i].Count;
                }
            }

            public TreeNode(TreeNode other)
            {
                this.xMid = other.xMid;
                this.intMin = other.intMin;
                this.intMax = other.intMax;
                if (other.left != null)
                    this.left = new TreeNode(other.left) {parent = this};
                if (other.right != null)
                    this.right = new TreeNode(other.right) {parent = this};
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}