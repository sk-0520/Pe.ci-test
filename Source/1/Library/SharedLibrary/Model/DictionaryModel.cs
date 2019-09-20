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
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
    [Obsolete]
    [Serializable]
    public class DictionaryModel<TKey, TValue>: Dictionary<TKey, TValue>, IModel, IIsDisposed, IXmlSerializable
    {
        #region define

        [Serializable, XmlRoot("Item")]
        public class TPiar
        {
            public TPiar()
                : this(default(TKey), default(TValue))
            { }
            public TPiar(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            [DataMember, XmlAttribute]
            public TKey Key { get; set; }
            [DataMember, XmlAttribute]
            public TValue Value { get; set; }
        }

        #endregion

        #region variable

        //[IgnoreDataMember, XmlIgnore]
        //IEnumerable<PropertyInfo> _propertyInfos = null;

        #endregion

        public DictionaryModel()
            : base()
        {
            Initialize();
        }

        public DictionaryModel(IDictionary<TKey, TValue> dictionary)
            : base(dictionary)
        {
            Initialize();
        }

        public DictionaryModel(IEqualityComparer<TKey> comparer)
            : base(comparer)
        {
            Initialize();
        }

        public DictionaryModel(int capacity)
            : base(capacity)
        {
            Initialize();
        }

        public DictionaryModel(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
            : base(dictionary, comparer)
        {
            Initialize();
        }

        public DictionaryModel(int capacity, IEqualityComparer<TKey> comparer)
            : base(capacity, comparer)
        {
            Initialize();
        }

        public DictionaryModel(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Initialize();
        }

        void Initialize()
        {
            IsDisposed = false;
        }

        ~DictionaryModel()
        {
            Dispose(false);
        }

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

            if(typeof(TValue) is IDisposable) {
                foreach(var value in Values.Cast<IDisposable>().ToArray()) {
                    value.Dispose();
                }
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

        /// <summary>
        /// http://qiita.com/rohinomiya/items/b88a5da3965a1c5bed0d
        /// </summary>
        /// <returns></returns>
        #region IXmlSerializable

        public virtual XmlSchema GetSchema()
        {
            return default(XmlSchema);
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            var serializer = new XmlSerializer(typeof(TPiar));

            reader.Read();

            if(reader.IsEmptyElement) {
                return;
            }

            while(reader.NodeType != XmlNodeType.EndElement) {
                var pair = serializer.Deserialize(reader) as TPiar;
                if(pair != null) {
                    Add(pair.Key, pair.Value);
                }
            }

            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            var serializer = new XmlSerializer(typeof(TPiar));
            foreach(var pair in this) {
                serializer.Serialize(writer, new TPiar(pair.Key, pair.Value));
            }
        }

        #endregion
    }
}
