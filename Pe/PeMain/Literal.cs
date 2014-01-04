/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:06
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PeMain
{
	/// <summary>
	/// 各種定数
	/// </summary>
	public static class Literal
	{
		/// <summary>
		/// このプログラムが使用するディレクトリ名
		/// </summary>
		private const string dirRootName = "Pe";
		
		public const string mainSettingFileName = "setting.xml";
		
		/// <summary>
		/// ツールバー フロート状態 設定サイズ
		/// </summary>
		public static readonly Size toolbarFloatSize = new Size(6, 1);
		public static readonly Size toolbarDesktopSize = new Size(1, 1);
		public const int toolbarTextWidth = 80;
		
		/// <summary>
		/// 起動ディレクトリ
		/// </summary>
		public static string PeRootDirPath
		{
			get
			{
				return Path.GetDirectoryName(Application.ExecutablePath);
			}
		}
		
		/// <summary>
		/// etc/
		/// </summary>
		public static string PeEtcDirPath
		{
			get
			{
				return Path.Combine(PeRootDirPath, "etc");
			}
		}
		
		/// <summary>
		/// etc/lang/
		/// </summary>
		public static string PeLanguageDirPath
		{
			get
			{
				return Path.Combine(PeEtcDirPath, "lang");
			}
		}
		
		/// <summary>
		/// ユーザー設定ルートディレクトリ
		/// </summary>
		public static string UserSettingDirPath 
		{
			get 
			{
				return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), dirRootName);
			}
		}
		/// <summary>
		/// ユーザー設定ファイル
		/// </summary>
		public static string UserMainSettingPath
		{
			get {
				return Path.Combine(UserSettingDirPath, mainSettingFileName);
			}
		}
	}
}
