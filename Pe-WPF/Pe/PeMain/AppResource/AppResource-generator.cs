
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
					//8
						#region application

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
						//8
						#region notFound

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
						//8
						#region launcherToolbarMain

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
						//8
						#region launcherCommand

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
						//8
						#region applicationTasktray

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
				}
}



