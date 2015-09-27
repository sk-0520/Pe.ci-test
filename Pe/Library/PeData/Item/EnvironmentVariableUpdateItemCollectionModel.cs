namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	/// <summary>
	/// 環境変数設定データ統括。
	/// </summary>
	[Serializable]
	public class EnvironmentVariableUpdateItemCollectionModel: TIdCollectionModel<string, EnvironmentVariableUpdateItemModel>
	{
		public EnvironmentVariableUpdateItemCollectionModel()
			: base()
		{ }
	}
}
