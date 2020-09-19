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
            intervals.Sort((x, y) => x.Min.CompareTo(y.Min));
            root = new TreeNode(MergeIntervals(intervals).ToArray());
        }

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
                    intMin = intervals[0].Min;
                    intMax = intervals[0].Max;
                }
            }
            result.Add(new MyRange(intMin, intMax));
            
            return result;
        }
        
        private class TreeNode
        {
            private int xMid;
            private int intMin, intMax; // Interval borders
        
            private TreeNode parent;
            private TreeNode left, right;
        
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
                        this.xMid = intervals[i].Min + middle; // TODO: Check borders
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