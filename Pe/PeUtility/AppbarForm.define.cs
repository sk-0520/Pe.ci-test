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
using SC.Windows;

namespace PeMain.UI
{
	public enum DockType
	{
		None,
		Left,
		Top,
		Right,
		Bottom,
	}
	
	public static class DockTypeConverter
	{
		public static ABE ToABE(this DockType type)
		{
			switch(type) {
				case DockType.Left: return ABE.ABE_LEFT;
				case DockType.Top: return ABE.ABE_TOP;
				case DockType.Bottom: return ABE.ABE_BOTTOM;
				case DockType.Right: return ABE.ABE_RIGHT;
				default:
					Debug.Assert(false, type.ToString());
					return ABE.ABE_LEFT; // dummy
			}
		}
		public static DockType ToDockType(this ABE abe)
		{
			switch(abe) {
				case ABE.ABE_LEFT: return DockType.Left;
				case ABE.ABE_TOP: return DockType.Top;
				case ABE.ABE_RIGHT: return DockType.Right;
				case ABE.ABE_BOTTOM: return DockType.Bottom;
				default:
					Debug.Assert(false, abe.ToString());
					return DockType.None; // dummy
			}
		}
	}
}
