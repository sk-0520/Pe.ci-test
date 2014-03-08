/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/06
 * 時刻: 21:20
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;

namespace PInvoke.Windows
{
	public enum SHCNE
	{
		SHCNE_ALLEVENTS = 0x7FFFFFFF,
		/// <summary>
		/// ファイルタイプの関連付けに変更された
		/// </summary>
		SHCNE_ASSOCCHANGED = 0x8000000,
		/// <summary>
		///  既存のフォルダの内容が変化したが、(フォルダ名の変化はない)
		/// </summary>
		SHCNE_UPDATEDIR = 0x1000,
	}
	
	public enum  SHCNF
	{
		/// <summary>
		///  	dwItem1、dwItem2はアイテムIDリストのアドレス
		/// </summary>
		SHCNF_IDLIST = 0x0000,
		/// <summary>
		/// dwItem1、dwItem2はDWORD値
		/// </summary>
		SHCNF_DWORD  = 0x0003,
	}
	
	public static partial class API
	{
		[DllImport("shell32.dll")]
		public static extern void SHChangeNotify(SHCNE wEventId, SHCNF uFlags, IntPtr dwItem1, IntPtr dwItem2);
	}
}
