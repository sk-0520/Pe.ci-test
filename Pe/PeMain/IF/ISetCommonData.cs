using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.IF
{
	/// <summary>
	/// CommonDataを受信可能とする。
	/// </summary>
	public interface ISetCommonData
	{
		void SetCommonData(CommonData commonData);
	}
}
