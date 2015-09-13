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

	public abstract class GuidModelBase: ModelBase, ITId<Guid>, IDeepClone
	{
		#region define

		//string pattern = @"[]";

		#endregion

		public GuidModelBase()
			: base()
		{
			Id = Guid.NewGuid();
		}

		#region ITId

		[DataMember, XmlAttribute]
		public Guid Id { get; set; }

		public bool IsSafeId(Guid id)
		{
			return true;
		}

		public Guid ToSafeId(Guid id)
		{
			return id;
		}

		#endregion

		#region IDeepClone

		public virtual void DeepCloneTo(IDeepClone target)
		{
			var obj = (GuidModelBase)target;

			obj.Id = Id;
		}

		public abstract IDeepClone DeepClone();

		#endregion
	}
}
