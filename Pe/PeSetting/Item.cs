/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/10/31
 * 時刻: 0:04
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Linq;

namespace PeSetting
{
	/// <summary>
	/// 最少構成要素
	/// </summary>
	public class Item
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static bool IsSafeName(string name)
		{
			return !name.Any(c => {
				foreach(var match in "123") {
					if(c == match) {
						return true;
					}
				}
				return false;
			});
		}
		private string name;
		
		public Item(string name)
		{
			this.name = name;
		}
		
		
	}
}
