using System;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class PropertyChangedHookerTest
    {
        [TestMethod]
        public void AddHook_1_Test()
        {
            var pch = new PropertyChangedHooker(TestLogger.Create(GetType()));
            Assert.ThrowsException<ArgumentNullException>(() => pch.AddHook(default(HookItem)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(new HookItem(null,null,null,null)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(new HookItem("",null,null,null)));
        }

        [TestMethod]
        public void AddHook_2_Test()
        {
            var pch = new PropertyChangedHooker(TestLogger.Create(GetType()));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(default(string)));
            Assert.ThrowsException<ArgumentException>(() => pch.AddHook(""));
            var result = pch.AddHook("A");
            Assert.AreEqual("A", result.NotifyPropertyName);
            Assert.AreEqual("A", result.RaisePropertyNames[0]);
        }
    }
}
