using System;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    /// <inheritdoc cref="ISkeletonImplements"/>
    internal class SkeletonImplements: ISkeletonImplements
    {
        public SkeletonImplements()
        {
        }

        #region property
        #endregion

        #region ISkeletonImplements

        public SkeletonImplements Clone()
        {
            return new SkeletonImplements();
        }
        ISkeletonImplements ISkeletonImplements.Clone() => Clone();

        /// <inheritdoc cref="ISkeletonImplements.CreateCommand(Action)"/>
        public ICommand CreateCommand(Action execute)
        {
            return new DelegateCommand(execute);
        }

        /// <inheritdoc cref="ISkeletonImplements.CreateCommand{TParameter}(Action{TParameter})"/>
        public ICommand CreateCommand<TParameter>(Action<TParameter> execute)
        {
            return new DelegateCommand<TParameter>(execute);
        }

        /// <inheritdoc cref="ISkeletonImplements.CreateCommand(Action, Func{bool})"/>
        public ICommand CreateCommand(Action execute, Func<bool> canExecute)
        {
            return new DelegateCommand(execute, canExecute);
        }

        /// <inheritdoc cref="ISkeletonImplements.CreateCommand{TParameter}(Action{TParameter}, Func{TParameter, bool})"/>
        public ICommand CreateCommand<TParameter>(Action<TParameter> execute, Func<TParameter, bool> canExecute)
        {
            return new DelegateCommand<TParameter>(execute, canExecute);
        }

        #endregion
    }
}
