namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	internal static class InitializeNoteIndexItem
	{
		public static void Correction(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
		{
			V_First(indexItem, previousVersion, nonProcess);
			V_Last(indexItem, previousVersion, nonProcess);
		}

		static void V_Last(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
		{
			indexItem.NoteKind = EnumUtility.GetNormalization(indexItem.NoteKind, NoteKind.Text);
			indexItem.Font.Size = Constants.noteFontSize.GetClamp(indexItem.Font.Size);

			if(string.IsNullOrWhiteSpace(indexItem.Font.Family)) {
				indexItem.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
			}

			if(SettingUtility.IsIllegalPlusNumber(indexItem.WindowWidth)) {
				indexItem.WindowWidth = Constants.noteDefualtSize.Width;
			}
			if(SettingUtility.IsIllegalPlusNumber(indexItem.WindowHeight)) {
				indexItem.WindowHeight = Constants.noteDefualtSize.Height;
			}
		}

		static void V_First(NoteIndexItemModel indexItem, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			indexItem.NoteKind = NoteKind.Text;
			indexItem.Font.Size = Constants.noteFontSize.median;
			indexItem.Font.Family = FontUtility.GetOriginalFontFamilyName(SystemFonts.MessageFontFamily);
			indexItem.WindowWidth = Constants.noteDefualtSize.Width;
			indexItem.WindowHeight = Constants.noteDefualtSize.Height;
			indexItem.IsLocked = false;
			indexItem.IsCompacted = false;
			indexItem.IsTopmost = false;
		}
	}
}
