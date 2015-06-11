namespace ContentTypeTextNet.Pe.PeMain.View
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Data;
	using System.Windows.Documents;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	/// <summary>
	/// AcceptWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class AcceptWindow: ViewModelCommonDataWindow<AcceptViewModel>
	{
		public AcceptWindow()
		{
			InitializeComponent();
		}

		#region CommonDataWindow

		protected override void CreateViewModel()
		{
			ViewModel = new AcceptViewModel(CommonData.MainSetting.RunningInformation);
		}

		protected override void ApplyViewModel()
		{
			DataContext = ViewModel;

			var acceptSource = File.ReadAllText(CommonData.Language.AcceptDocumentFilePath);
			var acceptMap = new Dictionary<string, string>() {
				{"WEB", Constants.UriAbout },
				{"DEVELOPMENT", Constants.UriDevelopment },
				{"MAIL", Constants.MailAbout },
				{"FORUM", Constants.UriForum },
				{"FEEDBACK", Constants.UriFeedback },
				{"HELP", Constants.UriHelp },
				{"STYLE", File.ReadAllText(Path.Combine(CommonData.VariableConstants.ApplicationStyleDirectoryPath, Constants.styleCommonFileName), Encoding.UTF8) },
				{"APP", Constants.programName },
				{"OK", CommonData.Language["accept/ok"] },
				{"NG", CommonData.Language["accept/ng"] },
				{"CHECK-RELEASE", CommonData.Language["update-check/release"] },
				{"CHECK-RC", CommonData.Language["update-check/rc"] },
			};
			var replacedAcceptSource = acceptSource.ReplaceRangeFromDictionary("${", "}", acceptMap);

			webDocument.NavigateToString(replacedAcceptSource);

			base.ApplyViewModel();
		}

		#endregion
	}
}
