namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class FileSystemInfoUtility
	{
		public static bool IsDirectory(this FileSystemInfo fs)
		{
			return fs.Attributes.HasFlag(FileAttributes.Directory);
		}

		public static bool IsHidden(this FileSystemInfo fs)
		{
			return fs.Attributes.HasFlag(FileAttributes.Hidden);
		}
	}
}
