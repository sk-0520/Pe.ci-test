using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Library.Shared.Embedded.Test.Model
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
