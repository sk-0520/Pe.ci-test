namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class NoteMenuViewModel: SingleModelWrapperViewModelBase<NoteIndexItemModel>
	{
		public NoteMenuViewModel(NoteIndexItemModel model)
			: base(model)
		{ }

		#region property
		#endregion

		#region command

		public ICommand SelectedNoteMeneItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if (Model.Visible) {
						} 
					}
				);

				return result;
			}
		}


		#endregion

		#region SingleModelWrapperViewModelBase

		public override string DisplayText
		{
			get
			{
				return DisplayTextUtility.GetDisplayName(Model);
			}
		}
		#endregion
	}
}
