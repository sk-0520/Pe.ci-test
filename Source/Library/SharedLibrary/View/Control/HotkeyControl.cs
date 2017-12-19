/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Library.SharedLibrary.View.Control
{
    /// <summary>
    /// <para>http://stackoverflow.com/questions/2136431/how-do-i-read-custom-keyboard-shortcut-from-user-in-wpf?answertab=votes#tab-top</para>
    /// </summary>
    public class HotkeyControl: TextBox
    {
        #region variable

        static readonly Key[] throughSystemKeys = {
            Key.Tab,
            Key.Escape,
            Key.ImeProcessed,
            Key.OemEnlw
        };

        static readonly Key[] ignoreKeys = {
            Key.LeftShift,
            Key.RightShift,
            Key.LeftCtrl,
            Key.RightCtrl,
            Key.LeftAlt,
            Key.RightAlt,
            Key.Back,
            Key.Delete,
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
        bool IsRegistered { get; set; }

        /// <summary>
        /// キー文字列の結合表記文字列。
        /// </summary>
        /// <returns></returns>
        protected virtual string DisplayAddText { get { return "+"; } }

        #endregion

        #region DependencyProperty

        public static readonly DependencyProperty HotkeyProperty = DependencyProperty.Register(
            DependencyPropertyUtility.GetName(nameof(HotkeyProperty)),
            typeof(HotKeyModel),
            typeof(HotkeyControl),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnHotkeyChanged)
            )
        );

        private static void OnHotkeyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var view = obj as HotkeyControl;
            if(view != null) {
                view.Hotkey = e.NewValue as HotKeyModel;
            }
        }

        public HotKeyModel Hotkey
        {
            get { return GetValue(HotkeyProperty) as HotKeyModel; }
            set
            {
                SetValue(HotkeyProperty, value);
                if(value == null) {
                    ResetKey();
                } else {
                    ModifierKeys = value.ModifierKeys;
                    Key = value.Key;
                    IsRegistered = value.IsRegistered;
                }
                SetText();
            }
        }
        #endregion

        #region TextBox

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if(throughSystemKeys.Any(k => k == e.Key)) {
                return;
            }

            e.Handled = true;

            var key = (e.Key == Key.System ? e.SystemKey : e.Key);
            if(ignoreKeys.Any(k => k == key)) {
                ResetKey();
                SetText();
                return;
            }

            SetKey(Keyboard.Modifiers, key);

            SetText();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if(throughSystemKeys.Any(k => k == e.Key)) {
                base.OnKeyUp(e);
                return;
            }

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

            Hotkey = new HotKeyModel() {
                ModifierKeys = this.ModifierKeys,
                Key = this.Key,
                IsRegistered = this.IsRegistered,
            };
        }

        protected void SetText()
        {
            SetText(ModifierKeys, Key);
        }

        void SetText(ModifierKeys mod, Key key)
        {
            var buffer = new StringBuilder();

            var modText = GetDisplayModTexts(mod);
            if(modText.Any()) {
                buffer.Append(string.Join(DisplayAddText, modText));
                if(key != Key.None) {
                    buffer.Append(DisplayAddText);
                }
            }
            buffer.Append(GetDisplayKeyText(key));

            Text = buffer.ToString();
        }

        IEnumerable<string> GetDisplayModTexts(ModifierKeys mod)
        {
            var mk = new[] {
                ModifierKeys.Alt,
                ModifierKeys.Control,
                ModifierKeys.Shift,
                ModifierKeys.Windows,
            };

            foreach(var m in mk) {
                if(ModifierKeys.HasFlag(m)) {
                    yield return GetDisplayModText(m);
                }
            }
        }

        /// <summary>
        /// 修飾キーを表記文字列に変換。
        /// </summary>
        /// <param name="mod">修飾キー。単一データで来る。</param>
        /// <returns></returns>
        protected virtual string GetDisplayModText(ModifierKeys mod)
        {
            return mod.ToString();
        }

        /// <summary>
        /// キーを表記文字列に変換。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected virtual string GetDisplayKeyText(Key key)
        {
            return key.ToString();
        }

        #endregion
    }
}
