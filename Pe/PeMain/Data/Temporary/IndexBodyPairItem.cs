namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class IndexBodyPairItem<TIndexBody>
		where TIndexBody : IndexBodyItemModelBase
	{
		public IndexBodyPairItem(Guid id, TIndexBody body)
		{
			Id = id;
			Body = body;
		}

		#region property

		public Guid Id { get; private set; }
		public TIndexBody Body { get; private set; }

		#endregion
	}
}
