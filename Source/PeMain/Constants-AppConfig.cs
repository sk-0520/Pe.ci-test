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
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

namespace ContentTypeTextNet.Pe.PeMain
{
    partial class Constants
    {
        #region app.config

        /// <summary>
        /// 文字列リテラルを書式で変換。
        /// 
        /// {...} を置き換える。
        /// * TIMESTAMP: そんとき
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private static string ReplaceAppConfig(string src)
        {
            var map = new Dictionary<string, string>() {
                { "TIMESTAMP", DateTime.Now.ToBinary().ToString() },
            };
            var replacedText = src.ReplaceRangeFromDictionary("{", "}", map);

            return replacedText;
        }

        public static string UriAbout { get { return appCaching.Get("uri-about"); } }
        public static string MailAbout { get { return appCaching.Get("mail-about"); } }
        public static string UriDevelopment { get { return appCaching.Get("uri-development"); } }
        public static string UriUpdate { get { return ReplaceAppConfig(appCaching.Get("uri-update")); } }
        public static string UriChangelogRelease { get { return ReplaceAppConfig(appCaching.Get("uri-changelog-release")); } }
        public static string UriChangelogRc { get { return ReplaceAppConfig(appCaching.Get("uri-changelog-rc")); } }
        public static string UriForum { get { return appCaching.Get("uri-forum"); } }
        public static string UriFeedback { get { return appCaching.Get("uri-feedback"); } }
        public static string UriUserInformation { get { return appCaching.Get("uri-user-information"); } }

        public static int LoggingStockCount { get { return appCaching.Get("logging-stock-count", int.Parse); } }
        public static int CacheIndexBodyTemplate { get { return appCaching.Get("cache-index-body-template", int.Parse); } }
        public static int CacheIndexBodyClipboard { get { return appCaching.Get("cache-index-body-clipboard", int.Parse); } }
        public static TimeSpan SaveIndexClipboardTime { get { return appCaching.Get("save-index-clipboard-time", TimeSpan.Parse); } }
        public static TimeSpan SaveIndexTemplateTime { get { return appCaching.Get("save-index-template-time", TimeSpan.Parse); } }
        public static TimeSpan SaveIndexNoteTime { get { return appCaching.Get("save-index-note-time", TimeSpan.Parse); } }

        public static int BackupSettingCount { get { return appCaching.Get("backup-setting", int.Parse); } }
        public static int BackupArchiveCount { get { return appCaching.Get("backup-archive", int.Parse); } }

        public static TimeSpan FullScreenIgnoreTime { get { return appCaching.Get("fullscreen-ignore-time", TimeSpan.Parse); } }

        public static TimeSpan TemplateBodyArchiveTimeSpan { get { return appCaching.Get("template-archive-time", TimeSpan.Parse); } }
        public static long TemplateBodyArchiveFileSize { get { return appCaching.Get("template-archive-size", long.Parse); } }

        public static TimeSpan NoteBodyArchiveTimeSpan { get { return appCaching.Get("note-archive-time", TimeSpan.Parse); } }
        public static long NoteBodyArchiveFileSize { get { return appCaching.Get("note-archive-size", long.Parse); } }

        public static TimeSpan ClipboardBodyArchiveTimeSpan { get { return appCaching.Get("clipboard-archive-time", TimeSpan.Parse); } }
        public static long ClipboardBodyArchiveFileSize { get { return appCaching.Get("clipboard-archive-size", long.Parse); } }

        public static TimeSpan IdleWatchTime { get { return appCaching.Get("idle-watch-time", TimeSpan.Parse); } }
        public static TimeSpan IdleJudgeTime { get { return appCaching.Get("idle-judge-time", TimeSpan.Parse); } }

        #endregion
    }
}
