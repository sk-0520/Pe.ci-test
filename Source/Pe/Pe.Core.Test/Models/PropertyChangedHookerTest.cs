using System;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class PropertyChangedHookerTest
    {
        [Fact]
        public void AddHook_1_Test()
        {
            var pch = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => pch.AddHook(default(HookItem)!));
            Assert.Throws<ArgumentException>(() => pch.AddHook(new HookItem(null!, null, null, null)));
            Assert.Throws<ArgumentException>(() => pch.AddHook(new HookItem("", null, null, null)));
        }

        [Fact]
        public void AddHook_2_Test()
        {
            var pch = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentException>(() => pch.AddHook(default(string)!));
            Assert.Throws<ArgumentException>(() => pch.AddHook(""));
            var result = pch.AddHook("A");
            Assert.Equal("A", result.NotifyPropertyName);
            Assert.Equal("A", result.RaisePropertyNames![0]);
        }

        [Fact]
        public void AddHook_3_Test()
        {
            var pch = new PropertyChangedHooker(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentException>(() => pch.AddHook(default(string)!, default(string)!));
            Assert.Throws<ArgumentException>(() => pch.AddHook("A", default(string)!));
            Assert.Throws<ArgumentException>(() => pch.AddHook(default(string)!, "B"));
            var result = pch.AddHook("a", "b");
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.Equal("b", result.RaisePropertyNames![0]);
        }
    }
}
