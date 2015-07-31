namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class LanguageSettingModel : SettingModelBase, IName, IDeepClone
	{
		public LanguageSettingModel()
			: base()
		{ }

		#region IName

		/// <summary>
		/// 言語名もしくは言語コード。
		/// </summary>
		[DataMember]
		public string Name { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (LanguageSettingModel)target;

			obj.Name = Name;
		}

		public IDeepClone DeepClone()
		{
			var result = new LanguageSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
