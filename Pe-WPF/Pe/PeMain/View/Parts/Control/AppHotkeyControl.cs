namespace ContentTypeTextNet.Pe.PeMain.View.Parts.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.View.Control;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	public class AppHotkeyControl: HotkeyControl, ICommonData
	{
		public AppHotkeyControl()
		{ }

		#region property

		protected object ExtensionData { get; private set; }

		#endregion

		#region ICommonData

		public void SetCommonData(CommonData commonData, object extensionData)
		{
			CommonData = commonData;
			ExtensionData = extensionData;

			SetText();
		}

		public CommonData CommonData { get; private set; }

		#endregion

		#region HotkeyControl

		protected override string GetDisplayModText(ModifierKeys mod)
		{
			if(CommonData != null) {
				return LanguageUtility.GetTextFromSingleModifierKey(mod, CommonData.Language);
			} else {
				return base.GetDisplayModText(mod);
			}
		}

		protected override string GetDisplayKeyText(Key key)
		{
			if(CommonData != null) {
				return LanguageUtility.GetTextFromSingleKey(key, CommonData.Language);
			} else {
				return base.GetDisplayKeyText(key);
			}
		}

		protected override string DisplayAddText 
		{ 
			get 
			{ 
			if(CommonData != null) {
				return LanguageUtility.GetKeySeparatorText(CommonData.Language);
			} else {
				return base.DisplayAddText;
			}
			} 
		}

		#endregion
	}
}
