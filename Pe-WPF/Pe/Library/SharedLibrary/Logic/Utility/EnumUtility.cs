namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class EnumUtility
	{
		public static IEnumerable<TEnum> GetMembers<TEnum>(Type type)
		{
			return Enum.GetValues(type).Cast<TEnum>();
		}

		public static IEnumerable<TEnum> GetMembers<TEnum>()
		{
			return GetMembers<TEnum>(typeof(TEnum));
		}
	}
}
