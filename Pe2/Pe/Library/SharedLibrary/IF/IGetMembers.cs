namespace ContentTypeTextNet.Pe.Library.SharedLibrary.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	public interface IGetMembers
	{
		IEnumerable<PropertyInfo> PropertyInfos { get; }
		IEnumerable<string> GetNameValueList();
	}
}
