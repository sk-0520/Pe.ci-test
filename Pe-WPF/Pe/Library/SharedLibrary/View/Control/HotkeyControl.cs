namespace ContentTypeTextNet.Library.SharedLibrary.View.Control
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Controls;
	using System.Windows.Input;

	/// <summary>
	/// <para>http://stackoverflow.com/questions/2136431/how-do-i-read-custom-keyboard-shortcut-from-user-in-wpf?answertab=votes#tab-top</para>
	/// </summary>
	public class HotkeyControl:TextBox
	{
		#region variable

		IList<Key> _ignoreKeys = new List<Key>() {
			Key.LeftShift,
			Key.RightShift,
			Key.LeftCtrl,
			Key.RightCtrl,
			Key.LeftAlt,
			Key.RightAlt,
			Key.LWin,
			Key.RWin,
		};

		#endregion

		public HotkeyControl()
			: base()
		{
			IsReadOnly = true;
			IsReadOnlyCaretVisible = true;
			ResetKey();
		}

		#region property

		ModifierKeys ModifierKeys { get; set; }
		Key Key { get; set; }

		#endregion

		#region TextBox

		protected override void OnPreviewKeyDown(KeyEventArgs e)
		{
			e.Handled = true; 
			Key key = (e.Key == Key.System ? e.SystemKey : e.Key);

			// Ignore modifier keys.
			if(this._ignoreKeys.Any(k => k == key)) {
				ResetKey();
				SetText();
				return;
			}

			SetKey(Keyboard.Modifiers, e.Key);

			SetText();
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			if(Key == Key.None) {
				ResetKey();
				SetText();
			}

			base.OnKeyUp(e);
		}

		#endregion

		#region function

		void ResetKey()
		{
			SetKey(ModifierKeys.None, Key.None);
		}

		void SetKey(ModifierKeys mod, Key key)
		{
			ModifierKeys = mod;
			Key = key;
		}

		void SetText()
		{
			SetText(ModifierKeys, Key);
		}

		void SetText(ModifierKeys mod, Key key)
		{
			var buffer = new StringBuilder();

			var modText = GetDisplayModTexts(mod);
			if(modText.Any()) {
				buffer.Append(string.Join(GetDisplayAddText(), modText));
				if(key != Key.None) {
					buffer.Append(GetDisplayAddText());
				}
			}
			buffer.Append(GetDisplayKeyText(key));

			Text = buffer.ToString();
		}

		protected virtual string GetDisplayAddText()
		{
			return "+";
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
