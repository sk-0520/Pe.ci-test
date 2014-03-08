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
using PInvoke.Windows;

namespace PeUtility
{
	public enum DesktopDockType
	{
		None,
		Left,
		Top,
		Right,
		Bottom,
	}
	
	public static class DesktopDockTypeConverter
	{
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
