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

namespace ContentTypeTextNet.Pe.PeMain
{
    public static class LanguageKey
    {
        #region 内部使用

        public const string applicationName = "APPLICATION:NAME";
        public const string applicationVersion = "APPLICATION:VERSION";
        public const string applicationVersionNumber = "APPLICATION:VERSION-NUMBER";
        public const string applicationVersionRevision = "APPLICATION:VERSION-REVISION";

        public const string timestamp = "TIMESTAMP";
        public const string year = "Y";
        public const string year04 = "Y:04";
        public const string month = "M";
        public const string month02 = "M:02";
        public const string monthShortName = "M:S";
        public const string monthLongName = "M:L";
        public const string day = "D";
        public const string day02 = "D:02";
        public const string hour = "h";
        public const string hour02 = "h:02";
        public const string minute = "m";
        public const string minute02 = "m:02";
        public const string second = "s";
        public const string second02 = "s:02";

        #endregion

        #region 言語ファイルの置き換え文字列

        public const string acceptWeb = "ACCEPT-WEB";
        public const string acceptDevelopment = "ACCEPT-DEVELOPMENT";
        public const string acceptMail = "ACCEPT-MAIL";
        public const string acceptForum = "ACCEPT-FORUM";
        public const string acceptFeedback = "ACCEPT-FEEDBACK";
        //public const string acceptHelp = "ACCEPT-HELP";
        public const string acceptStyle = "ACCEPT-STYLE";
        public const string acceptApplicationName = "ACCEPT-APP";
        public const string acceptOk = "ACCEPT-OK";
        public const string acceptNg = "ACCEPT-NG";
        public const string acceptRelease = "ACCEPT-CHECK-RELEASE";
        public const string acceptRc = "ACCEPT-CHECK-RC";
        public const string acceptSendUserInformation = "ACCEPT-SEND-USER-INFO";

        public const string commandItemTag = "LAUNCHER-ITEM-TAG";
        public const string commandItemName = "LAUNCHER-ITEM-NAME";
        public const string commandDrivePath = "DRIVE-PATH";
        public const string commandDriveVolume = "DRIVE-VOLUME";
        public const string customizeItem = "LAUNCHER-ITEM-CUSTOMIZE";
        public const string executeItem = "LAUNCHER-ITEM-EXECUTE";
        public const string streamItem = "LAUNCHER-ITEM-STREAM";
        public const string noteTitleCount = "NOTE-COUNT";

        public const string updateNowVersion = "UPDATE-NOW-VERSION";
        public const string updateNewVersion = "UPDATE-NEW-VERSION";
        public const string updateNewType = "UPDATE-NEW-TYPE";

        public const string clipboardType = "CLIP-TYPE";
        public const string clipboardImageWidth = "CLIP-IMAGE-WIDTH";
        public const string clipboardImageHeight = "CLIP-IMAGE-HEIGHT";
        public const string clipboardFileCount = "CLIP-FILES-COUNT";

        public const string logExecuteItemName = "LOG-EXECUTE-ITEM-NAME";
        public const string logClipboardWaitTimePrev = "LOG-CLIP-WAIT_TIME-PREV";
        public const string logClipboardWaitTimeCurrent = "LOG-CLIP-WAIT_TIME-CURRENT";
        public const string logClipboardWaitTimeSetting = "LOG-CLIP-WAIT_TIME-SETTING";
        public const string logClipboardWaitTimeWait = "LOG-CLIP-WAIT_TIME-WAIT";
        public const string logClipboardFilterType = "LOG-CLIP-FILTER-TYPE";
        public const string logClipboardFilterLengthCurrent = "LOG-CLIP-FILTER_LENGTH-CURRENT";
        public const string logClipboardFilterLengthSetting = "LOG-CLIP-FILTER_LENGTH-SETTING";
        public const string logClipboardFilterImageWidthCurrent = "LOG-CLIP-FILTER_IMAGE_WIDTH-CURRENT";
        public const string logClipboardFilterImageWidthSetting = "LOG-CLIP-FILTER_IMAGE_WIDTH-SETTING";
        public const string logClipboardFilterImageHeightCurrent = "LOG-CLIP-FILTER_IMAGE_HEIGHT-CURRENT";
        public const string logClipboardFilterImageHeightSetting = "LOG-CLIP-FILTER_IMAGE_HEIGHT-SETTING";
        public const string logWindowSaveType = "LOG-WINDOW-SAVE-TYPE";
        public const string logGroupChange = "LOG-GROUP-CHNAGE";

        public const string logPrivacySendDataId = "LOG-PRIVACY-SEND-DATA-ID";
        public const string logPrivacySendRecvData = "LOG-PRIVACY-SEND-RECV-DATA";
        public const string logPrivacySendRecvRaw = "LOG-PRIVACY-SEND-RECV-RAW";

        public const string formsConvertLauncherItemName = "FORMS-CONVERT-LAUNCHER-ITEM-NAME";
        public const string formsConvertLauncherItemType = "FORMS-CONVERT-LAUNCHER-ITEM-TYPE";
        public const string formsConvertLauncherItemTypeFile = "FORMS-CONVERT-LAUNCHER-ITEM-TYPE:FILE";

        public const string windowCollectionCount = "WINDOW-COLLECTION-COUNT";

        public const string toolbarScreenName = "TOOLBAR-SCREEN-NAME";

        public const string noteTitle = "NOTE-TITLE";

        #endregion

        #region HtmlViewer

        public const string htmlViewerTitleFeedBack = "html-viewer/feedback";

        #endregion
    }
}
