namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// ログ取インターフェイス。
	/// </summary>
	public interface ILogger
	{
		/// <summary>
		/// 出力。
		/// </summary>
		/// <param name="logType">ログ種別。どのように処理するかは実装任せ。</param>
		/// <param name="title">タイトル</param>
		/// <param name="detail">詳細</param>
		/// <param name="frame">フレーム。ユーザーコードでは使用しない</param>
		void Puts(LogType logType, string title, object detail, int frame = 2);

		/// <summary>
		/// デバッグ出力。
		/// 
		/// 内部的に Puts を呼び出す。
		/// </summary>
		/// <param name="title"></param>
		/// <param name="detail"></param>
		/// <param name="frame"></param>
		void PutsDebug(string title, Func<object> detail, int frame = 3);
	}
}
