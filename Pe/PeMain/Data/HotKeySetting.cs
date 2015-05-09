namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Windows.Forms;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

	/// <summary>
	/// ホットキー設定。
	/// </summary>
	[Serializable]
	public class HotKeySetting: Item, ICloneable
	{
		public HotKeySetting()
		{
			Key = Keys.None;
			Modifiers = MOD.None;
			
			IsRegistered = false;
		}
		
		/// <summary>
		/// キー
		/// </summary>
		public Keys Key { get; set; }
		/// <summary>
		/// 装飾キー。
		/// </summary>
		public MOD  Modifiers { get; set; }
		
		/// <summary>
		/// 登録済みか。
		/// </summary>
		[XmlIgnore()]
		public bool IsRegistered { get; set; }
		
		/// <summary>
		/// 有効なキー設定か。
		/// </summary>
		public bool Enabled
		{
			get
			{
				return Key != Keys.None && Modifiers != MOD.None;
			}
		}
		
		public Keys GetShorcutKey()
		{
			if(Enabled) {
				var mod = Keys.None;
				if(Modifiers.HasFlag(MOD.MOD_ALT)) {
					mod |= Keys.Alt;
				}
				if(Modifiers.HasFlag(MOD.MOD_CONTROL)) {
					mod |= Keys.Control;
				}
				if(Modifiers.HasFlag(MOD.MOD_SHIFT)) {
					mod |= Keys.Shift;
				}
				if(Modifiers.HasFlag(MOD.MOD_WIN)) {
					mod |= Keys.LWin | Keys.RWin;
				}
				
				return Key | mod;
			} else {
				return Keys.None;
			}
		}

		#region ICloneable

		public object Clone()
		{
			return new HotKeySetting() {
				Key = this.Key,
				Modifiers = this.Modifiers,
			};
		}
		#endregion
	}
}
