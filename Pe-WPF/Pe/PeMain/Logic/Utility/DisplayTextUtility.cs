namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Data.Temporary;

	public static class DisplayTextUtility
	{
		///// <summary>
		///// 文字列IDとINameを保持したデータから表示文字列を取得。
		///// </summary>
		///// <typeparam name="TModel"></typeparam>
		///// <param name="model"></param>
		///// <returns></returns>
		//public static string GetDisplayName<TModel,T>(TModel model)
		//	where TModel: ITId<T>, IName
		//	where T: IComparable
		//{
		//	return GetDisplayName(model, model);
		//}

		public static string GetDisplayName<T>(ITId<T> id, IName name)
			where T: IComparable
		{
			if (string.IsNullOrWhiteSpace(name.Name)) {
				return id.Id.ToString();
			}

			return name.Name ?? string.Empty;
		}

		//public static string GetDisplayName<T>(ITId<T> id)
		//	where T: IComparable
		//{
		//	var name = id as IName;
		//	if(name != null) {
		//		return GetDisplayName(id, name);
		//	}

		//	return id.Id == null ? id.Id.ToString(): string.Empty;
		//}

		public static string GetDisplayName(IName name)
		{
			return name.Name ?? string.Empty;
		}

		public static string MakeClipboardName(ClipboardItem clipboardItem, INonProcess nonProcess) 
		{
			return string.Empty; 
		}
	}
}
