namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;


	[Serializable]
	public class FontModel: ModelBase, IDeepClone
	{
		[DataMember]
		public string Family { get; set; }
		[DataMember]
		public double Size { get; set; }
		[DataMember]
		public bool Bold { get; set; }
		[DataMember]
		public bool Italic { get; set; }

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (FontModel)target;

			obj.Family = Family;
			obj.Bold = Bold;
			obj.Italic = Italic;
		}

		public IDeepClone DeepClone()
		{
			var result = new FontModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
