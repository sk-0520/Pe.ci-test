namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	/// <summary>
	/// モデルの基盤。
	/// <para>データ保持を生きがいにする。</para>
	/// </summary>
	[Serializable]
	public abstract class ModelBase: IModel
	{
		#region variable

		[IgnoreDataMember, XmlIgnore]
		IEnumerable<PropertyInfo> _propertyInfos = null;

		#endregion

		#region override

		public override string ToString()
		{
			return ReflectionUtility.GetObjectString(this);
		}

		#endregion

		#region IModel

		[IgnoreDataMember, XmlIgnore]
		public virtual string DisplayText
		{
			get { return GetType().FullName; }
		}

		[IgnoreDataMember, XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public IEnumerable<PropertyInfo> PropertyInfos
		{
			get
			{
				if(this._propertyInfos == null) {
					this._propertyInfos = GetType().GetProperties().Where(p => p.Name != "PropertyInfos").ToArray();
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
