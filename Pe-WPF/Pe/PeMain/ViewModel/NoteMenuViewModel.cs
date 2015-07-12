namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class NoteMenuViewModel: SingleModelWrapperViewModelBase<NoteIndexItemModel>
	{
		public NoteMenuViewModel(NoteIndexItemModel model)
			: base(model)
		{ }
	}
}
