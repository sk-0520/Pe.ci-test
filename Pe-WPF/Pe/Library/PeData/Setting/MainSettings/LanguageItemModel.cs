namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[Serializable]
	public class LanguageItemModel: ItemModelBase, IName
	{
		public LanguageItemModel()
			: base()
		{ }

		#region IName

		[DataMember]
		public string Name { get; set; }

		#endregion
	}
}
