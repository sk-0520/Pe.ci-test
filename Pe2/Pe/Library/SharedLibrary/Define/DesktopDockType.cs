﻿namespace ContentTypeTextNet.Library.SharedLibrary.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.PInvoke.Windows;

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
					throw new NotImplementedException();
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
					throw new NotImplementedException();
			}
		}
	}

}
