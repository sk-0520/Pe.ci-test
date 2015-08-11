namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	public static class EnumUtility
	{
		public static IEnumerable<TEnum> GetMembers<TEnum>(Type enumType)
		{
			return Enum.GetValues(enumType).Cast<TEnum>();
		}

		public static IEnumerable<TEnum> GetMembers<TEnum>()
		{
			return GetMembers<TEnum>(typeof(TEnum));
		}

		/// <summary>
		/// 指定値を正規化された enum の値に変換する。
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <param name="value"></param>
		/// <param name="defaultValue"></param>
		/// <returns>指定値がEnumに存在すれば指定値、存在しなければ<paramref name="defaultValue"/>を返す。</returns>
		public static TEnum GetNormalization<TEnum>(object value, TEnum defaultValue)
		{
			if(Enum.IsDefined(typeof(TEnum), value)) {
				return (TEnum)value;
			} else {
				return defaultValue;
			}
		}
	}
}
