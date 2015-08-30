namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	public abstract class ExWebBrowser: WebBrowser
	{
		public ExWebBrowser(): base()
		{ }
	}

	/// <summary>
	/// https://social.msdn.microsoft.com/Forums/ie/en-US/8b0712ca-0b92-4e3d-a243-27af57a57213/idochostshowui-problem-c-webbrowser?forum=ieextensiondevelopment
	/// </summary>
	public class ShowMessageEventArgs: EventArgs
	{
		public ShowMessageEventArgs(string text, string caption, int type, string helpFile, int helpContext)
		{ }

		public bool Handled { get; set; }
		public int Result { get; set; }
		public int Type { get; private set; }
		public int HelpContext { get; private set; }
		public string Text { get; private set; }
		public string Caption { get; private set; }
		public string HelpFile { get; private set; }
	}

	/// <summary>
	/// https://social.msdn.microsoft.com/Forums/ie/en-US/8b0712ca-0b92-4e3d-a243-27af57a57213/idochostshowui-problem-c-webbrowser?forum=ieextensiondevelopment
	/// </summary>
	public abstract class ShowWebBrowser: ExWebBrowser
	{
		protected class ShowWebBrowserSite: global::System.Windows.Forms.WebBrowser.WebBrowserSite, IDocHostShowUI
		{
			private readonly ShowWebBrowser host;

			public ShowWebBrowserSite(ShowWebBrowser host)
				: base(host)
			{
				this.host = host;
			}

			#region IDocHostShowUI
			public int ShowMessage(IntPtr hwnd, string lpstrText, string lpstrCaption, int dwType, string lpstrHelpFile, int dwHelpContext, out int lpResult)
			{
				var e = new ShowMessageEventArgs(lpstrText, lpstrCaption, dwType, lpstrHelpFile, dwHelpContext);
				this.host.OnShowMessage(e);

				if(e.Handled) {
					lpResult = e.Result;
					return 0;
				} else {
					lpResult = 0;
					return 1;
				}
			}

			public int ShowHelp(IntPtr hwnd, string pszHelpFile, int uCommand, int dwData, POINT ptMouse, object pDispatchObjectHit)
			{
				return 1;
			}
			#endregion
		}

		protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
		{
			return new ShowWebBrowserSite(this);
		}

		protected virtual void OnShowMessage(ShowMessageEventArgs e)
		{
			var handler = this.Events["ShowMessage"] as EventHandler<ShowMessageEventArgs>;

			if(handler != null) {
				handler(this, e);
			}
		}

		public event EventHandler<ShowMessageEventArgs> ShowMessage
		{
			add { this.Events.AddHandler("ShowMessage", value); }
			remove { this.Events.RemoveHandler("ShowMessage", value); }
		}
	}
}
