using System;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Applications
{
    public class StatementAccessorParameterTest
    {
        #region function

        [Fact]
        public void ConstructorIllegalTest()
        {
            Assert.Throws<ArgumentNullException>(() => new StatementAccessorParameter(null!));
            Assert.Throws<ArgumentException>(() => new StatementAccessorParameter(""));
            Assert.Throws<ArgumentException>(() => new StatementAccessorParameter("a"));
            Assert.Throws<ArgumentException>(() => new StatementAccessorParameter("a."));
            Assert.Throws<ArgumentException>(() => new StatementAccessorParameter(".a"));
            Assert.Throws<ArgumentException>(() => new StatementAccessorParameter(".a."));
            new StatementAccessorParameter("a.b");
        }

        [Theory]
        [InlineData("", "class", "method", "class.method")]
        [InlineData("namespace", "class", "method", "namespace.class.method")]
        [InlineData("namespace.A", "class", "method", "namespace.A.class.method")]
        [InlineData("namespace.A.B", "class", "method", "namespace.A.B.class.method")]
        public void ConstructorTest(string nameSpace, string className, string methodName, string input)
        {
            var actual = new StatementAccessorParameter(input);
            Assert.Equal(nameSpace, actual.Namespace);
            Assert.Equal(className, actual.ClassName);
            Assert.Equal(methodName, actual.MethodName);
        }


        #endregion
    }
}
