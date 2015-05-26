namespace ContentTypeTextNet.Pe.Library.SharedLibrary.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	public static class ReflectionUtility
	{
		/// <summary>
		/// メンバ名とその値を取得する。
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="propertyInfos"></param>
		/// <returns></returns>
		public static IDictionary<string, object> GetMembers(object obj, IEnumerable<PropertyInfo> propertyInfos)
		{
			var result = new Dictionary<string, object>(propertyInfos.Count());

			foreach(var propertyInfo in propertyInfos) {
				var value = propertyInfo.GetValue(obj, null);
				result.Add(propertyInfo.Name, obj);
			}

			return result;
		}

		public static IEnumerable<string> GetNameValueStrings(IDictionary<string, object> nameValues)
		{
			foreach(var pair in nameValues.OrderBy(p => p.Key)) {
				yield return string.Format("{0}=[{1}]", pair.Key, pair.Value ?? "null");
			}
		}

		public static string JoinNameValueStrings(IEnumerable<string> nameValues)
		{
			return string.Join(",", nameValues);
		}
	}
}
