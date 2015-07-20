namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Controls;

	/// <summary>
	/// <para>http://stackoverflow.com/questions/4202961/can-i-bind-html-to-a-wpf-web-browser-control?answertab=votes#tab-top</para>
	/// </summary>
	public class Browser
	{
		public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
				"Html",
				typeof(string),
				typeof(Browser),
				new FrameworkPropertyMetadata(OnHtmlChanged));

		[AttachedPropertyBrowsableForType(typeof(WebBrowser))]
		public static string GetHtml(WebBrowser d)
		{
			return (string)d.GetValue(HtmlProperty);
		}

		public static void SetHtml(WebBrowser d, string value)
		{
			d.SetValue(HtmlProperty, value);
		}

		static void OnHtmlChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
		{
			WebBrowser webBrowser = dependencyObject as WebBrowser;
			if(webBrowser != null)
				webBrowser.NavigateToString(e.NewValue as string ?? "&nbsp;");
		}
	}
}
