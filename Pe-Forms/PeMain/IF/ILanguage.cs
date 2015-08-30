using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
	/// <summary>
	/// Languageを設定可能とする。
	/// </summary>
	public interface ISetLanguage
	{
		/// <summary>
		/// Languageの設定。
		/// </summary>
		/// <param name="language"></param>
		void SetLanguage(Language language);
	}

	/// <summary>
	/// Languageを参照する。
	/// </summary>
	public interface ILanguage
	{
		/// <summary>
		/// 参照するLanguage。
		/// </summary>
		Language Language { get; }
	}
}
