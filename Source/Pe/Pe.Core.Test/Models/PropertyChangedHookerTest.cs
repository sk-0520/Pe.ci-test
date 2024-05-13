using System;
using System.Collections.Generic;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class HookItemTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            Action action = new Action(() => { });
            IReadOnlyHookItem actual = new HookItem(
                "notify",
                ["A", "B", "C"],
                [
                    ApplicationCommands.New,
                    ApplicationCommands.Undo,
                    ApplicationCommands.Cut,
                ],
                action
            );

            Assert.Equal("notify", actual.NotifyPropertyName);
            Assert.Equal(["A", "B", "C"], actual.RaisePropertyNames);
            Assert.Equal([
                ApplicationCommands.New,
                ApplicationCommands.Undo,
                ApplicationCommands.Cut,
            ], actual.RaiseCommands);
            Assert.Equal(action, actual.Callback);
        }

        #endregion
    }

    public class CachedHookItemTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            DelegateCommand dgCommand1 = new DelegateCommand(() => { });
            DelegateCommand dgCommand2 = new DelegateCommand(() => { });

            Action action1 = new Action(() => { });
            Action action2 = new Action(() => { });

            var actual = new CachedHookItem(
                ["raisePropertyName1", "raisePropertyName2"],
                [ApplicationCommands.New, ApplicationCommands.Undo],
                [dgCommand1, dgCommand2],
                [action1, action2]
            );

            Assert.Equal(["raisePropertyName1", "raisePropertyName2"], actual.RaisePropertyNames);
            Assert.Equal([ApplicationCommands.New, ApplicationCommands.Undo], actual.RaiseCommands);
            Assert.Equal([dgCommand1, dgCommand2], actual.RaiseDelegateCommands);
            Assert.Equal([action1, action2], actual.Callbacks);
        }

        #endregion
    }

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
