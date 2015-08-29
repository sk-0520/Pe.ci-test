namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class LanguageKey
	{
		#region common

		public const string application = "APPLICATION";
		public const string applicationVersion = "APPLICATION:VERSION";
		public const string applicationRevision = "APPLICATION:REVISION";

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

		#region replace

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

		#endregion
	}
}
