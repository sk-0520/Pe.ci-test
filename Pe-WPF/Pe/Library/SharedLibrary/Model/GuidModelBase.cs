namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public abstract class GuidModelBase: ModelBase, ITId<string>
	{
		#region define

		//string pattern = @"[]";

		#endregion

		public GuidModelBase()
			: base()
		{ }

		#region ITId

		[DataMember, XmlAttribute]
		public string Id { get; set; }

		public bool IsSafeId(string id)
		{
			Guid temp;
			return Guid.TryParse(id, out temp);
		}

		public string ToSafeId(string id)
		{
			return Guid.NewGuid().ToString();
		}

		#endregion
	}
}
