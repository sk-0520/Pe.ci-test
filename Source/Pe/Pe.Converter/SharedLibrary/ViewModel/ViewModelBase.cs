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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
    /// <summary>
    /// ViewModelの基底。
    /// </summary>
    internal abstract class ViewModelBase: NotifyPropertyChangedObject, IDisplayText, INotifyDataErrorInfo
    {
        #region event

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        #endregion

        #region variable

        Dictionary<string, List<string>> _errors = null;
        Cacher<string, DelegateCommand> _createdCommands = null;

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

        /// <summary>
        /// 変更があったか。
        /// <para>各変更通知時にフラグが設定される。</para>
        /// <para>フラグのリセットは<seealso cref="ResetChangeFlag"/>を用いる。</para>
        /// </summary>
        public bool IsChanged
        {
            get { return this._isChanged; }
            private set
            {
                if(this._isChanged != value) {
                    this._isChanged = value;
                    CallOnPropertyChangedEvent(this, new PropertyChangedEventArgs(nameof(IsChanged)));
                }
            }
        }

        /// <summary>
        /// エラー内容。
        /// </summary>
        protected Dictionary<string, List<string>> Errors
        {
            get
            {
                if(this._errors == null) {
                    this._errors = new Dictionary<string, List<string>>();
                }

                return this._errors;
            }
        }

        Cacher<string, DelegateCommand> CreatedCommands
        {
            get
            {
                if(this._createdCommands == null) {
                    this._createdCommands = new Cacher<string, DelegateCommand>();
                }

                return this._createdCommands;
            }
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

        /// <summary>
        /// コマンド生成。
        /// <para>生成されたコマンドはキャッシュされる。</para>
        /// </summary>
        /// <param name="executeCommand">コマンド用処理。</param>
        /// <param name="callerMemberName">[自動入力]</param>
        /// <param name="callerLineNumber">[自動入力]</param>
        /// <returns></returns>
        protected virtual ICommand CreateCommand(Action<object> executeCommand, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var key = MakeCommandKey(callerMemberName, callerLineNumber);
            return CreatedCommands.Get(key, () => new DelegateCommand(executeCommand));
        }

        /// <summary>
        /// コマンド生成。
        /// <para>生成されたコマンドはキャッシュされる。</para>
        /// </summary>
        /// <param name="executeCommand">コマンド用処理。</param>
        /// <param name="canExecuteCommand">コマンド実行可能判定用処理。</param>
        /// <param name="callerMemberName">[自動入力]</param>
        /// <param name="callerLineNumber">[自動入力]</param>
        /// <returns></returns>
        protected virtual ICommand CreateCommand(Action<object> executeCommand, Func<object, bool> canExecuteCommand, [CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var key = MakeCommandKey(callerMemberName, callerLineNumber);
            return CreatedCommands.Get(key, () => new DelegateCommand(executeCommand, canExecuteCommand));
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
            OnPropertyChanged(nameof(DisplayText));
        }

        /// <summary>
        /// プロパティのエラー検知用処理。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="callerMemberName"></param>
        protected void ValidateProperty(object value, [CallerMemberName] string callerMemberName = null)
        {
            var context = new ValidationContext(this) {
                MemberName = callerMemberName
            };
            var validationErrors = new List<ValidationResult>();
            if(!Validator.TryValidateProperty(value, context, validationErrors)) {
                var errors = validationErrors.Select(error => error.ErrorMessage);
                SetErrors(callerMemberName, errors);
            } else {
                ClearErrors(callerMemberName);
            }
        }

        /// <summary>
        /// 指定プロパティにエラー設定。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="errors"></param>
        protected void SetErrors(string propertyName, IEnumerable<string> errors)
        {
            var hasError = Errors.ContainsKey(propertyName);
            if(!hasError) {
                return;
            }

            var hasNewError = errors != null && errors.Any();
            if(!hasError && !hasNewError) {
                return;
            }

            if(hasNewError) {
                Errors[propertyName] = errors.ToList();
            } else {
                Errors.Remove(propertyName);
            }
        }

        protected void ClearErrors(string propertyName)
        {
            if(Errors.Remove(propertyName)) {
                OnErrorsChanged(propertyName);
            }
        }

        protected void OnErrorsChanged(string propertyName)
        {
            if(ErrorsChanged != null) {
                var e = new DataErrorsChangedEventArgs(propertyName);
                ErrorsChanged(this, e);
            }
        }

        protected bool HasError(string propertName)
        {
            List<string> list;
            if(Errors.TryGetValue(propertName, out list)) {
                return list != null && list.Any();
            }

            return false;
        }

        #endregion

        #region IDisplayText

        public virtual string DisplayText { get { return ToString(); } }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(this._createdCommands != null) {
                    foreach(var pair in this._createdCommands.ToArray()) {
                        pair.Value.Dispose();
                    }
                }

                if(disposing) {
                    if(this._createdCommands != null) {
                        this._createdCommands.Clear();
                        this._createdCommands = null;
                    }

                    foreach(var error in Errors.ToArray()) {
                        error.Value.Clear();
                    }
                    Errors.Clear();
                }

                IsChanged = false;
            }

            base.Dispose(disposing);
        }

        #endregion

        #region INotifyPropertyChanged

        protected override bool SetVariableValue<TValue>(ref TValue variable, TValue value, [CallerMemberName] string notifyPropertyName = "", PropertyCheck propertyCheck = PropertyCheck.None)
        {
            if(propertyCheck.HasFlag(PropertyCheck.Before)) {
                ValidateProperty(value, notifyPropertyName);
                if(HasError(notifyPropertyName)) {
                    return false;
                }
            }

            var isChanged = base.SetVariableValue<TValue>(ref variable, value, notifyPropertyName, propertyCheck);
            if(isChanged) {
                if(propertyCheck.HasFlag(PropertyCheck.After)) {
                    ValidateProperty(value, notifyPropertyName);
                }
            }
            return isChanged;
        }

        protected override bool SetPropertyValue<TValue>(object obj, TValue value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "", PropertyCheck propertyCheck = PropertyCheck.None)
        {
            if(propertyCheck.HasFlag(PropertyCheck.Before)) {
                ValidateProperty(value, notifyPropertyName);
                if(HasError(notifyPropertyName)) {
                    return false;
                }
            }

            var isChanged = base.SetPropertyValue<TValue>(obj, value, targetMemberName, notifyPropertyName, propertyCheck);
            if(isChanged) {
                if(propertyCheck.HasFlag(PropertyCheck.After)) {
                    ValidateProperty(value, notifyPropertyName);
                }
            }
            return isChanged;
        }

        /// <summary>
        /// PropertyChanged呼び出し。
        /// </summary>
        /// <param name="propertyName"></param>
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            IsChanged = true;
            base.OnPropertyChanged(propertyName);
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if(!string.IsNullOrEmpty(propertyName)) {
                List<string> errors;
                if(Errors.TryGetValue(propertyName, out errors)) {
                    return errors;
                }

                return null;
            }

            return Errors.SelectMany(e => e.Value);
        }

        #endregion

        #region INotifyDataErrorInfo

        public bool HasErrors => Errors.Any();

        #endregion
    }
}
