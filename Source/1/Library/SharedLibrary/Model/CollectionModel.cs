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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    /// <summary>
    /// コレクションデータ保持用モデル。
    /// <para><see cref="ObservableCollection{TValue}"/>の単純ラッパー。</para>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    [DataContract]
    public class CollectionModel<TValue>: ObservableCollection<TValue>, IIsDisposed, IModel
    {
        public CollectionModel()
            : base()
        {
            IsDisposed = false;
        }

        public CollectionModel(IEnumerable<TValue> items)
            : base(items)
        {
            IsDisposed = false;
        }

        ~CollectionModel()
        {
            Dispose(false);
        }

        #region function

        /// <summary>
        /// <see cref="Collection{T}.Add(T)"/> を内部的に繰り返すだけ。
        /// <para>速度的にどうとかじゃなくて毎度毎度foreachするのだりぃ。</para>
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<TValue> items)
        {
            foreach(var item in items) {
                Add(item);
            }
        }

        /// <summary>
        /// 現在データを破棄して指定データを再設定。
        /// </summary>
        /// <param name="items"></param>
        public void InitializeRange(IEnumerable<TValue> items)
        {
            Clear();
            AddRange(items);
        }

        /// <summary>
        /// 指定インデックスのデータを入れ替える。
        /// </summary>
        /// <param name="indexA"></param>
        /// <param name="indexB"></param>
        public void SwapIndex(int indexA, int indexB)
        {
            var itemA = Items[indexA];
            var itemB = Items[indexB];

            var temp = itemA;
            Items[indexB] = temp;
            Items[indexA] = itemB;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// 指定データを入れ替える。
        /// </summary>
        /// <param name="itemA"></param>
        /// <param name="itemB"></param>
        public void SwapObject(TValue itemA, TValue itemB)
        {
            var indexA = IndexOf(itemA);
            var indexB = IndexOf(itemB);

            SwapIndex(indexA, indexB);
        }

        #endregion

        #region IIsDisposed

        [field: NonSerialized]
        public event EventHandler Disposing;

        [IgnoreDataMember, XmlIgnore]
        public bool IsDisposed { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if(IsDisposed) {
                return;
            }

            if(Disposing != null) {
                Disposing(this, EventArgs.Empty);
            }

            if(disposing) {
                GC.SuppressFinalize(this);
            }

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region IModel

        [IgnoreDataMember, XmlIgnore]
        public virtual string DisplayText
        {
            get { return GetType().FullName; }
        }

        public virtual void Correction()
        { }

        #endregion
    }

    /// <summary>
    /// <see cref="CollectionModel{TValue}"/>生成のヘルパ。
    /// </summary>
    public static class CollectionModel
    {
        public static CollectionModel<TValue> Create<TValue>(IEnumerable<TValue> items)
        {
            return new CollectionModel<TValue>(items);
        }
    }
}
