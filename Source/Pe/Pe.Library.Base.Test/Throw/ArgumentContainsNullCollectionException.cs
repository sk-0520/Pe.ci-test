using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base.Throw;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test.Throw
{
    public class ArgumentContainsNullCollectionExceptionTest
    {
        #region function

        [Fact]
        public void Constructor_string_Test()
        {
            var actual = new ArgumentContainsNullCollectionException("param");
            Assert.Equal("param", actual.ParamName);
            Assert.Equal("sequence contains null (Parameter 'param')", actual.Message);
        }

        [Fact]
        public void Constructor_string_string_Test()
        {
            var actual = new ArgumentContainsNullCollectionException("param", "message");
            Assert.Equal("param", actual.ParamName);
            Assert.Equal("message (Parameter 'param')", actual.Message);
        }

        [Fact]
        public void ThrowIfContainsNullTest()
        {
            Assert.Throws<ArgumentContainsNullCollectionException>(() => ArgumentContainsNullCollectionException.ThrowIfContainsNull(new[] { new object(), null, new object() }, "param"));
        }

        #endregion
    }
}
