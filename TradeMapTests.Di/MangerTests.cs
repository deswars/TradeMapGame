using Xunit;
using TradeMap.Di;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeMap.GameLog;
using TradeMapTests.Di.ServiceStubs;
using TradeMap.Di.GameLog;

namespace TradeMapTests.Di
{
    public class MangerTests
    {
        [Fact()]
        public void GetServiceListTest()
        {
            GameLogImpl log = new();
            log.SetInfoLevel(InfoLevels.All);
            Manager man = new Manager(log);

            var list = man.GetServiceList();
            Assert.Equal(2, list.Count());
            Assert.True(list.ContainsKey("SN"));
            Assert.True(list.ContainsKey("ServiceUnnamed"));
            Assert.Equal(typeof(ServiceNamed), list["SN"]);
            Assert.Equal(typeof(ServiceUnnamed), list["ServiceUnnamed"]);
        }

        [Fact()]
        public void GetServicConstantListTest()
        {
            GameLogImpl log = new();
            log.SetInfoLevel(InfoLevels.All);
            Manager man = new Manager(log);

            var list = man.GetServicConstantList();
            Assert.Equal(10, list.Count());
            Assert.Equal(6, log.Entries.Count());
            Assert.Equal(4, log.Entries.Where(x => x.GetType() == typeof(LogEntryConstantTypeError)).Count());
            Assert.Equal(2, log.Entries.Where(x => x.GetType() == typeof(LogEntryConstantOverride)).Count());

            Assert.Contains("ServiceUnnamed-C1", list);
            Assert.Equal("1", list["ServiceUnnamed-C1"].Item2);
            Assert.Equal(typeof(int), list["ServiceUnnamed-C1"].Item1);
            
            Assert.Contains("ServiceUnnamed-C-N", list);
            Assert.Equal("2", list["ServiceUnnamed-C-N"].Item2);
            Assert.Equal(typeof(int), list["ServiceUnnamed-C-N"].Item1);
            
            Assert.Contains("ServiceUnnamed-C-NF", list);
            Assert.Equal("3", list["ServiceUnnamed-C-NF"].Item2);
            Assert.Equal(typeof(int), list["ServiceUnnamed-C-NF"].Item1);
            
            Assert.Contains("ServiceUnnamed-C-N-TII", list);
            Assert.Equal("4", list["ServiceUnnamed-C-N-TII"].Item2);
            Assert.Equal(typeof(int), list["ServiceUnnamed-C-N-TII"].Item1);
            
            Assert.Contains("SN-C1", list);
            Assert.Equal("1", list["SN-C1"].Item2);
            Assert.Equal(typeof(int), list["SN-C1"].Item1);
            
            Assert.Contains("SN-C-N", list);
            Assert.Equal("2", list["SN-C-N"].Item2);
            Assert.Equal(typeof(int), list["SN-C-N"].Item1);
            
            Assert.Contains("SN-C-NF", list);
            Assert.Equal("3", list["SN-C-NF"].Item2);
            Assert.Equal(typeof(int), list["SN-C-NF"].Item1);


            Assert.Contains("SN-C-N-TII", list);
            Assert.Equal("4", list["SN-C-N-TII"].Item2);
            Assert.Equal(typeof(int), list["SN-C-N-TII"].Item1);
            
            Assert.Contains("Common", list);
            Assert.Equal("1", list["Common"].Item2);
            Assert.Equal(typeof(string), list["Common"].Item1);

            Assert.Contains("CommonD", list);
            Assert.Equal(typeof(string), list["CommonD"].Item1);
        }

        [Fact()]
        public void GetServiceActionListTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void AssignConstantsTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void BuildEngineTurnTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}