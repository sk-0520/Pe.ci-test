namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class IndexBodyCaching : ILimitSize
	{
		#region variable

		int _limitSize;

		#endregion

		public IndexBodyCaching(int limitSize)
		{
			NoteItems = new FixedSizeCollectionModel<NoteBodyItemModel>();
			ClipboardItems = new FixedSizeCollectionModel<ClipboardBodyItemModel>();
			TemplateItems = new FixedSizeCollectionModel<TemplateBodyItemModel>();

			LimitSize = limitSize;
		}

		#region property

		public FixedSizeCollectionModel<NoteBodyItemModel> NoteItems { get; private set; }
		public FixedSizeCollectionModel<ClipboardBodyItemModel> ClipboardItems { get; private set; }
		public FixedSizeCollectionModel<TemplateBodyItemModel> TemplateItems { get; private set; }

		#endregion

		#region ILimitSize

		int LimitSize
		{
			get { return this._limitSize; }
			set
			{
				this._limitSize = value;
				var lists = new ILimitSize[] {
					NoteItems,
					ClipboardItems,
					TemplateItems,
				};
				foreach (var list in lists) {
					list.LimitSize = this._limitSize;
				}
			}
		}

		#endregion
	}
}
