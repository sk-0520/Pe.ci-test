using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.WebView
{
    public class WebViewinItializer
    {
        #region function

        public void Initialize(EnvironmentParameters environmentParameters)
        {
            var settings = new CefSharp.Wpf.CefSettings();

            settings.Locale = CultureService.Current.Culture.Parent.ToString();
            settings.AcceptLanguageList = CultureService.Current.Culture.Name;

            settings.CachePath = environmentParameters.TemporaryWebViewCacheDirectory.FullName;
            settings.UserDataPath = environmentParameters.MachineWebViewUserDirectory.FullName;

            settings.PersistSessionCookies = true;

            CefSharp.Cef.Initialize(settings);
        }

        #endregion
    }
}
