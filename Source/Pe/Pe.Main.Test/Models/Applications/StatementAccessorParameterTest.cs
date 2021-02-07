using System;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Applications
{
    [TestClass]
    public class StatementAccessorParameterTest
    {
        #region function

        [TestMethod]
        public void ConstructorIllegalTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new StatementAccessorParameter(null!));
            Assert.ThrowsException<ArgumentException>(() => new StatementAccessorParameter(""));
            Assert.ThrowsException<ArgumentException>(() => new StatementAccessorParameter("a"));
            Assert.ThrowsException<ArgumentException>(() => new StatementAccessorParameter("a."));
            Assert.ThrowsException<ArgumentException>(() => new StatementAccessorParameter(".a"));
            Assert.ThrowsException<ArgumentException>(() => new StatementAccessorParameter(".a."));
            new StatementAccessorParameter("a.b");
        }

        [TestMethod]
        [DataRow("", "class", "method", "class.method")]
        [DataRow("namespace", "class", "method", "namespace.class.method")]
        [DataRow("namespace.A", "class", "method", "namespace.A.class.method")]
        [DataRow("namespace.A.B", "class", "method", "namespace.A.B.class.method")]
        public void ConstructorTest(string nameSpace, string className, string methodName, string input)
        {
            var actual = new StatementAccessorParameter(input);
            Assert.AreEqual(nameSpace, actual.Namespace);
            Assert.AreEqual(className, actual.ClassName);
            Assert.AreEqual(methodName, actual.MethodName);
        }


        #endregion
    }
}
