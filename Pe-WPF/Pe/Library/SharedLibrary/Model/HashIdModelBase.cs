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

	public abstract class HashIdModelBase: ModelBase, ITId<string>
	{
		public HashIdModelBase()
			: base()
		{ }

		#region ITId

		[DataMember, XmlAttribute]
		public string Id { get; set; }

		public bool IsSafeId(string id)
		{
			throw new NotImplementedException();
		}

		public string ToSafeId(string id)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
