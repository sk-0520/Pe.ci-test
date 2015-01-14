//#define USE_MOUSE_HOOK

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using MouseKeyboardActivityMonitor;
	using MouseKeyboardActivityMonitor.WinApi;

	class Listener
	{
		private Hooker _globalHook;

		public Listener()
		{
			this._globalHook = new GlobalHooker();

#if USE_MOUSE_HOOK
			Mouse = new MouseHookListener(this._globalHook);
#endif
			Keyboard = new KeyboardHookListener(this._globalHook);
		}

#if USE_MOUSE_HOOK
		public MouseHookListener Mouse { get; set; }
#endif
		public KeyboardHookListener Keyboard { get; set; }

		public bool Enabled
		{
			get
			{
#if USE_MOUSE_HOOK
				return Mouse.Enabled | Keyboard.Enabled;
#else
				return Keyboard.Enabled;
#endif
			}
			set
			{
#if USE_MOUSE_HOOK
				Mouse.Enabled = value;
#endif
				Keyboard.Enabled = value;
			}
		}

		public DateTime PrevToolbarHiddenTime { get; set; }
	}
}
