namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	/// <summary>
	/// クリップボードのインデックス個別データ。
	/// </summary>
	public class ClipboardIndexItemModel: IndexItemModelBase
	{
		public ClipboardIndexItemModel()
			: base()
		{
			Hash = new HashItemModel();
		}

		#region property

		/// <summary>
		/// 保持するクリップボードの型。
		/// </summary>
		public ClipboardType Type { get; set; }
		/// <summary>
		/// 自身のデータ(インデックス + ボディ)を示すハッシュデータ。
		/// </summary>
		public HashItemModel Hash { get; set; }

		#endregion

		#region IndexItemModelBase

		public override void DeepCloneTo(IDeepClone target)
		{
			base.DeepCloneTo(target);

			var obj = (ClipboardIndexItemModel)target;

			obj.Type = Type;
			Hash.DeepCloneTo(obj.Hash);
		}

		public override IDeepClone DeepClone()
		{
			var result = new ClipboardIndexItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
