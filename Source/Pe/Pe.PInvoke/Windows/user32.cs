using System;
using System.Runtime.InteropServices;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    [StructLayout(LayoutKind.Sequential)]
    public struct LASTINPUTINFO
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 cbSize;
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 dwTime;
    }

    /// <summary>
    /// Description of MyClass.
    /// </summary>
    partial class NativeMethods
    {
        /// <summary>
        /// http://www.pinvoke.net/default.aspx/user32.registerhotkey
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <param name="fsModifiers"></param>
        /// <param name="vk"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, MOD fsModifiers, int vk);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hwnd, int id);

        /// <summary>
        /// 最後に発生した入力イベントの時刻を取得します。
        /// </summary>
        /// <remarks>
        /// <para>これは、入力のアイドル状態を検出する場合に使うことができます。</para>
        /// </remarks>
        /// <param name="plii">最後の入力イベントの時刻が入る、<see cref="LASTINPUTINFO"/> 構造体へのポインタを指定します。</param>
        /// <returns>関数が成功すると、0 以外の値が返ります。関数が失敗すると、0 が返ります。</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        /// <summary>
        /// システムを起動した後の経過時間を、ミリ秒（ms）単位で取得します。この時間は、システムタイマの分解能による制限を受けます。システムタイマの分解能を取得するには、GetSystemTimeAdjustment 関数を使います。
        /// </summary>
        /// <returns>関数が成功すると、システムを起動した後の経過時間が、ミリ秒単位で返ります。</returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount();
    }
}
