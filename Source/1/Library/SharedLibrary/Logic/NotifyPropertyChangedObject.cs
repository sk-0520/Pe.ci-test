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
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
    /// <summary>
    /// なんかをとりあえず通知する。
    /// </summary>
    public class NotifyPropertyChangedObject: DisposeFinalizeBase, INotifyPropertyChanged
    {
        #region function

        /// <summary>
        /// 変数変更用ヘルパ。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="variable">格納する変数。</param>
        /// <param name="value">変更データ。</param>
        /// <param name="notifyPropertyName">通知用プロパティ名。</param>
        /// <param name="propertyCheck">[インフラ] チェック方法。</param>
        /// <returns>変更があった場合は真を返す。</returns>
        protected virtual bool SetVariableValue<TValue>(ref TValue variable, TValue value, [CallerMemberName] string notifyPropertyName = "", PropertyCheck propertyCheck = PropertyCheck.None)
        {
#pragma warning disable RECS0030 // Suggests using the class declaring a static function when calling it
            if(!IComparable<TValue>.Equals(variable, value)) {
#pragma warning restore RECS0030 // Suggests using the class declaring a static function when calling it
                variable = value;
                OnPropertyChanged(notifyPropertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// プロパティ変更用ヘルパ。
        /// TODO: キャッシュする。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="obj">対象オブジェクト。</param>
        /// <param name="value">変更データ。</param>
        /// <param name="targetMemberName">対象オブジェクトのメンバ名。</param>
        /// <param name="notifyPropertyName">通知用プロパティ名。</param>
        /// <param name="propertyCheck"></param>
        /// <returns>変更があった場合は真を返す。</returns>
        protected virtual bool SetPropertyValue<TValue>(object obj, TValue value, [CallerMemberName] string targetMemberName = "", [CallerMemberName] string notifyPropertyName = "", PropertyCheck propertyCheck = PropertyCheck.None)
        {
            CheckUtility.DebugEnforceNotNull(obj);

            var type = obj.GetType();
            var propertyInfo = type.GetProperty(targetMemberName);

            var nowValue = (TValue)propertyInfo.GetValue(obj);

#pragma warning disable RECS0030 // Suggests using the class declaring a static function when calling it
            if(!IComparable<TValue>.Equals(nowValue, value)) {
#pragma warning restore RECS0030 // Suggests using the class declaring a static function when calling it
                propertyInfo.SetValue(obj, value);
                OnPropertyChanged(notifyPropertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 指定メンバが変更された通知する。
        /// </summary>
        /// <param name="propertyName">メンバ1。</param>
        /// <param name="propertyNames">メンバn。</param>
        protected void CallOnPropertyChange(string propertyName, params string[] propertyNames)
        {
            if(propertyNames == null || propertyNames.Length == 0) {
                OnPropertyChanged(propertyName);
            } else {
                var paramList = new List<string>(1 + propertyNames.Length);
                paramList.Add(propertyName);
                paramList.AddRange(propertyNames);
                CallOnPropertyChange(paramList);
            }
        }

        protected void CallOnPropertyChange(IEnumerable<string> propertyNames)
        {
            foreach(var propertyName in propertyNames) {
                OnPropertyChanged(propertyName);
            }
        }

        protected void CallOnPropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            var propertyChanged = PropertyChanged;

            if(propertyChanged != null) {
                propertyChanged(sender, e);
            }
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
            }

            base.Dispose(disposing);
        }

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// プロパティが変更された際に発生。
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChanged呼び出し。
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            CallOnPropertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
