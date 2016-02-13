/**
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using Library.PeData.Item;
using Data;
using Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Library.SharedLibrary.Logic;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class HtmlViewerUtility
    {
        #region define

        public const string appKey = "app-key";

        #endregion

        public static IEnumerable<string> ReadCommonStylesheet()
        {
            yield return File.ReadAllText(Path.Combine(Constants.ApplicationStyleDirectoryPath, Constants.styleCommonFileName));
        }

        public static IEnumerable< string> ReadCommonScript()
        {
            yield return File.ReadAllText(Path.Combine(Constants.ApplicationScriptDirectoryPath, Constants.ScriptJQueryFileName));
            yield return File.ReadAllText(Path.Combine(Constants.ApplicationScriptDirectoryPath, Constants.ScriptAutosizeFileName));
        }

        static string ＷrapElement(string tagName, string value, IReadOnlyDictionary<string, string> attribute = null)
        {
            if(string.IsNullOrEmpty(tagName)) {
                return string.Empty;
            }
            return string.Format("<{0}{2}>{1}</{0}>", tagName, value, attribute == null ? string.Empty: " " + string.Join(" ", attribute.Select(p => string.Format("{0}='{1}'", p.Key, p.Value))));
        }

        public static Dictionary<string,string> CreateBaseDictionary(ILanguage language, string customStyle, string customScript)
        {
            var result = new Dictionary<string, string>() {
                { "LANG", language.CultureCode },
                { "HTML:STYLE-COMMON", string.Join(Environment.NewLine,  ReadCommonStylesheet().Select(s => ＷrapElement("style", s))) },
                { "HTML:STYLE-CUSTOM", ＷrapElement("style", customStyle)},
                { "HTML:SCRIPT-COMMON", string.Join(Environment.NewLine, ReadCommonScript().Select(s => ＷrapElement("script", s))) },
                { "HTML:SCRIPT-CUSTOM", ＷrapElement("script", customScript) },
            };

            return result;
        }
        public static Dictionary<string, string> CreateBaseDictionary(ILanguage language)
        {
            return CreateBaseDictionary(language, null, null);
        }

        public static Dictionary<string, string> MakeCommonParameter(RunningInformationSettingModel runningInfo, ILanguage language, VariableConstants vc, ClipboardIndexItemCollectionModel clipboard, NoteIndexItemCollectionModel note, TemplateIndexItemCollectionModel template)
        {
            var result = new Dictionary<string, string>() {
                { "USER-SETTING:USER-ID", runningInfo.UserId },
                { "APPLICATION:BUILD", Constants.BuildType },
                { "ENVIRONMENT:OS", Environment.OSVersion.ToString() },
                { "ENVIRONMENT:CLR", RuntimeEnvironment.GetSystemVersion() },
                { "APPLICATION:CPU", Constants.BuildProcess },
                { "ENVIRONMENT:CPU", Environment.Is64BitOperatingSystem ? "64" : "32" },
                { "ENVIRONMENT:DISPLAY-COUNT", Screen.AllScreens.Length.ToString() },
                { "ENVIRONMENT:LANGUAGE", CultureInfo.InstalledUICulture.Name },
                { "APPLICATION:LANGUAGE", language.CultureCode },
                { "APPLICATION:FIRST-VERSION", runningInfo.FirstRunning.Version.ToString() },
                { "APPLICATION:FIRST-TIMESTAMP", runningInfo.FirstRunning.Timestamp.ToString(@"yyyy-MM-dd HH\:mm\:sszzz") },
                { "APPLICATION:EXECUTE-COUNT", runningInfo.ExecuteCount.ToString() },
                { "USER-SETTING:DIR", vc.UserSettingDirectoryPath },
                { "USER-SETTING:CLIPBOARD-COUNT", clipboard.Count.ToString() },
                { "USER-SETTING:TEMPLATE-COUNT", template.Count.ToString() },
                { "USER-SETTING:NOTE-COUNT", note.Count.ToString() },
            };
            using(var info = new AppInformationCollection()) {
                var infoMem = info.GetMemory();
                result["ENVIRONMENT:MEMORY"] = infoMem.Items["TotalVisibleMemorySize"].ToString();
            }

            return result;
        }
    }
}
