namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class CastUtility
	{
		/// <summary>
		/// オブジェクトがキャスト出来た場合に処理を行う。
		/// </summary>
		/// <typeparam name="TCast"></typeparam>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="arg"></param>
		/// <param name="action"></param>
		public static void AsAction<TCast>(object arg, Action<TCast> action)
			where TCast: class
		{
			var obj = arg as TCast;
			if(obj != null) {
				action(obj);
			}
		}
	}
}
