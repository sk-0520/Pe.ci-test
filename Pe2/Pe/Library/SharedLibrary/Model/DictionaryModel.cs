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
	public class DictionaryModel<TKey, TValue>: Dictionary<TKey, TValue>, IModel, IXmlSerializable
	{
		#region define

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
