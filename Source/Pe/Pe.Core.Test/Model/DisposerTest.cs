using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Model
{
    [TestClass]
    public class ActionDisposerTest
    {
        [TestMethod]
        public void UsingTest()
        {
            using(var dispoer = new ActionDisposer(disposing => {
                Assert.IsTrue(disposing);
            })) {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void FinalizeTest()
        {
            var dispoer = new ActionDisposer(disposing => {
                Assert.IsFalse(disposing);
            });
        }
    }
}
