namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class AcceptViewModel : HavingViewSingleModelWrapperViewModelBase<AcceptWindow, RunningInformationItemModel>
	{
		public AcceptViewModel(RunningInformationItemModel model, AcceptWindow view)
			: base(model, view)
		{ }

		#region property

		public bool CheckUpdateRelease
		{
			get { return Model.CheckUpdateRelease; }
			set { Model.CheckUpdateRelease = value; }
		}

		public bool CheckUpdateRC
		{
			get { return Model.CheckUpdateRC; }
			set { Model.CheckUpdateRC = value; }
		}

		public bool Accept
		{
			get { return Model.Accept; }
			set { Model.Accept = value; }
		}

		#endregion

		#region command


		public ICommand OkCommand
		{
			get
			{
				return new DelegateCommand(o => OnDailogCommand(o, true));
			}
		}

		public ICommand NgCommand
		{
			get
			{
				return new DelegateCommand(o => OnDailogCommand(o, false));
			}
		}

		#endregion

		#region function

		void OnDailogCommand(object sender, bool result)
		{
			Model.Accept = result;

			var window = sender as Window;
			if (window != null) {
				if (result) {
					window.DialogResult = true;
				} else {
					window.DialogResult = false;
				}
			}
		}

		public void SetAcceptDocument(WebBrowser browser, AppLanguageManager language, VariableConstants variableConstants)
		{
			var acceptSource = File.ReadAllText(language.AcceptDocumentFilePath);
			var acceptMap = new Dictionary<string, string>() {
				{"WEB", Constants.UriAbout },
				{"DEVELOPMENT", Constants.UriDevelopment },
				{"MAIL", Constants.MailAbout },
				{"FORUM", Constants.UriForum },
				{"FEEDBACK", Constants.UriFeedback },
				{"HELP", Constants.UriHelp },
				{"STYLE", File.ReadAllText(Path.Combine(variableConstants.ApplicationStyleDirectoryPath, Constants.styleCommonFileName), Encoding.UTF8) },
				{"APP", Constants.programName },
				{"OK", language["accept/ok"] },
				{"NG", language["accept/ng"] },
				{"CHECK-RELEASE", language["update-check/release"] },
				{"CHECK-RC", language["update-check/rc"] },
			};
			var replacedAcceptSource = acceptSource.ReplaceRangeFromDictionary("${", "}", acceptMap);

			browser.NavigateToString(replacedAcceptSource);
		}

		#endregion
	}
}
