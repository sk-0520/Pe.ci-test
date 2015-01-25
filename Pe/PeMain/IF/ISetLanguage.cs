using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
	/// <summary>
	/// 言語設定を外部から実施するためのインターフェイス。
	/// </summary>
	public interface ISetLanguage
	{
		/// <summary>
		/// Languageの受信。
		/// </summary>
		/// <param name="language"></param>
		void SetLanguage(Language language);
	}
}
