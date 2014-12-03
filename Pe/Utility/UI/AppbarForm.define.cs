/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:04
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// デスクトップへのドッキング位置。
	/// </summary>
	public enum DesktopDockType
	{
		/// <summary>
		/// ドッキングしない。
		/// </summary>
		None,
		/// <summary>
		/// 左。
		/// </summary>
		Left,
		/// <summary>
		/// 上。
		/// </summary>
		Top,
		/// <summary>
		/// 右。
		/// </summary>
		Right,
		/// <summary>
		/// 下。
		/// </summary>
		Bottom,
	}
	
	/// <summary>
	/// DesktopDockType 変換処理。
	/// </summary>
	public static class DesktopDockTypeConverter
	{
		/// <summary>
		/// ABEへ変換。
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static ABE ToABE(this DesktopDockType type)
		{
			switch(type) {
				case DesktopDockType.Left: return ABE.ABE_LEFT;
				case DesktopDockType.Top: return ABE.ABE_TOP;
				case DesktopDockType.Bottom: return ABE.ABE_BOTTOM;
				case DesktopDockType.Right: return ABE.ABE_RIGHT;
				default:
					Debug.Assert(false, type.ToString());
					return ABE.ABE_LEFT; // dummy
			}
		}

		/// <summary>
		/// DesktopDockTypeへ変換。
		/// </summary>
		/// <param name="abe"></param>
		/// <returns></returns>
		public static DesktopDockType ToDockType(this ABE abe)
		{
			switch(abe) {
				case ABE.ABE_LEFT: return DesktopDockType.Left;
				case ABE.ABE_TOP: return DesktopDockType.Top;
				case ABE.ABE_RIGHT: return DesktopDockType.Right;
				case ABE.ABE_BOTTOM: return DesktopDockType.Bottom;
				default:
					Debug.Assert(false, abe.ToString());
					return DesktopDockType.None; // dummy
			}
		}
	}
}
