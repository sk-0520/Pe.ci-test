namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public static class FormsConverter
	{
		public static Key GetKey(HotKeySetting setting)
		{
			return KeyConvertUtility.ConvertKey(setting.Key);
		}

		public static ModifierKeys GetModifierKeys(HotKeySetting setting)
		{
			var mod = (ContentTypeTextNet.Library.PInvoke.Windows.MOD)setting.Modifiers;
			return KeyConvertUtility.ConvertModifierKeys(mod);
		}
	}
}
