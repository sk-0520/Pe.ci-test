/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/03
 * 時刻: 22:56
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace Pe.IF
{
	/// <summary>
	/// データ初期化・クリアインターフェイス
	/// </summary>
	public interface IDataClean
	{
		/// <summary>
		/// データクリア。
		/// 
		/// 確保済み領域を綺麗にする。
		/// </summary>
		void Clear();
	}
}
