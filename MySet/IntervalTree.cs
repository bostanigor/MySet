using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MySet
{
    public class MyRange
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Count { get; set; }

        public MyRange(int min, int max)
        {
            this.Min = min;
            this.Max = max;
            this.Count = max - min + 1;
        }
    }

    public class IntervalTree
    {
        private TreeNode root;

        public IntervalTree(List<MyRange> intervals)
        {
            // Sort intervals by Min value
            intervals.Sort((x, y) => x.Min.CompareTo(y.Min));
            root = new TreeNode(MergeIntervals(intervals).ToArray());
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            foreach (var x in RecursiveIEnum(root))
                yield return x;
        }

        public bool Contains(int x)
        {
            var node = root;
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

        /// Merge overlapping intervals (ex. 0..10, 7..15 -> 0..15) 
        private List<MyRange> MergeIntervals(List<MyRange> intervals)
        {
            var result = new List<MyRange>();
            var intMin = intervals[0].Min;
            var intMax = intervals[0].Max;
            
            for (int i = 1; i < intervals.Count; i++)
            {
                if (intervals[i].Min < intMax)
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
        
        private IEnumerable<int> RecursiveIEnum(TreeNode node)
        {
            if (node.left != null)
                foreach (int x in RecursiveIEnum(node.left))
                    yield return x;
                    
            for (int x = node.intMin; x <= node.intMax; x++)
                yield return x;
            
            if (node.right != null)
                foreach (int x in RecursiveIEnum(node.right))
                    yield return x;
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

                        var leftIntervals = intervals[0..i];
                        var rightIntervals = intervals[(i + 1)..intervals.Length];
                        left = leftIntervals.Length > 0 ? new TreeNode(leftIntervals, this) : null;
                        right = rightIntervals.Length > 0 ? new TreeNode(rightIntervals, this) : null;
                        return;
                    }
                    middle -= intervals[i].Count;
                }
            }
        }
    }
}