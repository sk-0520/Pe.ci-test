namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
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
			OutputColor = new ColorPairItemModel();
			ErrorColor = new ColorPairItemModel();

			Font = new FontModel();
		}

		#region property

		[DataMember]
		public ColorPairItemModel OutputColor { get; set; }
		[DataMember]
		public ColorPairItemModel ErrorColor { get; set; }

		[DataMember]
		public FontModel Font { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (StreamSettingModel)target;

			OutputColor.DeepCloneTo(obj.OutputColor);
			ErrorColor.DeepCloneTo(obj.ErrorColor);

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
