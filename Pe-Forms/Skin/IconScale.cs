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

	/// <summary>
	/// IconScale共通処理。
	/// </summary>
	public static class IconScaleUtility
	{
		/// <summary>
		/// 高さをpx単位で取得。
		/// </summary>
		/// <param name="iconScale"></param>
		/// <returns></returns>
		public static int ToHeight(this IconScale iconScale)
		{
			return (int)iconScale;
		}

		/// <summary>
		/// 横幅をpx単位で取得。
		/// </summary>
		/// <param name="iconScale"></param>
		/// <returns></returns>
		public static int ToWidth(this IconScale iconScale)
		{
			return (int)iconScale;
		}

		/// <summary>
		/// 縦横をpx範囲で取得。
		/// </summary>
		/// <param name="iconScale"></param>
		/// <returns></returns>
		public static Size ToSize(this IconScale iconScale)
		{
			var w = iconScale.ToWidth();
			var h = iconScale.ToHeight();
			return new Size(w, h);
		}
	}
}
