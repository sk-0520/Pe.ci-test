namespace ContentTypeTextNet.Library.SharedLibrary.Define
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	public static class SharedConstants
	{
		public const string pack = "pack://application:,,,";

		public static Uri GetPackUri(string path)
		{
			return new Uri(pack + path);
		}

		public static Uri GetAssemblyUri(Assembly assembly, string path)
		{
			var asmName = "/" + assembly.GetName().Name;
			if(path.FirstOrDefault() == '/') {
				path = path.Substring(1);
			}
			return GetPackUri(asmName + ";component/" + path);
		}

		public static Uri GetEntryUri(string path)
		{
			return GetAssemblyUri(Assembly.GetEntryAssembly(), path);
		}
	}
}
