using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Unmanaged
{
    public class GlobalAllocTest
    {
        #region define

        private struct TestStruct
        {
#pragma warning disable CS0649
            public int Int;
            public byte Byte;
            public double Double;
#pragma warning restore CS0649
        }

        #endregion

        #region function

        [Fact]
        public void Test()
        {
            var test = new GlobalAlloc(16);
            Assert.Equal(16, test.Size);
            Assert.False(test.IsInvalid);

            var input = Enumerable.Range(0, 16).Select(a => (byte)a).ToArray();
            Marshal.Copy(input, 0, test.Heap, input.Length);
            var output = new byte[input.Length];
            Marshal.Copy(test.Heap, output, 0, input.Length);
            Assert.Equal(input, output);

            test.Dispose();
            Assert.Equal(16, test.Size);
            Assert.True(test.IsInvalid);
        }

        [Fact]
        public void Test_empty_Create()
        {
            using var test = GlobalAlloc.Create<TestStruct>();
            Assert.Equal(test.Size, Marshal.SizeOf<TestStruct>());
        }

        [Fact]
        public void Test_struct_Create()
        {
            var input = new TestStruct() {
                Byte = 64,
                Int = 123456,
                Double = 10.5
            };
            using var test = GlobalAlloc.Create(input);
            Assert.Equal(test.Size, Marshal.SizeOf(input));

            var actual = Marshal.PtrToStructure<TestStruct>(test.Heap);
            Assert.Equal(input.Byte, actual.Byte);
            Assert.Equal(input.Int, actual.Int);
            Assert.Equal(input.Double, actual.Double);
        }
        #endregion
    }
}
