using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class InitializerTest
    {
        #region define

        private class X: ISupportInitialize
        {
            #region property

            public bool IsInitializing { get; private set; }

            #endregion

            #region ISupportInitialize

            public void BeginInit()
            {
                IsInitializing = true;
            }

            public void EndInit()
            {
                IsInitializing = false;
            }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void BeginTest()
        {
            var test = new X();
            Assert.False(test.IsInitializing);
            using(var init = Initializer.Begin(test)) {
                Assert.Equal(test, init.Target);
                Assert.True(test.IsInitializing);
            }
            Assert.False(test.IsInitializing);
        }

        #endregion
    }
}
