
using System;
namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// アイコンのファイルパス。
	/// </summary>
	public class IconPath
	{
		/// <summary>
		/// アイコンのファイルパスを作成。
		/// </summary>
		public IconPath(): this(string.Empty, 0)
		{ }

		/// <summary>
		/// アイコンのファイルパスを作成。
		/// </summary>
		/// <param name="iconPath"></param>
		[Obsolete]
		public IconPath(string iconPath)
		{
			var index = iconPath.LastIndexOf(',');
			if(index == -1) {
				Path = iconPath;
				Index = 0;
			} else {
				Path = iconPath.Substring(0, index);
				Index = int.Parse(iconPath.Substring(index + 1));
			}
		}

		/// <summary>
		/// アイコンのファイルパスを作成。
		/// </summary>
		/// <param name="path"></param>
		/// <param name="index"></param>
		public IconPath(string path, int index)
		{
			Path = path;
			Index = index;
		}

		/// <summary>
		/// ファイルパス。
		/// </summary>
		public virtual string Path { get; set; }
		/// <summary>
		/// アイコンインデックス。
		/// </summary>
		public virtual int Index { get; set; }

		public override string ToString()
		{
			return string.Format("{0},{1}", Path, Index);
		}
	}
}
