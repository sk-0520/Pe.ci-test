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

	/// <summary>
	/// テンプレートインデックスのヘッダ部。
	/// </summary>
	public class TemplateIndexItemModel: IndexItemModelBase
	{
		public TemplateIndexItemModel()
			: base()
		{ }

		#region property

		/// <summary>
		/// 置換処理を行うか。
		/// </summary>
		[DataMember]
		public TemplateReplaceMode TemplateReplaceMode { get; set; }

		#endregion

		#region IndexItemModelBase

		public override void DeepCloneTo(IDeepClone target)
		{
			base.DeepCloneTo(target);

			var obj = (TemplateIndexItemModel)target;

			obj.TemplateReplaceMode = TemplateReplaceMode;
		}

		public override IDeepClone DeepClone()
		{
			var result = new TemplateIndexItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
