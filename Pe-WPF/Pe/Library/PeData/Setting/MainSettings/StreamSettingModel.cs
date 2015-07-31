namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[Serializable]
	public class StreamSettingModel : SettingModelBase, IDeepClone
	{
		public StreamSettingModel()
			: base()
		{ }

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (StreamSettingModel)target;


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
