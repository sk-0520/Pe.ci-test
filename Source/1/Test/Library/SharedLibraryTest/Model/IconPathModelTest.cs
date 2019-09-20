using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Model
{
    [TestFixture]
    class IconPathModelTest
    {
        [TestCase("", 0)]
        [TestCase("a", 0)]
        [TestCase("", 1)]
        [TestCase("b", 2)]
        public void DeepCloneTest(string path, int index)
        {
            var src = new IconPathModel() {
                Path = path,
                Index = index,
            };

            var dst = (IconPathModel)src.DeepClone();
            Assert.AreEqual(src.Path, dst.Path);
            Assert.AreEqual(src.Index, dst.Index);
        }
    }
}
