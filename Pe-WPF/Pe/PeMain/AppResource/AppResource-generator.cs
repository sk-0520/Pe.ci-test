
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
					//4
			
			#region commonFiltering

			public static string CommonFilteringPath
			{
				get { return commonFiltering; }
			}

			
					public static BitmapSource CommonFilteringImage
					{
						get { return GetImage(CommonFilteringPath); }
					}

					
			#endregion commonFiltering
			
						//4
			
			#region applicationIcon

			public static string ApplicationIconPath
			{
				get { return applicationIcon; }
			}

			
					public static BitmapSource GetApplicationIconIcon(IconScale iconScale, ILogger logger = null)
					{
						return GetIcon(ApplicationIconPath, iconScale, logger);
					}

					#region IconScale

											
						public static BitmapSource ApplicationIconIconSmall
						{
							get
							{
								return GetApplicationIconIcon(IconScale.Small);
							}
						}

												
						public static BitmapSource ApplicationIconIconNormal
						{
							get
							{
								return GetApplicationIconIcon(IconScale.Normal);
							}
						}

												
						public static BitmapSource ApplicationIconIconBig
						{
							get
							{
								return GetApplicationIconIcon(IconScale.Big);
							}
						}

												
						public static BitmapSource ApplicationIconIconLarge
						{
							get
							{
								return GetApplicationIconIcon(IconScale.Large);
							}
						}

											
					#endregion IconScale

					
			#endregion applicationIcon
			
				}
}



