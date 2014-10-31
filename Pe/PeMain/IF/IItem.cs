/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/13
 * 時刻: 11:33
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeMain.IF
{
	/// <summary>
	/// データ補正関係処理
	/// </summary>
	public interface ICorrectionItem
	{
		void CorrectionValue();
	}

	public interface INameItem
	{
		/// <summary>
		/// 名前
		/// </summary>
		string Name { get; set; }
	}
}
