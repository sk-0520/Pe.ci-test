namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	public class WindowItemCollectionModel: ObservableCollection<WindowItemModel>, IName
	{
		#region variable

		[IgnoreDataMember, XmlIgnore]
		IEnumerable<PropertyInfo> _propertyInfos = null;

		#endregion

		public WindowItemCollectionModel()
			: base()
		{ }

		#region property

		public DateTime DateTime { get; set; }

		#endregion

		#region IName

		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion

		#region IModel

		[IgnoreDataMember, XmlIgnore]
		public virtual string DisplayText
		{
			get { return GetType().FullName; }
		}

		[IgnoreDataMember, XmlIgnore]
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

		public virtual void Correction()
		{ }

		#endregion
	}
}
