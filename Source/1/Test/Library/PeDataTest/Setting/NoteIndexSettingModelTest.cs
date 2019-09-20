using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using NUnit.Framework;

namespace ContentTypeTextNet.Test.Library.PeDataTest.Setting
{
    [TestFixture]
    class NoteIndexSettingModelTest
    {
        [Test]
        public void DeepCloneTest()
        {
            var src = new NoteIndexSettingModel() {
            };

            var dst = (NoteIndexSettingModel)src.DeepClone();

            Assert.IsTrue(src.Items != dst.Items);
        }
    }
}
