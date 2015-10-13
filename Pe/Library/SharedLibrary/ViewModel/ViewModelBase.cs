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

    public abstract class ViewModelBase: DisposeFinalizeBase, INotifyPropertyChanged, IDisplayText
    {
        #region variable

        //Dictionary<string, DelegateCommand> _createdCommands = new Dictionary<string, DelegateCommand>();
        Caching<string, DelegateCommand> _createdCommands = new Caching<string, DelegateCommand>();

        bool _isChanged = false;

        #endregion

        public ViewModelBase()
            : base()
        {
            ResetChangeFlag();
        }

        #region property

        public bool IsChanged
        {
            get { return this._isChanged; }
            private set { SetVariableValue(ref this._isChanged, value); }
        }

        #endregion

        #region function

        /// <summary>
        /// 変数変更用ヘルパ。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="variable">格納する変数。</param>
        /// <param name="value">変更データ。</param>
        /// <param name="propertyName"></param>
        /// <returns>変更があった場合は真を返す。</returns>
        protected bool SetVariableValue<T>(ref T variable, T value, [CallerMemberName] string propertyName = "")
        {
            if(!IComparable<T>.Equals(variable, value)) {
                variable = value;
                OnPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// プロパティ変更用ヘルパ。
        /// TODO: キャッシュする。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">変更データ。</param>
        /// <param name="targetMemberName">対象オブジェクトのメンバ名。</param>
        /// <param name="propertyName"></param>
        /// <returns>変更があった場合は真を返す。</returns>
        protected bool SetPropertyValue<T>(object obj, T value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string callerPropertyName = "")
        {
            CheckUtility.DebugEnforceNotNull(obj);

            var type = obj.GetType();
            var propertyInfo = type.GetProperty(targetMemberName);

            var nowValue = (T)propertyInfo.GetValue(obj);

            if(!IComparable<T>.Equals(nowValue, value)) {
                propertyInfo.SetValue(obj, value);
                OnPropertyChanged(callerPropertyName);

                return true;
            }

            return false;
        }

        static string MakeCommandKey(string callerMember, int callerLineNumer)
        {
            return callerMember + "?" + callerLineNumer.ToString();
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

        protected void CallOnPropertyChange(string propertyName, params string[] propertyNames)
        {
            var paramList = new List<string>(1 + (propertyNames != null ? propertyNames.Length : 0));
            paramList.Add(propertyName);
            if(propertyNames != null && propertyNames.Any()) {
                paramList.AddRange(propertyNames);
            }
            CallOnPropertyChange(paramList);
        }
        protected void CallOnPropertyChange(IEnumerable<string> propertyNames)
        {
            foreach(var propertyName in propertyNames) {
                OnPropertyChanged(propertyName);
            }
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
                this._createdCommands = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// プロパティが変更された際に発生。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// PropertyChanged呼び出し。
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            IsChanged = true;
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion


    }
}
