namespace ContentTypeTextNet.Pe.PeMain.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	public static class DockTypeUtility
	{
		/// <summary>
		/// ABEへ変換。
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static ABE ToABE(DockType type)
		{
			switch (type) {
				case DockType.Left: return ABE.ABE_LEFT;
				case DockType.Top: return ABE.ABE_TOP;
				case DockType.Bottom: return ABE.ABE_BOTTOM;
				case DockType.Right: return ABE.ABE_RIGHT;
				default:
					throw new NotImplementedException();
			}
		}

		/// <summary>
		/// DesktopDockTypeへ変換。
		/// </summary>
		/// <param name="abe"></param>
		/// <returns></returns>
		public static DockType ToDockType(ABE abe)
		{
			switch (abe) {
				case ABE.ABE_LEFT: return DockType.Left;
				case ABE.ABE_TOP: return DockType.Top;
				case ABE.ABE_RIGHT: return DockType.Right;
				case ABE.ABE_BOTTOM: return DockType.Bottom;
				default:
					throw new NotImplementedException();
			}
		}
	}
}
