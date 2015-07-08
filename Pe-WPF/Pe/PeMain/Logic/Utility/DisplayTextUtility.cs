namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public static class DisplayTextUtility
	{
		/// <summary>
		/// 文字列IDとINameを保持したデータから表示文字列を取得。
		/// </summary>
		/// <typeparam name="TModel"></typeparam>
		/// <param name="model"></param>
		/// <returns></returns>
		public static string GetDisplayName<TModel>(TModel model)
			where TModel: ITId<string>, IName
		{
			return GetDisplayName(model, model);
		}

		public static string GetDisplayName(ITId<string> id, IName name)
		{
			if (string.IsNullOrWhiteSpace(name.Name)) {
				return id.Id;
			}

			return name.Name ?? string.Empty;
		}

		public static string GetDisplayName(ITId<string> id)
		{
			var name = id as IName;
			if(name != null) {
				return GetDisplayName(id, name);
			}

			return id.Id ?? string.Empty;
		}

		public static string GetDisplayName(IName name)
		{
			var id = name as ITId<string>;
			if(id != null) {
				return GetDisplayName(id, name);
			}

			return name.Name ?? string.Empty;
		}
	}
}
