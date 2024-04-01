using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class ActionDisposerTest
    {
        [Fact]
        public void UsingTest()
        {
            using(var disposer = new ActionDisposer(disposing => {
                Assert.True(disposing);
            })) {
                Assert.True(true);
            }
        }

        [Fact]
        public void FinalizeTest()
        {
            var disposer = new ActionDisposer(disposing => {
                Assert.False(disposing);
            });
        }
    }

}
