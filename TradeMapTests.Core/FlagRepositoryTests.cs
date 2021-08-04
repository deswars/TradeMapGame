using System.Collections.Generic;
using TradeMap.Core;
using Xunit;

namespace TradeMapTests.Core
{
    public class FlagRepositoryTests
    {
        [Fact()]
        public void ClearFlagsTest()
        {
            FlagRepository repo = new();

            string k1 = "a";
            string v1 = "v1";
            string k2 = "b";
            string v2 = "v1";
            repo.SetFlagStr(k1, v1);
            repo.SetFlagStr(k2, v2);

            string k3 = "b";
            int v3 = 2;
            string k4 = "c";
            int v4 = 5;
            repo.SetFlagInt(k3, v3);
            repo.SetFlagInt(k4, v4);

            string k5 = "c";
            double v5 = 5;
            string k6 = "a";
            double v6 = 3;
            repo.SetFlagDouble(k5, v5);
            repo.SetFlagDouble(k6, v6);

            repo.ClearFlags();
            Assert.False(repo.HasFlagStr(k1));
            Assert.False(repo.HasFlagStr(k2));
            Assert.False(repo.HasFlagInt(k3));
            Assert.False(repo.HasFlagInt(k4));
            Assert.False(repo.HasFlagDouble(k5));
            Assert.False(repo.HasFlagDouble(k6));
        }

        [Fact()]
        public void HasFlagStrTest()
        {
            FlagRepository repo = new();
            Assert.False(repo.HasFlagStr("a"));
            Assert.False(repo.HasFlagInt("a"));
            Assert.False(repo.HasFlagDouble("a"));

            string k1 = "a";
            string v1 = "v1";
            string k2 = "b";
            string v2 = "v1";
            repo.SetFlagStr(k1, v1);
            repo.SetFlagStr(k2, v2);

            string k3 = "b";
            int v3 = 2;
            string k4 = "c";
            int v4 = 5;
            repo.SetFlagInt(k3, v3);
            repo.SetFlagInt(k4, v4);

            string k5 = "c";
            double v5 = 5;
            string k6 = "a";
            double v6 = 3;
            repo.SetFlagDouble(k5, v5);
            repo.SetFlagDouble(k6, v6);

            Assert.True(repo.HasFlagStr(k1));
            Assert.True(repo.HasFlagStr(k2));
            Assert.False(repo.HasFlagStr(k4));

            Assert.True(repo.HasFlagInt(k3));
            Assert.True(repo.HasFlagInt(k4));
            Assert.False(repo.HasFlagInt(k1));

            Assert.True(repo.HasFlagDouble(k5));
            Assert.True(repo.HasFlagDouble(k6));
            Assert.False(repo.HasFlagDouble(k3));
        }

        [Fact()]
        public void GetFlagTest()
        {
            FlagRepository repo = new();
            Assert.Throws<KeyNotFoundException>(() => repo.GetFlagStr("a"));
            Assert.Throws<KeyNotFoundException>(() => repo.GetFlagInt("a"));
            Assert.Throws<KeyNotFoundException>(() => repo.GetFlagDouble("a"));

            string k1 = "a";
            string v1 = "v1";
            string k2 = "b";
            string v2 = "v1";
            repo.SetFlagStr(k1, v1);
            repo.SetFlagStr(k2, v2);

            string k3 = "b";
            int v3 = 2;
            string k4 = "c";
            int v4 = 5;
            repo.SetFlagInt(k3, v3);
            repo.SetFlagInt(k4, v4);

            string k5 = "c";
            double v5 = 5;
            string k6 = "a";
            double v6 = 3;
            repo.SetFlagDouble(k5, v5);
            repo.SetFlagDouble(k6, v6);

            Assert.Equal(v1, repo.GetFlagStr(k1));
            Assert.Equal(v2, repo.GetFlagStr(k2));
            Assert.Throws<KeyNotFoundException>(() => repo.GetFlagStr(k4));

            Assert.Equal(v3, repo.GetFlagInt(k3));
            Assert.Equal(v4, repo.GetFlagInt(k4));
            Assert.Throws<KeyNotFoundException>(() => repo.GetFlagInt(k1));

            Assert.Equal(v5, repo.GetFlagDouble(k5));
            Assert.Equal(v6, repo.GetFlagDouble(k6));
            Assert.Throws<KeyNotFoundException>(() => repo.GetFlagDouble(k3));
        }

        [Fact()]
        public void TryGetFlagTest()
        {
            FlagRepository repo = new();
            Assert.False(repo.TryGetFlagStr("a", out _));
            Assert.False(repo.TryGetFlagInt("a", out _));
            Assert.False(repo.TryGetFlagDouble("a", out _));

            string k1 = "a";
            string v1 = "v1";
            string k2 = "b";
            string v2 = "v1";
            repo.SetFlagStr(k1, v1);
            repo.SetFlagStr(k2, v2);

            string k3 = "b";
            int v3 = 2;
            string k4 = "c";
            int v4 = 5;
            repo.SetFlagInt(k3, v3);
            repo.SetFlagInt(k4, v4);

            string k5 = "c";
            double v5 = 5;
            string k6 = "a";
            double v6 = 3;
            repo.SetFlagDouble(k5, v5);
            repo.SetFlagDouble(k6, v6);

            bool result;
            result = repo.TryGetFlagStr(k1, out string str);
            Assert.True(result);
            Assert.Equal(v1, str);
            result = repo.TryGetFlagStr(k2, out str);
            Assert.True(result);
            Assert.Equal(v2, str);
            result = repo.TryGetFlagStr(k4, out _);
            Assert.False(result);

            result = repo.TryGetFlagInt(k3, out int i);
            Assert.True(result);
            Assert.Equal(v3, i);
            result = repo.TryGetFlagInt(k4, out i);
            Assert.True(result);
            Assert.Equal(v4, i);
            result = repo.TryGetFlagInt(k1, out _);
            Assert.False(result);

            result = repo.TryGetFlagDouble(k5, out double d);
            Assert.True(result);
            Assert.Equal(v5, d);
            result = repo.TryGetFlagDouble(k6, out d);
            Assert.True(result);
            Assert.Equal(v6, d);
            result = repo.TryGetFlagDouble(k3, out _);
            Assert.False(result);
        }

        [Fact()]
        public void RemoveFlagTest()
        {
            FlagRepository repo = new();

            string k1 = "a";
            string v1 = "v1";
            string k2 = "b";
            string v2 = "v1";
            repo.SetFlagStr(k1, v1);
            repo.SetFlagStr(k2, v2);

            string k3 = "b";
            int v3 = 2;
            string k4 = "c";
            int v4 = 5;
            repo.SetFlagInt(k3, v3);
            repo.SetFlagInt(k4, v4);

            string k5 = "c";
            double v5 = 5;
            string k6 = "a";
            double v6 = 3;
            repo.SetFlagDouble(k5, v5);
            repo.SetFlagDouble(k6, v6);

            repo.RemoveFlagStr(k1);
            Assert.False(repo.HasFlagStr(k1));
            Assert.True(repo.HasFlagStr(k2));

            repo.RemoveFlagInt(k4);
            Assert.False(repo.HasFlagInt(k4));
            Assert.True(repo.HasFlagInt(k3));

            repo.RemoveFlagDouble(k5);
            Assert.False(repo.HasFlagDouble(k5));
            Assert.True(repo.HasFlagDouble(k6));
        }
    }
}