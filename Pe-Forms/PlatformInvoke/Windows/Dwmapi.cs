using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows
{
	[StructLayout(LayoutKind.Sequential)]
	public struct MARGINS
	{
		public int leftWidth;
		public int rightWidth;
		public int topHeight;
		public int bottomHeight;
	}
	
	[Flags]
	public enum DWM_BB
	{
		DWM_BB_ENABLE = 1,
		DWM_BB_BLURREGION = 2,
		DWM_BB_TRANSITIONONMAXIMIZED = 4
	}
	
	[StructLayout(LayoutKind.Sequential)]
	public struct DWM_BLURBEHIND
	{
		public DWM_BB dwFlags;
		public bool fEnable;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security","CA2111:PointersShouldNotBeVisible")]
		public IntPtr hRgnBlur;
		public int fTransitionOnMaximized;

		public DWM_BLURBEHIND(bool enabled)
		{
			fEnable = enabled;
			hRgnBlur = IntPtr.Zero;
			fTransitionOnMaximized = 0;
			dwFlags = DWM_BB.DWM_BB_ENABLE;
		}

		public System.Drawing.Region Region
		{
			get { return System.Drawing.Region.FromHrgn(hRgnBlur); }
		}

		public bool TransitionOnMaximized
		{
			get { return fTransitionOnMaximized > 0; }
			set
			{
				fTransitionOnMaximized = value ? 1 : 0;
				dwFlags |= DWM_BB.DWM_BB_TRANSITIONONMAXIMIZED;
			}
		}

		public void SetRegion(System.Drawing.Graphics graphics, System.Drawing.Region region)
		{
			hRgnBlur = region.GetHrgn(graphics);
			dwFlags |= DWM_BB.DWM_BB_BLURREGION;
		}
	}
	
	partial class NativeMethods
	{
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/dwmNativeMethods.dwmiscompositionenabled
		/// </summary>
		/// <param name="enabled"></param>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll")]
		public static extern int DwmIsCompositionEnabled(out bool enabled);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll", PreserveSig = true)]
		public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll")]
		public static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);
		
		/// <summary>
		/// http://www.pinvoke.net/default.aspx/dwmNativeMethods.dwmgetcolorizationcolor
		/// </summary>
		/// <param name="ColorizationColor"></param>
		/// <param name="ColorizationOpaqueBlend"></param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll", PreserveSig = false)]
		public static extern void DwmGetColorizationColor(out uint ColorizationColor, [MarshalAs(UnmanagedType.Bool)]out bool ColorizationOpaqueBlend);
	}
}
