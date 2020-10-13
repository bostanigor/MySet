using System;
using System.Linq;
using IntervalSet;
using NUnit.Framework;

namespace TestIntervalSet
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BasicSets()
        {
            var set1 = new IntervalSet.IntervalSet(new MyRange(1, 5));
            Assert.AreEqual(set1.ToArray(), new int[] {1, 2, 3, 4, 5});
            var set2 = new IntervalSet.IntervalSet();
            Assert.AreEqual(set2.ToArray(), new int[] {});
            var set3 = new IntervalSet.IntervalSet(new MyRange(1, 3), new MyRange(5, 6));
            Assert.AreEqual(set3.ToArray(), new int[] {1, 2, 3, 5, 6});
            var set4 = new IntervalSet.IntervalSet(new MyRange(1, 1));
            Assert.AreEqual(set4.ToArray(), new int[] {1});
        }

        [Test]
        public void IntersectingIntervals()
        {
            var set1 = new IntervalSet.IntervalSet(new MyRange(1, 4), new MyRange(3, 5));
            Assert.AreEqual(set1.ToArray(), new int[] {1, 2, 3, 4, 5});
            var set2 = new IntervalSet.IntervalSet(new MyRange(1, 4), new MyRange(1, 4));
            Assert.AreEqual(set2.ToArray(), new int[] {1, 2, 3, 4});
            var set3 = new IntervalSet.IntervalSet(new MyRange(3, 5), new MyRange(1, 4));
            Assert.AreEqual(set3.ToArray(), new int[] {1, 2, 3, 4, 5});
            var set4 = new IntervalSet.IntervalSet(new MyRange(1, 4), new MyRange(5, 6));
            Assert.AreEqual(set4.ToArray(), new int[] {1, 2, 3, 4, 5, 6});
            var set5 = new IntervalSet.IntervalSet(new MyRange(1, 3), new MyRange(2, 3));
            Assert.AreEqual(set5.ToArray(), new int[] {1, 2, 3});
            var set6 = new IntervalSet.IntervalSet(new MyRange(1, 3), new MyRange(2, 5), new MyRange(3, 6));
            Assert.AreEqual(set6.ToArray(), new int[] {1, 2, 3, 4, 5, 6});
        }

        [Test]
        public void Contains()
        {
            var set1 = new IntervalSet.IntervalSet(new MyRange(1, 4));
            Assert.True(set1.Contains(1));
            Assert.True(set1.Contains(2));
            Assert.True(set1.Contains(3));
            Assert.True(set1.Contains(4));
            Assert.False(set1.Contains(0));
            Assert.False(set1.Contains(5));
            Assert.False(set1.Contains(Int32.MinValue));
            var set2 = new IntervalSet.IntervalSet();
            Assert.False(set2.Contains(0));
            Assert.False(set2.Contains(Int32.MinValue));
            Assert.False(set2.Contains(Int32.MaxValue));
            var set3 = new IntervalSet.IntervalSet(new MyRange(1, 3), new MyRange(2, 4), new MyRange(6, 6));
            Assert.True(set3.Contains(1));
            Assert.True(set3.Contains(2));
            Assert.True(set3.Contains(3));
            Assert.True(set3.Contains(4));
            Assert.True(set3.Contains(6));
            Assert.False(set3.Contains(0));
            Assert.False(set3.Contains(5));
            Assert.False(set3.Contains(Int32.MinValue));
        }

        [Test]
        public void SetsUnion()
        {
            var set1 = 
                new IntervalSet.IntervalSet(new MyRange(1, 3)).Union(
                new IntervalSet.IntervalSet(new MyRange(4, 6)));
            Assert.AreEqual(set1.ToArray(), new int[] {1, 2, 3, 4, 5, 6});
            var set2 = 
                new IntervalSet.IntervalSet(new MyRange(1, 3)).Union(
                new IntervalSet.IntervalSet());
            Assert.AreEqual(set2.ToArray(), new int[] {1, 2, 3});
            var set3 = 
                new IntervalSet.IntervalSet(new MyRange(4, 5)).Union(
                new IntervalSet.IntervalSet(new MyRange(1, 2)));
            Assert.AreEqual(set3.ToArray(), new int[] {1, 2, 4, 5});
            var set4 = 
                new IntervalSet.IntervalSet(new MyRange(1, 3)).Union(
                new IntervalSet.IntervalSet(new MyRange(2, 4)));
            Assert.AreEqual(set4.ToArray(), new int[] {1, 2, 3, 4});
            var set5 = 
                new IntervalSet.IntervalSet(new MyRange(1, 3)).Union(
                new IntervalSet.IntervalSet(new MyRange(2, 4), new MyRange(6, 7)));
            Assert.AreEqual(set5.ToArray(), new int[] {1, 2, 3, 4, 6, 7});
            var set6 = 
                new IntervalSet.IntervalSet(new MyRange(1, 1), new MyRange(3, 4), new MyRange(5, 6)).Union(
                new IntervalSet.IntervalSet(new MyRange(0, 7)));
            var test = set6.ToArray();
            Assert.AreEqual(set6.ToArray(), new int[] {0, 1, 2, 3, 4, 5, 6, 7});
        }

        [Test]
        public void SetsIntersection()
        {
            var set1 = 
                new IntervalSet.IntervalSet(new MyRange(1, 3)).Intersection(
                new IntervalSet.IntervalSet(new MyRange(4, 6)));
            Assert.AreEqual(set1.ToArray(), new int[] {});
            var set2 = 
                new IntervalSet.IntervalSet(new MyRange(1, 3)).Intersection(
                new IntervalSet.IntervalSet());
            Assert.AreEqual(set2.ToArray(), new int[] {});
            var set3 = 
                new IntervalSet.IntervalSet(new MyRange(1, 5)).Intersection(
                new IntervalSet.IntervalSet(new MyRange(4, 6)));
            Assert.AreEqual(set3.ToArray(), new int[] {4, 5});
            var set4 = 
                new IntervalSet.IntervalSet(new MyRange(1, 3)).Intersection(
                new IntervalSet.IntervalSet(new MyRange(3, 7)));
            Assert.AreEqual(set4.ToArray(), new int[] {3});
            var set5 = 
                new IntervalSet.IntervalSet(new MyRange(1, 4), new MyRange(6, 9)).Intersection(
                new IntervalSet.IntervalSet(new MyRange(3, 7)));
            Assert.AreEqual(set5.ToArray(), new int[] {3, 4, 6, 7});
        }

        [Test]
        public void SetsDifference()
        {
            var set1 = 
                new IntervalSet.IntervalSet(new MyRange(1, 3), new MyRange(5, 5)).Difference(
                new IntervalSet.IntervalSet());
            Assert.AreEqual(set1.ToArray(), new int[] {1, 2, 3, 5});
            var set2 = 
                new IntervalSet.IntervalSet().Difference(
                new IntervalSet.IntervalSet(new MyRange(1, 3), new MyRange(5, 5)));
            Assert.AreEqual(set2.ToArray(), new int[] {});
            var set3 = 
                new IntervalSet.IntervalSet(new MyRange(1, 5)).Difference(
                new IntervalSet.IntervalSet(new MyRange(6, 8)));
            Assert.AreEqual(set3.ToArray(), new int[] {1, 2, 3, 4, 5});
            var set4 = 
                new IntervalSet.IntervalSet(new MyRange(5, 7)).Difference(
                new IntervalSet.IntervalSet(new MyRange(0, 3)));
            Assert.AreEqual(set4.ToArray(), new int[] {5, 6, 7});
            var set5 = 
                new IntervalSet.IntervalSet(new MyRange(1, 5)).Difference(
                new IntervalSet.IntervalSet(new MyRange(3, 7)));
            Assert.AreEqual(set5.ToArray(), new int[] {1, 2});
            var set6 = 
                new IntervalSet.IntervalSet(new MyRange(1, 4), new MyRange(6, 8)).Difference(
                new IntervalSet.IntervalSet(new MyRange(3, 7)));
            Assert.AreEqual(set6.ToArray(), new int[] {1, 2, 8});
            var set7 = 
                new IntervalSet.IntervalSet(new MyRange(1, 5)).Difference(
                new IntervalSet.IntervalSet(new MyRange(3, 3)));
            Assert.AreEqual(set7.ToArray(), new int[] {1, 2, 4, 5});
        }
    }
}