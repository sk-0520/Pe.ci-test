/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// コマンド。
    /// </summary>
    public class DelegateCommand: DisposeFinalizeBase, ICommand
    {
        public DelegateCommand(Action<object> executeCommand)
        {
            if(executeCommand == null) {
                throw new ArgumentNullException(nameof(executeCommand));
            }
            ExecuteCommand = executeCommand;
        }

        public DelegateCommand(Action<object> command, Func<object, bool> canExecuteCommand)
            : this(command)
        {
            if(canExecuteCommand == null) {
                throw new ArgumentNullException(nameof(canExecuteCommand));
            }

            CanExecuteCommand = canExecuteCommand;
        }

        #region property

        /// <summary>
        /// コマンド。
        /// </summary>
        public Action<object> ExecuteCommand { get; private set; }
        /// <summary>
        /// 実行可否。
        /// </summary>
        public Func<object, bool> CanExecuteCommand { get; private set; }

        protected IList<WeakReference> CanExecuteChangedList { get; } = new List<WeakReference>();

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                CanExecuteCommand = null;
                ExecuteCommand = null;
                foreach(var wr in CanExecuteChangedList.ToArray()) {
                    CommandManager.RequerySuggested -= (EventHandler)wr.Target;
                }
                CanExecuteChangedList.Clear();
            }

            base.Dispose(disposing);
        }

        #endregion

        #region ICommand

        public void Execute(object parameter)
        {
            ExecuteCommand(parameter);
        }

        public bool CanExecute(object parameter)
        {
            if(CanExecuteCommand != null) {
                return CanExecuteCommand(parameter);
            } else {
                return true;
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedList.Add(new WeakReference(value));
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                var targets = CanExecuteChangedList.Where(i => (EventHandler)i.Target == value).ToArray();
                foreach(var target in targets) {
                    CanExecuteChangedList.Remove(target);
                }
            }
        }

        #endregion
    }


}
