using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
	/// <summary>
	/// CommonDataを受信可能とする。
	/// </summary>
	public interface ISetCommonData
	{
		/// <summary>
		/// CommonDataの受信。
		/// </summary>
		/// <param name="commonData"></param>
		void SetCommonData(CommonData commonData);
	}

	/// <summary>
	/// CommonDataの参照。
	/// </summary>
	public interface ICommonData
	{
		/// <summary>
		/// 参照するCommonData。
		/// </summary>
		CommonData CommonData { get; }
	}
}
