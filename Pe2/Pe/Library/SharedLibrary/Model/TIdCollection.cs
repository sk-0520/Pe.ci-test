namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[DataContract]
	public class TIdCollection<T>: ModelBase
		where T: ITId<T>
	{
		public TIdCollection()
			: base()
		{ }


	}
}
