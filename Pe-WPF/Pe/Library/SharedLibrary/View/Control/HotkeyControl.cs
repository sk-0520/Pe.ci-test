namespace ContentTypeTextNet.Library.SharedLibrary.View.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using System.Windows.Input;

	public class HotkeyControl:TextBox
	{
		public HotkeyControl()
			: base()
		{ }

		#region property

		ModifierKeys ModifierKeys { get; set; }
		Key Key { get; set; }

		#endregion

		#region TextBox

		protected override void OnKeyDown(KeyEventArgs e)
		{
			ModifierKeys = Keyboard.Modifiers;
			Key = e.Key;

			SetText(ModifierKeys, Key);

			e.Handled = true;
		}

		#endregion

		#region function

		void SetText(ModifierKeys mod, Key key)
		{
			var buffer = new StringBuilder();

			var modText = GetDisplayModTexts(mod);
			if(modText.Any()) {
				buffer.Append(string.Join(GetDisplayAddText(), GetDisplayKeyText(key)));
			}
			buffer.Append(GetDisplayKeyText(key));

			Text = buffer.ToString();
		}

		protected virtual string GetDisplayAddText()
		{
			return " + ";
		}

		IEnumerable<string> GetDisplayModTexts(ModifierKeys mod)
		{
			var mk = new[] {
				ModifierKeys.Alt,
				ModifierKeys.Control,
				ModifierKeys.Shift,
			};

			foreach(var m in mk) {
				if(ModifierKeys.HasFlag(m)) {
					yield return GetDisplayModText(m);
				}
			}
			
		}

		protected virtual string GetDisplayModText(ModifierKeys mod)
		{
			return mod.ToString();
		}

		protected virtual string GetDisplayKeyText(Key key)
		{
			return key.ToString();
		}

		#endregion
	}
}
