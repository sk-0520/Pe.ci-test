namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class NoteMenuViewModel : SingleModelWrapperViewModelBase<NoteIndexItemModel>, IMenuItem
	{
		public NoteMenuViewModel(NoteIndexItemModel model, IAppSender appSender)
			: base(model)
		{
			AppSender = appSender;
		}

		#region property
		#endregion

		#region command

		public ICommand SelectedNoteMeneItemCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if (!Model.Visible) {
							
						} 
					}
				);

				return result;
			}
		}


		#endregion

		#region IMenuItem

		public ImageSource MenuImage { get { return null; } }
		public override string DisplayText { get { return DisplayTextUtility.GetDisplayName(Model); } }
		public ICommand MenuSelectedCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
					}
				);

				return result;
			}
		}

		#endregion

		#region IHavingAppSender

		public IAppSender AppSender { get; private set; }

		#endregion
	}
}
