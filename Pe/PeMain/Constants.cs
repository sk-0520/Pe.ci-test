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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using System.Windows;
using System.Windows.Media;
using ContentTypeTextNet.Pe.PeMain.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain
{
    /// <summary>
    /// 定数。
    /// </summary>
    public static partial class Constants
    {
        //static Constants()
        //{
        //    FullScreenIgnoreTime = appCaching.Get("fullscreen-ignore-time"]);
        //}

        static readonly ConfigurationCaching appCaching = new ConfigurationCaching();

        [ConstantsProperty]
        const string applicationName = "Pe";
        public const string updateProgramDirectoryName = "Updater";
        public const string updateProgramName = "Updater" + ".exe";
        /// <summary>
        /// 前回バージョンがこれ未満なら使用許諾を表示
        /// </summary>
        [ConstantsProperty]
        static readonly Version acceptVersion = new Version(0, 72, 0, 0);

        /// <summary>
        /// Forms版のバージョン。
        /// </summary>
        [ConstantsProperty]
        static readonly Version formsVersion = new Version(0, 62, 0, 42674);

        const string shortcutNameDebug = applicationName + "(DEBUG).lnk";
        const string shortcutNameBeta = applicationName + "(BETA).lnk";
        const string shortcutNameRelease = applicationName + ".lnk";

        const string buildTypeDebug = "DEBUG";
        const string buildTypeBeta = "β";
        const string buildTypeRelease = "RELEASE";

        [ConstantsProperty]
        static readonly string buildProcess = Environment.Is64BitProcess ? "64" : "32";

        public const string keyGuidName = "${GUID}";
        public const string keyIndexExt = "${EXT}";

        public const string binDirectoryName = "bin";
        public const string sbinDirectoryName = "sbin";
        public const string libraryDirectoryName = "lib";
        public const string etcDirectoryName = "etc";
        public const string languageDirectoryName = "lang";
        public const string styleDirectoryName = "style";
        public const string scriptDirectoryName = "script";
        const string htmlDirectoryName = "html";
        public const string documentDirectoryName = "doc";
        const string helpDirectoryName = "help";

        public const string logDirectoryName = "logs";
        public const string settingDirectoryName = "setting";
        public const string backupDirectoryName = "backup";
        public const string archiveDirectoryName = "archive";

        public const string formsMainSettingFileName = "mainsetting.xml";
        public const string formsLauncherItemsSettingFileName = "launcher-items.xml";

        public const string mainSettingFileName = "main-setting.json";
        public const string launcherItemSettingFileName = "launcher-items.json";
        public const string launcherGroupItemSettingFileName = "group-items.json";

        public const string changelogFileName = "changelog.xml";
        public const string componentListFileNam = "components.xml";

        public const int indexBodyCachingSize = 3;
        public const string indexBodyBaseFileName = keyGuidName + "." + keyIndexExt;

        public const string noteSaveDirectoryName = "notes";
        public const string noteIndexFileName = "note-index.json";

        public const string clipboardSaveDirectoryName = "clipboards";
        public const string clipboardIndexFileName = "clipboard-index.json";

        public const string templateSaveDirectoryName = "templates";
        public const string templateIndexFileName = "template-index.json";

        [ConstantsProperty]
        public const string styleCommonFileName = "common.css";
        [ConstantsProperty]
        const string scriptJQueryFileName = "jquery.js";
        [ConstantsProperty]
        const string scriptAutosizeFileName = "autosize.js";
        [ConstantsProperty]
        const string htmlFeedbackFileName = "feedback.html";
        [ConstantsProperty]
        const string styleFeedbackFileName = "feedback.css";

        [ConstantsProperty]
        const string helpIndexFileName = "help.html";

        public const string languageDefaultFileName = "default.xml";
        public const string languageSearchPattern = "*.xml";

        [ConstantsProperty]
        public const string archiveSearchPattern = "*.zip";
        [ConstantsProperty]
        public const string backupSearchPattern = "*.zip";

        public const string dialogFilterText = "*.txt";
        public const string dialogFilterRtf = "*.rtf";
        public const string dialogFilterHtml = "*.html";
        public const string dialogFilterPng = "*.png";
        public const string dialogFilterLog = "*.log";
        public const string dialogFilterAll = "*.*";

        const string formatTimestampFileName = "yyyy-MM-dd_HH-mm-ss";
        [ConstantsProperty]
        const string formatGuidFileName = "d";

        public const string languageAcceptDocumentExtension = "accept.html";

        public const string logFileExtension = "log";

        [ConstantsProperty]
        const string extensionBinaryFile = "dat";
        [ConstantsProperty]
        const string extensionJsonFile = "json";

        [ConstantsProperty]
        const string indexBinaryFileSearchPattern = "*." + extensionBinaryFile;
        [ConstantsProperty]
        const string indexJsonFileSearchPattern = "*." + extensionJsonFile;

        [ConstantsProperty]
        static readonly string bodyArchiveFileName = Guid.Empty.ToString(formatGuidFileName) + ".zip";

        [ConstantsProperty]
        const string extensionTemporaryFile = "tmp";
        [ConstantsProperty]
        const string temporaryFileSearchPattern = "*." + extensionTemporaryFile;

        public const FileType fileTypeMainSetting = FileType.Json;
        public const FileType fileTypeLauncherItemSetting = FileType.Json;
        public const FileType fileTypeLauncherGroupSetting = FileType.Json;
        public const FileType fileTypeNoteIndex = FileType.Json;
        public const FileType fileTypeNoteBody = FileType.Json;
        public const FileType fileTypeTemplateIndex = FileType.Json;
        public const FileType fileTypeTemplateBody = FileType.Json;
        public const FileType fileTypeClipboardIndex = FileType.Json;
        public const FileType fileTypeClipboardBody = FileType.Binary;

        public static readonly TimeSpan iconLoadWaitTime = TimeSpan.FromMilliseconds(250);
        public const int iconLoadRetryMax = 3;

        public const int updateArchiveCount = 15;
#if DEBUG
        public static readonly TimeSpan updateWaitTime = TimeSpan.FromSeconds(1);
#else
		public static readonly TimeSpan updateWaitTime = TimeSpan.FromSeconds(30);
#endif

        public const int screenCountChangeRetryCount = 10;
        public static readonly TimeSpan screenCountChangeWaitTime = TimeSpan.FromMilliseconds(250);

        static readonly TripleRange<double> defaultFontSize = new TripleRange<double>(
            8,
            12.5,
            72
        );

        [ConstantsRange]
        public static readonly TripleRange<int> loggingStockCount = new TripleRange<int>(
            0,
            1 * 1024,
            5 * 1024
        );

        public static Size loggingDefaultWindowSize = new Size(320, 480);

        [ConstantsRange]
        public static readonly TripleRange<double> streamFontSize = new TripleRange<double>(
            defaultFontSize.minimum,
            defaultFontSize.median,
            defaultFontSize.maximum
        );
        public static readonly ColorPairItemModel streamOutputColor = new ColorPairItemModel(
            Colors.White,
            Colors.Black
        );
        public static readonly ColorPairItemModel streamErrorColor = new ColorPairItemModel(
            Colors.Red,
            Colors.Black
        );

        [ConstantsRange]
        public static readonly TripleRange<TimeSpan> commandHideTime = new TripleRange<TimeSpan>(
            TimeSpan.FromMilliseconds(250),
            TimeSpan.FromSeconds(2),
            TimeSpan.FromSeconds(10)
        );
        [ConstantsRange]
        public static readonly TripleRange<double> commandWindowWidth = new TripleRange<double>(
            250,
            400,
            800
        );
        [ConstantsRange]
        public static readonly TripleRange<double> commandFontSize = new TripleRange<double>(
            defaultFontSize.minimum,
            defaultFontSize.median,
            defaultFontSize.maximum
        );

        [ConstantsRange]
        public static readonly TripleRange<int> toolbarTextLength = new TripleRange<int>(
            20,
            80,
            200
        );
        [ConstantsRange]
        public static readonly TripleRange<double> toolbarFontSize = new TripleRange<double>(
            8,
            14,
            64
        );

        [ConstantsRange]
        public static readonly TripleRange<TimeSpan> toolbarHideWaitTime = new TripleRange<TimeSpan>(
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(3),
            TimeSpan.FromSeconds(10)
        );

        [ConstantsRange]
        public static readonly TripleRange<TimeSpan> toolbarHideAnimateTime = new TripleRange<TimeSpan>(
            TimeSpan.FromMilliseconds(100),
            TimeSpan.FromMilliseconds(250),
            TimeSpan.FromSeconds(1)
        );

        public static readonly TimeSpan clipboardGetDataRetryWaitTime = TimeSpan.FromMilliseconds(250);
        public static int clipboardGetDataRetryMaxCount = 3;

        public const ClipboardType clipboardCaptureType = ClipboardType.All;
        public const ClipboardType clipboardLimitType = ClipboardType.All ^ ClipboardType.Text;

        [ConstantsRange]
        public static readonly TripleRange<TimeSpan> clipboardWaitTime = new TripleRange<TimeSpan>(
            TimeSpan.FromMilliseconds(50),
            TimeSpan.FromMilliseconds(500),
            TimeSpan.FromSeconds(1)
        );

        [ConstantsRange]
        public static readonly TripleRange<int> clipboardSaveCount = new TripleRange<int>(
            0,
            1024,
            1024 * 10
        );

        [ConstantsRange]
        public static readonly TripleRange<int> clipboardDuplicationCount = new TripleRange<int>(
            -1,
            -1,
            256
        );

        public const double clipboardItemsListWidth = 220;
        public static readonly Size clipboardDefaultWindowSize = new Size(580, 380);
        [ConstantsRange]
        public static readonly TripleRange<double> clipboardFontSize = new TripleRange<double>(
            defaultFontSize.minimum,
            defaultFontSize.median,
            defaultFontSize.maximum
        );
        /// <summary>
        /// 文字列。
        /// <para>Unicode文字数。</para>
        /// </summary>
        [ConstantsRange]
        public static TripleRange<int> clipboardLimitTextSize = new TripleRange<int>(
            500,
            1000,
            10000
        );
        /// <summary>
        /// RTF。
        /// <para>byte数。</para>
        /// </summary>
        [ConstantsRange]
        public static TripleRange<int> clipboardLimitRtfSize = new TripleRange<int>(
            1 * 1024,
            10 * 1024,
            10 * 1024 * 1024
        );
        [ConstantsRange]
        public static TripleRange<int> clipboardLimitHtmlSize = new TripleRange<int>(
            1 * 1024,
            5 * 10 * 1024,
            30 * 1024 * 1024
        );
        static TripleRange<int> clipboardLimitImageSize = new TripleRange<int>(
            1,
            2 * 1024,
            8 * 1024
        );
        [ConstantsRange]
        public static TripleRange<int> clipboardLimitImageWidthSize = new TripleRange<int>(
            clipboardLimitImageSize.minimum,
            clipboardLimitImageSize.median,
            clipboardLimitImageSize.maximum
        );
        [ConstantsRange]
        public static TripleRange<int> clipboardLimitImageHeightSize = new TripleRange<int>(
            clipboardLimitImageSize.minimum,
            clipboardLimitImageSize.median,
            clipboardLimitImageSize.maximum
        );

        public const double templateItemsListWidth = 180;
        public const double templateReplaceListWidth = 100;
        public static readonly Size templateDefaultWindowSize = new Size(580, 380);
        [ConstantsRange]
        public static readonly TripleRange<double> templateFontSize = new TripleRange<double>(
            defaultFontSize.minimum,
            defaultFontSize.median,
            defaultFontSize.maximum
        );
        static readonly Color templateStandardControlColor = Color.FromRgb(0, 255, 0);
        static readonly Color templateClassControlColor = Color.FromRgb(0, 255, 0);
        static readonly Color templateExpressionControlColor = Color.FromRgb(0, 255, 0);
        [ConstantsProperty]
        static readonly ColorPairItemModel templateT4ControlColor = new ColorPairItemModel(
            Colors.Black,
            Colors.Yellow
        );
        [ConstantsProperty]
        static readonly ColorPairItemModel templateT4ClassColor = new ColorPairItemModel(
            Colors.SteelBlue,
            Colors.Yellow
        );
        [ConstantsProperty]
        static readonly ColorPairItemModel templateT4ExpressionColor = new ColorPairItemModel(
            Colors.Blue,
            Colors.Yellow
        );

        [ConstantsRange]
        public static readonly TripleRange<TimeSpan> windowSaveIntervalTime = new TripleRange<TimeSpan>(
            TimeSpan.FromSeconds(5),
            TimeSpan.FromMinutes(10),
            TimeSpan.FromHours(1)
        );
        [ConstantsRange]
        public static readonly TripleRange<int> windowSaveCount = new TripleRange<int>(
            3,
            10,
            20
        );

        public static readonly Thickness noteCaptionPadding = new Thickness(2);
        public const double noteCaptionHeight = 20;
        public static readonly ColorPairItemModel noteColor = new ColorPairItemModel(
            Colors.Black,
            Color.FromRgb(250, 250, 180)
        );

        [ConstantsRange]
        public static readonly TripleRange<double> noteFontSize = new TripleRange<double>(
            defaultFontSize.minimum,
            defaultFontSize.median,
            defaultFontSize.maximum
        );
        public static readonly Size noteDefualtSize = new Size(200, 200);

        [ConstantsProperty]
        const string htmlViewerTagReplaceBreak = ",,";

        /// <summary>
        /// 実行パス
        /// </summary>
        public static readonly string applicationExecutablePath = Assembly.GetExecutingAssembly().Location;
        /// <summary>
        /// 起動ディレクトリ
        /// </summary>
        public static readonly string applicationRootDirectoryPath = Path.GetDirectoryName(applicationExecutablePath);
        /// <summary>
        /// バージョン番号。
        /// </summary>
        public static readonly Version applicationVersionNumber = Assembly.GetExecutingAssembly().GetName().Version;
        /// <summary>
        /// バージョンリビジョン。
        /// </summary>
        public static readonly string applicationVersionRevision = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
        /// <summary>
        /// アプリケーションバージョン。
        /// </summary>
        public static readonly string applicationVersion = applicationVersionNumber.ToString() + "-" + applicationVersionRevision;
        /// <summary>
        /// スタートアップ用ショートカットファイルパス。
        /// </summary>
        [ConstantsProperty]
        static readonly string startupShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), ShortcutName);

        #region

        [ConstantsProperty]
        const string issue_355_logFileName = "session-ending.log";

        [ConstantsProperty]
        const int issue_363_oldMediumCount = 50;


        #endregion

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

        #endregion

        #region property

        //public static string ApplicationName { get { return applicationName; } }
        //public static string BuildType { get { return buildType; } }
        //public static string BuildProcess { get { return buildProcess; } }
        public static string ApplicationVersion { get { return applicationVersion; } }
        public static Version ApplicationVersionNumber { get { return applicationVersionNumber; } }
        public static string ApplicationVersionRevision { get { return applicationVersionRevision; } }

        /// <summary>
        /// bin/
        /// </summary>
        public static string ApplicationBinDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, binDirectoryName); } }
        /// <summary>
        /// sbin/
        /// </summary>
        public static string ApplicationSBinDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, sbinDirectoryName); } }
        /// <summary>
        /// lib/
        /// </summary>
        public static string ApplicationLibraryDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, libraryDirectoryName); } }
        /// <summary>
        /// etc/
        /// </summary>
        public static string ApplicationEtcDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, etcDirectoryName); } }
        /// <summary>
        /// etc/lang
        /// </summary>
        public static string ApplicationLanguageDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, languageDirectoryName); } }
        /// <summary>
        /// etc/style
        /// </summary>
        public static string ApplicationStyleDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, styleDirectoryName); } }
        /// <summary>
        /// etc/script
        /// </summary>
        public static string ApplicationScriptDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, scriptDirectoryName); } }
        /// <summary>
        /// etc/html
        /// </summary>
        public static string ApplicationHtmlDirectoryPath { get { return Path.Combine(ApplicationEtcDirectoryPath, htmlDirectoryName); } }
        /// <summary>
        /// doc/
        /// </summary>
        public static string ApplicationDocumentDirectoryPath { get { return Path.Combine(applicationRootDirectoryPath, documentDirectoryName); } }
        /// <summary>
        /// doc/help
        /// </summary>
        public static string ApplicationHelpDirectoryPath { get { return Path.Combine(ApplicationDocumentDirectoryPath, helpDirectoryName); } }

        #endregion

        #region function

        public static string GetTimestampFileName(DateTime dateTime)
        {
            return dateTime.ToString(formatTimestampFileName);
        }

        public static string GetNowTimestampFileName()
        {
            return GetTimestampFileName(DateTime.Now);
        }

        /// <summary>
        /// 一時ファイル用拡張子の作成
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryExtension(string role)
        {
            return "." + Constants.GetNowTimestampFileName() + "." + role + "." + extensionTemporaryFile;
        }

        #endregion
    }
}
