using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class PropertyChangedHookerTest
    {
        [TestMethod]
        public void AddHook_1_Test()
        {
            var pch = new PropertyChangedHooker(new CurrentDispatcherWapper(), NullLogger.Instance);
            Assert.ThrowsException<ArgumentNullException>(() => pch.AddHook(default(HookItem)!));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(new HookItem(null!, null, null, null)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(new HookItem("", null, null, null)));
        }

        [TestMethod]
        public void AddHook_2_Test()
        {
            var pch = new PropertyChangedHooker(new CurrentDispatcherWapper(), NullLogger.Instance);
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(default(string)!));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(""));
            var result = pch.AddHook("A");
            Assert.AreEqual("A", result.NotifyPropertyName);
            Assert.AreEqual("A", result.RaisePropertyNames![0]);
        }

        [TestMethod]
        public void AddHook_3_Test()
        {
            var pch = new PropertyChangedHooker(new CurrentDispatcherWapper(), NullLogger.Instance);
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(default(string)!, default(string)!));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook("A", default(string)!));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(default(string)!, "B"));
            var result = pch.AddHook("a", "b");
            Assert.AreEqual("a", result.NotifyPropertyName);
            Assert.AreEqual("b", result.RaisePropertyNames![0]);
        }
    }
}
