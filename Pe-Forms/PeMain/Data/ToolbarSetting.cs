namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// 
	/// </summary>
	public static class ToolbarPositionConverter
	{
		public static ToolbarPosition ToToolbarPosition(DesktopDockType value)
		{
			return new Dictionary<DesktopDockType, ToolbarPosition>() {
				{ DesktopDockType.Left,   ToolbarPosition.DesktopLeft },
				{ DesktopDockType.Top,    ToolbarPosition.DesktopTop },
				{ DesktopDockType.Right,  ToolbarPosition.DesktopRight },
				{ DesktopDockType.Bottom, ToolbarPosition.DesktopBottom },
			}[value];
		}
		public static DesktopDockType ToDockType(ToolbarPosition value)
		{
			return new Dictionary<ToolbarPosition, DesktopDockType>() {
				{ToolbarPosition.DesktopLeft,   DesktopDockType.Left },
				{ToolbarPosition.DesktopTop,    DesktopDockType.Top },
				{ToolbarPosition.DesktopRight,  DesktopDockType.Right },
				{ToolbarPosition.DesktopBottom, DesktopDockType.Bottom },
			}[value];
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class ToolbarSetting: Item, IDeepClone
	{
		public ToolbarSetting()
		{
			ToolbarGroup = new ToolbarGroup();
			
			Items = new HashSet<ToolbarItem>();
		}
		
		public ToolbarGroup ToolbarGroup { get; set; }
		public HashSet<ToolbarItem> Items { get; set; }

		#region IDeepClone

		public IDeepClone DeepClone()
		{
			var result = new ToolbarSetting() {
				ToolbarGroup = (ToolbarGroup)this.ToolbarGroup.DeepClone(),
			};

			foreach(var item in Items) {
				result.Items.Add((ToolbarItem)item.DeepClone());
			}

			return result;
		}

		#endregion
	}
}
