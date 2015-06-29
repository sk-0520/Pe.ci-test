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
			if (string.IsNullOrWhiteSpace(model.Name)) {
				return model.Id;
			}

			return model.Name;
		}
	}
}
