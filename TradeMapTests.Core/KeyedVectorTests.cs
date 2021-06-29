using Xunit;
using TradeMap.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeMapTests.Core
{
    public class KeyedVectorTests
    {
        [Fact()]
        public void KeyedVectorTest()
        {
            List<string> keys = new() { "a", "b", "c" };
            KeyedVector<string> vec = new(keys);

            Assert.Equal(0, vec["a"]);
            Assert.Equal(0, vec["b"]);
            Assert.Equal(0, vec["c"]);

            vec["b"] = 1;
            vec["a"] = 2;
            vec["c"] = 3;
            vec["b"] = 4;

            Assert.Equal(2, vec["a"]);
            Assert.Equal(4, vec["b"]);
            Assert.Equal(3, vec["c"]);
        }

        [Theory]
        [MemberData(nameof(SingleVector))]
        public void CloneTest(List<string> keys, KeyedVector<string> vec)
        {
            var cloned = vec.Clone();
            foreach (var key in keys)
            {
                Assert.Equal(vec[key], cloned[key]);
            }
        }

        [Theory]
        [MemberData(nameof(SingleVector))]
        public void ZeroedTest(List<string> keys, KeyedVector<string> vec)
        {
            var zeroed = vec.Zeroed();
            foreach (var key in keys)
            {
                Assert.Equal(0, zeroed[key]);
            }
        }

        [Theory]
        [MemberData(nameof(DoubleVector))]
        public void AddTest(List<string> keys, KeyedVector<string> vec1, KeyedVector<string> vec2)
        {
            var originalVec = vec1.Clone();
            vec1.Add(vec2);
            foreach (var key in keys)
            {
                Assert.Equal(originalVec[key] + vec2[key], vec1[key]);
            }
        }

        [Theory]
        [MemberData(nameof(DoubleVector))]
        public void AddNewTest(List<string> keys, KeyedVector<string> vec1, KeyedVector<string> vec2)
        {
            var originalVec = vec1.Clone();
            var resultVect = vec1.AddNew(vec2);
            foreach (var key in keys)
            {
                Assert.Equal(originalVec[key], vec1[key]);
                Assert.Equal(vec1[key] + vec2[key], resultVect[key]);
            }
        }

        [Theory]
        [MemberData(nameof(DoubleVector))]
        public void SubTest(List<string> keys, KeyedVector<string> vec1, KeyedVector<string> vec2)
        {
            var originalVec = vec1.Clone();
            vec1.Sub(vec2);
            foreach (var key in keys)
            {
                Assert.Equal(originalVec[key] - vec2[key], vec1[key]);
            }
        }

        [Theory]
        [MemberData(nameof(DoubleVector))]
        public void SubNewTest(List<string> keys, KeyedVector<string> vec1, KeyedVector<string> vec2)
        {
            var originalVec = vec1.Clone();
            var resultVect = vec1.SubNew(vec2);
            foreach (var key in keys)
            {
                Assert.Equal(originalVec[key], vec1[key]);
                Assert.Equal(vec1[key] - vec2[key], resultVect[key]);
            }
        }

        [Theory]
        [MemberData(nameof(SingleVector))]
        public void NegTest(List<string> keys, KeyedVector<string> vec)
        {
            var originalVec = vec.Clone();
            vec.Neg();
            foreach (var key in keys)
            {
                Assert.Equal(-originalVec[key], vec[key]);
            }
        }

        [Theory]
        [MemberData(nameof(SingleVector))]
        public void NegNewTest(List<string> keys, KeyedVector<string> vec)
        {
            var originalVec = vec.Clone();
            var resultVec = vec.NegNew();
            foreach (var key in keys)
            {
                Assert.Equal(originalVec[key], vec[key]);
                Assert.Equal(-vec[key], resultVec[key]);
            }
        }

        [Theory]
        [MemberData(nameof(FilterVector))]
        public void FilterTest(List<string> keys, KeyedVector<string> vec, List<string> filterKeys)
        {
            var originalVec = vec.Clone();
            var newVec = vec.Filter(filterKeys);
            foreach (var key in keys)
            {
                Assert.Equal(originalVec[key], vec[key]);
                if (filterKeys.Contains(key))
                {
                    Assert.Equal(vec[key], newVec[key]);
                }
                else
                {
                    Assert.Equal(0, newVec[key]);
                }
            }
        }

        [Theory]
        [MemberData(nameof(FilterVector))]
        public void FilterSmallerTest(List<string> keys, KeyedVector<string> vec, List<string> filterKeys)
        {
            var originalVec = vec.Clone();
            var newVec = vec.FilterSmaller(filterKeys);
            Assert.Equal(filterKeys.Count, newVec.Count());
            foreach (var key in keys)
            {
                Assert.Equal(originalVec[key], vec[key]);
                if (filterKeys.Contains(key))
                {
                    Assert.Equal(vec[key], newVec[key]);
                }
            }
        }

        [Theory]
        [MemberData(nameof(CompareVector))]
        public void IsSmallerTest(KeyedVector<string> vec1, KeyedVector<string> vec2)
        {
            bool smaller = vec1.IsSmaller(vec2);
            bool expected = !vec1.Where(x => x.Value > vec2[x.Key]).Any();
            Assert.Equal(expected, smaller);
        }

        [Theory]
        [MemberData(nameof(CompareVector))]
        public void IsBiggerTest(KeyedVector<string> vec1, KeyedVector<string> vec2)
        {
            bool smaller = vec2.IsBigger(vec1);
            bool expected = !vec1.Where(x => x.Value > vec2[x.Key]).Any();
            Assert.Equal(expected, smaller);
        }

        public static IEnumerable<object[]> SingleVector()
        {
            List<string> keys = new() { "a", "b", "c" };
            return new List<Object[]>
            {
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 1, ["b"] = 2, ["c"] = 3 } },
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 4, ["b"] = 5, ["c"] = 6 } },
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 5, ["b"] = 4, ["c"] = 3 } }
            };
        }

        public static IEnumerable<object[]> DoubleVector()
        {
            List<string> keys = new() { "a", "b", "c" };
            return new List<Object[]>
            {
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 1, ["b"] = 2, ["c"] = 3 }, new KeyedVector<string>(keys) { ["a"] = 4, ["b"] = 5, ["c"] = 6 } },
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 4, ["b"] = 5, ["c"] = 6 }, new KeyedVector<string>(keys) { ["a"] = 1, ["b"] = 2, ["c"] = 3 } },
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new KeyedVector<string>(keys) { ["a"] = 10, ["b"] = 2, ["c"] = 3 } },
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new KeyedVector<string>(keys) { ["a"] = 1, ["b"] = 2, ["c"] = 3 } }
            };
        }

        public static IEnumerable<object[]> FilterVector()
        {
            List<string> keys = new() { "a", "b", "c" };
            return new List<Object[]>
            {
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 1, ["b"] = 2, ["c"] = 3 }, new List<string>() { "a" } },
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 4, ["b"] = 5, ["c"] = 6 }, new List<string>() { "a", "b" } },
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new List<string>() { "b" } },
                new object[] { keys, new KeyedVector<string>(keys) { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new List<string>() { "c" } }
            };
        }

        public static IEnumerable<object[]> CompareVector()
        {
            List<string> keys = new() { "a", "b", "c" };
            return new List<Object[]>
            {
                new object[] { new KeyedVector<string>(keys) { ["a"] = 1, ["b"] = 2, ["c"] = 3 }, new KeyedVector<string>(keys) { ["a"] = 4, ["b"] = 5, ["c"] = 6 } },
                new object[] { new KeyedVector<string>(keys) { ["a"] = 4, ["b"] = 5, ["c"] = 6 }, new KeyedVector<string>(keys) { ["a"] = 1, ["b"] = 2, ["c"] = 3 } },
                new object[] { new KeyedVector<string>(keys) { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new KeyedVector<string>(keys) { ["a"] = 10, ["b"] = 2, ["c"] = 3 } },
                new object[] { new KeyedVector<string>(keys) { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new KeyedVector<string>(keys) { ["a"] = 1, ["b"] = 2, ["c"] = 3 } }
            };
        }
    }
}