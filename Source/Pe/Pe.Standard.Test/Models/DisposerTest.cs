using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Test.Models
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
