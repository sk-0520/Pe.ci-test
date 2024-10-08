using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection.Test
{
    public class DiDefaultParameterTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var actual = new DiDefaultParameter(typeof(DiDefaultParameterTest));
            Assert.Equal(typeof(DiDefaultParameterTest), actual.Type);
        }

        [Fact]
        public void GetPair_Value_Test()
        {
            var test = new DiDefaultParameter(typeof(int));
            var actual = test.GetPair();
            Assert.Equal(typeof(int), actual.Key);
            Assert.Equal(0, actual.Value);
        }

        [Fact]
        public void GetPair_Object_Test()
        {
            var test = new DiDefaultParameter(typeof(DiDefaultParameterTest));
            var actual = test.GetPair();
            Assert.Equal(typeof(DiDefaultParameterTest), actual.Key);
            Assert.Null(actual.Value);
        }

        [Fact]
        public void Create_Value_Test()
        {
            var test = DiDefaultParameter.Create<int>();
            var actual = test.GetPair();
            Assert.Equal(typeof(int), actual.Key);
            Assert.Equal(0, actual.Value);
        }

        [Fact]
        public void Create_Object_Test()
        {
            var test = DiDefaultParameter.Create<DiDefaultParameterTest>();
            var actual = test.GetPair();
            Assert.Equal(typeof(DiDefaultParameterTest), actual.Key);
            Assert.Null(actual.Value);
        }

        #endregion
    }
}
