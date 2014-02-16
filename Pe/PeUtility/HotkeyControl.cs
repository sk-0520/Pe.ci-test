// http://tech.reboot.pro/showthread.php?tid=2582
/*
 * Hotkey Control
 * Copyright (C) 2012 ByteBlast
 * Removal of this copyright is prohibited
 */
using System;
using System.Collections;
using System.Windows.Forms;
using PI.Windows;

namespace PeUtility
{
	public class HotKeyControl : RichTextBox
	{
		/*
		 * Property to contain the Hotkey value.
		 */
		private Keys _Hotkey = Keys.None;
		public Keys Hotkey
		{
			get
			{
				return _Hotkey;
			}
			set
			{
				_Hotkey = value;
				Redraw();
			}
		}

		/*
		 * Property to contain the Modifiers value.
		 */
		private MOD _Modifiers = MOD.None;
		public MOD Modifiers
		{
			get
			{
				return _Modifiers;
			}
			set
			{
				_Modifiers = value;
				Redraw();
			}
		}


		public virtual void ResetHotKeys()
		{
			Hotkey = Keys.None;
			Modifiers = MOD.None;
			//this.Text = MOD.None;
			Multiline = false;
			Cursor = Cursors.IBeam;
		}

		/*
		 * When the class is constructed: Modify some properties by default and add event call backs.
		 */
		public HotKeyControl()
		{
			this.Cursor = Cursors.Arrow;
			this.ReadOnly = true;
			this.BackColor = System.Drawing.Color.White;

			this.KeyPress += new KeyPressEventHandler(Control_KeyPress);
			this.KeyDown += new KeyEventHandler(Control_KeyDown);
			this.KeyUp += new KeyEventHandler(Control_KeyUp);
			this.SelectionChanged += new EventHandler(Control_SelectionChanged);
		}

		/*
		 * Bypassess the controls default handling.
		 */
		private void Control_KeyPress(object Sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		/*
		 * Obtain the Modifiers and Key then 'draw' the string.
		 */
		private void Control_KeyDown(object Sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
			{
				ResetHotKeys();
				return;
			}
			
			MOD mod = MOD.None;
			if((e.Modifiers & Keys.LWin) == Keys.LWin) {
				mod |= MOD.MOD_WIN;
			}  else if((e.Modifiers & Keys.LWin) == Keys.RWin) {
				mod |= MOD.MOD_WIN;
			}
			if((e.Modifiers & Keys.Alt) == Keys.Alt) {
				mod |= MOD.MOD_ALT;
			}
			if((e.Modifiers & Keys.Shift) == Keys.Shift) {
				mod |= MOD.MOD_SHIFT;
			}
			if((e.Modifiers & Keys.Control) == Keys.Control) {
				mod |= MOD.MOD_CONTROL;
			}
			
			Modifiers = mod;
			Hotkey = e.KeyCode;
			Redraw();
		}


		/*
		 * This isn't really required.
		 */
		private void Control_KeyUp(object Sender, KeyEventArgs e)
		{
			if (Hotkey == Keys.None && Control.ModifierKeys == Keys.None)
			{
				ResetHotKeys();
			}
		}

		/*
		 * This prevents the user from selecting text (my preference).
		 */
		private void Control_SelectionChanged(object Sender, EventArgs e)
		{
			if (this.SelectionStart != this.TextLength)
			{
				this.SelectionStart = this.TextLength;
			}
		}
		
		protected virtual string ToValueString()
		{
			if (Modifiers == MOD.None) {
				return (Modifiers.ToString()).Replace(", ", " + ");
			}
			


			//if (Modifiers != MOD.None) {
			return (Modifiers.ToString()).Replace(", ", " + ") + " + " + Hotkey.ToString();
			//}
		}

		/*
		 * This method is called to print the hotkey combination.
		 */
		protected virtual void Redraw()
		{
			if (Hotkey == Keys.Menu || Hotkey == Keys.ShiftKey || Hotkey == Keys.ControlKey)
			{
				Hotkey = Keys.None;
				return;
			}			
			this.Text = ToValueString();
		}
	}
}
