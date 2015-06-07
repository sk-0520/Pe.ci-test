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

	[DataContract, Serializable]
	public class IconItemModel: IconPathModel, IDeepClone
	{
		public IconItemModel()
			: base()
		{ }

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
