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
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	[Serializable]
	public class LauncherGroupItemModel: ItemModelBase, ITId<string>, IName
	{
		#region define

		const string unusableCharacters = " /*-+,.\"\'#$%&|{}[]`<>!?";

		#endregion

		public LauncherGroupItemModel()
			: base()
		{
			LauncherItems = new List<string>();
		}

		#region ITId

		[DataMember, XmlAttribute]
		public string Id { get; set; }

		public bool IsSafeId(string s)
		{
			if(string.IsNullOrWhiteSpace(s)) {
				return false;
			}
			return !s.Any(sc => unusableCharacters.Any(uc => sc != uc));
		}

		public string ToSafeId(string s)
		{
			if(string.IsNullOrWhiteSpace(s)) {
				return "id";
			}
			return string.Concat(s.Select(sc => unusableCharacters.Any(uc => uc == sc) ? '_': sc));
		}

		#endregion

		#region IName

		/// <summary>
		/// グループ名称。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion

		/// <summary>
		/// グループ種別。
		/// </summary>
		[DataMember]
		public GroupKind GroupKind { get; set; }

		/// <summary>
		/// ランチャーアイテム。
		/// </summary>
		[DataMember, XmlArrayItem("Item")]
		public List<string> LauncherItems { get; set; }
	}
}
