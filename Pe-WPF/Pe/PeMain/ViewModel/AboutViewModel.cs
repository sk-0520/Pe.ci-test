namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class AboutViewModel: HavingViewModelBase<AboutWindow>, IHavingAppNonProcess 
	{
		public AboutViewModel(AboutWindow view, AboutNotifiyItem notifiy, IAppNonProcess appNonProcess)
			: base(view)
		{
			CheckUtility.DebugEnforceNotNull(notifiy);

			Notifiy = notifiy;
			AppNonProcess = appNonProcess;
		}

		#region property

		AboutNotifiyItem Notifiy { get; set; }

		#endregion

		#region command

		public ICommand OpenApplicationDirectoryCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						ExecuteUtility.OpenDirectory(Constants.applicationRootDirectoryPath, AppNonProcess, null);
					}
				);

				return result;
			}
		}

		public ICommand OpenUserSettingDirectoryCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var path = Environment.ExpandEnvironmentVariables(AppNonProcess.VariableConstants.UserSettingDirectoryPath);
						ExecuteUtility.OpenDirectory(path, AppNonProcess, null);
					}
				);

				return result;
			}
		}

		public ICommand UpdateCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var messageResult = MessageBox.Show(
							AppNonProcess.Language["about/update/dialog/caption"],
							AppNonProcess.Language["about/update/dialog/message"],
							MessageBoxButton.YesNo,
							MessageBoxImage.Information
						);
						if(messageResult == MessageBoxResult.Yes) {
							Notifiy.CheckUpdate = true;

							if(HasView) {
								View.Close();
							}
						}
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
