namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	[Serializable]
	public class EnvironmentVariablesItemModel: ItemModelBase, IDeepClone
	{
		public EnvironmentVariablesItemModel()
			: base()
		{
			Update = new EnvironmentVariableUpdateItemCollectionModel();
			Remove = new CollectionModel<string>();
		}

		/// <summary>
		/// 環境変数を変更するか。
		/// </summary>
		[DataMember]
		public bool Edit { get; set; }

		/// <summary>
		/// 追加・変更対象
		/// </summary>
		[DataMember]
		public EnvironmentVariableUpdateItemCollectionModel Update { get; set; }

		/// <summary>
		/// 削除変数
		/// </summary>
		[DataMember]
		public CollectionModel<string> Remove { get; set; }

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (EnvironmentVariablesItemModel)target;
			obj.Edit = Edit;
			obj.Update.AddRange(Update);
			obj.Remove.AddRange(Remove);
		}

		public IDeepClone DeepClone()
		{
			var result = new EnvironmentVariablesItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
