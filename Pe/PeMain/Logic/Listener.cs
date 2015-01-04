using System;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.WinApi;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	class Listener
	{
		private Hooker _globalHook;

		public Listener()
		{
			this._globalHook = new GlobalHooker();

			Mouse = new MouseHookListener(this._globalHook);
			Keyboard = new KeyboardHookListener(this._globalHook);
		}

		public MouseHookListener Mouse { get; set; }
		public KeyboardHookListener Keyboard { get; set; }

		public bool Enabled
		{
			get
			{
				return Mouse.Enabled | Keyboard.Enabled;
			}
			set
			{
				Mouse.Enabled = value;
				Keyboard.Enabled = value;
			}
		}

		public DateTime PrevToolbarHiddenTime { get; set; }
	}
}
