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

	[DataContract, Serializable]
	public sealed class IconItemModel : IconPathModel, IItemModel, IDeepClone
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
			if (IsDisposed) {
				return;
			}
			IsDisposed = true;
			GC.SuppressFinalize(this);
		}

		#endregion

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new IconItemModel() {
				Path = this.Path,
				Index = this.Index,
			};

			return result;
		}

		#endregion
	}
}
