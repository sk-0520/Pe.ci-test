using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class FileRotatorTest
    {
        #region function

        [TestMethod]
        public void ExecuteRegex_0()
        {
            var dir = new DirectoryInfo("nul");
            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteRegex(dir, new Regex("."), 10, ex => true);
            Assert.AreEqual(-1, actual);
        }

        [TestMethod]
        public void ExecuteRegex()
        {
            var dir = TestIO.InitializeMethod(this);
            TestIO.CreateEmptyFile(dir, "target_1.dmy");
            TestIO.CreateEmptyFile(dir, "target_2.dmy");
            TestIO.CreateEmptyFile(dir, "target_3.dmy");
            TestIO.CreateEmptyFile(dir, "target_4.dmy");
            TestIO.CreateEmptyFile(dir, "target_5.dmy");
            
            var fileRotator = new FileRotator();
            var actual = fileRotator.ExecuteRegex(dir, new Regex("^target"), 3, ex => true);
            Assert.AreEqual(2, actual);
        }

        #endregion
    }
}
