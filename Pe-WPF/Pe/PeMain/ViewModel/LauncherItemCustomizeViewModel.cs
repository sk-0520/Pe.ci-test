namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.View;
	using ContentTypeTextNet.Pe.PeMain.ViewModel.Control;

	public class LauncherItemCustomizeViewModel: LauncherItemEditViewModel, IHavingView<LauncherItemCustomizeWindow>
	{
		public LauncherItemCustomizeViewModel(LauncherItemModel model, LauncherItemCustomizeWindow view, LauncherIconCaching launcherIconCaching, INonProcess nonPorocess)
			: base(model, launcherIconCaching, nonPorocess)
		{
			View = view;
		}

		#region property
		#endregion

		#region command

		public ICommand SaveCommand
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

		public ICommand CancelCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						if(HasView) {
							View.Close();
						}
					}
				);

				return result;
			}
		}

		#endregion

		#region LauncherItemCustomizeWindow

		public LauncherItemCustomizeWindow View { get; private set; }

		public bool HasView { get { return View != null; } }

		#endregion
	}
}
