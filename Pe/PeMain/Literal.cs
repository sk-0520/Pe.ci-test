/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:06
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;

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
			get { return Path.Combine(UserSettingDirPath, mainSettingFileName); }
		}
	}
}
