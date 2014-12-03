/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/11/03
 * 時刻: 20:45
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using ContentTypeTextNet.Pe.Library.Utility;
using PeMain.IF;

namespace PeMain.Data
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
