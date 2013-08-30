/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/06/30
 * 時刻: 13:24
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;
using System.Text;


namespace ShareLib
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	public struct SHFILEINFO 
	{
		public IntPtr hIcon;
		public int iIcon;
		public uint dwAttributes;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		public string szDisplayName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string szTypeName;
	}
	
	public enum SHGFI: uint
	{
		/// <summary>
		/// アイコン・リソースの取得
		/// </summary>
        SHGFI_ICON = 0x100,
        /// <summary>
        /// 大きいアイコン
        /// </summary>
        SHGFI_LARGEICON = 0x00,
        /// <summary>
        /// 小さいアイコン
        /// </summary>
        SHGFI_SMALLICON = 0x01,
        /// <summary>
        /// ファイルの種類
        /// </summary>
        SHGFI_TYPENAME = 0x400,
        /// <summary>
        /// 
        /// </summary>
        SHGFI_USEFILEATTRIBUTES = 0x10,
        
        SHGFI_SYSICONINDEX = 0x4000,
	}
	
        
	public enum SHIL: int
	{
	    SHIL_EXTRALARGE = 0x2,
	    SHIL_JUMBO = 0x4,
	}
			
	/// <summary>
	/// Description of Shellapi.
	/// </summary>
	public static partial class WindowsAPI
	{
		[DllImport("shell32.dll")]
		public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, SHGFI uFlags);

		//[DllImport("shell32.dll", EntryPoint = "#727")]
		[DllImport("shell32.dll")]
		public static extern int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

		 [DllImport( "shlwapi.dll", CharSet = CharSet.Unicode )]
		public static extern long StrFormatByteSize( long fileSize, [MarshalAs( UnmanagedType.LPTStr )] StringBuilder buffer, int bufferSize );
	}
}
