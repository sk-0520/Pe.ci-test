namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class DateTimeExtension
	{
		public static string ToDetailTimestampString(this DateTime value)
		{
			return value.ToString("o");
		}
	}
}
