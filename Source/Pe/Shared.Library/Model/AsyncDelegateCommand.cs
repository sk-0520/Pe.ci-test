using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public interface IAsyncCommand
    {
        #region property

        bool IsExecuting { get; }
        bool SingleExecute { get; set; }

        #endregion
    }

    public class AsyncDelegateCommand : DelegateCommand, IAsyncCommand
    {
        public AsyncDelegateCommand(Action executeMethod)
            : base(executeMethod)
        { }

        public AsyncDelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        { }

        #region DelegateCommand

        protected override bool CanExecute(object parameter)
        {
            if(SingleExecute) {
                if(IsExecuting) {
                    return false;
                }
            }
            return base.CanExecute(parameter);
        }

        protected override void Execute(object parameter)
        {
            try {
                IsExecuting = true;
                base.Execute(parameter);
            } finally {
                IsExecuting = false;
            }
        }

        #endregion

        #region IAsyncCommand

        public bool IsExecuting { get; private set; }
        public bool SingleExecute { get; set; }
        #endregion
    }

    public class AsyncDelegateCommand<T> : DelegateCommand<T>
    {
        public AsyncDelegateCommand(Action<T> executeMethod)
            : base(executeMethod)
        { }

        public AsyncDelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        { }

        #region DelegateCommand

        protected override bool CanExecute(object parameter)
        {
            if(SingleExecute) {
                if(IsExecuting) {
                    return false;
                }
            }
            return base.CanExecute(parameter);
        }

        protected override void Execute(object parameter)
        {
            try {
                IsExecuting = true;
                base.Execute(parameter);
            } finally {
                IsExecuting = false;
            }
        }

        #endregion

        #region IAsyncCommand

        public bool IsExecuting { get; private set; }
        public bool SingleExecute { get; set; }

        #endregion
    }
}
