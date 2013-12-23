/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 15:32
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeUtility
{
	/// <summary>
	/// Description of Functions.
	/// </summary>
	public static class Functions
	{
		public static bool IsIn<T>(this T value, params T[] targets)
			where T: IComparable<T>
		{
			foreach(var target in targets) {
				if(value.CompareTo(target) == 0) {
					return true;
				}
			}
			return false;
		}
	}
}
