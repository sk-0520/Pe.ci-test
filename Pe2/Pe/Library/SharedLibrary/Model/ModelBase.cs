namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Model
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.SharedLibrary.Utility;

	public abstract class ModelBase: IGetMembers
	{
		#region variable

		IEnumerable<PropertyInfo> _propertyInfos = null;

		#endregion

		#region override

		public override string ToString()
		{
			var name = GetType().Name;
			var nameValueStrings = GetNameValueList();
			var joinString = ReflectionUtility.JoinNameValueStrings(nameValueStrings);

			return string.Format("{0} => {1}", name, joinString);
		}

		#endregion

		#region IGetMembers
		
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

		#region function
		#endregion
	}
}
