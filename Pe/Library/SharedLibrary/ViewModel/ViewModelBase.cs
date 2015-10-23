/**
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
namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

    public abstract class ViewModelBase: NotifyPropertyChangedObject, IDisplayText
    {
        #region variable

        Caching<string, DelegateCommand> _createdCommands = new Caching<string, DelegateCommand>();

        bool _isChanged = false;

        #endregion

        public ViewModelBase()
            : base()
        {
            ResetChangeFlag();

            //WeakPropertyChangedEventLisner = new WeakEventListener<PropertyChangedEventManager, PropertyChangedEventArgs>(CallOnPropertyChangedEvent);
        }

        #region property

        //protected WeakEventListener<PropertyChangedEventManager, PropertyChangedEventArgs> WeakPropertyChangedEventLisner { get; private set; }

        public bool IsChanged
        {
            get { return this._isChanged; }
            private set { SetVariableValue(ref this._isChanged, value); }
        }

        #endregion

        #region function

        static string MakeCommandKey(string callerMember, int callerLineNumer)
        {
            var sb = new StringBuilder(callerMember.Length + 1 + 8);
            sb
                .Append(callerMember)
                .Append('?')
                .Append(callerLineNumer)
            ;

            return sb.ToString();
        }

        protected virtual ICommand CreateCommand(Action<object> executeCommand, [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLineNumer = -1)
        {
            var key = MakeCommandKey(callerMember, callerLineNumer);
            return this._createdCommands.Get(key, () => new DelegateCommand(executeCommand));
        }

        protected virtual ICommand CreateCommand(Action<object> executeCommand, Func<object, bool> canExecuteCommand, [CallerMemberName] string callerMember = "", [CallerLineNumber] int callerLineNumer = -1)
        {
            var key = MakeCommandKey(callerMember, callerLineNumer);
            return this._createdCommands.Get(key, () => new DelegateCommand(executeCommand, canExecuteCommand));
        }

        /// <summary>
        /// 変更状態をリセット。
        /// </summary>
        protected void ResetChangeFlag()
        {
            IsChanged = false;
        }

        /// <summary>
        /// 表示要素の更新。
        /// <para>各要素は必要なクラスで適時実装すること。</para>
        /// </summary>
        protected virtual void CallOnPropertyChangeDisplayItem()
        {
            OnPropertyChanged("DisplayText");
        }

        #endregion

        #region IDisplayText

        public virtual string DisplayText { get { return ToString(); } }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                foreach(var pair in this._createdCommands) {
                    pair.Value.Dispose();
                }
                this._createdCommands.Clear();
                this._createdCommands = null;
                IsChanged = false;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// PropertyChanged呼び出し。
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            IsChanged = true;
            base.OnPropertyChanged(propertyName);
        }

        #endregion


    }
}
