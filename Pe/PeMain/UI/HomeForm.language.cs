using System;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class HomeForm
	{
		void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);
		}
	}
}
