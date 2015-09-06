namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.PeMain.View;
using ContentTypeTextNet.Pe.PeMain.View.Parts;

	public class HomeViewModel: HavingViewModelBase<HomeWindow>
	{
		public HomeViewModel(HomeWindow view)
			: base(view)
		{
		}

		#region property
		#endregion

		#region command

		public ICommand CloseCommand
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

		public ICommand ShowNotifyAreaCommand
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

		public ICommand ResistStartupCommand
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

		public ICommand ResistItemsCommand
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
	}
}
