using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Test;
using Xunit;

namespace Pe.CommonTest.Test
{
    public class PrivateObjectTest
    {
        #region define

        private sealed class Class
        {
            #region variable

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:読み取り専用修飾子を追加します", Justification = "<保留中>")]
            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
#pragma warning disable 0169
            private int Field;
#pragma warning restore 0169

            #endregion

            #region proeprty

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private int Property { get; set; }

            #endregion

            #region function

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private void Method()
            {
                //nop
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:使用されていないプライベート メンバーを削除する", Justification = "<保留中>")]
            private int Method(int a)
            {
                return a + a;
            }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void FieldTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Throws<TestPrivateObjectFieldException>(() => test.SetField("<Field>", null));
            Assert.Throws<TestPrivateObjectFieldException>(() => test.GetField("<Field>"));

            Assert.Equal(0, test.GetField("Field"));
            test.SetField("Field", 123);
            Assert.Equal(123, test.GetField("Field"));
        }

        [Fact]
        public void PropertyTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Throws<TestPrivateObjectPropertyException>(() => test.SetProperty("<Property>", null));
            Assert.Throws<TestPrivateObjectPropertyException>(() => test.GetProperty("<Property>"));

            Assert.Equal(0, test.GetProperty("Property"));
            test.SetProperty("Property", 123);
            Assert.Equal(123, test.GetProperty("Property"));
        }

        [Fact]
        public void MethodTest()
        {
            var obj = new Class();
            var test = new PrivateObject(obj);

            Assert.Null(test.Invoke("Method"));
            Assert.Equal(2, test.Invoke("Method", 1));
            Assert.Equal(2, test.Invoke("Method", 1));

            Assert.Throws<TestPrivateObjectMethodException>(() => test.Invoke("<Method>"));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (short)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (long)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (float)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (double)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", (decimal)1));
            Assert.Throws<TestPrivateObjectMethodParametersException>(() => test.Invoke("Method", "A"));
        }

        #endregion
    }
}
