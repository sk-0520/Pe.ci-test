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
            using(var a = new ApplicationDatabaseAccessor(new ApplicationDatabaseConnectionFactory(), new NullLogger())) {
                Assert.IsTrue(true);
            }
            Assert.IsTrue(true);
        }
    }
}
