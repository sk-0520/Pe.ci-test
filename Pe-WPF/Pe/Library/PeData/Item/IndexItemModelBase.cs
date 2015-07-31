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

	public abstract class IndexItemModelBase: GuidModelBase, IName, IDeepClone
	{
		public IndexItemModelBase()
			: base()
		{
			History = new HistoryItemModel();
		}

		#region IName

		[DataMember, XmlAttribute]
		public string Name { get; set; }

		[DataMember]
		public HistoryItemModel History { get; set; }

		#endregion

		#region IDeepClone

		public virtual void DeepCloneTo(IDeepClone target)
		{
			var obj = (IndexItemModelBase)target;

			obj.Name = Name;
			History.DeepCloneTo(obj);
		}

		public abstract IDeepClone DeepClone();

		#endregion
	}
}
