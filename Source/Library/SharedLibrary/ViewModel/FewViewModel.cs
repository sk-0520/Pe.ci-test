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
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.Library.SharedLibrary.ViewModel
{
    /// <summary>
    /// 単一のプロパティ値を保持する。
    /// <para>ViewModelが変数ごっちゃごちゃでわけわからん状態を抑える目的。</para>
    /// <para>劣化しまくったReactivePropertyみたいな。</para>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class FewViewModel<TValue>: ViewModelBase
    {
        /// <summary>
        /// <typeparamref name="TValue"/>のデフォルト値で生成。
        /// </summary>
        public FewViewModel()
            : this(default(TValue))
        { }

        public FewViewModel(Action<TValue> changeAction)
            : this(default(TValue), changeAction)
        { }

        /// <summary>
        /// 初期値を指定して生成。
        /// </summary>
        public FewViewModel(TValue value)
            : this(value, null)
        { }

        public FewViewModel(TValue value, Action<TValue> changeAction)
        {
            this._value = value;
            InitialValue = value;
            ChangeAction = changeAction;
        }

        #region variable

        TValue _value;

        #endregion

        #region property

        /// <summary>
        /// 初期値。
        /// </summary>
        public TValue InitialValue { get; private set; }

        /// <summary>
        /// 変更時の処理。
        /// <para><typeparamref name="TValue"/>は前回値。</para>
        /// </summary>
        Action<TValue> ChangeAction { get; set; }

        /// <summary>
        /// 値。
        /// </summary>
        public TValue Value
        {
            get { return this._value; }
            set
            {
                var prev = this._value;
                if(SetVariableValue(ref this._value, value)) {
                    if(ChangeAction != null) {
                        ChangeAction(prev);
                    }
                }
            }
        }

        #endregion

        #region function

        /// <summary>
        /// 変更通知を強制。
        /// <para><see cref="ViewModelBase"/>と違って頻繁にあるかもなので外部化。</para>
        /// </summary>
        public void Notice()
        {
            CallOnPropertyChange(nameof(Value));
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(ChangeAction != null) {
                        ChangeAction = null;
                    }
                }
                InitialValue = default(TValue);
                this._value = default(TValue);
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
