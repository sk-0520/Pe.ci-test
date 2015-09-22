namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	/// <summary>
	/// ノートインデックスのボディ部データ。
	/// </summary>
	[DataContract, Serializable]
	public class NoteBodyItemModel : IndexBodyItemModelBase
	{
		public NoteBodyItemModel()
			: base()
		{ }

		#region property

		/// <summary>
		/// テキストデータ。
		/// </summary>
		[DataMember]
		public string Text { get; set; }

		#endregion

		#region IndexBodyItemModelBase

		public override IndexKind IndexKind { get { return IndexKind.Note; } }

		public override void DeepCloneTo(IDeepClone target)
		{
			base.DeepCloneTo(target);

			var obj = (NoteBodyItemModel)target;
			obj.Text = Text;
		}

		public override IDeepClone DeepClone()
		{
			var result = new NoteBodyItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
