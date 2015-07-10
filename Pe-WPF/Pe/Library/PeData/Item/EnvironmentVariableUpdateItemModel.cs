namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	public class EnvironmentVariableUpdateItemModel: HashIdModelBase
	{
		public EnvironmentVariableUpdateItemModel()
		{ }

		#region property

		[DataMember]
		public string Value { get; set; }

		#endregion
	}
}
