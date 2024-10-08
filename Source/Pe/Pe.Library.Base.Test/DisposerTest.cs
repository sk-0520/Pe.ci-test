using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
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

    public class DisposableStockerTest
    {
        #region function

        [Fact]
        public void Test()
        {
            var stocks = new List<int>();

            var test = new DisposableStocker();
            test.Add(ActionDisposerHelper.Create(a => stocks.Add(10)));
            test.AddRange(new[] {
                ActionDisposerHelper.Create(a => stocks.Add(20)),
                ActionDisposerHelper.Create(a => stocks.Add(30)),
            });

            Assert.Empty(stocks);

            test.Dispose();

            Assert.Equal(3, stocks.Count);
            Assert.Equal(30, stocks[0]);
            Assert.Equal(20, stocks[1]);
            Assert.Equal(10, stocks[2]);

            test.Dispose();
        }

        [Fact]
        public void Add_throw_Test()
        {
            var test = new DisposableStocker();
            Assert.Throws<ArgumentNullException>(() => test.Add(default(IDisposable)!));
        }

        [Fact]
        public void AddRange_throw_Test()
        {
            var test = new DisposableStocker();
            Assert.Throws<ArgumentNullException>(() => test.AddRange(default(IDisposable[])!));
        }

        #endregion
    }

    public class ArrayPoolValueTest
    {
        #region function

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(64)]
        [InlineData(128)]
        [InlineData(256)]
        [InlineData(512)]
        [InlineData(1024)]
        [InlineData(2048)]
        [InlineData(4096)]
        [InlineData(8192)]
        [InlineData(16384)]
        [InlineData(32768)]
        [InlineData(65536)]
        [InlineData(131072)]
        [InlineData(262144)]
        [InlineData(524288)]
        [InlineData(1048576)]
        [InlineData(2097152)]
        [InlineData(4194304)]
        [InlineData(8388608)]
        [InlineData(16777216)]
        [InlineData(33554432)]
        public void ConstructorTest(int size)
        {
            using var array = new ArrayPoolValue<byte>(size);
            Assert.True(size <= array.Items.Length);
            Assert.Equal(size, array.Length);
        }

        [Fact]
        public void ConstructorPoolTest()
        {
            var ap = ArrayPool<byte>.Create();
            using var array = new ArrayPoolValue<byte>(128, ap);
            Assert.True(128 <= array.Items.Length);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(127, 255)]
        public void Index(int index, byte input)
        {
            using var array = new ArrayPoolValue<byte>(128);
            array.Items[index] = input;
            Assert.Equal(input, array.Items[index]);
            Assert.Equal(input, array[index]);
        }

        #endregion
    }

    public class ArrayPoolObjectTest
    {
        #region function

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(32)]
        [InlineData(64)]
        [InlineData(128)]
        [InlineData(256)]
        [InlineData(512)]
        [InlineData(1024)]
        [InlineData(2048)]
        [InlineData(4096)]
        [InlineData(8192)]
        [InlineData(16384)]
        [InlineData(32768)]
        [InlineData(65536)]
        [InlineData(131072)]
        [InlineData(262144)]
        [InlineData(524288)]
        [InlineData(1048576)]
        [InlineData(2097152)]
        [InlineData(4194304)]
        [InlineData(8388608)]
        [InlineData(16777216)]
        [InlineData(33554432)]
        public void ConstructorTest(int size)
        {
            using var array = new ArrayPoolObject<byte>(size);
            Assert.True(size <= array.Items.Length);
            Assert.Equal(size, array.Length);
        }

        [Fact]
        public void ConstructorPoolTest()
        {
            var ap = ArrayPool<byte>.Create();
            using var array = new ArrayPoolObject<byte>(128, ap);
            Assert.True(128 <= array.Items.Length);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(127, 255)]
        public void Index(int index, byte input)
        {
            using var array = new ArrayPoolObject<byte>(128);
            array.Items[index] = input;
            Assert.Equal(input, array.Items[index]);
            Assert.Equal(input, array[index]);
        }

        #endregion
    }
}
