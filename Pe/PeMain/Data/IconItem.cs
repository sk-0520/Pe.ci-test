using System;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// アイコン。
	/// </summary>
	[Serializable]
	public class IconItem: IconPath, IItem, ICloneable
	{
		public IconItem(): base()
		{ }
		
		public IconItem(string iconPath): base(iconPath)
		{ }
		
		public IconItem(string path, int index): base(path, index)
		{ }

		public IconItem(IconPath iconPath): base(iconPath.Path, iconPath.Index)
		{ }

		#region ICloneable implementation
		public object Clone()
		{
			return new IconItem(Path, Index);
		}
		#endregion
		
		#region Equals and GetHashCode implementation
		public override bool Equals(object obj)
		{
			var other = obj as IconItem;
			if (other == null) {
				return false;
			} else {
				return Index == other.Index && Path == other.Path;
			}
		}

		
		public override int GetHashCode()
		{
			return Path.GetHashCode() + Index.GetHashCode();
		}


		#endregion
	}
}
