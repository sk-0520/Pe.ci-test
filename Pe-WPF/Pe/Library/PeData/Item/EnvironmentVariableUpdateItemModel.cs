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
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	public class EnvironmentVariableUpdateItemModel: ItemModelBase, HashId
	{
		#region define

		const string unusableCharacters = " /*-+,.\"\'#$%&|{}[]`<>!?";

		#endregion


		public EnvironmentVariableUpdateItemModel()
		{ }

		#region property

		[DataMember]
		public string Value { get; set; }

		#endregion

		#region ITId

		[DataMember, XmlAttribute]
		public string Id { get; set; }

		public bool IsSafeId(string s)
		{
			if (string.IsNullOrWhiteSpace(s)) {
				return false;
			}
			return !s.Any(sc => unusableCharacters.Any(uc => sc == uc));
		}

		public string ToSafeId(string s)
		{
			if (string.IsNullOrWhiteSpace(s)) {
				return "id";
			}
			return string.Concat(s.Select(sc => unusableCharacters.Any(uc => uc == sc) ? '_' : sc));
		}

		#endregion
	}
}
