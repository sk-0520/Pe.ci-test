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
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.Define;
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

		public ICommand OpenHistoryCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var path = Environment.ExpandEnvironmentVariables(Path.Combine(AppNonProcess.VariableConstants.ApplicationDocumentDirectoryPath, Constants.changelogFileName));
						ExecuteUtility.OpenFile(path, AppNonProcess);
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
							AppNonProcess.Language["about/update/dialog/message"],
							AppNonProcess.Language["about/update/dialog/caption"],
							MessageBoxButton.YesNo,
							MessageBoxImage.Information
						);
						if(messageResult == MessageBoxResult.Yes) {
							Notifiy.CheckUpdate = true;

							CloseView();
						}
					}
				);

				return result;
			}
		}

		public ICommand CloseCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						CloseView();
					}
				);

				return result;
			}
		}

		public ICommand OpenLinkCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var command = (string)o;
					}
				);

				return result;
			}
		}

		public ICommand CopyLinkCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						var copyKind = (AboutCopyKind)o;
					}
				);

				return result;
			}
		}

		#endregion

		#region function

		void CloseView()
		{
			if(HasView) {
				View.Close();
			}
		}

		#endregion

		#region IHavingAppNonProcess

		public IAppNonProcess AppNonProcess { get; private set; }

		#endregion
	}
}
