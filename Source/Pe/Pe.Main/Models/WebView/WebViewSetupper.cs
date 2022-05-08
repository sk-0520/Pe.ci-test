using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp.Wpf;
using CefSharp.Wpf.Experimental;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    public static class WebViewSetupper
    {
        #region function

        public static void SetupDefault(ChromiumWebBrowser browser)
        {
            browser.WpfKeyboardHandler = new WpfImeKeyboardHandler(browser);
        }

        #endregion
    }
}
