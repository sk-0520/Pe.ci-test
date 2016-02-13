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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain
{
    public static class TemplateReplaceKey
    {
        #region text

        public const string textClipboard = "CLIP";
        public const string textClipboardNobreak = "CLIP:NOBREAK";
        public const string textClipboardHead = "CLIP:HEAD";
        public const string textClipboardTail = "CLIP:TAIL";

        public const string textApplicationName = LanguageKey.applicationName;
        public const string textApplicationVersion = LanguageKey.applicationVersion;
        public const string textApplicationVersionNumber = LanguageKey.applicationVersionNumber;
        public const string textApplicationVersionRevision = LanguageKey.applicationVersionRevision;

        public const string textTimestamp = LanguageKey.timestamp;
        public const string textYear = LanguageKey.year;
        public const string textYear04 = LanguageKey.year04;
        public const string textMonth = LanguageKey.month;
        public const string textMonth02 = LanguageKey.month02;
        public const string textMonthShortName = LanguageKey.monthShortName;
        public const string textMonthLongName = LanguageKey.monthLongName;
        public const string textDay = LanguageKey.day;
        public const string textDay02 = LanguageKey.day02;
        public const string textHour = LanguageKey.hour;
        public const string textHour02 = LanguageKey.hour02;
        public const string textMinute = LanguageKey.minute;
        public const string textMinute02 = LanguageKey.minute02;
        public const string textSecond = LanguageKey.second;
        public const string textSecond02 = LanguageKey.second02;

        #endregion

        #region program

        public const string programCode = "<#  #>";
        public const string programExpr = "<#=  #>";
        public const string programDefine = "<#+  #>";
        public const string programTimestamp = LanguageKey.timestamp;
        public const string programClipboard = textClipboard;
        public const string programApplicationName = LanguageKey.applicationName;
        public const string programApplicationVersion = LanguageKey.applicationVersion;
        public const string programApplicationVersionNumber = LanguageKey.applicationVersionNumber;
        public const string programApplicationVersionRevision = LanguageKey.applicationVersionRevision;

        #endregion

        public static IReadOnlyList<string> TextKeyList = new List<string>() {
            textClipboard,
            textClipboardNobreak,
            textClipboardHead,
            textClipboardTail,

            textTimestamp,
            textYear,
            textYear04,
            textMonth,
            textMonth02,
            textMonthShortName,
            textMonthLongName,
            textDay,
            textDay02,
            textHour,
            textHour02,
            textMinute,
            textMinute02,
            textSecond,
            textSecond02,

            textApplicationName,
            textApplicationVersion,
            textApplicationVersionNumber,
            textApplicationVersionRevision,
        };

        public static IReadOnlyList<string> ProgramKeyList = new List<string>() {
            programCode,
            programExpr,

            programTimestamp,
            programClipboard,
            programApplicationName,
            programApplicationVersion,
            programApplicationVersionNumber,
            programApplicationVersionRevision,

            programDefine,
        };

        public static IReadOnlyDictionary<string, Type> ProgramTypes = new Dictionary<string, Type>() {
            { programCode, null },
            { programExpr, null },

            { LanguageKey.timestamp, typeof(DateTime) },
            { textClipboard, typeof(string) },
            { LanguageKey.applicationName, typeof(string) },
            { LanguageKey.applicationVersion, typeof(string) },
            { LanguageKey.applicationVersionNumber, typeof(Version) },
            { LanguageKey.applicationVersionRevision, typeof(string) },

            { programDefine, null },
        };

        public static IReadOnlyList<string> caretInSpaceKeys = new List<string>() {
            programCode,
            programExpr,
            programDefine,
        };

    }
}
