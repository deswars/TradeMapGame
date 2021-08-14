using TradeMap.Core;
using TradeMap.Core.Map.ReadOnly;
using TradeMap.Engine.Map;
using Xunit;

namespace TradeMapTests.Engine.Map
{
    public class SquareMapTests
    {
        [Fact()]
        public void AddSettlementTest()
        {
            SquareMap map = new(100, 100, null);
            Settlement set1 = new("a", new Point(10, 10), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);
            Settlement set2 = new("b", new Point(10, 10), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);
            Settlement set3 = new("c", new Point(20, 20), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);

            Assert.True(map.AddSettlement(set1));
            Assert.False(map.AddSettlement(set2));
            Assert.True(map.AddSettlement(set3));
            Assert.Equal(2, map.Settlements.Count);
        }

        [Fact()]
        public void RemoveSettlementTest()
        {
            SquareMap map = new(100, 100, null);
            Settlement set1 = new("a", new Point(10, 10), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);
            Settlement set2 = new("b", new Point(10, 10), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);
            Settlement set3 = new("c", new Point(20, 20), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);
            map.AddSettlement(set1);
            map.AddSettlement(set2);
            map.AddSettlement(set3);

            Assert.True(map.RemoveSettlement(set1));
            Assert.Equal(1, map.Settlements.Count);
            Assert.False(map.RemoveSettlement(set2));
            Assert.Single(map.Settlements);
        }

        [Fact()]
        public void GetNeigborCellsTest()
        {
            SquareMap map = new(100, 100, null);

            Point p1 = new(10, 10);
            Assert.Equal(1, map.GetNeigborCells(p1, 0).Count);
            Point p2 = new(0, 10);
            Assert.Equal(13, map.GetNeigborCells(p1, 2).Count);
            Assert.Equal(9, map.GetNeigborCells(p2, 2).Count);

            Assert.Equal(317, map.GetNeigborCells(p1, 10).Count);

            var res = map.GetNeigborCells(p1, 2000);
            Assert.Equal(10000, res.Count);
        }

        [Fact()]
        public void GetNeigborSettlementsTest()
        {
            SquareMap map = new(100, 100, null);
            Settlement set1 = new("a", new Point(10, 10), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);
            Settlement set2 = new("b", new Point(20, 10), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);
            Settlement set3 = new("c", new Point(20, 20), new KeyedVectorFull<IResourceType>(), new KeyedVectorFull<IResourceType>(), 0, 0, 0);
            map.AddSettlement(set1);
            map.AddSettlement(set2);
            map.AddSettlement(set3);

            var res1 = map.GetNeigborSettlements(set1.Position, 0);
            Assert.Single(res1);
            var res2 = map.GetNeigborSettlements(set1.Position, 10);
            Assert.Equal(2, res2.Count);
            var res3 = map.GetNeigborSettlements(set1.Position, 20);
            Assert.Equal(3, res3.Count);
        }
    }
}