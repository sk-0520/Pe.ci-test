/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 22:03
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeMain
{
	/// <summary>
	/// Description of DataMember.
	/// </summary>
	public class ItemData<T>
	{
		private T _data;
		
		public DataMember(T data)
		{
			this._data = data;
		}
		/// <summary>
		/// 表示文字列
		/// </summary>
		public virtual string DisplayMember
		{
			get 
			{
				return this._data.ToString();
			}
		}
		/// <summary>
		/// 設定データ
		/// </summary>
		public T DataMember
		{
			get
			{
				return this._data;
			}
		}
	}
}
