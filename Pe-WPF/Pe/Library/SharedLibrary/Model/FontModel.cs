namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;


	[Serializable]
	public class FontModel: ModelBase
	{
		[DataMember]
		public string Family { get; set; }
		[DataMember]
		public double Size { get; set; }
		[DataMember]
		public bool Bold { get; set; }
		[DataMember]
		public bool Italic { get; set; }
	}
}
