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
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;

	public class IndexBodyCaching : ILimitSize
	{
		#region variable

		int _limitSize;

		#endregion

		public IndexBodyCaching(int limitSize)
		{
			NoteItems = new IndexBodyPairItemCollection<NoteBodyItemModel>();
			ClipboardItems = new IndexBodyPairItemCollection<ClipboardBodyItemModel>();
			TemplateItems = new IndexBodyPairItemCollection<TemplateBodyItemModel>();

			LimitSize = limitSize;
		}

		#region property

		public IndexBodyPairItemCollection<NoteBodyItemModel> NoteItems { get; private set; }
		public IndexBodyPairItemCollection<ClipboardBodyItemModel> ClipboardItems { get; private set; }
		public IndexBodyPairItemCollection<TemplateBodyItemModel> TemplateItems { get; private set; }

		#endregion

		#region ILimitSize

		public int LimitSize
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
