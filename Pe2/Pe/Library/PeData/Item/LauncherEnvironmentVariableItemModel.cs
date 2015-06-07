namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[DataContract]
	public class LauncherEnvironmentVariableItemModel: ItemModelBase, IDeepClone
	{
		public LauncherEnvironmentVariableItemModel()
			: base()
		{ }

		/// <summary>
		/// 環境変数を変更するか。
		/// </summary>
		[DataMember]
		public bool Edit { get; set; }

		/// <summary>
		/// 追加・変更対象
		/// </summary>
		[DataMember]
		public Dictionary<string, string> Update { get; set; }

		/// <summary>
		/// 削除変数
		/// </summary>
		[DataMember]
		public List<string> Remove { get; set; }

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new LauncherEnvironmentVariableItemModel() {
				Edit = this.Edit,
			};

			// 二回も生成するのかー。。。
			result.Update = new Dictionary<string, string>(Update);
			result.Remove.AddRange(Remove);

			return result;
		}

		#endregion
	}
}
