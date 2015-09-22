namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	/// <summary>
	/// アイコンのパスを保持。
	/// </summary>
	[Serializable]
	public sealed class IconItemModel: IconPathModel, IItemModel, IDeepClone
	{
		public IconItemModel()
			: base()
		{
			IsDisposed = false;
		}

		~IconItemModel()
		{
			Dispose();
		}

		#region IItemModel

		public bool IsDisposed { get; set; }

		public void Dispose()
		{
			// IItemModelのIFに合わせるためだけの実装

			if (IsDisposed) {
				return;
			}
			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (IconItemModel)target;

			obj.Path = Path;
			obj.Index = Index;
		}

		public IDeepClone DeepClone()
		{
			var result = new IconItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
