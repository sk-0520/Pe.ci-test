using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class ICommandExtensionsTest
    {
        #region define

        private class Command: ICommand
        {
            #region variable

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
        public void ExecuteIfCanExecute_can_Test()
        {
            var test = new Command(true);
            Assert.True(test.ExecuteIfCanExecute(default));
            Assert.True(test.Called);
        }

        [Fact]
        public void ExecuteIfCanExecute_canNot_Test()
        {
            var test = new Command(false);
            Assert.False(test.ExecuteIfCanExecute(default));
            Assert.False(test.Called);
        }

        #endregion
    }
}
