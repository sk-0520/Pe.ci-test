namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	internal static class InitializeNoteIndexItem
	{
		public static void Correction(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
		{
			V_First(indexItem, previousVersion, nonProcess);
		}

		static void V_First(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
		{ }
	}
}
