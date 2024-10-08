using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class EnumUtilityTest
    {
        #region define

        enum E0
        { }

        enum E1
        {
            A,
        }

        enum E2
        {
            A,
            B,
        }

        enum E3
        {
            A,
            B,
            C,
        }

        #endregion

        #region function

        [Fact]
        public void NormalizeTest()
        {
            Assert.Equal(E2.A, EnumUtility.Normalize("A", E2.A));
            Assert.Equal(E2.B, EnumUtility.Normalize("B", E2.A));
            Assert.Equal(E2.A, EnumUtility.Normalize("C", E2.A));
            Assert.Equal(E2.B, EnumUtility.Normalize("C", E2.B));
            Assert.Equal(E2.A, EnumUtility.Normalize("💩", E2.A));
            Assert.Equal(E2.B, EnumUtility.Normalize("💩", E2.B));
        }

        #endregion
    }

}
