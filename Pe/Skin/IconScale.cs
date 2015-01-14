namespace ContentTypeTextNet.Pe.Library.Skin
{
	using System.Drawing;

	/// <summary>
	/// アイコンサイズ。
	/// </summary>
	public enum IconScale
	{
		/// <summary>
		/// 16px
		/// </summary>
		Small = 16,
		/// <summary>
		/// 32px
		/// </summary>
		Normal = 32,
		/// <summary>
		/// 48px
		/// </summary>
		Big = 48,
		/// <summary>
		/// 256px
		/// </summary>
		Large = 256,
	}

	public static class IconScaleUtility
	{
		public static int ToHeight(this IconScale iconScale)
		{
			return (int)iconScale;
		}
		public static int ToWidth(this IconScale iconScale)
		{
			return (int)iconScale;
		}
		public static Size ToSize(this IconScale iconScale)
		{
			var w = iconScale.ToWidth();
			var h = iconScale.ToHeight();
			return new Size(w, h);
		}
	}
}
