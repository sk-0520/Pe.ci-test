using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

    public enum WM: uint
    {
        WM_DESTROY = 0x0002,
        WM_ACTIVATE = 0x06,
        WM_WINDOWPOSCHANGED = 0x47,
        WM_HOTKEY = 0x0312,
        WM_MOUSEACTIVATE = 0x21,
        WM_NCLBUTTONDOWN = 0xa1,
        WM_NCHITTEST = 0x84,
        WM_WINDOWPOSCHANGING = 0x0046,
        WM_MOVING = 0x0216,
        WM_COMMAND = 0x0111,
        WM_SETTINGCHANGE = 0x001a,
        WM_NCPAINT = 0x0085,
        WM_SYSCOMMAND = 0x112,
        WM_NCRBUTTONDOWN = 0xA4,
        WM_NCRBUTTONUP = 0x00A5,
        WM_DEVICECHANGE = 0x0219,
        WM_SETCURSOR = 0x0020,
        WM_CONTEXTMENU = 0x007b,
        WM_DWMCOLORIZATIONCOLORCHANGED = 0x320,
        WM_DWMCOMPOSITIONCHANGED = 0x031e,
        WM_DRAWCLIPBOARD = 0x0308,
        WM_CHANGECBCHAIN = 0x030d,
        WM_CLIPBOARDUPDATE = 0x031d,
        WM_HSCROLL = 0x114,
        WM_VSCROLL = 0x0115,
        WM_SETREDRAW = 0x000b,
        WM_ENTERSIZEMOVE = 0x0231,
        WM_EXITSIZEMOVE = 0x0232,
        WM_MOUSEWHEEL = 0x020a,
        WM_MOUSEHWHEEL = 0x020e,
        WM_NOTIFY = 0x004e,
        WM_ERASEBKGND = 0x0014,
        WM_CLOSE = 0x0010,
        WM_SETTEXT = 0x000c,
        WM_PASTE = 0x0302,
        WM_SIZING = 0x0214,
        WM_NCLBUTTONDBLCLK = 0x00a3,
        WM_NCRBUTTONDBLCLK = 0x00a6,
        WM_NCMBUTTONDOWN = 0x00a7,
        WM_NCMBUTTONUP = 0x00a8,
        WM_NCMBUTTONDBLCLK = 0x00a9,
        WM_SYSKEYDOWN = 0x0104,
        WM_SYSKEYUP = 0x0105,
        WM_XBUTTONUP = 0x020C,
        WM_XBUTTONDOWN = 0x020B,
        WM_MOUSEMOVE = 0x0200,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205,
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_LBUTTONDBLCLK = 0x0203,
        WM_MBUTTONDOWN = 0x0207,
        WM_MBUTTONUP = 0x0208,
        WM_KEYDOWN = 0x0100,
        WM_KEYUP = 0x0101,

    }

    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:列挙値を重複させることはできない")]
    public enum WS: uint
    {
        WS_OVERLAPPED = 0,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x8000000,
        WS_CLIPSIBLINGS = 0x4000000,
        WS_CLIPCHILDREN = 0x2000000,
        WS_MAXIMIZE = 0x1000000,
        WS_CAPTION = 0xC00000,      // WS_BORDER or WS_DLGFRAME
        WS_BORDER = 0x800000,
        WS_DLGFRAME = 0x400000,
        WS_VSCROLL = 0x200000,
        WS_HSCROLL = 0x100000,
        WS_SYSMENU = 0x80000,
        WS_THICKFRAME = 0x40000,
        WS_GROUP = 0x20000,
        WS_TABSTOP = 0x10000,
        WS_MINIMIZEBOX = 0x20000,
        WS_MAXIMIZEBOX = 0x10000,
        WS_TILED = WS_OVERLAPPED,
        WS_ICONIC = WS_MINIMIZE,
        WS_SIZEBOX = WS_THICKFRAME,
    }

    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:列挙値を重複させることはできない")]
    public enum WS_EX: uint
    {
        WS_EX_DLGMODALFRAME = 0x0001,
        WS_EX_NOPARENTNOTIFY = 0x0004,
        WS_EX_TOPMOST = 0x0008,
        WS_EX_ACCEPTFILES = 0x0010,
        WS_EX_TRANSPARENT = 0x0020,
        WS_EX_MDICHILD = 0x0040,
        WS_EX_TOOLWINDOW = 0x0080,
        WS_EX_WINDOWEDGE = 0x0100,
        WS_EX_CLIENTEDGE = 0x0200,
        WS_EX_CONTEXTHELP = 0x0400,
        WS_EX_RIGHT = 0x1000,
        WS_EX_LEFT = 0x0000,
        WS_EX_RTLREADING = 0x2000,
        WS_EX_LTRREADING = 0x0000,
        WS_EX_LEFTSCROLLBAR = 0x4000,
        WS_EX_RIGHTSCROLLBAR = 0x0000,
        WS_EX_CONTROLPARENT = 0x10000,
        WS_EX_STATICEDGE = 0x20000,
        WS_EX_APPWINDOW = 0x40000,
        WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
        WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
        WS_EX_LAYERED = 0x00080000,
        WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
        WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000,
    }

    public enum CS
    {
        CS_DROPSHADOW = 0x20000,
    }

    public enum WM_COMMAND_SUB
    {
        Refresh = 0x7103,
    }

    /// <summary>
    /// http://chokuto.ifdef.jp/urawaza/message/WM_SYSCOMMAND.html
    /// </summary>
    public enum SC
    {
        /// <summary>
        /// ウィンドウサイズを変更します。
        /// </summary>
        SC_SIZE = 0xF000,
        /// <summary>
        /// ウィンドウを移動します。
        /// </summary>
        SC_MOVE = 0xF010,
        /// <summary>
        /// ウィンドウを最小化します。
        /// </summary>
        SC_MINIMIZE = 0xF020,
        /// <summary>
        /// ウィンドウを最大化します。
        /// </summary>
        SC_MAXIMIZE = 0xF030,
        /// <summary>
        /// 次のウィンドウに移動します。
        /// </summary>
        SC_NEXTWINDOW = 0xF040,
        /// <summary>
        /// 前のウィンドウに移動します。
        /// </summary>
        SC_PREVWINDOW = 0xF050,
        /// <summary>
        /// ウィンドウをクローズします。
        /// </summary>
        SC_CLOSE = 0xF060,
        /// <summary>
        /// 垂直にスクロールします。
        /// </summary>
        SC_VSCROLL = 0xF070,
        /// <summary>
        /// 水平にスクロールします。
        /// </summary>
        SC_HSCROLL = 0xF080,
        /// <summary>
        /// マウスクリックによりメニューを取得します。
        /// </summary>
        SC_MOUSEMENU = 0xF090,
        /// <summary>
        /// キー操作によりメニューを取得します。
        /// </summary>
        SC_KEYMENU = 0xF100,
        /// <summary>
        /// ウィンドウを元の位置とサイズに戻します。
        /// </summary>
        SC_RESTORE = 0xF120,
        /// <summary>
        /// スタートメニューを表示します。
        /// </summary>
        SC_TASKLIST = 0xF130,
        /// <summary>
        /// system.ini の [boot] セクションで指定されたスクリーンセーバーを起動します。
        /// </summary>
        SC_SCREENSAVE = 0xF140,
        /// <summary>
        /// アプリケーション指定のホットキーに関連付けられたウィンドウをアクティブにします。
        /// lParam パラメータの下位ワードがアクティブになるウィンドウのハンドルを示します。
        /// </summary>
        SC_HOTKEY = 0xF150,
        /// <summary>
        /// ユーザーがシステムメニューをダブルクリックしたことにより、デフォルトメニューアイテムを選択します。
        /// </summary>
        SC_DEFAULT = 0xF160,
        /// <summary>
        /// ディスプレイを表示状態を設定します。このコマンドは、バッテリーを使用するコンピュータなどのパワーセブ機能をもつデバイスをサポートします。 lParam パラメータが 1 のときはディスプレイが省電力表示に移行することを示し、 2 のときはディスプレイ表示を停止しようとしていることを示します。
        /// </summary>
        SC_MONITORPOWER = 0xF170,
        /// <summary>
        /// マウスポインタをクエスチョンマークに変えます。この後にユーザーがダイアログボックス内のコントロールをクリックすると、コントロールは WM_HELP メッセージを受け取ります。
        /// </summary>
        SC_CONTEXTHELP = 0xF180
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:列挙値を重複させることはできない")]
    public enum SB
    {
        SB_LINEUP = 0,
        SB_LINELEFT = 0,
        SB_LINEDOWN = 1,
        SB_LINERIGHT = 1,
        SB_PAGEUP = 2,
        SB_PAGELEFT = 2,
        SB_PAGEDOWN = 3,
        SB_PAGERIGHT = 3,
        SB_THUMBPOSITION = 4,
        SB_THUMBTRACK = 5,
        SB_TOP = 6,
        SB_LEFT = 6,
        SB_BOTTOM = 7,
        SB_RIGHT = 7,
        SB_ENDSCROLL = 8,

        /// <summary>
        /// 指定したウィンドウの標準的な水平スクロールバー内で、スクロールボックスの位置を設定します。hWnd パラメータで、ウィンドウのハンドルを指定しなければなりません。
        /// </summary>
        SB_HORZ = 0,
        /// <summary>
        /// 指定したウィンドウの標準的な垂直スクロールバー内で、スクロールボックスの位置を設定します。hWnd パラメータで、ウィンドウのハンドルを指定しなければなりません。
        /// </summary>
        SB_VERT = 1,
        /// <summary>
        /// スクロールバーコントロール内で、スクロールボックスの位置を設定します。hWnd パラメータで、スクロールバーコントロールのハンドルを指定しなければなりません。
        /// </summary>
        SB_CTL = 2,

    }

    public enum MA: int
    {
        MA_ACTIVATE = 1,
        MA_ACTIVATEANDEAT = 2,
        MA_NOACTIVATE = 3,
        MA_NOACTIVATEANDEAT = 4,
    }

    [Flags]
    public enum MOD: uint
    {
        None = 0,
        MOD_ALT = 0x0001,
        MOD_CONTROL = 0x0002,
        MOD_SHIFT = 0x0004,
        MOD_WIN = 0x0008,
        MOD_NOREPEAT = 0x4000
    }

    public enum HT
    {
        HTERROR = (-2),
        HTTRANSPARENT = (-1),
        HTNOWHERE = 0,
        HTCLIENT = 1,
        HTCAPTION = 2,
        HTSYSMENU = 3,
        HTGROWBOX = 4,
        HTSIZE = HTGROWBOX,
        HTMENU = 5,
        HTHSCROLL = 6,
        HTVSCROLL = 7,
        HTMINBUTTON = 8,
        HTMAXBUTTON = 9,
        HTLEFT = 10,
        HTRIGHT = 11,
        HTTOP = 12,
        HTTOPLEFT = 13,
        HTTOPRIGHT = 14,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        HTBORDER = 18,
        HTREDUCE = HTMINBUTTON,
        HTZOOM = HTMAXBUTTON,
        HTSIZEFIRST = HTLEFT,
        HTSIZELAST = HTBOTTOMRIGHT,
        HTOBJECT = 19,
        HTCLOSE = 20,
        HTHELP = 21,
    }

    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:列挙値を重複させることはできない")]
    public enum SWP: int
    {
        SWP_ASYNCWINDOWPOS = 0x4000,
        SWP_DEFERERASE = 0x2000,
        SWP_DRAWFRAME = 0x0020,
        SWP_FRAMECHANGED = 0x0020,
        SWP_HIDEWINDOW = 0x0080,
        SWP_NOACTIVATE = 0x0010,
        SWP_NOCOPYBITS = 0x0100,
        SWP_NOMOVE = 0x0002,
        SWP_NOOWNERZORDER = 0x0200,
        SWP_NOREDRAW = 0x0008,
        SWP_NOREPOSITION = 0x0200,
        SWP_NOSENDCHANGING = 0x0400,
        SWP_NOSIZE = 0x0001,
        SWP_NOZORDER = 0x0004,
        SWP_SHOWWINDOW = 0x0040,
    }

    public enum HWND
    {
        HWND_TOP = 0,
        HWND_BOTTOM = 1,
        HWND_TOPMOST = -1,
        HWND_NOTOPMOST = -2,
        HWND_BROADCAST = 0xffff,
        HWND_MESSAGE = -3,
    }


    /// <summary>
    ///
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:列挙値を重複させることはできない")]
    public enum SW: uint
    {
        /// <summary>
        ///        Hides the window and activates another window.
        /// </summary>
        SW_HIDE = 0,

        /// <summary>
        ///        Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
        /// </summary>
        SW_SHOWNORMAL = 1,

        /// <summary>
        ///        Activates and displays a window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when displaying the window for the first time.
        /// </summary>
        SW_NORMAL = 1,

        /// <summary>
        ///        Activates the window and displays it as a minimized window.
        /// </summary>
        SW_SHOWMINIMIZED = 2,

        /// <summary>
        ///        Activates the window and displays it as a maximized window.
        /// </summary>
        SW_SHOWMAXIMIZED = 3,

        /// <summary>
        ///        Maximizes the specified window.
        /// </summary>
        SW_MAXIMIZE = 3,

        /// <summary>
        ///        Displays a window in its most recent size and position. This value is similar to <see cref="SW.SW_SHOWNORMAL"/>, except the window is not activated.
        /// </summary>
        SW_SHOWNOACTIVATE = 4,

        /// <summary>
        ///        Activates the window and displays it in its current size and position.
        /// </summary>
        SW_SHOW = 5,

        /// <summary>
        ///        Minimizes the specified window and activates the next top-level window in the z-order.
        /// </summary>
        SW_MINIMIZE = 6,

        /// <summary>
        ///        Displays the window as a minimized window. This value is similar to <see cref="SW.SW_SHOWMINIMIZED"/>, except the window is not activated.
        /// </summary>
        SW_SHOWMINNOACTIVE = 7,

        /// <summary>
        ///        Displays the window in its current size and position. This value is similar to <see cref="SW.SW_SHOW"/>, except the window is not activated.
        /// </summary>
        SW_SHOWNA = 8,

        /// <summary>
        ///        Activates and displays the window. If the window is minimized or maximized, the system restores it to its original size and position. An application should specify this flag when restoring a minimized window.
        /// </summary>
        SW_RESTORE = 9
    }

    /// <summary>
    ///
    /// </summary>
    [Flags]
    public enum SMTO: uint
    {
        SMTO_NORMAL = 0x0,
        SMTO_BLOCK = 0x1,
        SMTO_ABORTIFHUNG = 0x2,
        SMTO_NOTIMEOUTIFNOTHUNG = 0x8
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:列挙値を重複させることはできない")]
    public enum SM
    {
        SM_CXSCREEN = 0,
        SM_CYSCREEN = 1,
        SM_CXVSCROLL = 2,
        SM_CYHSCROLL = 3,
        SM_CYCAPTION = 4,
        SM_CXBORDER = 5,
        SM_CYBORDER = 6,
        SM_CXDLGFRAME = 7,
        SM_CXFIXEDFRAME = 7,
        SM_CYDLGFRAME = 8,
        SM_CYFIXEDFRAME = 8,
        SM_CYVTHUMB = 9,
        SM_CXHTHUMB = 10,
        SM_CXICON = 11,
        SM_CYICON = 12,
        SM_CXCURSOR = 13,
        SM_CYCURSOR = 14,
        SM_CYMENU = 15,
        SM_CXFULLSCREEN = 16,
        SM_CYFULLSCREEN = 17,
        SM_CYKANJIWINDOW = 18,
        SM_MOUSEPRESENT = 19,
        SM_CYVSCROLL = 20,
        SM_CXHSCROLL = 21,
        SM_DEBUG = 22,
        SM_SWAPBUTTON = 23,
        SM_CXMIN = 28,
        SM_CYMIN = 29,
        SM_CXSIZE = 30,
        SM_CYSIZE = 31,
        SM_CXSIZEFRAME = 32,
        SM_CXFRAME = 32,
        SM_CYSIZEFRAME = 33,
        SM_CYFRAME = 33,
        SM_CXMINTRACK = 34,
        SM_CYMINTRACK = 35,
        SM_CXDOUBLECLK = 36,
        SM_CYDOUBLECLK = 37,
        SM_CXICONSPACING = 38,
        SM_CYICONSPACING = 39,
        SM_MENUDROPALIGNMENT = 40,
        SM_PENWINDOWS = 41,
        SM_DBCSENABLED = 42,
        SM_CMOUSEBUTTONS = 43,
        SM_SECURE = 44,
        SM_CXEDGE = 45,
        SM_CYEDGE = 46,
        SM_CXMINSPACING = 47,
        SM_CYMINSPACING = 48,
        SM_CXSMICON = 49,
        SM_CYSMICON = 50,
        SM_CYSMCAPTION = 51,
        SM_CXSMSIZE = 52,
        SM_CYSMSIZE = 53,
        SM_CXMENUSIZE = 54,
        SM_CYMENUSIZE = 55,
        SM_ARRANGE = 56,
        SM_CXMINIMIZED = 57,
        SM_CYMINIMIZED = 58,
        SM_CXMAXTRACK = 59,
        SM_CYMAXTRACK = 60,
        SM_CXMAXIMIZED = 61,
        SM_CYMAXIMIZED = 62,
        SM_NETWORK = 63,
        SM_CLEANBOOT = 67,
        SM_CXDRAG = 68,
        SM_CYDRAG = 69,
        SM_SHOWSOUNDS = 70,
        SM_CXMENUCHECK = 71,
        SM_CYMENUCHECK = 72,
        SM_SLOWMACHINE = 73,
        SM_MIDEASTENABLED = 74,
        SM_MOUSEWHEELPRESENT = 75,
        SM_XVIRTUALSCREEN = 76,
        SM_YVIRTUALSCREEN = 77,
        SM_CXVIRTUALSCREEN = 78,
        SM_CYVIRTUALSCREEN = 79,
        SM_CMONITORS = 80,
        SM_SAMEDISPLAYFORMAT = 81,
        SM_IMMENABLED = 82,
        SM_CXFOCUSBORDER = 83,
        SM_CYFOCUSBORDER = 84,
        SM_TABLETPC = 86,
        SM_MEDIACENTER = 87,
        SM_STARTER = 88,
        SM_SERVERR2 = 89,
        SM_MOUSEHORIZONTALWHEELPRESENT = 91,
        SM_CXPADDEDBORDER = 92,
        SM_DIGITIZER = 94,
        SM_MAXIMUMTOUCHES = 95,
        SM_REMOTESESSION = 0x1000,
        SM_SHUTTINGDOWN = 0x2000,
        SM_REMOTECONTROL = 0x2001,
    }

    #region SPI
    /// <summary>
    /// SPI_ System-wide parameter - Used in SystemParametersInfo function
    /// </summary>
    public enum SPI: uint
    {
        /// <summary>
        /// Determines whether the warning beeper is on.
        /// The pvParam parameter must point to a BOOL variable that receives TRUE if the beeper is on, or FALSE if it is off.
        /// </summary>
        SPI_GETBEEP = 0x0001,

        /// <summary>
        /// Turns the warning beeper on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
        /// </summary>
        SPI_SETBEEP = 0x0002,

        /// <summary>
        /// Retrieves the two mouse threshold values and the mouse speed.
        /// </summary>
        SPI_GETMOUSE = 0x0003,

        /// <summary>
        /// Sets the two mouse threshold values and the mouse speed.
        /// </summary>
        SPI_SETMOUSE = 0x0004,

        /// <summary>
        /// Retrieves the border multiplier factor that determines the width of a window's sizing border.
        /// The pvParam parameter must point to an integer variable that receives this value.
        /// </summary>
        SPI_GETBORDER = 0x0005,

        /// <summary>
        /// Sets the border multiplier factor that determines the width of a window's sizing border.
        /// The uiParam parameter specifies the new value.
        /// </summary>
        SPI_SETBORDER = 0x0006,

        /// <summary>
        /// Retrieves the keyboard repeat-speed setting, which is a value in the range from 0 (approximately 2.5 repetitions per second)
        /// through 31 (approximately 30 repetitions per second). The actual repeat rates are hardware-dependent and may vary from
        /// a linear scale by as much as 20%. The pvParam parameter must point to a DWORD variable that receives the setting
        /// </summary>
        SPI_GETKEYBOARDSPEED = 0x000A,

        /// <summary>
        /// Sets the keyboard repeat-speed setting. The uiParam parameter must specify a value in the range from 0
        /// (approximately 2.5 repetitions per second) through 31 (approximately 30 repetitions per second).
        /// The actual repeat rates are hardware-dependent and may vary from a linear scale by as much as 20%.
        /// If uiParam is greater than 31, the parameter is set to 31.
        /// </summary>
        SPI_SETKEYBOARDSPEED = 0x000B,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SPI_LANGDRIVER = 0x000C,

        /// <summary>
        /// Sets or retrieves the width, in pixels, of an icon cell. The system uses this rectangle to arrange icons in large icon view.
        /// To set this value, set uiParam to the new value and set pvParam to null. You cannot set this value to less than SM_CXICON.
        /// To retrieve this value, pvParam must point to an integer that receives the current value.
        /// </summary>
        SPI_ICONHORIZONTALSPACING = 0x000D,

        /// <summary>
        /// Retrieves the screen saver time-out value, in seconds. The pvParam parameter must point to an integer variable that receives the value.
        /// </summary>
        SPI_GETSCREENSAVETIMEOUT = 0x000E,

        /// <summary>
        /// Sets the screen saver time-out value to the value of the uiParam parameter. This value is the amount of time, in seconds,
        /// that the system must be idle before the screen saver activates.
        /// </summary>
        SPI_SETSCREENSAVETIMEOUT = 0x000F,

        /// <summary>
        /// Determines whether screen saving is enabled. The pvParam parameter must point to a bool variable that receives TRUE
        /// if screen saving is enabled, or FALSE otherwise.
        /// Does not work for Windows 7: http://msdn.microsoft.com/en-us/library/windows/desktop/ms724947(v=vs.85).aspx
        /// </summary>
        SPI_GETSCREENSAVEACTIVE = 0x0010,

        /// <summary>
        /// Sets the state of the screen saver. The uiParam parameter specifies TRUE to activate screen saving, or FALSE to deactivate it.
        /// </summary>
        SPI_SETSCREENSAVEACTIVE = 0x0011,

        /// <summary>
        /// Retrieves the current granularity value of the desktop sizing grid. The pvParam parameter must point to an integer variable
        /// that receives the granularity.
        /// </summary>
        SPI_GETGRIDGRANULARITY = 0x0012,

        /// <summary>
        /// Sets the granularity of the desktop sizing grid to the value of the uiParam parameter.
        /// </summary>
        SPI_SETGRIDGRANULARITY = 0x0013,

        /// <summary>
        /// Sets the desktop wallpaper. The value of the pvParam parameter determines the new wallpaper. To specify a wallpaper bitmap,
        /// set pvParam to point to a null-terminated string containing the name of a bitmap file. Setting pvParam to "" removes the wallpaper.
        /// Setting pvParam to SETWALLPAPER_DEFAULT or null reverts to the default wallpaper.
        /// </summary>
        SPI_SETDESKWALLPAPER = 0x0014,

        /// <summary>
        /// Sets the current desktop pattern by causing Windows to read the Pattern= setting from the WIN.INI file.
        /// </summary>
        SPI_SETDESKPATTERN = 0x0015,

        /// <summary>
        /// Retrieves the keyboard repeat-delay setting, which is a value in the range from 0 (approximately 250 ms delay) through 3
        /// (approximately 1 second delay). The actual delay associated with each value may vary depending on the hardware. The pvParam parameter must point to an integer variable that receives the setting.
        /// </summary>
        SPI_GETKEYBOARDDELAY = 0x0016,

        /// <summary>
        /// Sets the keyboard repeat-delay setting. The uiParam parameter must specify 0, 1, 2, or 3, where zero sets the shortest delay
        /// (approximately 250 ms) and 3 sets the longest delay (approximately 1 second). The actual delay associated with each value may
        /// vary depending on the hardware.
        /// </summary>
        SPI_SETKEYBOARDDELAY = 0x0017,

        /// <summary>
        /// Sets or retrieves the height, in pixels, of an icon cell.
        /// To set this value, set uiParam to the new value and set pvParam to null. You cannot set this value to less than SM_CYICON.
        /// To retrieve this value, pvParam must point to an integer that receives the current value.
        /// </summary>
        SPI_ICONVERTICALSPACING = 0x0018,

        /// <summary>
        /// Determines whether icon-title wrapping is enabled. The pvParam parameter must point to a bool variable that receives TRUE
        /// if enabled, or FALSE otherwise.
        /// </summary>
        SPI_GETICONTITLEWRAP = 0x0019,

        /// <summary>
        /// Turns icon-title wrapping on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
        /// </summary>
        SPI_SETICONTITLEWRAP = 0x001A,

        /// <summary>
        /// Determines whether pop-up menus are left-aligned or right-aligned, relative to the corresponding menu-bar item.
        /// The pvParam parameter must point to a bool variable that receives TRUE if left-aligned, or FALSE otherwise.
        /// </summary>
        SPI_GETMENUDROPALIGNMENT = 0x001B,

        /// <summary>
        /// Sets the alignment value of pop-up menus. The uiParam parameter specifies TRUE for right alignment, or FALSE for left alignment.
        /// </summary>
        SPI_SETMENUDROPALIGNMENT = 0x001C,

        /// <summary>
        /// Sets the width of the double-click rectangle to the value of the uiParam parameter.
        /// The double-click rectangle is the rectangle within which the second click of a double-click must fall for it to be registered
        /// as a double-click.
        /// To retrieve the width of the double-click rectangle, call GetSystemMetrics with the SM_CXDOUBLECLK flag.
        /// </summary>
        SPI_SETDOUBLECLKWIDTH = 0x001D,

        /// <summary>
        /// Sets the height of the double-click rectangle to the value of the uiParam parameter.
        /// The double-click rectangle is the rectangle within which the second click of a double-click must fall for it to be registered
        /// as a double-click.
        /// To retrieve the height of the double-click rectangle, call GetSystemMetrics with the SM_CYDOUBLECLK flag.
        /// </summary>
        SPI_SETDOUBLECLKHEIGHT = 0x001E,

        /// <summary>
        /// Retrieves the logical font information for the current icon-title font. The uiParam parameter specifies the size of a LOGFONT structure,
        /// and the pvParam parameter must point to the LOGFONT structure to fill in.
        /// </summary>
        SPI_GETICONTITLELOGFONT = 0x001F,

        /// <summary>
        /// Sets the double-click time for the mouse to the value of the uiParam parameter. The double-click time is the maximum number
        /// of milliseconds that can occur between the first and second clicks of a double-click. You can also call the SetDoubleClickTime
        /// function to set the double-click time. To get the current double-click time, call the GetDoubleClickTime function.
        /// </summary>
        SPI_SETDOUBLECLICKTIME = 0x0020,

        /// <summary>
        /// Swaps or restores the meaning of the left and right mouse buttons. The uiParam parameter specifies TRUE to swap the meanings
        /// of the buttons, or FALSE to restore their original meanings.
        /// </summary>
        SPI_SETMOUSEBUTTONSWAP = 0x0021,

        /// <summary>
        /// Sets the font that is used for icon titles. The uiParam parameter specifies the size of a LOGFONT structure,
        /// and the pvParam parameter must point to a LOGFONT structure.
        /// </summary>
        SPI_SETICONTITLELOGFONT = 0x0022,

        /// <summary>
        /// This flag is obsolete. Previous versions of the system use this flag to determine whether ALT+TAB fast task switching is enabled.
        /// For Windows 95, Windows 98, and Windows NT version 4.0 and later, fast task switching is always enabled.
        /// </summary>
        SPI_GETFASTTASKSWITCH = 0x0023,

        /// <summary>
        /// This flag is obsolete. Previous versions of the system use this flag to enable or disable ALT+TAB fast task switching.
        /// For Windows 95, Windows 98, and Windows NT version 4.0 and later, fast task switching is always enabled.
        /// </summary>
        SPI_SETFASTTASKSWITCH = 0x0024,

        //#if(WINVER >= 0x0400)
        /// <summary>
        /// Sets dragging of full windows either on or off. The uiParam parameter specifies TRUE for on, or FALSE for off.
        /// Windows 95:  This flag is supported only if Windows Plus! is installed. See SPI_GETWINDOWSEXTENSION.
        /// </summary>
        SPI_SETDRAGFULLWINDOWS = 0x0025,

        /// <summary>
        /// Determines whether dragging of full windows is enabled. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if enabled, or FALSE otherwise.
        /// Windows 95:  This flag is supported only if Windows Plus! is installed. See SPI_GETWINDOWSEXTENSION.
        /// </summary>
        SPI_GETDRAGFULLWINDOWS = 0x0026,

        /// <summary>
        /// Retrieves the metrics associated with the nonclient area of nonminimized windows. The pvParam parameter must point
        /// to a NONCLIENTMETRICS structure that receives the information. Set the cbSize member of this structure and the uiParam parameter
        /// to sizeof(NONCLIENTMETRICS).
        /// </summary>
        SPI_GETNONCLIENTMETRICS = 0x0029,

        /// <summary>
        /// Sets the metrics associated with the nonclient area of nonminimized windows. The pvParam parameter must point
        /// to a NONCLIENTMETRICS structure that contains the new parameters. Set the cbSize member of this structure
        /// and the uiParam parameter to sizeof(NONCLIENTMETRICS). Also, the lfHeight member of the LOGFONT structure must be a negative value.
        /// </summary>
        SPI_SETNONCLIENTMETRICS = 0x002A,

        /// <summary>
        /// Retrieves the metrics associated with minimized windows. The pvParam parameter must point to a MINIMIZEDMETRICS structure
        /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(MINIMIZEDMETRICS).
        /// </summary>
        SPI_GETMINIMIZEDMETRICS = 0x002B,

        /// <summary>
        /// Sets the metrics associated with minimized windows. The pvParam parameter must point to a MINIMIZEDMETRICS structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(MINIMIZEDMETRICS).
        /// </summary>
        SPI_SETMINIMIZEDMETRICS = 0x002C,

        /// <summary>
        /// Retrieves the metrics associated with icons. The pvParam parameter must point to an ICONMETRICS structure that receives
        /// the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(ICONMETRICS).
        /// </summary>
        SPI_GETICONMETRICS = 0x002D,

        /// <summary>
        /// Sets the metrics associated with icons. The pvParam parameter must point to an ICONMETRICS structure that contains
        /// the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(ICONMETRICS).
        /// </summary>
        SPI_SETICONMETRICS = 0x002E,

        /// <summary>
        /// Sets the size of the work area. The work area is the portion of the screen not obscured by the system taskbar
        /// or by application desktop toolbars. The pvParam parameter is a pointer to a RECT structure that specifies the new work area rectangle,
        /// expressed in virtual screen coordinates. In a system with multiple display monitors, the function sets the work area
        /// of the monitor that contains the specified rectangle.
        /// </summary>
        SPI_SETWORKAREA = 0x002F,

        /// <summary>
        /// Retrieves the size of the work area on the primary display monitor. The work area is the portion of the screen not obscured
        /// by the system taskbar or by application desktop toolbars. The pvParam parameter must point to a RECT structure that receives
        /// the coordinates of the work area, expressed in virtual screen coordinates.
        /// To get the work area of a monitor other than the primary display monitor, call the GetMonitorInfo function.
        /// </summary>
        SPI_GETWORKAREA = 0x0030,

        /// <summary>
        /// Windows Me/98/95:  Pen windows is being loaded or unloaded. The uiParam parameter is TRUE when loading and FALSE
        /// when unloading pen windows. The pvParam parameter is null.
        /// </summary>
        SPI_SETPENWINDOWS = 0x0031,

        /// <summary>
        /// Retrieves information about the HighContrast accessibility feature. The pvParam parameter must point to a HIGHCONTRAST structure
        /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(HIGHCONTRAST).
        /// For a general discussion, see remarks.
        /// Windows NT:  This value is not supported.
        /// </summary>
        /// <remarks>
        /// There is a difference between the High Contrast color scheme and the High Contrast Mode. The High Contrast color scheme changes
        /// the system colors to colors that have obvious contrast; you switch to this color scheme by using the Display Options in the control panel.
        /// The High Contrast Mode, which uses SPI_GETHIGHCONTRAST and SPI_SETHIGHCONTRAST, advises applications to modify their appearance
        /// for visually-impaired users. It involves such things as audible warning to users and customized color scheme
        /// (using the Accessibility Options in the control panel). For more information, see HIGHCONTRAST on MSDN.
        /// For more information on general accessibility features, see Accessibility on MSDN.
        /// </remarks>
        SPI_GETHIGHCONTRAST = 0x0042,

        /// <summary>
        /// Sets the parameters of the HighContrast accessibility feature. The pvParam parameter must point to a HIGHCONTRAST structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(HIGHCONTRAST).
        /// Windows NT:  This value is not supported.
        /// </summary>
        SPI_SETHIGHCONTRAST = 0x0043,

        /// <summary>
        /// Determines whether the user relies on the keyboard instead of the mouse, and wants applications to display keyboard interfaces
        /// that would otherwise be hidden. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if the user relies on the keyboard; or FALSE otherwise.
        /// Windows NT:  This value is not supported.
        /// </summary>
        SPI_GETKEYBOARDPREF = 0x0044,

        /// <summary>
        /// Sets the keyboard preference. The uiParam parameter specifies TRUE if the user relies on the keyboard instead of the mouse,
        /// and wants applications to display keyboard interfaces that would otherwise be hidden; uiParam is FALSE otherwise.
        /// Windows NT:  This value is not supported.
        /// </summary>
        SPI_SETKEYBOARDPREF = 0x0045,

        /// <summary>
        /// Determines whether a screen reviewer utility is running. A screen reviewer utility directs textual information to an output device,
        /// such as a speech synthesizer or Braille display. When this flag is set, an application should provide textual information
        /// in situations where it would otherwise present the information graphically.
        /// The pvParam parameter is a pointer to a BOOL variable that receives TRUE if a screen reviewer utility is running, or FALSE otherwise.
        /// Windows NT:  This value is not supported.
        /// </summary>
        SPI_GETSCREENREADER = 0x0046,

        /// <summary>
        /// Determines whether a screen review utility is running. The uiParam parameter specifies TRUE for on, or FALSE for off.
        /// Windows NT:  This value is not supported.
        /// </summary>
        SPI_SETSCREENREADER = 0x0047,

        /// <summary>
        /// Retrieves the animation effects associated with user actions. The pvParam parameter must point to an ANIMATIONINFO structure
        /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(ANIMATIONINFO).
        /// </summary>
        SPI_GETANIMATION = 0x0048,

        /// <summary>
        /// Sets the animation effects associated with user actions. The pvParam parameter must point to an ANIMATIONINFO structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(ANIMATIONINFO).
        /// </summary>
        SPI_SETANIMATION = 0x0049,

        /// <summary>
        /// Determines whether the font smoothing feature is enabled. This feature uses font antialiasing to make font curves appear smoother
        /// by painting pixels at different gray levels.
        /// The pvParam parameter must point to a BOOL variable that receives TRUE if the feature is enabled, or FALSE if it is not.
        /// Windows 95:  This flag is supported only if Windows Plus! is installed. See SPI_GETWINDOWSEXTENSION.
        /// </summary>
        SPI_GETFONTSMOOTHING = 0x004A,

        /// <summary>
        /// Enables or disables the font smoothing feature, which uses font antialiasing to make font curves appear smoother
        /// by painting pixels at different gray levels.
        /// To enable the feature, set the uiParam parameter to TRUE. To disable the feature, set uiParam to FALSE.
        /// Windows 95:  This flag is supported only if Windows Plus! is installed. See SPI_GETWINDOWSEXTENSION.
        /// </summary>
        SPI_SETFONTSMOOTHING = 0x004B,

        /// <summary>
        /// Sets the width, in pixels, of the rectangle used to detect the start of a drag operation. Set uiParam to the new value.
        /// To retrieve the drag width, call GetSystemMetrics with the SM_CXDRAG flag.
        /// </summary>
        SPI_SETDRAGWIDTH = 0x004C,

        /// <summary>
        /// Sets the height, in pixels, of the rectangle used to detect the start of a drag operation. Set uiParam to the new value.
        /// To retrieve the drag height, call GetSystemMetrics with the SM_CYDRAG flag.
        /// </summary>
        SPI_SETDRAGHEIGHT = 0x004D,

        /// <summary>
        /// Used internally; applications should not use this value.
        /// </summary>
        SPI_SETHANDHELD = 0x004E,

        /// <summary>
        /// Retrieves the time-out value for the low-power phase of screen saving. The pvParam parameter must point to an integer variable
        /// that receives the value. This flag is supported for 32-bit applications only.
        /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
        /// Windows 95:  This flag is supported for 16-bit applications only.
        /// </summary>
        SPI_GETLOWPOWERTIMEOUT = 0x004F,

        /// <summary>
        /// Retrieves the time-out value for the power-off phase of screen saving. The pvParam parameter must point to an integer variable
        /// that receives the value. This flag is supported for 32-bit applications only.
        /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
        /// Windows 95:  This flag is supported for 16-bit applications only.
        /// </summary>
        SPI_GETPOWEROFFTIMEOUT = 0x0050,

        /// <summary>
        /// Sets the time-out value, in seconds, for the low-power phase of screen saving. The uiParam parameter specifies the new value.
        /// The pvParam parameter must be null. This flag is supported for 32-bit applications only.
        /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
        /// Windows 95:  This flag is supported for 16-bit applications only.
        /// </summary>
        SPI_SETLOWPOWERTIMEOUT = 0x0051,

        /// <summary>
        /// Sets the time-out value, in seconds, for the power-off phase of screen saving. The uiParam parameter specifies the new value.
        /// The pvParam parameter must be null. This flag is supported for 32-bit applications only.
        /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
        /// Windows 95:  This flag is supported for 16-bit applications only.
        /// </summary>
        SPI_SETPOWEROFFTIMEOUT = 0x0052,

        /// <summary>
        /// Determines whether the low-power phase of screen saving is enabled. The pvParam parameter must point to a BOOL variable
        /// that receives TRUE if enabled, or FALSE if disabled. This flag is supported for 32-bit applications only.
        /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
        /// Windows 95:  This flag is supported for 16-bit applications only.
        /// </summary>
        SPI_GETLOWPOWERACTIVE = 0x0053,

        /// <summary>
        /// Determines whether the power-off phase of screen saving is enabled. The pvParam parameter must point to a BOOL variable
        /// that receives TRUE if enabled, or FALSE if disabled. This flag is supported for 32-bit applications only.
        /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
        /// Windows 95:  This flag is supported for 16-bit applications only.
        /// </summary>
        SPI_GETPOWEROFFACTIVE = 0x0054,

        /// <summary>
        /// Activates or deactivates the low-power phase of screen saving. Set uiParam to 1 to activate, or zero to deactivate.
        /// The pvParam parameter must be null. This flag is supported for 32-bit applications only.
        /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
        /// Windows 95:  This flag is supported for 16-bit applications only.
        /// </summary>
        SPI_SETLOWPOWERACTIVE = 0x0055,

        /// <summary>
        /// Activates or deactivates the power-off phase of screen saving. Set uiParam to 1 to activate, or zero to deactivate.
        /// The pvParam parameter must be null. This flag is supported for 32-bit applications only.
        /// Windows NT, Windows Me/98:  This flag is supported for 16-bit and 32-bit applications.
        /// Windows 95:  This flag is supported for 16-bit applications only.
        /// </summary>
        SPI_SETPOWEROFFACTIVE = 0x0056,

        /// <summary>
        /// Reloads the system cursors. Set the uiParam parameter to zero and the pvParam parameter to null.
        /// </summary>
        SPI_SETCURSORS = 0x0057,

        /// <summary>
        /// Reloads the system icons. Set the uiParam parameter to zero and the pvParam parameter to null.
        /// </summary>
        SPI_SETICONS = 0x0058,

        /// <summary>
        /// Retrieves the input locale identifier for the system default input language. The pvParam parameter must point
        /// to an HKL variable that receives this value. For more information, see Languages, Locales, and Keyboard Layouts on MSDN.
        /// </summary>
        SPI_GETDEFAULTINPUTLANG = 0x0059,

        /// <summary>
        /// Sets the default input language for the system shell and applications. The specified language must be displayable
        /// using the current system character set. The pvParam parameter must point to an HKL variable that contains
        /// the input locale identifier for the default language. For more information, see Languages, Locales, and Keyboard Layouts on MSDN.
        /// </summary>
        SPI_SETDEFAULTINPUTLANG = 0x005A,

        /// <summary>
        /// Sets the hot key set for switching between input languages. The uiParam and pvParam parameters are not used.
        /// The value sets the shortcut keys in the keyboard property sheets by reading the registry again. The registry must be set before this flag is used. the path in the registry is \HKEY_CURRENT_USER\keyboard layout\toggle. Valid values are "1" = ALT+SHIFT, "2" = CTRL+SHIFT, and "3" = none.
        /// </summary>
        SPI_SETLANGTOGGLE = 0x005B,

        /// <summary>
        /// Windows 95:  Determines whether the Windows extension, Windows Plus!, is installed. Set the uiParam parameter to 1.
        /// The pvParam parameter is not used. The function returns TRUE if the extension is installed, or FALSE if it is not.
        /// </summary>
        SPI_GETWINDOWSEXTENSION = 0x005C,

        /// <summary>
        /// Enables or disables the Mouse Trails feature, which improves the visibility of mouse cursor movements by briefly showing
        /// a trail of cursors and quickly erasing them.
        /// To disable the feature, set the uiParam parameter to zero or 1. To enable the feature, set uiParam to a value greater than 1
        /// to indicate the number of cursors drawn in the trail.
        /// Windows 2000/NT:  This value is not supported.
        /// </summary>
        SPI_SETMOUSETRAILS = 0x005D,

        /// <summary>
        /// Determines whether the Mouse Trails feature is enabled. This feature improves the visibility of mouse cursor movements
        /// by briefly showing a trail of cursors and quickly erasing them.
        /// The pvParam parameter must point to an integer variable that receives a value. If the value is zero or 1, the feature is disabled.
        /// If the value is greater than 1, the feature is enabled and the value indicates the number of cursors drawn in the trail.
        /// The uiParam parameter is not used.
        /// Windows 2000/NT:  This value is not supported.
        /// </summary>
        SPI_GETMOUSETRAILS = 0x005E,

        /// <summary>
        /// Windows Me/98:  Used internally; applications should not use this flag.
        /// </summary>
        SPI_SETSCREENSAVERRUNNING = 0x0061,

        /// <summary>
        /// Same as SPI_SETSCREENSAVERRUNNING.
        /// </summary>
        SPI_SCREENSAVERRUNNING = SPI_SETSCREENSAVERRUNNING,
        //#endif /* WINVER >= 0x0400 */

        /// <summary>
        /// Retrieves information about the FilterKeys accessibility feature. The pvParam parameter must point to a FILTERKEYS structure
        /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(FILTERKEYS).
        /// </summary>
        SPI_GETFILTERKEYS = 0x0032,

        /// <summary>
        /// Sets the parameters of the FilterKeys accessibility feature. The pvParam parameter must point to a FILTERKEYS structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(FILTERKEYS).
        /// </summary>
        SPI_SETFILTERKEYS = 0x0033,

        /// <summary>
        /// Retrieves information about the ToggleKeys accessibility feature. The pvParam parameter must point to a TOGGLEKEYS structure
        /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(TOGGLEKEYS).
        /// </summary>
        SPI_GETTOGGLEKEYS = 0x0034,

        /// <summary>
        /// Sets the parameters of the ToggleKeys accessibility feature. The pvParam parameter must point to a TOGGLEKEYS structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(TOGGLEKEYS).
        /// </summary>
        SPI_SETTOGGLEKEYS = 0x0035,

        /// <summary>
        /// Retrieves information about the MouseKeys accessibility feature. The pvParam parameter must point to a MOUSEKEYS structure
        /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(MOUSEKEYS).
        /// </summary>
        SPI_GETMOUSEKEYS = 0x0036,

        /// <summary>
        /// Sets the parameters of the MouseKeys accessibility feature. The pvParam parameter must point to a MOUSEKEYS structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(MOUSEKEYS).
        /// </summary>
        SPI_SETMOUSEKEYS = 0x0037,

        /// <summary>
        /// Determines whether the Show Sounds accessibility flag is on or off. If it is on, the user requires an application
        /// to present information visually in situations where it would otherwise present the information only in audible form.
        /// The pvParam parameter must point to a BOOL variable that receives TRUE if the feature is on, or FALSE if it is off.
        /// Using this value is equivalent to calling GetSystemMetrics (SM_SHOWSOUNDS). That is the recommended call.
        /// </summary>
        SPI_GETSHOWSOUNDS = 0x0038,

        /// <summary>
        /// Sets the parameters of the SoundSentry accessibility feature. The pvParam parameter must point to a SOUNDSENTRY structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(SOUNDSENTRY).
        /// </summary>
        SPI_SETSHOWSOUNDS = 0x0039,

        /// <summary>
        /// Retrieves information about the StickyKeys accessibility feature. The pvParam parameter must point to a STICKYKEYS structure
        /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(STICKYKEYS).
        /// </summary>
        SPI_GETSTICKYKEYS = 0x003A,

        /// <summary>
        /// Sets the parameters of the StickyKeys accessibility feature. The pvParam parameter must point to a STICKYKEYS structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(STICKYKEYS).
        /// </summary>
        SPI_SETSTICKYKEYS = 0x003B,

        /// <summary>
        /// Retrieves information about the time-out period associated with the accessibility features. The pvParam parameter must point
        /// to an ACCESSTIMEOUT structure that receives the information. Set the cbSize member of this structure and the uiParam parameter
        /// to sizeof(ACCESSTIMEOUT).
        /// </summary>
        SPI_GETACCESSTIMEOUT = 0x003C,

        /// <summary>
        /// Sets the time-out period associated with the accessibility features. The pvParam parameter must point to an ACCESSTIMEOUT
        /// structure that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(ACCESSTIMEOUT).
        /// </summary>
        SPI_SETACCESSTIMEOUT = 0x003D,

        //#if(WINVER >= 0x0400)
        /// <summary>
        /// Windows Me/98/95:  Retrieves information about the SerialKeys accessibility feature. The pvParam parameter must point
        /// to a SERIALKEYS structure that receives the information. Set the cbSize member of this structure and the uiParam parameter
        /// to sizeof(SERIALKEYS).
        /// Windows Server 2003, Windows XP/2000/NT:  Not supported. The user controls this feature through the control panel.
        /// </summary>
        SPI_GETSERIALKEYS = 0x003E,

        /// <summary>
        /// Windows Me/98/95:  Sets the parameters of the SerialKeys accessibility feature. The pvParam parameter must point
        /// to a SERIALKEYS structure that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter
        /// to sizeof(SERIALKEYS).
        /// Windows Server 2003, Windows XP/2000/NT:  Not supported. The user controls this feature through the control panel.
        /// </summary>
        SPI_SETSERIALKEYS = 0x003F,
        //#endif /* WINVER >= 0x0400 */

        /// <summary>
        /// Retrieves information about the SoundSentry accessibility feature. The pvParam parameter must point to a SOUNDSENTRY structure
        /// that receives the information. Set the cbSize member of this structure and the uiParam parameter to sizeof(SOUNDSENTRY).
        /// </summary>
        SPI_GETSOUNDSENTRY = 0x0040,

        /// <summary>
        /// Sets the parameters of the SoundSentry accessibility feature. The pvParam parameter must point to a SOUNDSENTRY structure
        /// that contains the new parameters. Set the cbSize member of this structure and the uiParam parameter to sizeof(SOUNDSENTRY).
        /// </summary>
        SPI_SETSOUNDSENTRY = 0x0041,

        //#if(_WIN32_WINNT >= 0x0400)
        /// <summary>
        /// Determines whether the snap-to-default-button feature is enabled. If enabled, the mouse cursor automatically moves
        /// to the default button, such as OK or Apply, of a dialog box. The pvParam parameter must point to a BOOL variable
        /// that receives TRUE if the feature is on, or FALSE if it is off.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_GETSNAPTODEFBUTTON = 0x005F,

        /// <summary>
        /// Enables or disables the snap-to-default-button feature. If enabled, the mouse cursor automatically moves to the default button,
        /// such as OK or Apply, of a dialog box. Set the uiParam parameter to TRUE to enable the feature, or FALSE to disable it.
        /// Applications should use the ShowWindow function when displaying a dialog box so the dialog manager can position the mouse cursor.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_SETSNAPTODEFBUTTON = 0x0060,
        //#endif /* _WIN32_WINNT >= 0x0400 */

        //#if (_WIN32_WINNT >= 0x0400) || (_WIN32_WINDOWS > 0x0400)
        /// <summary>
        /// Retrieves the width, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent
        /// to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the width.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_GETMOUSEHOVERWIDTH = 0x0062,

        /// <summary>
        /// Retrieves the width, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent
        /// to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the width.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_SETMOUSEHOVERWIDTH = 0x0063,

        /// <summary>
        /// Retrieves the height, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent
        /// to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the height.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_GETMOUSEHOVERHEIGHT = 0x0064,

        /// <summary>
        /// Sets the height, in pixels, of the rectangle within which the mouse pointer has to stay for TrackMouseEvent
        /// to generate a WM_MOUSEHOVER message. Set the uiParam parameter to the new height.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_SETMOUSEHOVERHEIGHT = 0x0065,

        /// <summary>
        /// Retrieves the time, in milliseconds, that the mouse pointer has to stay in the hover rectangle for TrackMouseEvent
        /// to generate a WM_MOUSEHOVER message. The pvParam parameter must point to a UINT variable that receives the time.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_GETMOUSEHOVERTIME = 0x0066,

        /// <summary>
        /// Sets the time, in milliseconds, that the mouse pointer has to stay in the hover rectangle for TrackMouseEvent
        /// to generate a WM_MOUSEHOVER message. This is used only if you pass HOVER_DEFAULT in the dwHoverTime parameter in the call to TrackMouseEvent. Set the uiParam parameter to the new time.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_SETMOUSEHOVERTIME = 0x0067,

        /// <summary>
        /// Retrieves the number of lines to scroll when the mouse wheel is rotated. The pvParam parameter must point
        /// to a UINT variable that receives the number of lines. The default value is 3.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_GETWHEELSCROLLLINES = 0x0068,

        /// <summary>
        /// Sets the number of lines to scroll when the mouse wheel is rotated. The number of lines is set from the uiParam parameter.
        /// The number of lines is the suggested number of lines to scroll when the mouse wheel is rolled without using modifier keys.
        /// If the number is 0, then no scrolling should occur. If the number of lines to scroll is greater than the number of lines viewable,
        /// and in particular if it is WHEEL_PAGESCROLL (#defined as UINT_MAX), the scroll operation should be interpreted
        /// as clicking once in the page down or page up regions of the scroll bar.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_SETWHEELSCROLLLINES = 0x0069,

        /// <summary>
        /// Retrieves the time, in milliseconds, that the system waits before displaying a shortcut menu when the mouse cursor is
        /// over a submenu item. The pvParam parameter must point to a DWORD variable that receives the time of the delay.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_GETMENUSHOWDELAY = 0x006A,

        /// <summary>
        /// Sets uiParam to the time, in milliseconds, that the system waits before displaying a shortcut menu when the mouse cursor is
        /// over a submenu item.
        /// Windows 95:  Not supported.
        /// </summary>
        SPI_SETMENUSHOWDELAY = 0x006B,

        /// <summary>
        /// Determines whether the IME status window is visible (on a per-user basis). The pvParam parameter must point to a BOOL variable
        /// that receives TRUE if the status window is visible, or FALSE if it is not.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETSHOWIMEUI = 0x006E,

        /// <summary>
        /// Sets whether the IME status window is visible or not on a per-user basis. The uiParam parameter specifies TRUE for on or FALSE for off.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETSHOWIMEUI = 0x006F,
        //#endif

        //#if(WINVER >= 0x0500)
        /// <summary>
        /// Retrieves the current mouse speed. The mouse speed determines how far the pointer will move based on the distance the mouse moves.
        /// The pvParam parameter must point to an integer that receives a value which ranges between 1 (slowest) and 20 (fastest).
        /// A value of 10 is the default. The value can be set by an end user using the mouse control panel application or
        /// by an application using SPI_SETMOUSESPEED.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETMOUSESPEED = 0x0070,

        /// <summary>
        /// Sets the current mouse speed. The pvParam parameter is an integer between 1 (slowest) and 20 (fastest). A value of 10 is the default.
        /// This value is typically set using the mouse control panel application.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETMOUSESPEED = 0x0071,

        /// <summary>
        /// Determines whether a screen saver is currently running on the window station of the calling process.
        /// The pvParam parameter must point to a BOOL variable that receives TRUE if a screen saver is currently running, or FALSE otherwise.
        /// Note that only the interactive window station, "WinSta0", can have a screen saver running.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETSCREENSAVERRUNNING = 0x0072,

        /// <summary>
        /// Retrieves the full path of the bitmap file for the desktop wallpaper. The pvParam parameter must point to a buffer
        /// that receives a null-terminated path string. Set the uiParam parameter to the size, in characters, of the pvParam buffer. The returned string will not exceed MAX_PATH characters. If there is no desktop wallpaper, the returned string is empty.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETDESKWALLPAPER = 0x0073,
        //#endif /* WINVER >= 0x0500 */

        //#if(WINVER >= 0x0500)
        /// <summary>
        /// Determines whether active window tracking (activating the window the mouse is on) is on or off. The pvParam parameter must point
        /// to a BOOL variable that receives TRUE for on, or FALSE for off.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETACTIVEWINDOWTRACKING = 0x1000,

        /// <summary>
        /// Sets active window tracking (activating the window the mouse is on) either on or off. Set pvParam to TRUE for on or FALSE for off.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETACTIVEWINDOWTRACKING = 0x1001,

        /// <summary>
        /// Determines whether the menu animation feature is enabled. This master switch must be on to enable menu animation effects.
        /// The pvParam parameter must point to a BOOL variable that receives TRUE if animation is enabled and FALSE if it is disabled.
        /// If animation is enabled, SPI_GETMENUFADE indicates whether menus use fade or slide animation.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETMENUANIMATION = 0x1002,

        /// <summary>
        /// Enables or disables menu animation. This master switch must be on for any menu animation to occur.
        /// The pvParam parameter is a BOOL variable; set pvParam to TRUE to enable animation and FALSE to disable animation.
        /// If animation is enabled, SPI_GETMENUFADE indicates whether menus use fade or slide animation.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETMENUANIMATION = 0x1003,

        /// <summary>
        /// Determines whether the slide-open effect for combo boxes is enabled. The pvParam parameter must point to a BOOL variable
        /// that receives TRUE for enabled, or FALSE for disabled.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETCOMBOBOXANIMATION = 0x1004,

        /// <summary>
        /// Enables or disables the slide-open effect for combo boxes. Set the pvParam parameter to TRUE to enable the gradient effect,
        /// or FALSE to disable it.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETCOMBOBOXANIMATION = 0x1005,

        /// <summary>
        /// Determines whether the smooth-scrolling effect for list boxes is enabled. The pvParam parameter must point to a BOOL variable
        /// that receives TRUE for enabled, or FALSE for disabled.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETLISTBOXSMOOTHSCROLLING = 0x1006,

        /// <summary>
        /// Enables or disables the smooth-scrolling effect for list boxes. Set the pvParam parameter to TRUE to enable the smooth-scrolling effect,
        /// or FALSE to disable it.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETLISTBOXSMOOTHSCROLLING = 0x1007,

        /// <summary>
        /// Determines whether the gradient effect for window title bars is enabled. The pvParam parameter must point to a BOOL variable
        /// that receives TRUE for enabled, or FALSE for disabled. For more information about the gradient effect, see the GetSysColor function.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETGRADIENTCAPTIONS = 0x1008,

        /// <summary>
        /// Enables or disables the gradient effect for window title bars. Set the pvParam parameter to TRUE to enable it, or FALSE to disable it.
        /// The gradient effect is possible only if the system has a color depth of more than 256 colors. For more information about
        /// the gradient effect, see the GetSysColor function.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETGRADIENTCAPTIONS = 0x1009,

        /// <summary>
        /// Determines whether menu access keys are always underlined. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if menu access keys are always underlined, and FALSE if they are underlined only when the menu is activated by the keyboard.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETKEYBOARDCUES = 0x100A,

        /// <summary>
        /// Sets the underlining of menu access key letters. The pvParam parameter is a BOOL variable. Set pvParam to TRUE to always underline menu
        /// access keys, or FALSE to underline menu access keys only when the menu is activated from the keyboard.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETKEYBOARDCUES = 0x100B,

        /// <summary>
        /// Same as SPI_GETKEYBOARDCUES.
        /// </summary>
        SPI_GETMENUUNDERLINES = SPI_GETKEYBOARDCUES,

        /// <summary>
        /// Same as SPI_SETKEYBOARDCUES.
        /// </summary>
        SPI_SETMENUUNDERLINES = SPI_SETKEYBOARDCUES,

        /// <summary>
        /// Determines whether windows activated through active window tracking will be brought to the top. The pvParam parameter must point
        /// to a BOOL variable that receives TRUE for on, or FALSE for off.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETACTIVEWNDTRKZORDER = 0x100C,

        /// <summary>
        /// Determines whether or not windows activated through active window tracking should be brought to the top. Set pvParam to TRUE
        /// for on or FALSE for off.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETACTIVEWNDTRKZORDER = 0x100D,

        /// <summary>
        /// Determines whether hot tracking of user-interface elements, such as menu names on menu bars, is enabled. The pvParam parameter
        /// must point to a BOOL variable that receives TRUE for enabled, or FALSE for disabled.
        /// Hot tracking means that when the cursor moves over an item, it is highlighted but not selected. You can query this value to decide
        /// whether to use hot tracking in the user interface of your application.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETHOTTRACKING = 0x100E,

        /// <summary>
        /// Enables or disables hot tracking of user-interface elements such as menu names on menu bars. Set the pvParam parameter to TRUE
        /// to enable it, or FALSE to disable it.
        /// Hot-tracking means that when the cursor moves over an item, it is highlighted but not selected.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETHOTTRACKING = 0x100F,

        /// <summary>
        /// Determines whether menu fade animation is enabled. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// when fade animation is enabled and FALSE when it is disabled. If fade animation is disabled, menus use slide animation.
        /// This flag is ignored unless menu animation is enabled, which you can do using the SPI_SETMENUANIMATION flag.
        /// For more information, see AnimateWindow.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETMENUFADE = 0x1012,

        /// <summary>
        /// Enables or disables menu fade animation. Set pvParam to TRUE to enable the menu fade effect or FALSE to disable it.
        /// If fade animation is disabled, menus use slide animation. he The menu fade effect is possible only if the system
        /// has a color depth of more than 256 colors. This flag is ignored unless SPI_MENUANIMATION is also set. For more information,
        /// see AnimateWindow.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETMENUFADE = 0x1013,

        /// <summary>
        /// Determines whether the selection fade effect is enabled. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if enabled or FALSE if disabled.
        /// The selection fade effect causes the menu item selected by the user to remain on the screen briefly while fading out
        /// after the menu is dismissed.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETSELECTIONFADE = 0x1014,

        /// <summary>
        /// Set pvParam to TRUE to enable the selection fade effect or FALSE to disable it.
        /// The selection fade effect causes the menu item selected by the user to remain on the screen briefly while fading out
        /// after the menu is dismissed. The selection fade effect is possible only if the system has a color depth of more than 256 colors.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETSELECTIONFADE = 0x1015,

        /// <summary>
        /// Determines whether ToolTip animation is enabled. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if enabled or FALSE if disabled. If ToolTip animation is enabled, SPI_GETTOOLTIPFADE indicates whether ToolTips use fade or slide animation.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETTOOLTIPANIMATION = 0x1016,

        /// <summary>
        /// Set pvParam to TRUE to enable ToolTip animation or FALSE to disable it. If enabled, you can use SPI_SETTOOLTIPFADE
        /// to specify fade or slide animation.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETTOOLTIPANIMATION = 0x1017,

        /// <summary>
        /// If SPI_SETTOOLTIPANIMATION is enabled, SPI_GETTOOLTIPFADE indicates whether ToolTip animation uses a fade effect or a slide effect.
        ///  The pvParam parameter must point to a BOOL variable that receives TRUE for fade animation or FALSE for slide animation.
        ///  For more information on slide and fade effects, see AnimateWindow.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETTOOLTIPFADE = 0x1018,

        /// <summary>
        /// If the SPI_SETTOOLTIPANIMATION flag is enabled, use SPI_SETTOOLTIPFADE to indicate whether ToolTip animation uses a fade effect
        /// or a slide effect. Set pvParam to TRUE for fade animation or FALSE for slide animation. The tooltip fade effect is possible only
        /// if the system has a color depth of more than 256 colors. For more information on the slide and fade effects,
        /// see the AnimateWindow function.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETTOOLTIPFADE = 0x1019,

        /// <summary>
        /// Determines whether the cursor has a shadow around it. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if the shadow is enabled, FALSE if it is disabled. This effect appears only if the system has a color depth of more than 256 colors.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETCURSORSHADOW = 0x101A,

        /// <summary>
        /// Enables or disables a shadow around the cursor. The pvParam parameter is a BOOL variable. Set pvParam to TRUE to enable the shadow
        /// or FALSE to disable the shadow. This effect appears only if the system has a color depth of more than 256 colors.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETCURSORSHADOW = 0x101B,

        //#if(_WIN32_WINNT >= 0x0501)
        /// <summary>
        /// Retrieves the state of the Mouse Sonar feature. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if enabled or FALSE otherwise. For more information, see About Mouse Input on MSDN.
        /// Windows 2000/NT, Windows 98/95:  This value is not supported.
        /// </summary>
        SPI_GETMOUSESONAR = 0x101C,

        /// <summary>
        /// Turns the Sonar accessibility feature on or off. This feature briefly shows several concentric circles around the mouse pointer
        /// when the user presses and releases the CTRL key. The pvParam parameter specifies TRUE for on and FALSE for off. The default is off.
        /// For more information, see About Mouse Input.
        /// Windows 2000/NT, Windows 98/95:  This value is not supported.
        /// </summary>
        SPI_SETMOUSESONAR = 0x101D,

        /// <summary>
        /// Retrieves the state of the Mouse ClickLock feature. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if enabled, or FALSE otherwise. For more information, see About Mouse Input.
        /// Windows 2000/NT, Windows 98/95:  This value is not supported.
        /// </summary>
        SPI_GETMOUSECLICKLOCK = 0x101E,

        /// <summary>
        /// Turns the Mouse ClickLock accessibility feature on or off. This feature temporarily locks down the primary mouse button
        /// when that button is clicked and held down for the time specified by SPI_SETMOUSECLICKLOCKTIME. The uiParam parameter specifies
        /// TRUE for on,
        /// or FALSE for off. The default is off. For more information, see Remarks and About Mouse Input on MSDN.
        /// Windows 2000/NT, Windows 98/95:  This value is not supported.
        /// </summary>
        SPI_SETMOUSECLICKLOCK = 0x101F,

        /// <summary>
        /// Retrieves the state of the Mouse Vanish feature. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if enabled or FALSE otherwise. For more information, see About Mouse Input on MSDN.
        /// Windows 2000/NT, Windows 98/95:  This value is not supported.
        /// </summary>
        SPI_GETMOUSEVANISH = 0x1020,

        /// <summary>
        /// Turns the Vanish feature on or off. This feature hides the mouse pointer when the user types; the pointer reappears
        /// when the user moves the mouse. The pvParam parameter specifies TRUE for on and FALSE for off. The default is off.
        /// For more information, see About Mouse Input on MSDN.
        /// Windows 2000/NT, Windows 98/95:  This value is not supported.
        /// </summary>
        SPI_SETMOUSEVANISH = 0x1021,

        /// <summary>
        /// Determines whether native User menus have flat menu appearance. The pvParam parameter must point to a BOOL variable
        /// that returns TRUE if the flat menu appearance is set, or FALSE otherwise.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETFLATMENU = 0x1022,

        /// <summary>
        /// Enables or disables flat menu appearance for native User menus. Set pvParam to TRUE to enable flat menu appearance
        /// or FALSE to disable it.
        /// When enabled, the menu bar uses COLOR_MENUBAR for the menubar background, COLOR_MENU for the menu-popup background, COLOR_MENUHILIGHT
        /// for the fill of the current menu selection, and COLOR_HILIGHT for the outline of the current menu selection.
        /// If disabled, menus are drawn using the same metrics and colors as in Windows 2000 and earlier.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETFLATMENU = 0x1023,

        /// <summary>
        /// Determines whether the drop shadow effect is enabled. The pvParam parameter must point to a BOOL variable that returns TRUE
        /// if enabled or FALSE if disabled.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETDROPSHADOW = 0x1024,

        /// <summary>
        /// Enables or disables the drop shadow effect. Set pvParam to TRUE to enable the drop shadow effect or FALSE to disable it.
        /// You must also have CS_DROPSHADOW in the window class style.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETDROPSHADOW = 0x1025,

        /// <summary>
        /// Retrieves a BOOL indicating whether an application can reset the screensaver's timer by calling the SendInput function
        /// to simulate keyboard or mouse input. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if the simulated input will be blocked, or FALSE otherwise.
        /// </summary>
        SPI_GETBLOCKSENDINPUTRESETS = 0x1026,

        /// <summary>
        /// Determines whether an application can reset the screensaver's timer by calling the SendInput function to simulate keyboard
        /// or mouse input. The uiParam parameter specifies TRUE if the screensaver will not be deactivated by simulated input,
        /// or FALSE if the screensaver will be deactivated by simulated input.
        /// </summary>
        SPI_SETBLOCKSENDINPUTRESETS = 0x1027,
        //#endif /* _WIN32_WINNT >= 0x0501 */

        /// <summary>
        /// Determines whether UI effects are enabled or disabled. The pvParam parameter must point to a BOOL variable that receives TRUE
        /// if all UI effects are enabled, or FALSE if they are disabled.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETUIEFFECTS = 0x103E,

        /// <summary>
        /// Enables or disables UI effects. Set the pvParam parameter to TRUE to enable all UI effects or FALSE to disable all UI effects.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETUIEFFECTS = 0x103F,

        /// <summary>
        /// Retrieves the amount of time following user input, in milliseconds, during which the system will not allow applications
        /// to force themselves into the foreground. The pvParam parameter must point to a DWORD variable that receives the time.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000,

        /// <summary>
        /// Sets the amount of time following user input, in milliseconds, during which the system does not allow applications
        /// to force themselves into the foreground. Set pvParam to the new timeout value.
        /// The calling thread must be able to change the foreground window, otherwise the call fails.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001,

        /// <summary>
        /// Retrieves the active window tracking delay, in milliseconds. The pvParam parameter must point to a DWORD variable
        /// that receives the time.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETACTIVEWNDTRKTIMEOUT = 0x2002,

        /// <summary>
        /// Sets the active window tracking delay. Set pvParam to the number of milliseconds to delay before activating the window
        /// under the mouse pointer.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETACTIVEWNDTRKTIMEOUT = 0x2003,

        /// <summary>
        /// Retrieves the number of times SetForegroundWindow will flash the taskbar button when rejecting a foreground switch request.
        /// The pvParam parameter must point to a DWORD variable that receives the value.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_GETFOREGROUNDFLASHCOUNT = 0x2004,

        /// <summary>
        /// Sets the number of times SetForegroundWindow will flash the taskbar button when rejecting a foreground switch request.
        /// Set pvParam to the number of times to flash.
        /// Windows NT, Windows 95:  This value is not supported.
        /// </summary>
        SPI_SETFOREGROUNDFLASHCOUNT = 0x2005,

        /// <summary>
        /// Retrieves the caret width in edit controls, in pixels. The pvParam parameter must point to a DWORD that receives this value.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETCARETWIDTH = 0x2006,

        /// <summary>
        /// Sets the caret width in edit controls. Set pvParam to the desired width, in pixels. The default and minimum value is 1.
        /// Windows NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETCARETWIDTH = 0x2007,

        //#if(_WIN32_WINNT >= 0x0501)
        /// <summary>
        /// Retrieves the time delay before the primary mouse button is locked. The pvParam parameter must point to DWORD that receives
        /// the time delay. This is only enabled if SPI_SETMOUSECLICKLOCK is set to TRUE. For more information, see About Mouse Input on MSDN.
        /// Windows 2000/NT, Windows 98/95:  This value is not supported.
        /// </summary>
        SPI_GETMOUSECLICKLOCKTIME = 0x2008,

        /// <summary>
        /// Turns the Mouse ClickLock accessibility feature on or off. This feature temporarily locks down the primary mouse button
        /// when that button is clicked and held down for the time specified by SPI_SETMOUSECLICKLOCKTIME. The uiParam parameter
        /// specifies TRUE for on, or FALSE for off. The default is off. For more information, see Remarks and About Mouse Input on MSDN.
        /// Windows 2000/NT, Windows 98/95:  This value is not supported.
        /// </summary>
        SPI_SETMOUSECLICKLOCKTIME = 0x2009,

        /// <summary>
        /// Retrieves the type of font smoothing. The pvParam parameter must point to a UINT that receives the information.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETFONTSMOOTHINGTYPE = 0x200A,

        /// <summary>
        /// Sets the font smoothing type. The pvParam parameter points to a UINT that contains either FE_FONTSMOOTHINGSTANDARD,
        /// if standard anti-aliasing is used, or FE_FONTSMOOTHINGCLEARTYPE, if ClearType is used. The default is FE_FONTSMOOTHINGSTANDARD.
        /// When using this option, the fWinIni parameter must be set to SPIF_SENDWININICHANGE | SPIF_UPDATEINIFILE; otherwise,
        /// SystemParametersInfo fails.
        /// </summary>
        SPI_SETFONTSMOOTHINGTYPE = 0x200B,

        /// <summary>
        /// Retrieves a contrast value that is used in ClearType™ smoothing. The pvParam parameter must point to a UINT
        /// that receives the information.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETFONTSMOOTHINGCONTRAST = 0x200C,

        /// <summary>
        /// Sets the contrast value used in ClearType smoothing. The pvParam parameter points to a UINT that holds the contrast value.
        /// Valid contrast values are from 1000 to 2200. The default value is 1400.
        /// When using this option, the fWinIni parameter must be set to SPIF_SENDWININICHANGE | SPIF_UPDATEINIFILE; otherwise,
        /// SystemParametersInfo fails.
        /// SPI_SETFONTSMOOTHINGTYPE must also be set to FE_FONTSMOOTHINGCLEARTYPE.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETFONTSMOOTHINGCONTRAST = 0x200D,

        /// <summary>
        /// Retrieves the width, in pixels, of the left and right edges of the focus rectangle drawn with DrawFocusRect.
        /// The pvParam parameter must point to a UINT.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETFOCUSBORDERWIDTH = 0x200E,

        /// <summary>
        /// Sets the height of the left and right edges of the focus rectangle drawn with DrawFocusRect to the value of the pvParam parameter.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETFOCUSBORDERWIDTH = 0x200F,

        /// <summary>
        /// Retrieves the height, in pixels, of the top and bottom edges of the focus rectangle drawn with DrawFocusRect.
        /// The pvParam parameter must point to a UINT.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_GETFOCUSBORDERHEIGHT = 0x2010,

        /// <summary>
        /// Sets the height of the top and bottom edges of the focus rectangle drawn with DrawFocusRect to the value of the pvParam parameter.
        /// Windows 2000/NT, Windows Me/98/95:  This value is not supported.
        /// </summary>
        SPI_SETFOCUSBORDERHEIGHT = 0x2011,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SPI_GETFONTSMOOTHINGORIENTATION = 0x2012,

        /// <summary>
        /// Not implemented.
        /// </summary>
        SPI_SETFONTSMOOTHINGORIENTATION = 0x2013,
    }
    #endregion // SPI

    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:列挙値を重複させることはできない")]
    public enum SPIF
    {
        None = 0x00,
        /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
        SPIF_UPDATEINIFILE = 0x01,
        /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
        SPIF_SENDCHANGE = 0x02,
        /// <summary>Same as SPIF_SENDCHANGE.</summary>
        SPIF_SENDWININICHANGE = 0x02
    }

    [Flags]
    public enum AW
    {
        AW_HOR_POSITIVE = 0x00000001,
        AW_HOR_NEGATIVE = 0x00000002,
        AW_VER_POSITIVE = 0x00000004,
        AW_VER_NEGATIVE = 0x00000008,
        AW_CENTER = 0x00000010,
        AW_HIDE = 0x00010000,
        AW_ACTIVATE = 0x00020000,
        AW_SLIDE = 0x00040000,
        AW_BLEND = 0x00080000
    }

    [Flags]
    public enum DI
    {
        /// <summary>
        /// ユーザーが指定したイメージではなく、システムの既定のイメージを使って、アイコンまたはカーソルを描画します。
        /// </summary>
        DI_COMPAT = 4,
        /// <summary>
        /// cxWidth と cyWidth の各パラメータで 0 が指定されている場合、アイコンまたはカーソル用のシステムメトリックの値で指定された幅と高さを使って、アイコンまたはカーソルをで描画します。cxWidth と cyWidth の各パラメータで 0 を指定し、このフラグを指定しなかった場合、この関数はリソースの実際のサイズを使います。
        /// </summary>
        DI_DEFAULTSIZE = 8,
        /// <summary>
        /// イメージを使ってアイコンまたはカーソルを描画します。
        /// </summary>
        DI_IMAGE = 2,
        /// <summary>
        /// マスクを使ってアイコンまたはカーソルを描画します。
        /// </summary>
        DI_MASK = 1,
        /// <summary>
        /// DI_IMAGE と DI_MASK の組み合わせです。
        /// </summary>
        DI_NORMAL = 3,
    }

    public enum GW: uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6
    }

    [Flags]
    public enum KEYEVENTF: uint
    {
        KEYEVENTF_KEYDOWN = 0x0000,
        KEYEVENTF_EXTENDEDKEY = 0x0001,
        KEYEVENTF_KEYUP = 0x0002,
    }

    public enum GWL
    {
        GWL_WNDPROC = -4,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20,
        GWL_USERDATA = -21,
        GWL_ID = -12
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:列挙値を重複させることはできない")]
    public enum VK: int
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_CANCEL = 0x03,
        VK_MBUTTON = 0x04,
        VK_BACK = 0x08,
        VK_TAB = 0x09,
        VK_CLEAR = 0x0C,
        VK_RETURN = 0x0D,
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_PAUSE = 0x13,
        VK_CAPITAL = 0x14,
        VK_KANA = 0x15,
        VK_HANGEUL = 0x15,
        VK_HANGUL = 0x15,
        VL_JUNJA = 0x17,
        VK_FINAL = 0x18,
        VK_HANJA = 0x19,
        VK_KANJI = 0x19,
        VK_ESCAPE = 0x1B,
        VK_CONVERT = 0x1C,
        VK_NONCONVERT = 0x1D,
        VK_ACCEPT = 0x1E,
        VK_MODECHANGE = 0x1F,
        VK_SPACE = 0x20,
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_SELECT = 0x29,
        VK_PRINT = 0x2A,
        VK_EXECUTE = 0x2B,
        VK_SNAPSHOT = 0x2C,
        VK_INSERT = 0x2D,
        VK_DELETE = 0x2E,
        VK_HELP = 0x2F,
        VK_LWIN = 0x5B,
        VK_RWIN = 0x5C,
        VK_APPS = 0x5D,
        VK_NUMPAD0 = 0x60,
        VK_NUMPAD1 = 0x61,
        VK_NUMPAD2 = 0x62,
        VK_NUMPAD3 = 0x63,
        VK_NUMPAD4 = 0x64,
        VK_NUMPAD5 = 0x65,
        VK_NUMPAD6 = 0x66,
        VK_NUMPAD7 = 0x67,
        VK_NUMPAD8 = 0x68,
        VK_NUMPAD9 = 0x69,
        VK_MULTIPLY = 0x6A,
        VK_ADD = 0x6B,
        VK_SEPARATOR = 0x6C,
        VK_SUBTRACT = 0x6D,
        VK_DECIMAL = 0x6E,
        VK_DIVIDE = 0x6F,
        VK_F1 = 0x70,
        VK_F2 = 0x71,
        VK_F3 = 0x72,
        VK_F4 = 0x73,
        VK_F5 = 0x74,
        VK_F6 = 0x75,
        VK_F7 = 0x76,
        VK_F8 = 0x77,
        VK_F9 = 0x78,
        VK_F10 = 0x79,
        VK_F11 = 0x7A,
        VK_F12 = 0x7B,
        VK_F13 = 0x7C,
        VK_F14 = 0x7D,
        VK_F15 = 0x7E,
        VK_F16 = 0x7F,
        VK_F17 = 0x80,
        VK_F18 = 0x81,
        VK_F19 = 0x82,
        VK_F20 = 0x83,
        VK_F21 = 0x84,
        VK_F22 = 0x85,
        VK_F23 = 0x86,
        VK_F24 = 0x87,
        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,
        VK_LSHIFT = 0xA0,
        VK_RSHIFT = 0xA1,
        VK_LCONTROL = 0xA2,
        VK_RCONTROL = 0xA3,
        VK_LMENU = 0xA4,
        VK_RMENU = 0xA5,
        VK_PROCESSKEY = 0xE5,
        VK_ATTN = 0xF6,
        VK_CRSEL = 0xF7,
        VK_EXSEL = 0xF8,
        VK_EREOF = 0xF9,
        VK_PLAY = 0xFA,
        VK_ZOOM = 0xFB,
        VK_NONAME = 0xFC,
        VK_PA1 = 0xFD,
        VK_OME_CLEAR = 0xFE,
    }

    public enum MK
    {
        /// <summary>
        /// The CTRL key is down.
        /// </summary>
        MK_CONTROL = 0x0008,
        /// <summary>
        /// The left mouse button is down.
        /// </summary>
        MK_LBUTTON = 0x0001,
        /// <summary>
        /// The middle mouse button is down.
        /// </summary>
        MK_MBUTTON = 0x0010,
        /// <summary>
        /// The right mouse button is down.
        /// </summary>
        MK_RBUTTON = 0x0002,
        /// <summary>
        /// The SHIFT key is down.
        /// </summary>
        MK_SHIFT = 0x0004,
        /// <summary>
        /// The first X button is down.
        /// </summary>
        MK_XBUTTON1 = 0x0020,
        /// <summary>
        /// The second X button is down.
        /// </summary>
        MK_XBUTTON2 = 0x0040,
    }


    public enum XBUTTON
    {
        XBUTTON1 = 0x0001,
        XBUTTON2 = 0x0002
    }

    public enum WH
    {
        WH_CALLWNDPROC = 4,
        WH_CALLWNDPROCRET = 12,
        WH_CBT = 5,
        WH_DEBUG = 9,
        WH_FOREGROUNDIDLE = 11,
        WH_GETMESSAGE = 3,
        WH_JOURNALPLAYBACK = 1,
        WH_JOURNALRECORD = 0,
        WH_KEYBOARD = 2,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE = 7,
        WH_MOUSE_LL = 14,
        WH_MSGFILTER = -1,
        WH_SHELL = 10,
        WH_SYSMSGFILTER = 6,
    }

    /// <summary>
    /// http://pinvoke.net/default.aspx/Structures.WINDOWPOS
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr hwnd;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public SWP flags;
    }

    public enum IDC: int
    {
        IDC_ARROW = 32512,
        IDC_IBEAM = 32513,
        IDC_WAIT = 32514,
        IDC_CROSS = 32515,
        IDC_UPARROW = 32516,
        IDC_SIZE = 32640,
        IDC_ICON = 32641,
        IDC_SIZENWSE = 32642,
        IDC_SIZENESW = 32643,
        IDC_SIZEWE = 32644,
        IDC_SIZENS = 32645,
        IDC_SIZEALL = 32646,
        IDC_NO = 32648,
        IDC_HAND = 32649,
        IDC_APPSTARTING = 32650,
        IDC_HELP = 32651,
    }

    public enum WMSZ
    {
        WMSZ_LEFT = 1,
        WMSZ_RIGHT = 2,
        WMSZ_TOP = 3,
        WMSZ_TOPLEFT = 4,
        WMSZ_TOPRIGHT = 5,
        WMSZ_BOTTOM = 6,
        WMSZ_BOTTOMLEFT = 7,
        WMSZ_BOTTOMRIGHT = 8,
    }

    public enum MONITOR: uint
    {
        MONITOR_DEFAULTTONULL = 0x00000000,
        MONITOR_DEFAULTTOPRIMARY = 0x00000001,
        MONITOR_DEFAULTTONEAREST = 0x00000002
    }


    public enum CWP: uint
    {
        CWP_ALL = 0x0000,
        CWP_SKIPINVISIBLE = 0x0001,
        CWP_SKIPDISABLED = 0x0002,
        CWP_SKIPTRANSPARENT = 0x0004
    }

    public enum GA: uint
    {
        /// <summary>
        /// 親ウィンドウを取得します。これには、GetParent 関数で取得されるような、オーナーウィンドウは含みません。
        /// </summary>
        GA_PARENT = 1,
        /// <summary>
        /// 親ウィンドウのチェーンをたどってルートウィンドウを取得します。
        /// </summary>
        GA_ROOT = 2,
        /// <summary>
        /// <see cref="NativeMethods.GetParent "/> 関数が返す親ウィンドウのチェーンをたどって所有されているルートウィンドウを取得します。
        /// </summary>
        GA_ROOTOWNER = 3
    }

    public enum OBJID: uint
    {
        OBJID_HSCROLL = 0xFFFFFFFA,
        OBJID_VSCROLL = 0xFFFFFFFB,
        OBJID_CLIENT = 0xFFFFFFFC,

    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct SCROLLINFO
    {
        public uint cbSize;
        public uint fMask;
        public int nMin;
        public int nMax;
        public uint nPage;
        public int nPos;
        public int nTrackPos;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SCROLLBARINFO
    {
        public int cbSize;
        public RECT rcScrollBar;
        public int dxyLineButton;
        public int xyThumbTop;
        public int xyThumbBottom;
        public int reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public int[] rgstate;
    }
    [Flags]
    public enum SIF
    {
        /// <summary>
        /// スクロールページを、lpsi パラメータが指す 構造体の nPage メンバに格納します。
        /// </summary>
        SIF_PAGE = 0x2,
        /// <summary>
        /// スクロール位置を、lpsi パラメータが指す SCROLLINFO 構造体の nPos メンバに格納します。
        /// </summary>
        SIF_POS = 0x4,
        /// <summary>
        /// スクロール範囲を、lpsi パラメータが指す SCROLLINFO 構造体の nMin と nMax の各メンバに格納します。
        /// </summary>
        SIF_RANGE = 0x1,
        /// <summary>
        /// スクロールボックス（つまみ）の現在の位置を、lpsi パラメータが指す SCROLLINFO 構造体の nTrackPos メンバに格納します。
        /// </summary>
        SIF_TRACKPOS = 0x10,
        SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS
    }

    [Flags]
    public enum SW_scroll: uint
    {
        /// <summary>
        /// SW_INVALIDATE フラグと共にこのフラグを指定すると、スクロール後、WM_ERASEBKGND メッセージをウィンドウへ送信し、新たに無効になったリージョンを消去します。
        /// </summary>
        SW_ERASE = 0x0004,
        /// <summary>
        /// スクロール後、hrgnUpdate パラメータが識別しているリージョンを無効にします。
        /// </summary>
        SW_INVALIDATE = 0x0002,
        /// <summary>
        /// prcScroll パラメータが指す長方形と重なり合う、すべての子ウィンドウをスクロールします。dx と dy の各パラメータで指定したピクセル数だけ、子ウィンドウをスクロールします。システムは、prcScroll パラメータが指す長方形と重なる子ウィンドウへ、WM_MOVE メッセージを送信します。それらのウィンドウが移動の対象にならない場合でも、送信の対象になります。
        /// </summary>
        SW_SCROLLCHILDREN = 0x0001,
        /// <summary>
        /// Windows 98 と Windows 2000：スムーズスクロールを行います。flags パラメータの HIWORD 部分で、スムーズスクロール操作を行う回数を指定します。
        /// </summary>
        SW_SMOOTHSCROLL = 0x0010,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct FLASHWINFO
    {
        public UInt32 cbSize;
        public IntPtr hwnd;
        public FLASHW dwFlags;
        public UInt32 uCount;
        public UInt32 dwTimeout;
    }
    public enum FLASHW: uint
    {
        /// <summary>
        /// Stop flashing. The system restores the window to its original state.
        /// </summary>
        FLASHW_STOP = 0,

        /// <summary>
        /// Flash the window caption
        /// </summary>
        FLASHW_CAPTION = 1,

        /// <summary>
        /// Flash the taskbar button.
        /// </summary>
        FLASHW_TRAY = 2,

        /// <summary>
        /// Flash both the window caption and taskbar button.
        /// This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
        /// </summary>
        FLASHW_ALL = 3,

        /// <summary>
        /// Flash continuously, until the FLASHW_STOP flag is set.
        /// </summary>
        FLASHW_TIMER = 4,

        /// <summary>
        /// Flash continuously until the window comes to the foreground.
        /// </summary>
        FLASHW_TIMERNOFG = 12
    }

    [StructLayout(LayoutKind.Sequential)]
    public class KBDLLHOOKSTRUCT
    {
        public uint vkCode;
        public uint scanCode;
        public LLKHF flags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [Flags]
    public enum LLKHF: uint
    {
        LLKHF_EXTENDED = 0x01,
        LLKHF_INJECTED = 0x10,
        LLKHF_ALTDOWN = 0x20,
        LLKHF_UP = 0x80,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSLLHOOKSTRUCT
    {
        public POINT pt;
        public int mouseData; // be careful, this must be ints, not uints (was wrong before I changed it...). regards, cmew.
        public int flags;
        public int time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public KEYEVENTF dwFlags;
        public int time;
        public UIntPtr dwExtraInfo;
    }

    [Flags]
    public enum MOUSEEVENTF: uint
    {
        MOUSEEVENTF_ABSOLUTE = 0x8000,
        MOUSEEVENTF_HWHEEL = 0x01000,
        MOUSEEVENTF_MOVE = 0x0001,
        MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,
        MOUSEEVENTF_LEFTDOWN = 0x0002,
        MOUSEEVENTF_LEFTUP = 0x0004,
        MOUSEEVENTF_RIGHTDOWN = 0x0008,
        MOUSEEVENTF_RIGHTUP = 0x0010,
        MOUSEEVENTF_MIDDLEDOWN = 0x0020,
        MOUSEEVENTF_MIDDLEUP = 0x0040,
        MOUSEEVENTF_VIRTUALDESK = 0x4000,
        MOUSEEVENTF_WHEEL = 0x0800,
        MOUSEEVENTF_XDOWN = 0x0080,
        MOUSEEVENTF_XUP = 0x0100
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public MOUSEEVENTF dwFlags;
        public uint time;
        public UIntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public int uMsg;
        public short wParamL;
        public short wParamH;
    }

    public enum INPUT_type: int
    {
        INPUT_MOUSE = 0,
        INPUT_KEYBOARD = 1,
        INPUT_HARDWARE = 2,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public INPUT_type type;
        public INPUT_data data;
    };


    [StructLayout(LayoutKind.Explicit)]
    public struct INPUT_data
    {
        [FieldOffset(0)] public MOUSEINPUT mi;
        [FieldOffset(0)] public KEYBDINPUT ki;
        [FieldOffset(0)] public HARDWAREINPUT hi;
    };

    public enum HC
    {
        HC_ACTION = 0,
        HC_NOREMOVE = 3,
    }

    /// <summary>
    /// https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mouse_event
    /// http://tokovalue.jp/function/MapVirtualKey.htm
    /// </summary>
    public enum MAPVK: uint
    {
        /// <summary>
        /// uCode は仮想キーコードであり、スキャンコードへ変換される。左右のキーを区別しない仮想キーコードのときは、関数は左側のスキャンコードを返す。スキャンコードに変換されないときは、関数は 0 を返す。
        /// </summary>
        MAPVK_VK_TO_VSC = 0,
        /// <summary>
        /// uCode はスキャンコードであり、仮想キーコードへ変換される。この仮想キーコードは、左右のキーを区別する。変換されないときは、関数は 0 を返する。
        /// </summary>
        MAPVK_VSC_TO_VK = 1,
        /// <summary>
        /// uCode は仮想キーコードであり、戻り値の下位ワードにシフトなしの ASCII 値が格納される。デッドキー（ 分音符号）は、戻り値の上位ビットをセットすることにより明示される。変換されないときは、関数は 0 を返す。
        /// </summary>
        MAPVK_VK_TO_CHAR = 2,
        /// <summary>
        /// Windows NT/2000：uCode はスキャンコードであり、左右のキーを区別する仮想キーコードへ変換される。変換されないときは、関数は 0 を返す。
        /// </summary>
        MAPVK_VSC_TO_VK_EX = 3,
    }
    partial class NativeMethods
    {
        /// <summary>
        /// http://www.pinvoke.net/default.aspx/user32.sendmessage
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, WM Msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool PostMessage(IntPtr hWnd, WM Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool keybd_event(byte bVk, byte bScan, KEYEVENTF dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool keybd_event(byte bVk, byte bScan, KEYEVENTF dwFlags, int dwExtraInfo);

        /// <summary>
        /// http://www.pinvoke.net/default.aspx/user32.registerwindowmessage
        /// </summary>
        /// <param name="lpString"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        /// <summary>
        /// http://pinvoke.net/default.aspx/user32/DestroyIcon.html
        /// </summary>
        /// <param name="hIcon"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam, SMTO fuFlags, uint uTimeout, out UIntPtr lpdwResult);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetSystemMetrics(SM smIndex);

        //		[DllImport("user32.dll", SetLastError = true)]
        //		[return: MarshalAs(UnmanagedType.Bool)]
        //		public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref T pvParam, SPIF fWinIni); // T = any type

        [DllImport("user32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref int pvParam, SPIF fWinIni);

        // For setting a string parameter
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, SPIF fWinIni);

        // For reading a string parameter
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, StringBuilder pvParam, SPIF fWinIni);

        //		[DllImport("user32.dll", SetLastError = true)]
        //		[return: MarshalAs(UnmanagedType.Bool)]
        //		public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref ANIMATIONINFO pvParam, SPIF fWinIni);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SWP uFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32")]
        public static extern bool AnimateWindow(IntPtr hwnd, int time, AW flags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyHeight, int istepIfAniCur, IntPtr hbrFlickerFreeDraw, DI diFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string lpFileName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr hCursor);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern bool SetSystemCursor(IntPtr hcur, uint id);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr LoadCursor(IntPtr hInstance, string lpCursorName);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IDC lpCursorName);

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        //[DllImport("user32.dll")]
        //public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenIcon(IntPtr hWnd);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, SW nCmdShow);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern uint GetDoubleClickTime();

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern uint GetClipboardSequenceNumber();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool AddClipboardFormatListener(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern uint EnumClipboardFormats(uint format);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern int CountClipboardFormats();

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern int GetClipboardFormatName(uint format, [Out] StringBuilder lpszFormatName, int cchMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr GetWindow(IntPtr hWnd, GW uCommand);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        internal static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr GetWindowLong64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        internal static extern int SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        internal static extern IntPtr SetWindowLong64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", SetLastError = true)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
        public static extern IntPtr MonitorFromPoint(POINT pt, MONITOR dwFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT p);

        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPointEx(IntPtr hWndParent, POINT pt, CWP uFlags);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);


        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GA flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetScrollPos(IntPtr hWnd, SB nBar);
        [DllImport("user32.dll")]
        public static extern int SetScrollPos(IntPtr hWnd, SB nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        public static extern bool GetScrollRange(IntPtr hWnd, SB nBar, out int lpMinPos, out int lpMaxPos);
        [DllImport("user32.dll")]
        public static extern bool SetScrollRange(IntPtr hWnd, SB nBar, int nMinPos, int nMaxPos, bool bRedraw);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetScrollInfo(IntPtr hwnd, SB fnBar, ref SCROLLINFO lpsi);
        [DllImport("user32.dll")]
        public static extern int SetScrollInfo(IntPtr hwnd, SB fnBar, [In] ref SCROLLINFO lpsi, bool fRedraw);


        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetScrollBarInfo")]
        public static extern int GetScrollBarInfo(IntPtr hWnd, OBJID idObject, ref SCROLLBARINFO psbi);


        [DllImport("user32.dll")]
        public static extern int ScrollWindowEx(IntPtr hWnd, int dx, int dy, [In] ref RECT prcScroll, [In] ref RECT prcClip, IntPtr hrgnUpdate, ref RECT prcUpdate, SW_scroll flags);


        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwndParent, NativeMethods.EnumWindowsProc lpEnumFunc, IntPtr lParam);


        // TODO: GetDpiForMonitor, windows7が死滅するか動的にとるか後で考える
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(WH hookType, HookProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);
        // <summary>
        ///     Passes the hook information to the next hook procedure in the current hook chain. A hook procedure can call this
        ///     function either before or after processing the hook information.
        ///     <para>
        ///     See [ https://msdn.microsoft.com/en-us/library/windows/desktop/ms644974%28v=vs.85%29.aspx ] for more
        ///     information.
        ///     </para>
        /// </summary>
        /// <param name="hhk">C++ ( hhk [in, optional]. Type: HHOOK )<br />This parameter is ignored. </param>
        /// <param name="nCode">
        ///     C++ ( nCode [in]. Type: int )<br />The hook code passed to the current hook procedure. The next
        ///     hook procedure uses this code to determine how to process the hook information.
        /// </param>
        /// <param name="wParam">
        ///     C++ ( wParam [in]. Type: WPARAM )<br />The wParam value passed to the current hook procedure. The
        ///     meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <param name="lParam">
        ///     C++ ( lParam [in]. Type: LPARAM )<br />The lParam value passed to the current hook procedure. The
        ///     meaning of this parameter depends on the type of hook associated with the current hook chain.
        /// </param>
        /// <returns>
        ///     C++ ( Type: LRESULT )<br />This value is returned by the next hook procedure in the chain. The current hook
        ///     procedure must also return this value. The meaning of the return value depends on the hook type. For more
        ///     information, see the descriptions of the individual hook procedures.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///     Hook procedures are installed in chains for particular hook types. <see cref="CallNextHookEx" /> calls the
        ///     next hook in the chain.
        ///     </para>
        ///     <para>
        ///     Calling CallNextHookEx is optional, but it is highly recommended; otherwise, other applications that have
        ///     installed hooks will not receive hook notifications and may behave incorrectly as a result. You should call
        ///     <see cref="CallNextHookEx" /> unless you absolutely need to prevent the notification from being seen by other
        ///     applications.
        ///     </para>
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        // overload for use with LowLevelKeyboardProc
        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, WM wParam, [In] KBDLLHOOKSTRUCT lParam);

        // overload for use with LowLevelMouseProc
        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, WM wParam, [In] MSLLHOOKSTRUCT lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MAPVK uMapType);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();
    }



}
