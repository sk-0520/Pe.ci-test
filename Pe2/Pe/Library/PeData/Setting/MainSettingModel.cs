namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// 各設定の統括。
	/// <para>その他の大きいやつは別クラスで管理しとく。</para>
	/// </summary>
	[DataContract, Serializable]
	public sealed class MainSettingModel: SettingModelBase
	{
	}
}
