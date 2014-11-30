/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/11/2014
 * 時刻: 00:14
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using PeMain.Data;

namespace PeMain.IF
{
	/// <summary>
	/// ログ取インターフェイス。
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// 出力。
		/// </summary>
		/// <param name="logType">ログ種別</param>
		/// <param name="title">タイトル</param>
		/// <param name="detail">詳細</param>
		/// <param name="frame">フレーム。ユーザーコードでは使用しない</param>
		void Puts(LogType logType, string title, object detail, int frame = 2);
	}
}
