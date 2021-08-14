using System.Collections.Generic;
using System.Linq;
using TradeMap.Core;
using Xunit;

namespace TradeMapTests.Core
{
    public class KeyedVectorTests
    {
        [Fact()]
        public void KeyedVectorTest()
        {
            List<string> keys = new() { "a", "b", "c" };
            KeyedVectorFull<string>.SetAvailableKeys(keys);
            KeyedVectorFull<string> vec = new();

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
            Assert.Throws<KeyNotFoundException>(() => vec["d"] = 0);
        }

        [Theory]
        [MemberData(nameof(SingleVector))]
        public void CloneTest(List<string> keys, KeyedVectorFull<string> vec)
        {
            var cloned = vec.Clone();
            foreach (var key in keys)
            {
                Assert.Equal(vec[key], cloned[key]);
            }
        }

        [Theory]
        [MemberData(nameof(DoubleVector))]
        public void AddTest(List<string> keys, KeyedVectorFull<string> vec1, KeyedVectorPartial<string> vec2)
        {
            var originalVec = vec1.Clone();
            var commonKeys = vec2.Select(x => x.Key).ToHashSet();
            vec1.Add(vec2);
            foreach (var key in keys)
            {
                if (commonKeys.Contains(key))
                {
                    Assert.Equal(originalVec[key] + vec2[key], vec1[key]);
                }
                else
                {
                    Assert.Equal(originalVec[key], vec1[key]);
                }
            }
        }

        [Theory]
        [MemberData(nameof(DoubleVector))]
        public void SubTest(List<string> keys, KeyedVectorFull<string> vec1, KeyedVectorPartial<string> vec2)
        {
            var originalVec = vec1.Clone();
            var commonKeys = vec2.Select(x => x.Key).ToHashSet();
            vec1.Sub(vec2);
            foreach (var key in keys)
            {
                if (commonKeys.Contains(key))
                {
                    Assert.Equal(originalVec[key] - vec2[key], vec1[key]);
                }
                else
                {
                    Assert.Equal(originalVec[key], vec1[key]);
                }
            }
        }

        [Theory]
        [MemberData(nameof(SingleVector))]
        public void NegTest(List<string> keys, KeyedVectorFull<string> vec)
        {
            var originalVec = vec.Clone();
            vec.Neg();
            foreach (var key in keys)
            {
                Assert.Equal(-originalVec[key], vec[key]);
            }
        }

        [Theory]
        [MemberData(nameof(FilterVector))]
        public void FilterTest(List<string> keys, KeyedVectorFull<string> vec, List<string> filterKeys)
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
        public void FilterSmallerTest(List<string> keys, KeyedVectorFull<string> vec, List<string> filterKeys)
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
        public void IsSmallerTest(KeyedVectorFull<string> vec1, KeyedVectorPartial<string> vec2)
        {
            bool smaller = vec1.IsSmaller(vec2);
            bool expected = !vec2.Where(x => x.Value < vec1[x.Key]).Any();
            Assert.Equal(expected, smaller);
        }

        [Theory]
        [MemberData(nameof(CompareVector))]
        public void IsBiggerTest(KeyedVectorFull<string> vec1, KeyedVectorPartial<string> vec2)
        {
            bool smaller = vec1.IsBigger(vec2);
            bool expected = !vec2.Where(x => x.Value > vec1[x.Key]).Any();
            Assert.Equal(expected, smaller);
        }

        public static IEnumerable<object[]> SingleVector()
        {
            List<string> keys = new() { "a", "b", "c" };
            KeyedVectorFull<string>.SetAvailableKeys(keys);
            return new List<object[]>
            {
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 2, ["c"] = 3 } },
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 4, ["b"] = 5, ["c"] = 6 } },
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 5, ["b"] = 4, ["c"] = 3 } }
            };
        }

        public static IEnumerable<object[]> DoubleVector()
        {
            List<string> keys = new() { "a", "b", "c" };
            List<string> keysSmall = new() { "a", "c" };
            KeyedVectorFull<string>.SetAvailableKeys(keys);
            return new List<object[]>
            {
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 2, ["c"] = 3 }, new KeyedVectorFull<string>() { ["a"] = 4, ["b"] = 5, ["c"] = 6 } },
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 4, ["b"] = 5, ["c"] = 6 }, new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 2, ["c"] = 3 } },
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, (new KeyedVectorFull<string>() { ["a"] = 10, ["b"] = 2, ["c"] = 3 }).FilterSmaller(keysSmall) },
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, (new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 2, ["c"] = 3 }).FilterSmaller(keysSmall) }
            };
        }

        public static IEnumerable<object[]> FilterVector()
        {
            List<string> keys = new() { "a", "b", "c" };
            KeyedVectorFull<string>.SetAvailableKeys(keys);
            return new List<object[]>
            {
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 2, ["c"] = 3 }, new List<string>() { "a" } },
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 4, ["b"] = 5, ["c"] = 6 }, new List<string>() { "a", "b" } },
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new List<string>() { "b" } },
                new object[] { keys, new KeyedVectorFull<string>() { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new List<string>() { "c" } }
            };
        }

        public static IEnumerable<object[]> CompareVector()
        {
            List<string> keys = new() { "a", "b", "c" };
            KeyedVectorFull<string>.SetAvailableKeys(keys);
            return new List<object[]>
            {
                new object[] { new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 2, ["c"] = 3 }, new KeyedVectorFull<string>() { ["a"] = 4, ["b"] = 5, ["c"] = 6 } },
                new object[] { new KeyedVectorFull<string>() { ["a"] = 4, ["b"] = 5, ["c"] = 6 }, new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 2, ["c"] = 3 } },
                new object[] { new KeyedVectorFull<string>() { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new KeyedVectorFull<string>() { ["a"] = 10, ["b"] = 2, ["c"] = 3 } },
                new object[] { new KeyedVectorFull<string>() { ["a"] = 5, ["b"] = 4, ["c"] = 3 }, new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 2, ["c"] = 3 } },
                new object[] { new KeyedVectorFull<string>() { ["a"] = 1, ["b"] = 10, ["c"] = 3 }, new KeyedVectorFull<string>() { ["a"] = 5, ["c"] = 5 } },
                new object[] { new KeyedVectorFull<string>() { ["a"] = 5, ["b"] = 0, ["c"] = 3 }, new KeyedVectorFull<string>() { ["a"] = 1, ["c"] = 3 } },
            };
        }
    }
}