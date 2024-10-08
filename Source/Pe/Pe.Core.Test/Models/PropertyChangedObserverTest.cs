using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using ContentTypeTextNet.Pe.Library.Base.Throw;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class ObserverItemTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            Action action = new Action(() => { });
            IReadOnlyObserveItem actual = new ObserveItem(
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

    public class CachedObserverItemTest
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            DelegateCommand dgCommand1 = new DelegateCommand(() => { });
            DelegateCommand dgCommand2 = new DelegateCommand(() => { });

            Action action1 = new Action(() => { });
            Action action2 = new Action(() => { });

            var actual = new CachedObserveItem(
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

    public class PropertyChangedObserverTest
    {
        #region define

        private class Command: ICommand
        {
            #region variable

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2933:Fields that are only assigned in the constructor should be \"readonly\"", Justification = "<保留中>")]
            private bool _canExecute;

            #endregion

            public Command(bool canExecute)
            {
                this._canExecute = canExecute;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }

            #region property

            public bool Called { get; private set; } = false;

            #endregion

            #region ICommand

            public event EventHandler? CanExecuteChanged;

            public bool CanExecute(object? parameter)
            {
                return this._canExecute;
            }

            public void Execute(object? parameter)
            {
                Called = true;
            }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void AddObserver_HookItem_Test()
        {
            var test = new PropertyChangedObserver(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(ObserveItem)!));
            Assert.Throws<ArgumentException>(() => test.AddObserver(new ObserveItem(null!, null, null, null)));
            Assert.Throws<ArgumentException>(() => test.AddObserver(new ObserveItem("", null, null, null)));
        }

        [Fact]
        public void AddObserver_string_Test()
        {
            var test = new PropertyChangedObserver(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!));
            Assert.Throws<ArgumentException>(() => test.AddObserver(""));
            var result = test.AddObserver("A");
            Assert.Equal("A", result.NotifyPropertyName);
            Assert.NotNull(result.RaisePropertyNames);
            Assert.Single(result.RaisePropertyNames);
            Assert.Contains("A", result.RaisePropertyNames);
        }

        [Fact]
        public void AddObserver_string_string_Test()
        {
            var test = new PropertyChangedObserver(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, default(string)!));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver("A", default(string)!));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, "B"));
            var result = test.AddObserver("a", "b");
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.NotNull(result.RaisePropertyNames);
            Assert.Single(result.RaisePropertyNames);
            Assert.Contains("b", result.RaisePropertyNames);
        }

        [Fact]
        public void AddObserver_string_IEnumerableXstringX_Test()
        {
            var test = new PropertyChangedObserver(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, Array.Empty<string>()));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver("A", (IEnumerable<string>)null!));
            Assert.Throws<ArgumentEmptyCollectionException>(() => test.AddObserver("A", Array.Empty<string>()));

            var result = test.AddObserver("a", new[] { "A", "B" });
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.NotNull(result.RaisePropertyNames);
            Assert.Equal(2, result.RaisePropertyNames.Count);
            Assert.Contains("A", result.RaisePropertyNames);
            Assert.Contains("B", result.RaisePropertyNames);
        }

        [Fact]
        public void AddObserver_string_ICommand_Test()
        {
            var test = new PropertyChangedObserver(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, default(ICommand)!));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, new Command(true)));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver("A", default(ICommand)!));

            var command = new Command(true);
            var result = test.AddObserver("a", command);
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.Null(result.RaisePropertyNames);
            Assert.NotNull(result.RaiseCommands);
            Assert.Single(result.RaiseCommands);
            Assert.Contains(command, result.RaiseCommands);
        }

        [Fact]
        public void AddObserver_string_IEnumerableXICommandX_Test()
        {
            var test = new PropertyChangedObserver(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, default(ICommand[])!));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, new [] { new Command(true)}));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver("A", default(ICommand[])!));
            Assert.Throws<ArgumentEmptyCollectionException>(() => test.AddObserver("A", Array.Empty<ICommand>()));

            var command1 = new Command(true);
            var command2 = new Command(true);
            var result = test.AddObserver("a", new [] { command1 , command2});
            Assert.Equal("a", result.NotifyPropertyName);
            Assert.Null(result.RaisePropertyNames);
            Assert.NotNull(result.RaiseCommands);
            Assert.Equal(2, result.RaiseCommands.Count);
            Assert.Contains(command1, result.RaiseCommands);
            Assert.Contains(command2, result.RaiseCommands);
        }

        [Fact]
        public void AddObserver_string_Action_Test()
        {
            var test = new PropertyChangedObserver(new CurrentDispatcherWrapper(), NullLogger.Instance);
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, default(Action)!));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver(default(string)!, () => { }));
            Assert.Throws<ArgumentNullException>(() => test.AddObserver("A", default(Action)!));

            var action = () => { };
            var result = test.AddObserver("a", action);

            Assert.Equal("a", result.NotifyPropertyName);
            Assert.Null(result.RaisePropertyNames);
            Assert.Null(result.RaiseCommands);
            Assert.NotNull(result.Callback);
            Assert.Equal(action, result.Callback);
        }

        #endregion
    }
}
