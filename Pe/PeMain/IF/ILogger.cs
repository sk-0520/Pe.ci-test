using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.IF
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
