using System;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	public enum HashType
	{
		SHA1,
		MD5,
		CRC32,
	}

	public class EnumToBooleanConverter: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.Equals(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.Equals(true) ? parameter : Binding.DoNothing;
		}
	}
	public class HashModel
	{
		public HashModel()
		{
			HashSetting = new HashSetting();
		}

		public HashSetting HashSetting { get; set; }

		public string EventName { get; set; }
		public string FilePath { get; set; }
		public HashType HashType { get; set; }
		public string SHA1 { get; set; }
		public string MD5 { get; set; }
		public string CRC32 { get; set; }
		public int PercentSHA1 { get; set; }
		public int PercentMD5 { get; set; }
		public int PercentCRC32 { get; set; }

		public string Compare { get; set; }
	}
}
