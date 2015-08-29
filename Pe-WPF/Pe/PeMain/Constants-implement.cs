namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	partial class Constants
	{
		#region property

		public static string ShortcutName
		{
			get
			{
#if DEBUG
				return shortcutNameDebug;
#elif BETA
				return shortcutNameBeta;
#else
				return shortcutNameRelease;
#endif
			}

		}

		public static string BuildType
		{
			get
			{
#if DEBUG
				return buildTypeDebug;
#elif BETA
				return buildTypeBeta;
#else
				return buildTypeRelease;
#endif
			}

		}

		#endregion
	}
}
