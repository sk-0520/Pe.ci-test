﻿namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;
	using System.IO;
	using System.Diagnostics;

	/// <summary>
	/// 定数。
	/// </summary>
	public static class Constants
	{
		public const string programName = "Pe2";
		/// <summary>
		/// 前回バージョンがこれ未満なら使用許諾を表示
		/// </summary>
		public static readonly Version acceptVersion = new Version(1, 0, 0, 0);

#if DEBUG
		public const string shortcutName = programName + "(DEBUG).lnk";
#elif BETA
		public const string shortcutName = programName + "(BETA).lnk";
#else
		public const string shortcutName = programName + ".lnk";
#endif
		public const string buildType =
#if DEBUG
			"DEBUG";
#elif BETA
			"β";
#else
			"RELEASE";
#endif
		public static readonly string buildProcess = Environment.Is64BitProcess ? "64" : "32";


		public const string binDirectoryName = "bin";
		public const string sbinDirectoryName = "sbin";
		public const string libraryDirectoryName = "lib";
		public const string etcDirectoryName = "etc";
		public const string languageDirectoryName = "lang";
		public const string styleDirectoryName = "style";
		public const string scriptDirectoryName = "script";
		public const string documentDirectoryName = "doc";

		public const string logDirectoryName = "logs";
		public const string settingDirectoryName = "setting";
		public const string backupDirectoryName = "backup";
		public const string archiveDirectoryName = "archive";

		public const string mainSettingFileName = "main-setting.json";
		public const string launcherItemSettingFileName = "item-setting.json";
		public const string launcherGroupItemSettingFileName = "group-item.json";

		public const string languageDefaultFileName = "default.xml";
		public const string languageSearchPattern = "*.xml";

		public const string timestampFileName = "yyyy-MM-dd_HH-mm-ss";

		/// <summary>
		/// 実行パス
		/// </summary>
		public static readonly string applicationExecutablePath = Assembly.GetExecutingAssembly().Location;
		/// <summary>
		/// 起動ディレクトリ
		/// </summary>
		public static readonly string applicationRootDirectoryPath = Path.GetDirectoryName(applicationExecutablePath);
		/// <summary>
		/// アセンブリバージョン。
		/// </summary>
		public static readonly Version assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
		/// <summary>
		/// 識別リビジョン。
		/// </summary>
		public static readonly string applicationRevision = FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
		/// <summary>
		/// アプリケーションバージョン。
		/// </summary>
		public static readonly string applicationVersion = assemblyVersion.ToString() + "-" + applicationRevision;
		/// <summary>
		/// スタートアップ用ショートカットファイルパス。
		/// </summary>
		public static readonly string startupShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), shortcutName);

	}
}
