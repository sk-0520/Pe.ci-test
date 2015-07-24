
namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	partial class AppResource
	{
		#region Icon: application

		public static string ApplicationPath
		{
			get { return application; }
		}


		public static BitmapSource GetApplicationIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationPath, iconScale, logger);
		}

		#region IconScale

		public static BitmapSource ApplicationIconSmall
		{
			get
			{
				return GetApplicationIcon(IconScale.Small);
			}
		}
		public static BitmapSource ApplicationIconNormal
		{
			get
			{
				return GetApplicationIcon(IconScale.Normal);
			}
		}
		public static BitmapSource ApplicationIconBig
		{
			get
			{
				return GetApplicationIcon(IconScale.Big);
			}
		}
		public static BitmapSource ApplicationIconLarge
		{
			get
			{
				return GetApplicationIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion application
		#region Icon: notFound

		public static string NotFoundPath
		{
			get { return notFound; }
		}


		public static BitmapSource GetNotFoundIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(NotFoundPath, iconScale, logger);
		}

		#region IconScale

		public static BitmapSource NotFoundIconSmall
		{
			get
			{
				return GetNotFoundIcon(IconScale.Small);
			}
		}
		public static BitmapSource NotFoundIconNormal
		{
			get
			{
				return GetNotFoundIcon(IconScale.Normal);
			}
		}
		public static BitmapSource NotFoundIconBig
		{
			get
			{
				return GetNotFoundIcon(IconScale.Big);
			}
		}
		public static BitmapSource NotFoundIconLarge
		{
			get
			{
				return GetNotFoundIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion notFound
		#region Icon: launcherToolbarMain

		public static string LauncherToolbarMainPath
		{
			get { return launcherToolbarMain; }
		}


		public static BitmapSource GetLauncherToolbarMainIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(LauncherToolbarMainPath, iconScale, logger);
		}

		#region IconScale

		public static BitmapSource LauncherToolbarMainIconSmall
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Small);
			}
		}
		public static BitmapSource LauncherToolbarMainIconNormal
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Normal);
			}
		}
		public static BitmapSource LauncherToolbarMainIconBig
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Big);
			}
		}
		public static BitmapSource LauncherToolbarMainIconLarge
		{
			get
			{
				return GetLauncherToolbarMainIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion launcherToolbarMain
		#region Icon: launcherCommand

		public static string LauncherCommandPath
		{
			get { return launcherCommand; }
		}


		public static BitmapSource GetLauncherCommandIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(LauncherCommandPath, iconScale, logger);
		}

		#region IconScale

		public static BitmapSource LauncherCommandIconSmall
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Small);
			}
		}
		public static BitmapSource LauncherCommandIconNormal
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Normal);
			}
		}
		public static BitmapSource LauncherCommandIconBig
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Big);
			}
		}
		public static BitmapSource LauncherCommandIconLarge
		{
			get
			{
				return GetLauncherCommandIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion launcherCommand
		#region Icon: applicationTasktray

		public static string ApplicationTasktrayPath
		{
			get { return applicationTasktray; }
		}


		public static BitmapSource GetApplicationTasktrayIcon(IconScale iconScale, ILogger logger = null)
		{
			return GetIcon(ApplicationTasktrayPath, iconScale, logger);
		}

		#region IconScale

		public static BitmapSource ApplicationTasktrayIconSmall
		{
			get
			{
				return GetApplicationTasktrayIcon(IconScale.Small);
			}
		}
		public static BitmapSource ApplicationTasktrayIconNormal
		{
			get
			{
				return GetApplicationTasktrayIcon(IconScale.Normal);
			}
		}
		public static BitmapSource ApplicationTasktrayIconBig
		{
			get
			{
				return GetApplicationTasktrayIcon(IconScale.Big);
			}
		}
		public static BitmapSource ApplicationTasktrayIconLarge
		{
			get
			{
				return GetApplicationTasktrayIcon(IconScale.Large);
			}
		}

		#endregion IconScale

		#endregion applicationTasktray
		#region Image: commonFiltering

		public static string CommonFilteringPath
		{
			get { return commonFiltering; }
		}


		public static BitmapSource CommonFilteringImage
		{
			get { return GetImage(CommonFilteringPath); }
		}

		#endregion commonFiltering
		#region Image: commonTemplate

		public static string CommonTemplatePath
		{
			get { return commonTemplate; }
		}


		public static BitmapSource CommonTemplateImage
		{
			get { return GetImage(CommonTemplatePath); }
		}

		#endregion commonTemplate
		#region Image: commonClipboard

		public static string CommonClipboardPath
		{
			get { return commonClipboard; }
		}


		public static BitmapSource CommonClipboardImage
		{
			get { return GetImage(CommonClipboardPath); }
		}

		#endregion commonClipboard
		#region Image: commonSend

		public static string CommonSendPath
		{
			get { return commonSend; }
		}


		public static BitmapSource CommonSendImage
		{
			get { return GetImage(CommonSendPath); }
		}

		#endregion commonSend
		#region Image: commonPin

		public static string CommonPinPath
		{
			get { return commonPin; }
		}


		public static BitmapSource CommonPinImage
		{
			get { return GetImage(CommonPinPath); }
		}

		#endregion commonPin
		#region Image: commonWindowList

		public static string CommonWindowListPath
		{
			get { return commonWindowList; }
		}


		public static BitmapSource CommonWindowListImage
		{
			get { return GetImage(CommonWindowListPath); }
		}

		#endregion commonWindowList
		#region Image: commonAdd

		public static string CommonAddPath
		{
			get { return commonAdd; }
		}


		public static BitmapSource CommonAddImage
		{
			get { return GetImage(CommonAddPath); }
		}

		#endregion commonAdd
		#region Image: commonRemove

		public static string CommonRemovePath
		{
			get { return commonRemove; }
		}


		public static BitmapSource CommonRemoveImage
		{
			get { return GetImage(CommonRemovePath); }
		}

		#endregion commonRemove
		#region Image: commonSave

		public static string CommonSavePath
		{
			get { return commonSave; }
		}


		public static BitmapSource CommonSaveImage
		{
			get { return GetImage(CommonSavePath); }
		}

		#endregion commonSave
		#region Image: commonUsingClipboard

		public static string CommonUsingClipboardPath
		{
			get { return commonUsingClipboard; }
		}


		public static BitmapSource CommonUsingClipboardImage
		{
			get { return GetImage(CommonUsingClipboardPath); }
		}

		#endregion commonUsingClipboard
		#region Image: commonUp

		public static string CommonUpPath
		{
			get { return commonUp; }
		}


		public static BitmapSource CommonUpImage
		{
			get { return GetImage(CommonUpPath); }
		}

		#endregion commonUp
		#region Image: commonDown

		public static string CommonDownPath
		{
			get { return commonDown; }
		}


		public static BitmapSource CommonDownImage
		{
			get { return GetImage(CommonDownPath); }
		}

		#endregion commonDown
	}
}

