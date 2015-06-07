namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
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

	[DataContract, Serializable]
	public class DictionaryModel<TKey, TValue>: Dictionary<TKey, TValue>, IModel, IXmlSerializable, IIsDisposed
	{
		#region define

		[DataContract, Serializable, XmlRoot("Item")]
		public class TPiar
		{
			public TPiar(TKey key, TValue value)
			{
				Key = key;
				Value = value;
			}

			public TKey Key { get; set; }
			public TValue Value { get; set; }
		}

		#endregion

		#region variable

		IEnumerable<PropertyInfo> _propertyInfos = null;

		#endregion

		public DictionaryModel()
			: base()
		{ }

		public DictionaryModel(IDictionary<TKey, TValue> dictionary)
			: base(dictionary)
		{ }

		public DictionaryModel(IEqualityComparer<TKey> comparer)
			: base(comparer)
		{ }

		public DictionaryModel(int capacity)
			: base(capacity)
		{ }

		public DictionaryModel(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: base(dictionary, comparer)
		{ }

		public DictionaryModel(int capacity, IEqualityComparer<TKey> comparer)
			: base(capacity, comparer)
		{ }

		public DictionaryModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

		void Initialize()
		{
			IsDisposed = false;
		}

		#region IIsDisposed

		public bool IsDisposed { get; private set; }

		~DictionaryModel()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if(IsDisposed) {
				return;
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

		public IEnumerable<PropertyInfo> PropertyInfos
		{
			get
			{
				if(this._propertyInfos == null) {
					this._propertyInfos = GetType().GetProperties();
				}

				return this._propertyInfos;
			}
		}

		public IEnumerable<string> GetNameValueList()
		{
			var nameValueMap = ReflectionUtility.GetMembers(this, PropertyInfos);
			return ReflectionUtility.GetNameValueStrings(nameValueMap);
		}

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
