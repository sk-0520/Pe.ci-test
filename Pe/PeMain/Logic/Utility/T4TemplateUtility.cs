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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.PeMain.Logic.T4Template;
using Mono.TextTemplating;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    /// <summary>
    /// T4Templateの便利処理。
    /// </summary>
    public static class T4TemplateUtility
    {
        public static string TransformText(string templateContent)
        {
            var host = new TextTemplatingEngineHost();
            host.Session = new TextTemplatingSession();

            var engine = new Microsoft.VisualStudio.TextTemplating.Engine();
            return engine.ProcessTemplate(templateContent, host);
        }

        public static string TransformTextWidthVariable(string templateContent, IDictionary<string, object> variable)
        {
            var host = new TextTemplatingEngineHost();
            host.Session = new TextTemplatingSession();
            foreach(var pair in variable) {
                host.Session.Add(pair);
            }

            var engine = new Microsoft.VisualStudio.TextTemplating.Engine();
            return engine.ProcessTemplate(templateContent, host);
        }
    }
}
