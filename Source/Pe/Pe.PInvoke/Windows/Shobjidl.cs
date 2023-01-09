using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ContentTypeTextNet.Pe.PInvoke.Windows
{
    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1712:列挙値の前に型名を付けないでください", Justification = "WindowsAPI")]
    public enum SIIGBF
    {
        SIIGBF_RESIZETOFIT = 0x00,
        SIIGBF_BIGGERSIZEOK = 0x01,
        SIIGBF_MEMORYONLY = 0x02,
        SIIGBF_ICONONLY = 0x04,
        SIIGBF_THUMBNAILONLY = 0x08,
        SIIGBF_INCACHEONLY = 0x10,
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1712:列挙値の前に型名を付けないでください", Justification = "WindowsAPI")]
    public enum SIGDN: uint
    {
        SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
        SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
        SIGDN_FILESYSPATH = 0x80058000,
        SIGDN_NORMALDISPLAY = 0,
        SIGDN_PARENTRELATIVE = 0x80080001,
        SIGDN_PARENTRELATIVEEDITING = 0x80031001,
        SIGDN_PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
        SIGDN_PARENTRELATIVEPARSING = 0x80018001,
        SIGDN_URL = 0x80068000,
    }

    /// <summary>
    /// IShellLink.GetPath fFlags: Flags that specify the type of path information to retrieve
    ///
    /// http://pinvoke.net/default.aspx/Enums/SLGP_FLAGS.html
    /// </summary>
    [Flags]
    public enum SLGP_FLAGS
    {
        /// <summary>Retrieves the standard short (8.3 format) file name</summary>
        SLGP_SHORTPATH = 0x1,
        /// <summary>Retrieves the Universal Naming Convention (UNC) path name of the file</summary>
        SLGP_UNCPRIORITY = 0x2,
        /// <summary>Retrieves the raw path name. A raw path is something that might not exist and may include environment variables that need to be expanded</summary>
        SLGP_RAWPATH = 0x4
    }

    /// <summary>
    /// IShellLink.Resolve fFlags
    ///
    /// http://www.pinvoke.net/default.aspx/Enums/SLR_FLAGS.html
    /// </summary>
    [Flags]
    public enum SLR_FLAGS
    {
        /// <summary>
        /// Do not display a dialog box if the link cannot be resolved. When SLR_NO_UI is set,
        /// the high-order word of fFlags can be set to a time-out value that specifies the
        /// maximum amount of time to be spent resolving the link. The function returns if the
        /// link cannot be resolved within the time-out duration. If the high-order word is set
        /// to zero, the time-out duration will be set to the default value of 3,000 milliseconds
        /// (3 seconds). To specify a value, set the high word of fFlags to the desired time-out
        /// duration, in milliseconds.
        /// </summary>
        SLR_NO_UI = 0x1,
        /// <summary>Obsolete and no longer used</summary>
        SLR_ANY_MATCH = 0x2,
        /// <summary>If the link object has changed, update its path and list of identifiers.
        /// If SLR_UPDATE is set, you do not need to call IPersistFile::IsDirty to determine
        /// whether or not the link object has changed.</summary>
        SLR_UPDATE = 0x4,
        /// <summary>Do not update the link information</summary>
        SLR_NOUPDATE = 0x8,
        /// <summary>Do not execute the search heuristics</summary>
        SLR_NOSEARCH = 0x10,
        /// <summary>Do not use distributed link tracking</summary>
        SLR_NOTRACK = 0x20,
        /// <summary>Disable distributed link tracking. By default, distributed link tracking tracks
        /// removable media across multiple devices based on the volume name. It also uses the
        /// Universal Naming Convention (UNC) path to track remote file systems whose drive letter
        /// has changed. Setting SLR_NOLINKINFO disables both types of tracking.</summary>
        SLR_NOLINKINFO = 0x40,
        /// <summary>Call the Microsoft Windows Installer</summary>
        SLR_INVOKE_MSI = 0x80
    }

    public enum THUMBBUTTONFLAGS
    {

        THBF_ENABLED = 0,
        THBF_DISABLED = 0x1,
        THBF_DISMISSONCLICK = 0x2,
        THBF_NOBACKGROUND = 0x4,
        THBF_HIDDEN = 0x8,
        THBF_NONINTERACTIVE = 0x10
    }

    public enum TBPFLAG
    {
        TBPF_NOPROGRESS = 0,
        TBPF_INDETERMINATE = 0x1,
        TBPF_NORMAL = 0x2,
        TBPF_ERROR = 0x4,
        TBPF_PAUSED = 0x8
    }


    public enum THUMBBUTTONMASK
    {
        THB_BITMAP = 0x1,
        THB_ICON = 0x2,
        THB_TOOLTIP = 0x4,
        THB_FLAGS = 0x8
    }

    [StructLayout(LayoutKind.Sequential, Pack = 8, CharSet = CharSet.Unicode)]
    public struct THUMBBUTTON
    {
        public THUMBBUTTONMASK dwMask;
        public uint iId;
        public uint iBitmap;
        public IntPtr hIcon;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 259)]
        public string szTip;

        public THUMBBUTTONFLAGS dwFlags;
    }


    [ComImportAttribute]
    [Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItemImageFactory
    {
        void GetImage(
            [In, MarshalAs(UnmanagedType.Struct)] SIZE size,
            [In] SIIGBF flags,
            [Out] out IntPtr phbm
        );
    }

    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    public interface IShellItem
    {
        void BindToHandler(IntPtr pbc,
            [MarshalAs(UnmanagedType.LPStruct)] Guid bhid,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            out IntPtr ppv
        );

        void GetParent(out IShellItem ppsi);

        void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);

        void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);

        void Compare(IShellItem psi, uint hint, out int piOrder);
    };

    /// <summary>
    /// The IShellLink interface allows Shell links to be created, modified, and resolved
    ///
    /// http://www.pinvoke.net/default.aspx/Interfaces/IShellLinkW.html
    /// </summary>
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214F9-0000-0000-C000-000000000046")]
    public interface IShellLink
    {
        /// <summary>Retrieves the path and file name of a Shell link object</summary>
        void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out WIN32_FIND_DATA pfd, SLGP_FLAGS fFlags);
        /// <summary>Retrieves the list of item identifiers for a Shell link object</summary>
        void GetIDList(out IntPtr ppidl);
        /// <summary>Sets the pointer to an item identifier list (PIDL) for a Shell link object.</summary>
        void SetIDList(IntPtr pidl);
        /// <summary>Retrieves the description string for a Shell link object</summary>
        void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
        /// <summary>Sets the description for a Shell link object. The description can be any application-defined string</summary>
        void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        /// <summary>Retrieves the name of the working directory for a Shell link object</summary>
        void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
        /// <summary>Sets the name of the working directory for a Shell link object</summary>
        void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
        /// <summary>Retrieves the command-line arguments associated with a Shell link object</summary>
        void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
        /// <summary>Sets the command-line arguments for a Shell link object</summary>
        void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
        /// <summary>Retrieves the hot key for a Shell link object</summary>
        void GetHotkey(out short pwHotkey);
        /// <summary>Sets a hot key for a Shell link object</summary>
        void SetHotkey(short wHotkey);
        /// <summary>Retrieves the show command for a Shell link object</summary>
        void GetShowCmd(out int piShowCmd);
        /// <summary>Sets the show command for a Shell link object. The show command sets the initial show state of the window.</summary>
        void SetShowCmd(int iShowCmd);
        /// <summary>Retrieves the location (path and index) of the icon for a Shell link object</summary>
        void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath,
            int cchIconPath, out int piIcon);
        /// <summary>Sets the location (path and index) of the icon for a Shell link object</summary>
        void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
        /// <summary>Sets the relative path to the Shell link object</summary>
        void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
        /// <summary>Attempts to find the target of a Shell link, even if it has been moved or renamed</summary>
        void Resolve(IntPtr hwnd, SLR_FLAGS fFlags);
        /// <summary>Sets the path and file name of a Shell link object</summary>
        void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }

    /// <summary>
    /// Extends ITaskbarList2 by exposing methods that support the unified launching and switching taskbar button
    /// functionality added in Windows 7. This functionality includes thumbnail representations and switch
    /// targets based on individual tabs in a tabbed application, thumbnail toolbars, notification and
    /// status overlays, and progress indicators.
    ///
    /// http://www.pinvoke.net/default.aspx/shell32/ITaskbarList3.html
    /// </summary>
    [ComImport,
    Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf"),
    InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface ITaskbarList3
    {
        // ITaskbarList

        /// <summary>
        /// Initializes the taskbar list object. This method must be called before any other ITaskbarList methods can be called.
        /// </summary>
        void HrInit();

        /// <summary>
        /// Adds an item to the taskbar.
        /// </summary>
        /// <param name="hWnd">A handle to the window to be added to the taskbar.</param>
        void AddTab(IntPtr hWnd);

        /// <summary>
        /// Deletes an item from the taskbar.
        /// </summary>
        /// <param name="hWnd">A handle to the window to be deleted from the taskbar.</param>
        void DeleteTab(IntPtr hWnd);

        /// <summary>
        /// Activates an item on the taskbar. The window is not actually activated; the window's item on the taskbar is merely displayed as active.
        /// </summary>
        /// <param name="hWnd">A handle to the window on the taskbar to be displayed as active.</param>
        void ActivateTab(IntPtr hWnd);

        /// <summary>
        /// Marks a taskbar item as active but does not visually activate it.
        /// </summary>
        /// <param name="hWnd">A handle to the window to be marked as active.</param>
        void SetActiveAlt(IntPtr hWnd);

        // ITaskbarList2

        /// <summary>
        /// Marks a window as full-screen
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="fFullscreen"></param>
        void MarkFullscreenWindow(IntPtr hWnd, int fFullscreen);

        /// <summary>
        /// Displays or updates a progress bar hosted in a taskbar button to show the specific percentage
        /// completed of the full operation.
        /// </summary>
        /// <param name="hWnd">The handle of the window whose associated taskbar button is being used as
        /// a progress indicator.</param>
        /// <param name="ullCompleted">An application-defined value that indicates the proportion of the
        /// operation that has been completed at the time the method is called.</param>
        /// <param name="ullTotal">An application-defined value that specifies the value ullCompleted will
        /// have when the operation is complete.</param>
        void SetProgressValue(IntPtr hWnd, ulong ullCompleted, ulong ullTotal);

        /// <summary>
        /// Sets the type and state of the progress indicator displayed on a taskbar button.
        /// </summary>
        /// <param name="hWnd">The handle of the window in which the progress of an operation is being
        /// shown. This window's associated taskbar button will display the progress bar.</param>
        /// <param name="tbpFlags">Flags that control the current state of the progress button. Specify
        /// only one of the following flags; all states are mutually exclusive of all others.</param>
        void SetProgressState(IntPtr hWnd, TBPFLAG tbpFlags);

        /// <summary>
        /// Informs the taskbar that a new tab or document thumbnail has been provided for display in an
        /// application's taskbar group flyout.
        /// </summary>
        /// <param name="hWndTab">Handle of the tab or document window. This value is required and cannot
        /// be NULL.</param>
        /// <param name="hWndMDI">Handle of the application's main window. This value tells the taskbar
        /// which application's preview group to attach the new thumbnail to. This value is required and
        /// cannot be NULL.</param>
        void RegisterTab(IntPtr hWndTab, IntPtr hWndMDI);

        /// <summary>
        /// Removes a thumbnail from an application's preview group when that tab or document is closed in the application.
        /// </summary>
        /// <param name="hWndTab">The handle of the tab window whose thumbnail is being removed. This is the same
        /// value with which the thumbnail was registered as part the group through ITaskbarList3::RegisterTab.
        /// This value is required and cannot be NULL.</param>
        void UnregisterTab(IntPtr hWndTab);

        /// <summary>
        /// Inserts a new thumbnail into a tabbed-document interface (TDI) or multiple-document interface
        /// (MDI) application's group flyout or moves an existing thumbnail to a new position in the
        /// application's group.
        /// </summary>
        /// <param name="hWndTab">The handle of the tab window whose thumbnail is being placed. This value
        /// is required, must already be registered through ITaskbarList3::RegisterTab, and cannot be NULL.</param>
        /// <param name="hWndInsertBefore">The handle of the tab window whose thumbnail that hwndTab is
        /// inserted to the left of. This handle must already be registered through ITaskbarList3::RegisterTab.
        /// If this value is NULL, the new thumbnail is added to the end of the list.</param>
        void SetTabOrder(IntPtr hWndTab, IntPtr hWndInsertBefore);

        /// <summary>
        /// Informs the taskbar that a tab or document window has been made the active window.
        /// </summary>
        /// <param name="hWndTab">Handle of the active tab window. This handle must already be registered
        /// through ITaskbarList3::RegisterTab. This value can be NULL if no tab is active.</param>
        /// <param name="hWndMDI">Handle of the application's main window. This value tells the taskbar
        /// which group the thumbnail is a member of. This value is required and cannot be NULL.</param>
        /// <param name="tbatFlags">None, one, or both of the following values that specify a thumbnail
        /// and peek view to use in place of a representation of the specific tab or document.</param>
        void SetTabActive(IntPtr hWndTab, IntPtr hWndMDI, UInt32 tbatFlags);

        /// <summary>
        /// Adds a thumbnail toolbar with a specified set of buttons to the thumbnail image of a window in a
        /// taskbar button flyout.
        /// </summary>
        /// <param name="hWnd">The handle of the window whose thumbnail representation will receive the toolbar.
        /// This handle must belong to the calling process.</param>
        /// <param name="cButtons">The number of buttons defined in the array pointed to by pButton. The maximum
        /// number of buttons allowed is 7.</param>
        /// <param name="pButton">A pointer to an array of THUMBBUTTON structures. Each THUMBBUTTON defines an
        /// individual button to be added to the toolbar. Buttons cannot be added or deleted later, so this must
        /// be the full defined set. Buttons also cannot be reordered, so their order in the array, which is the
        /// order in which they are displayed left to right, will be their permanent order.</param>
        void ThumbBarAddButtons(
        IntPtr hWnd,
        uint cButtons,
        [MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] pButton);

        void ThumbBarUpdateButtons(
        IntPtr hWnd,
        uint cButtons,
        [MarshalAs(UnmanagedType.LPArray)] THUMBBUTTON[] pButton);

        /// <summary>
        /// Specifies an image list that contains button images for a toolbar embedded in a thumbnail image of a
        /// window in a taskbar button flyout.
        /// </summary>
        /// <param name="hWnd">The handle of the window whose thumbnail representation contains the toolbar to be
        /// updated. This handle must belong to the calling process.</param>
        /// <param name="himl">The handle of the image list that contains all button images to be used in the toolbar.</param>
        void ThumbBarSetImageList(IntPtr hWnd, IntPtr himl);

        /// <summary>
        /// Applies an overlay to a taskbar button to indicate application status or a notification to the user.
        /// </summary>
        /// <param name="hWnd">The handle of the window whose associated taskbar button receives the overlay.
        /// This handle must belong to a calling process associated with the button's application and must be
        /// a valid HWND or the call is ignored.</param>
        /// <param name="hIcon">The handle of an icon to use as the overlay. This should be a small icon,
        /// measuring 16x16 pixels at 96 dots per inch (dpi). If an overlay icon is already applied to the
        /// taskbar button, that existing overlay is replaced.</param>
        /// <param name="pszDescription">A pointer to a string that provides an alt text version of the
        /// information conveyed by the overlay, for accessibility purposes.</param>
        void SetOverlayIcon(IntPtr hWnd, IntPtr hIcon, string pszDescription);

        /// <summary>
        /// Specifies or updates the text of the tooltip that is displayed when the mouse pointer rests on an
        /// individual preview thumbnail in a taskbar button flyout.
        /// </summary>
        /// <param name="hWnd">The handle to the window whose thumbnail displays the tooltip. This handle must
        /// belong to the calling process.</param>
        /// <param name="pszTip">The pointer to the text to be displayed in the tooltip. This value can be NULL,
        /// in which case the title of the window specified by hwnd is used as the tooltip.</param>
        void SetThumbnailTooltip(IntPtr hWnd, string pszTip);

        /// <summary>
        /// Selects a portion of a window's client area to display as that window's thumbnail in the taskbar.
        /// </summary>
        /// <param name="hWnd">The handle to a window represented in the taskbar.</param>
        /// <param name="prcClip">A pointer to a RECT structure that specifies a selection within the window's
        /// client area, relative to the upper-left corner of that client area. To clear a clip that is already
        /// in place and return to the default display of the thumbnail, set this parameter to NULL.</param>
        void SetThumbnailClip(IntPtr hWnd, IntPtr prcClip);
    }

    [Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
    [ClassInterface(ClassInterfaceType.None)]
    [ComImport]
    public class TaskbarList { }

    #region pinvoke.net にはあったけど MSDN 見つけられんかったからヘッダファイル分かんね

    [ComImport]
    [Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IServiceProvider
    {
        void QueryService(ref Guid guidService, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
    }
    #endregion

    partial class NativeMethods
    {
        [DllImport("shell32.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
        public static extern void SHCreateItemFromParsingName(
            [In][MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            [In] IntPtr pbc,
            [In][MarshalAs(UnmanagedType.LPStruct)] Guid riid,
            [Out][MarshalAs(UnmanagedType.Interface, IidParameterIndex = 2)] out IShellItem ppv
        );

        [DllImport("shell32.dll", SetLastError = true)]
        public static extern void SetCurrentProcessExplicitAppUserModelID(
            [MarshalAs(UnmanagedType.LPWStr)] string AppID
        );
    }
}
