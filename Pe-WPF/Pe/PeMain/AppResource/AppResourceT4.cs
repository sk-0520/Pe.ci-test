
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

		/// <summary>
	/// Peで使用するリソース関係。
	/// <para>自分の持ち物くらい好きにさわらせてくれ。</para>
	/// </summary>
	public static partial class AppResourceT4
	{
		#region static

		static IconCaching<string> _iconCaching = new IconCaching<string>();
		static Caching<string,BitmapSource> _imageCaching = new Caching<string,BitmapSource>();

		#endregion

		#region function

		static BitmapSource GetImage(string path)
		{
			return _imageCaching.Get(path, () => {
				var uri = SharedConstants.GetEntryUri(path);
				return new BitmapImage(uri);
			});
		}

		static BitmapSource GetIcon(string path, IconScale iconScale, ILogger logger = null)
		{
			return _iconCaching[iconScale].Get(path, () => {
				using(var icon = new IconWrapper(path, iconScale)) {
					return icon.MakeBitmapSource();
				}
			});
		}

		#endregion
	}

	public enum AppResourceType
	{
		Image,
		Icon
	}

	public class AppResourceAttribute: Attribute
	{
		public AppResourceAttribute(AppResourceType appResourceType)
		{
			AppResourceType = appResourceType;
		}

		#region property

		public AppResourceType AppResourceType { get; private set; }

		#endregion
	}

	partial class AppResourceT4
	{
		[AppResource(AppResourceType.Image)]
		const string commonFiltering = "/Resources/Image/Common/Filtering.png";

		[AppResource(AppResourceType.Icon)]
		const string applicationIcon = "/Resources/Icon/App.ico";
	}

	partial class AppResourceT4
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



