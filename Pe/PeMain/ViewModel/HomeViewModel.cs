namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;
	using ContentTypeTextNet.Pe.PeMain.View.Parts;

	public class HomeViewModel: HavingViewModelBase<HomeWindow>, IHavingAppNonProcess
	{
		public HomeViewModel(HomeWindow view, IAppNonProcess appNonProcess)
			: base(view)
		{
			AppNonProcess = appNonProcess;
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
						SystemExecuteUtility.OpenNotificationAreaHistory(AppNonProcess);
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

						var startupPath = Environment.ExpandEnvironmentVariables(Constants.StartupShortcutPath);

						var image = MessageBoxImage.Information;
						string message;
						if(!File.Exists(startupPath)) {
							try {
								AppUtility.MakeAppShortcut(startupPath);
								message = AppNonProcess.Language["home/startup/dialog/message"];
							} catch(Exception ex) {
								AppNonProcess.Logger.Error(ex);
								message = ex.Message;
								image = MessageBoxImage.Error;
							}
						} else {
							message = AppNonProcess.Language["home/startup/exists"];
							AppNonProcess.Logger.Information(message, startupPath);
						}
						MessageBox.Show(message, AppNonProcess.Language["home/startup/dialog/caption"], MessageBoxButton.OK, image);
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

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion
	}
}
