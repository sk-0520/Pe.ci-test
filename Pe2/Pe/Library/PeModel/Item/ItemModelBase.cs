namespace ContentTypeTextNet.Pe.Library.PeModel.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// 設定項目データとして使用する基底モデル。
	/// </summary>
	public abstract class ItemModelBase: DisposeFinalizeModelBase
	{
		public ItemModelBase():base()
		{ }
	}
}
