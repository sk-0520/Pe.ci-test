namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	/// <summary>
	/// テンプレートインデックス統括データ。
	/// </summary>
	public class TemplateIndexItemCollectionModel: IndexItemCollectionModel<TemplateIndexItemModel>
	{
		public TemplateIndexItemCollectionModel()
			: base()
		{ }
	}
}
