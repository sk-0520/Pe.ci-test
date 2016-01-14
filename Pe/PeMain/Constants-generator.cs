namespace ContentTypeTextNet.Pe.PeMain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;
    using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using ContentTypeTextNet.Pe.PeMain.Logic;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.Library.PeData.Item;

    partial class Constants
    {
        /*
        このソースは自動生成のため Constants-generator.tt を編集すること。

        生成元ConstantsPropertyフィールド数: 29
        生成元ConstantsRangeフィールド数: 22
        */

        // ConstantsPropertyAttribute
        #region acceptVersion
        
        public static Version AcceptVersion
        {
            get
            {
                return acceptVersion;
            }
        }

        #endregion
        #region formsVersion
        
        public static Version FormsVersion
        {
            get
            {
                return formsVersion;
            }
        }

        #endregion
        #region buildProcess
        
        public static String BuildProcess
        {
            get
            {
                return buildProcess;
            }
        }

        #endregion
        #region bodyArchiveTimeSpan
        
        public static TimeSpan BodyArchiveTimeSpan
        {
            get
            {
                return bodyArchiveTimeSpan;
            }
        }

        #endregion
        #region templateT4ControlColor
        
        public static ColorPairItemModel TemplateT4ControlColor
        {
            get
            {
                return templateT4ControlColor;
            }
        }

        #endregion
        #region templateT4ClassColor
        
        public static ColorPairItemModel TemplateT4ClassColor
        {
            get
            {
                return templateT4ClassColor;
            }
        }

        #endregion
        #region templateT4ExpressionColor
        
        public static ColorPairItemModel TemplateT4ExpressionColor
        {
            get
            {
                return templateT4ExpressionColor;
            }
        }

        #endregion
        #region startupShortcutPath
        
        public static String StartupShortcutPath
        {
            get
            {
                return startupShortcutPath;
            }
        }

        #endregion
        #region applicationName
        
        public static String ApplicationName
        {
            get
            {
                return applicationName;
            }
        }

        #endregion
        #region styleCommonFileName
        
        public static String StyleCommonFileName
        {
            get
            {
                return styleCommonFileName;
            }
        }

        #endregion
        #region scriptJQueryFileName
        
        public static String ScriptJQueryFileName
        {
            get
            {
                return scriptJQueryFileName;
            }
        }

        #endregion
        #region scriptAutosizeFileName
        
        public static String ScriptAutosizeFileName
        {
            get
            {
                return scriptAutosizeFileName;
            }
        }

        #endregion
        #region htmlFeedbackFileName
        
        public static String HtmlFeedbackFileName
        {
            get
            {
                return htmlFeedbackFileName;
            }
        }

        #endregion
        #region styleFeedbackFileName
        
        public static String StyleFeedbackFileName
        {
            get
            {
                return styleFeedbackFileName;
            }
        }

        #endregion
        #region helpIndexFileName
        
        public static String HelpIndexFileName
        {
            get
            {
                return helpIndexFileName;
            }
        }

        #endregion
        #region archiveSearchPattern
        
        public static String ArchiveSearchPattern
        {
            get
            {
                return archiveSearchPattern;
            }
        }

        #endregion
        #region backupSearchPattern
        
        public static String BackupSearchPattern
        {
            get
            {
                return backupSearchPattern;
            }
        }

        #endregion
        #region formatGuidFileName
        
        public static String FormatGuidFileName
        {
            get
            {
                return formatGuidFileName;
            }
        }

        #endregion
        #region extensionBinaryFile
        
        public static String ExtensionBinaryFile
        {
            get
            {
                return extensionBinaryFile;
            }
        }

        #endregion
        #region extensionJsonFile
        
        public static String ExtensionJsonFile
        {
            get
            {
                return extensionJsonFile;
            }
        }

        #endregion
        #region indexBinaryFileSearchPattern
        
        public static String IndexBinaryFileSearchPattern
        {
            get
            {
                return indexBinaryFileSearchPattern;
            }
        }

        #endregion
        #region indexJsonFileSearchPattern
        
        public static String IndexJsonFileSearchPattern
        {
            get
            {
                return indexJsonFileSearchPattern;
            }
        }

        #endregion
        #region bodyArchiveFileName
        
        public static String BodyArchiveFileName
        {
            get
            {
                return bodyArchiveFileName;
            }
        }

        #endregion
        #region bodyArchiveFileSize
        
        public static Int32 BodyArchiveFileSize
        {
            get
            {
                return bodyArchiveFileSize;
            }
        }

        #endregion
        #region extensionTemporaryFile
        
        public static String ExtensionTemporaryFile
        {
            get
            {
                return extensionTemporaryFile;
            }
        }

        #endregion
        #region temporaryFileSearchPattern
        
        public static String TemporaryFileSearchPattern
        {
            get
            {
                return temporaryFileSearchPattern;
            }
        }

        #endregion
        #region htmlViewerTagReplaceBreak
        
        public static String HtmlViewerTagReplaceBreak
        {
            get
            {
                return htmlViewerTagReplaceBreak;
            }
        }

        #endregion
        #region issue_355_logFileName
        
        public static String Issue_355_logFileName
        {
            get
            {
                return issue_355_logFileName;
            }
        }

        #endregion
        #region issue_363_oldMediumCount
        
        public static Int32 Issue_363_oldMediumCount
        {
            get
            {
                return issue_363_oldMediumCount;
            }
        }

        #endregion

        // ConstantsRangeAttribute
        #region loggingStockCount

        /// <summary>
        /// Constants.loggingStockCount.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 LoggingStockMinimumCount
        {
            get
            {
                return loggingStockCount.minimum;
            }
        }
        

        /// <summary>
        /// Constants.loggingStockCount.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 LoggingStockMedianCount
        {
            get
            {
                return loggingStockCount.median;
            }
        }
        

        /// <summary>
        /// Constants.loggingStockCount.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 LoggingStockMaximumCount
        {
            get
            {
                return loggingStockCount.maximum;
            }
        }
        

        #endregion
        #region streamFontSize

        /// <summary>
        /// Constants.streamFontSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double StreamFontMinimumSize
        {
            get
            {
                return streamFontSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.streamFontSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double StreamFontMedianSize
        {
            get
            {
                return streamFontSize.median;
            }
        }
        

        /// <summary>
        /// Constants.streamFontSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double StreamFontMaximumSize
        {
            get
            {
                return streamFontSize.maximum;
            }
        }
        

        #endregion
        #region commandHideTime

        /// <summary>
        /// Constants.commandHideTime.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan CommandHideMinimumTime
        {
            get
            {
                return commandHideTime.minimum;
            }
        }
        

        /// <summary>
        /// Constants.commandHideTime.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan CommandHideMedianTime
        {
            get
            {
                return commandHideTime.median;
            }
        }
        

        /// <summary>
        /// Constants.commandHideTime.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan CommandHideMaximumTime
        {
            get
            {
                return commandHideTime.maximum;
            }
        }
        

        #endregion
        #region commandWindowWidth

        /// <summary>
        /// Constants.commandWindowWidth.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double CommandWindowMinimumWidth
        {
            get
            {
                return commandWindowWidth.minimum;
            }
        }
        

        /// <summary>
        /// Constants.commandWindowWidth.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double CommandWindowMedianWidth
        {
            get
            {
                return commandWindowWidth.median;
            }
        }
        

        /// <summary>
        /// Constants.commandWindowWidth.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double CommandWindowMaximumWidth
        {
            get
            {
                return commandWindowWidth.maximum;
            }
        }
        

        #endregion
        #region commandFontSize

        /// <summary>
        /// Constants.commandFontSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double CommandFontMinimumSize
        {
            get
            {
                return commandFontSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.commandFontSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double CommandFontMedianSize
        {
            get
            {
                return commandFontSize.median;
            }
        }
        

        /// <summary>
        /// Constants.commandFontSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double CommandFontMaximumSize
        {
            get
            {
                return commandFontSize.maximum;
            }
        }
        

        #endregion
        #region toolbarTextLength

        /// <summary>
        /// Constants.toolbarTextLength.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ToolbarTextMinimumLength
        {
            get
            {
                return toolbarTextLength.minimum;
            }
        }
        

        /// <summary>
        /// Constants.toolbarTextLength.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ToolbarTextMedianLength
        {
            get
            {
                return toolbarTextLength.median;
            }
        }
        

        /// <summary>
        /// Constants.toolbarTextLength.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ToolbarTextMaximumLength
        {
            get
            {
                return toolbarTextLength.maximum;
            }
        }
        

        #endregion
        #region toolbarFontSize

        /// <summary>
        /// Constants.toolbarFontSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double ToolbarFontMinimumSize
        {
            get
            {
                return toolbarFontSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.toolbarFontSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double ToolbarFontMedianSize
        {
            get
            {
                return toolbarFontSize.median;
            }
        }
        

        /// <summary>
        /// Constants.toolbarFontSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double ToolbarFontMaximumSize
        {
            get
            {
                return toolbarFontSize.maximum;
            }
        }
        

        #endregion
        #region toolbarHideWaitTime

        /// <summary>
        /// Constants.toolbarHideWaitTime.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ToolbarHideWaitMinimumTime
        {
            get
            {
                return toolbarHideWaitTime.minimum;
            }
        }
        

        /// <summary>
        /// Constants.toolbarHideWaitTime.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ToolbarHideWaitMedianTime
        {
            get
            {
                return toolbarHideWaitTime.median;
            }
        }
        

        /// <summary>
        /// Constants.toolbarHideWaitTime.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ToolbarHideWaitMaximumTime
        {
            get
            {
                return toolbarHideWaitTime.maximum;
            }
        }
        

        #endregion
        #region toolbarHideAnimateTime

        /// <summary>
        /// Constants.toolbarHideAnimateTime.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ToolbarHideAnimateMinimumTime
        {
            get
            {
                return toolbarHideAnimateTime.minimum;
            }
        }
        

        /// <summary>
        /// Constants.toolbarHideAnimateTime.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ToolbarHideAnimateMedianTime
        {
            get
            {
                return toolbarHideAnimateTime.median;
            }
        }
        

        /// <summary>
        /// Constants.toolbarHideAnimateTime.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ToolbarHideAnimateMaximumTime
        {
            get
            {
                return toolbarHideAnimateTime.maximum;
            }
        }
        

        #endregion
        #region clipboardWaitTime

        /// <summary>
        /// Constants.clipboardWaitTime.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ClipboardWaitMinimumTime
        {
            get
            {
                return clipboardWaitTime.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardWaitTime.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ClipboardWaitMedianTime
        {
            get
            {
                return clipboardWaitTime.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardWaitTime.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan ClipboardWaitMaximumTime
        {
            get
            {
                return clipboardWaitTime.maximum;
            }
        }
        

        #endregion
        #region clipboardSaveCount

        /// <summary>
        /// Constants.clipboardSaveCount.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardSaveMinimumCount
        {
            get
            {
                return clipboardSaveCount.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardSaveCount.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardSaveMedianCount
        {
            get
            {
                return clipboardSaveCount.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardSaveCount.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardSaveMaximumCount
        {
            get
            {
                return clipboardSaveCount.maximum;
            }
        }
        

        #endregion
        #region clipboardDuplicationCount

        /// <summary>
        /// Constants.clipboardDuplicationCount.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardDuplicationMinimumCount
        {
            get
            {
                return clipboardDuplicationCount.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardDuplicationCount.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardDuplicationMedianCount
        {
            get
            {
                return clipboardDuplicationCount.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardDuplicationCount.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardDuplicationMaximumCount
        {
            get
            {
                return clipboardDuplicationCount.maximum;
            }
        }
        

        #endregion
        #region clipboardFontSize

        /// <summary>
        /// Constants.clipboardFontSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double ClipboardFontMinimumSize
        {
            get
            {
                return clipboardFontSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardFontSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double ClipboardFontMedianSize
        {
            get
            {
                return clipboardFontSize.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardFontSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double ClipboardFontMaximumSize
        {
            get
            {
                return clipboardFontSize.maximum;
            }
        }
        

        #endregion
        #region clipboardLimitTextSize

        /// <summary>
        /// Constants.clipboardLimitTextSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitTextMinimumSize
        {
            get
            {
                return clipboardLimitTextSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitTextSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitTextMedianSize
        {
            get
            {
                return clipboardLimitTextSize.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitTextSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitTextMaximumSize
        {
            get
            {
                return clipboardLimitTextSize.maximum;
            }
        }
        

        #endregion
        #region clipboardLimitRtfSize

        /// <summary>
        /// Constants.clipboardLimitRtfSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitRtfMinimumSize
        {
            get
            {
                return clipboardLimitRtfSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitRtfSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitRtfMedianSize
        {
            get
            {
                return clipboardLimitRtfSize.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitRtfSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitRtfMaximumSize
        {
            get
            {
                return clipboardLimitRtfSize.maximum;
            }
        }
        

        #endregion
        #region clipboardLimitHtmlSize

        /// <summary>
        /// Constants.clipboardLimitHtmlSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitHtmlMinimumSize
        {
            get
            {
                return clipboardLimitHtmlSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitHtmlSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitHtmlMedianSize
        {
            get
            {
                return clipboardLimitHtmlSize.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitHtmlSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitHtmlMaximumSize
        {
            get
            {
                return clipboardLimitHtmlSize.maximum;
            }
        }
        

        #endregion
        #region clipboardLimitImageWidthSize

        /// <summary>
        /// Constants.clipboardLimitImageWidthSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitImageWidthMinimumSize
        {
            get
            {
                return clipboardLimitImageWidthSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitImageWidthSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitImageWidthMedianSize
        {
            get
            {
                return clipboardLimitImageWidthSize.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitImageWidthSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitImageWidthMaximumSize
        {
            get
            {
                return clipboardLimitImageWidthSize.maximum;
            }
        }
        

        #endregion
        #region clipboardLimitImageHeightSize

        /// <summary>
        /// Constants.clipboardLimitImageHeightSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitImageHeightMinimumSize
        {
            get
            {
                return clipboardLimitImageHeightSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitImageHeightSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitImageHeightMedianSize
        {
            get
            {
                return clipboardLimitImageHeightSize.median;
            }
        }
        

        /// <summary>
        /// Constants.clipboardLimitImageHeightSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 ClipboardLimitImageHeightMaximumSize
        {
            get
            {
                return clipboardLimitImageHeightSize.maximum;
            }
        }
        

        #endregion
        #region templateFontSize

        /// <summary>
        /// Constants.templateFontSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double TemplateFontMinimumSize
        {
            get
            {
                return templateFontSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.templateFontSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double TemplateFontMedianSize
        {
            get
            {
                return templateFontSize.median;
            }
        }
        

        /// <summary>
        /// Constants.templateFontSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double TemplateFontMaximumSize
        {
            get
            {
                return templateFontSize.maximum;
            }
        }
        

        #endregion
        #region windowSaveIntervalTime

        /// <summary>
        /// Constants.windowSaveIntervalTime.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan WindowSaveIntervalMinimumTime
        {
            get
            {
                return windowSaveIntervalTime.minimum;
            }
        }
        

        /// <summary>
        /// Constants.windowSaveIntervalTime.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan WindowSaveIntervalMedianTime
        {
            get
            {
                return windowSaveIntervalTime.median;
            }
        }
        

        /// <summary>
        /// Constants.windowSaveIntervalTime.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static TimeSpan WindowSaveIntervalMaximumTime
        {
            get
            {
                return windowSaveIntervalTime.maximum;
            }
        }
        

        #endregion
        #region windowSaveCount

        /// <summary>
        /// Constants.windowSaveCount.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 WindowSaveMinimumCount
        {
            get
            {
                return windowSaveCount.minimum;
            }
        }
        

        /// <summary>
        /// Constants.windowSaveCount.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 WindowSaveMedianCount
        {
            get
            {
                return windowSaveCount.median;
            }
        }
        

        /// <summary>
        /// Constants.windowSaveCount.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Int32 WindowSaveMaximumCount
        {
            get
            {
                return windowSaveCount.maximum;
            }
        }
        

        #endregion
        #region noteFontSize

        /// <summary>
        /// Constants.noteFontSize.minimum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double NoteFontMinimumSize
        {
            get
            {
                return noteFontSize.minimum;
            }
        }
        

        /// <summary>
        /// Constants.noteFontSize.median取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double NoteFontMedianSize
        {
            get
            {
                return noteFontSize.median;
            }
        }
        

        /// <summary>
        /// Constants.noteFontSize.maximum取得用プロパティ。
        /// <para>XAMLで使用することを想定</para>
        /// </summary>
        public static Double NoteFontMaximumSize
        {
            get
            {
                return noteFontSize.maximum;
            }
        }
        

        #endregion

    }
}
