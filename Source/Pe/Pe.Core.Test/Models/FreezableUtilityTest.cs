using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class FreezableUtilityTest
    {
        #region define

        private class TestClass: Freezable
        {
            #region variable

            private bool _canFreeze;

            #endregion

            public TestClass(bool canFreeze)
            {
                this._canFreeze = canFreeze;
            }

            #region Freezable

            public new bool CanFreeze => this._canFreeze;

            protected override Freezable CreateInstanceCore()
            {
                return this;
            }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void SafeFreezeTest()
        {
            var test = new TestClass(true);
            var actual = FreezableUtility.SafeFreeze(test);
            Assert.True(actual);
        }

        [Fact]
        public void SafeFreeze_null_Test()
        {
            TestClass? test = null;
            var actual = FreezableUtility.SafeFreeze(test);
            Assert.False(actual);
        }

        [Fact]
        public void GetSafeFreezeTest()
        {
            var test = new TestClass(true);
            var actual = FreezableUtility.GetSafeFreeze(test);
            Assert.Equal(test, actual);
        }

        [Fact]
        public void GetSafeFreeze_null_Test()
        {
            TestClass? test = null;
            var actual = FreezableUtility.GetSafeFreeze(test);
            Assert.Null(actual);
        }

        #endregion
    }
}
