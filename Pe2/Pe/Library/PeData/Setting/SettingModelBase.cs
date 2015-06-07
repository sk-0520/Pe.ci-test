namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	/// <summary>
	/// 設定統括データとして使用する基底モデル。
	/// </summary>
	public abstract class SettingModelBase: PeDataBase, ISettingModel
	{
 		public SettingModelBase()
			:base()
		{ }
	}
}
