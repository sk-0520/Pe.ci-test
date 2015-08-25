namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.View;

	public class UpdateConfirmViewModel: HavingViewModelBase<UpdateConfirmWindow>
	{
		public UpdateConfirmViewModel(UpdateConfirmWindow view, Updater updater)
			:base(view)
		{
			Updater = updater;
		}

		#region property

		Updater Updater { get; set; }

		public string NewVersion { get { return Updater.Information.Version; } }
		public bool IsRcVersion { get { return Updater.Information.IsRcVersion; } }

		public Brush VersionForeground
		{
			get
			{
				if(Updater.Information.IsRcVersion) {
					return new SolidColorBrush() {
						Color = Colors.Red,
					};
				} else {
					if(HasView) {
						return View.Foreground;
					} else {
						return SystemColors.WindowTextBrush;
					}
				}
			}
		}
		public Brush VersionBackground
		{
			get
			{
				if(Updater.Information.IsRcVersion) {
					return new SolidColorBrush() {
						Color = Colors.Black,
					};
				} else {
					return new SolidColorBrush() {
						Color = Colors.Transparent,
					};
				}
			}
		}

		#endregion

		#region command
		
		public ICommand CancelCommand
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

		public ICommand UpdateCommand
		{
			get
			{
				var result = CreateCommand(
					o => {
						Updater.ApprovalUpdate = true;
						CloseView();
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

		public void SetUpdateDocument(WebBrowser browser)
		{
			byte[] httpData = null;
			using(var web = new WebClient()) {
				var url = Updater.Information.IsRcVersion ? Constants.UriChangelogRc: Constants.UriChangelogRelease; 
				httpData = web.DownloadData(url);
			}
			if(HasView) {
				browser.NavigateToStream(new MemoryStream(httpData));
			}
		}

		#endregion
	}
}
