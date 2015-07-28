namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	/// <summary>
	/// 標準入出力関連。
	/// </summary>
	[Serializable]
	public class LauncherStdStreamItemModel: ItemModelBase, IDeepClone
	{
		public LauncherStdStreamItemModel()
			: base()
		{ }

		/// <summary>
		/// 標準出力(とエラー)を取得するか。
		/// </summary>
		[DataMember]
		public bool OutputWatch { get; set; }
		/// <summary>
		/// 標準入力へ入力するか。
		/// </summary>
		[DataMember]
		public bool IsEnabledInput { get; set; }

		#region IDeepClone

		public virtual void DeepCloneTo(IDeepClone target)
		{
			var obj = (LauncherStdStreamItemModel)target;

			obj.OutputWatch = OutputWatch;
			obj.IsEnabledInput = IsEnabledInput;
		}

		public IDeepClone DeepClone()
		{
			var result = new LauncherStdStreamItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
