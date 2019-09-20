using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pe.Test.Model.Database
{
    [TestClass]
    public class ApplicationDatabaseAccessorTest
    {
        [TestMethod]
        public void ConstructorTest()
        {
            using(var a = new ApplicationDatabaseAccessor(new ApplicationDatabaseFactory(), new TestLogger())) {
                Assert.IsTrue(true);
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void BatchTest()
        {
            using(var a = new ApplicationDatabaseAccessor(new ApplicationDatabaseFactory(), new TestLogger())) {
                var batchResult1 = a.Batch(c => {
                    c.Execute("create table TEST ( num integer, str text )");
                    c.Execute("insert into TEST(num, str) values (@Num, @Str)", new { Num = 1, Str = "a" });
                    c.Execute("insert into TEST(num, str) values (@Num, @Str)", new { Num = 2, Str = "b" });
                    c.Execute("insert into TEST(num, str) values (@Num, @Str)", new { Num = 3, Str = "c" });
                    return false;
                });
                Assert.IsTrue(batchResult1.Success, batchResult1.FailureValue?.ToString());

                var batchResult2 = a.Batch(c => {
                    c.Execute("create table TEST ( num integer, str text )");
                    c.Execute("insert into TEST(num, str) values (@Num, @Str)", new { Num = 1, Str = "a" });
                    c.Execute("insert into TEST(num, str) values (@Num, @Str)", new { Num = 2, Str = "b" });
                    c.Execute("insert into TEST(num, str) values (@Num, @Str)", new { Num = 3, Str = "c" });
                    return true;
                });
                Assert.IsTrue(batchResult2.Success, batchResult2.FailureValue?.ToString());

                var maxNum = a.Query<int>("select MAX(num) from TEST").First();
                Assert.AreNotEqual(maxNum, 1);
                Assert.AreEqual(maxNum, 3);

                var num2 = a.Query<string>("select str from TEST where TEST.num = @Num", new { Num = 2 }).First();
                Assert.AreNotEqual(num2, "a");
                Assert.AreNotEqual(num2, "c");
                Assert.AreEqual(num2, "b");
            }
        }

    }
}
