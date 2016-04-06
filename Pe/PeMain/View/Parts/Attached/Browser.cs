/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Attached
{
    /// <summary>
    /// <para>http://stackoverflow.com/questions/4202961/can-i-bind-html-to-a-wpf-web-browser-control?answertab=votes#tab-top</para>
    /// </summary>
    public class Browser
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached(
            "Html",
            typeof(string),
            typeof(Browser),
            new FrameworkPropertyMetadata(OnHtmlChanged)
        );

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
            if(webBrowser != null) {
                var bom = new byte[] { 0xef, 0xbb, 0xbf };
                var buffer = Encoding.UTF8.GetBytes(e.NewValue as string ?? "&nbsp;");
                var stream = new MemoryStream();
                stream.Write(bom, 0, bom.Length);
                stream.Write(buffer, 0, buffer.Length);
                stream.Seek(0, SeekOrigin.Begin);
                webBrowser.NavigateToStream(stream);
            }
        }
    }
}
