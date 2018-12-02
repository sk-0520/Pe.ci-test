using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class EnumUtilityTest
    {
        [TestMethod]
        [DataRow(IconScale.Small, IconScale.Small, IconScale.Small)]
        [DataRow(IconScale.Small, IconScale.Small, IconScale.Normal)]
        [DataRow(IconScale.Small, IconScale.Small, IconScale.Big)]
        [DataRow(IconScale.Small, IconScale.Small, IconScale.Large)]
        [DataRow(IconScale.Small, 16, IconScale.Large)]
        [DataRow(IconScale.Large, 17, IconScale.Large)]
        [DataRow(IconScale.Normal, 32, IconScale.Large)]
        public void GetNormalization_IconScale_Test(IconScale result, object test, IconScale def)
        {
            var r = EnumUtility.GetNormalization(test, def);
            Assert.AreEqual(r, result);
        }
    }
}
