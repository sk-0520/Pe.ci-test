namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;

	public class IconCaching<TChildKey>: Dictionary<IconScale, Caching<TChildKey, BitmapSource>>
	{
		public IconCaching()
		{
			Initialize();
		}

		#region function

		protected void Initialize()
		{
			foreach(var iconScale in EnumUtility.GetMembers<IconScale>()) {
				this.Add(iconScale, new Caching<TChildKey, BitmapSource>());
			}
		}

		public new void Clear()
		{
			foreach(var value in this.Values) {
				value.Clear();
			}

			base.Clear();

			Initialize();
		}

		#endregion
	}
}
