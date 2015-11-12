namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.ComponentModel;
	using System.Drawing;
	using System.IO;
	using System.Xml;

	/// <summary>
	/// プロパティに関する共通処理。
	/// </summary>
	public static class PropertyUtility
	{
		public static byte[] MixinImageGetter(Image image)
		{
			if(image == null) {
				return null;
			}

			var converter = TypeDescriptor.GetConverter(image.GetType());
			var binary = (byte[])converter.ConvertTo(image, typeof(byte[]));
			return binary;
		}

		public static Image MixinImageSetter(byte[] binary)
		{
			if(binary == null) {
				return null;
			}

			using(var stream = new MemoryStream(binary)) {
				return new Bitmap(stream);
			}
		}

		//[XmlElement("Name", DataType = "duration")]
		public static string MixinTimeSpanGetter(TimeSpan time)
		{
			return XmlConvert.ToString(time);
		}

		public static TimeSpan MixinTimeSpanSetter(string time)
		{
			if(!string.IsNullOrWhiteSpace(time)) {
				return XmlConvert.ToTimeSpan(time);
			}
			
			return default(TimeSpan);
		}

		public static string MixinColorGetter(Color color)
		{
			return ColorTranslator.ToHtml(color);
		}
		public static Color MixinColorSetter(string color)
		{
			return ColorTranslator.FromHtml(color);
		}
	}
}
