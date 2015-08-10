namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class StreamSettingModel : SettingModelBase, IDeepClone
	{
		public StreamSettingModel()
			: base()
		{
			Normal = new ColorPairItemModel();
			Error = new ColorPairItemModel();

			Font = new FontModel();
		}

		#region property

		public ColorPairItemModel Normal { get; set; }
		public ColorPairItemModel Error{ get; set; }

		public FontModel Font { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (StreamSettingModel)target;

			Normal.DeepCloneTo(obj.Normal);
			Error.DeepCloneTo(obj.Error);

			Font.DeepCloneTo(obj.Font);
		}

		public IDeepClone DeepClone()
		{
			var result = new StreamSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
