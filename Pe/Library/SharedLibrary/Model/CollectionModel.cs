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
namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
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

    /// <summary>
    /// コレクションデータ保持用モデル。
    /// <para>ObservableCollectionの単純ラッパー。</para>
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    [Serializable]
    public class CollectionModel<TValue>: ObservableCollection<TValue>, IIsDisposed, IModel
    {
        #region variable

        //[IgnoreDataMember, XmlIgnore]
        //IEnumerable<PropertyInfo> _propertyInfos = null;

        #endregion

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
        /// Addを内部的に繰り返すだけ。
        /// <para>速度的にどうとかじゃなくて毎度毎度foreachするのだりぃ。</para>
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<TValue> items)
        {
            foreach(var item in items) {
                Add(item);
            }
        }

        public void InitializeRange(IEnumerable<TValue> items)
        {
            Clear();
            AddRange(items);
        }

        public void SwapIndex(int indexA, int indexB)
        {
            var itemA = Items[indexA];
            var itemB = Items[indexB];

            //RemoveAt(indexA);
            //RemoveAt(indexB);

            //Insert(indexA, itemB);
            //Insert(indexB, itemA);
            var temp = itemA;
            Items[indexB] = temp;
            Items[indexA] = itemB;

            //var eventA = new NotifyCollectionChangedEventArgs(
            //	NotifyCollectionChangedAction.Reset,
            //	itemA,
            //	indexA
            //);
            //var eventB = new NotifyCollectionChangedEventArgs(
            //	NotifyCollectionChangedAction.Move,
            //	itemB,
            //	indexB
            //);
            //OnCollectionChanged(eventA);
            //OnCollectionChanged(eventB);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void SwapObject(TValue itemA, TValue itemB)
        {
            var indexA = IndexOf(itemA);
            var indexB = IndexOf(itemB);

            //RemoveAt(indexA);
            //RemoveAt(indexB);

            //Insert(indexA, itemB);
            //Insert(indexB, itemA);
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

            IsDisposed = true;
            GC.SuppressFinalize(this);
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

        //[IgnoreDataMember, XmlIgnore]
        //public IEnumerable<PropertyInfo> PropertyInfos
        //{
        //    get
        //    {
        //        if(this._propertyInfos == null) {
        //            this._propertyInfos = ReflectionUtility.FilterSharedLibrary(GetType().GetProperties());
        //        }

        //        return this._propertyInfos;
        //    }
        //}

        //public IEnumerable<string> GetNameValueList()
        //{
        //    var nameValueMap = ReflectionUtility.GetMembers(this, PropertyInfos);
        //    return ReflectionUtility.GetNameValueStrings(nameValueMap);
        //}

        public virtual void Correction()
        { }

        #endregion
    }
}
