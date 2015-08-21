namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.Logic.Utility;

	/// <summary>
	/// Peで使用するリソース関係。
	/// <para>自分の持ち物くらい好きにさわらせてくれ。</para>
	/// <para>細かい実装は PeMain/AppResource に分離。本ファイルでは定数定義のみにとどめる。</para>
	/// </summary>
	public static partial class AppResource
	{
		[AppResource(AppResourceType.Icon)]
		const string application="/Resources/Icon/App.ico";
		[AppResource(AppResourceType.Icon)]
		const string notFound = "/Resources/Icon/NotFound.ico";
		[AppResource(AppResourceType.Icon)]
		const string launcherToolbarMain = "/Resources/Icon/LauncherToolbarMain.ico";
		[AppResource(AppResourceType.Icon)]
		const string launcherCommand = "/Resources/Icon/LauncherCommand.ico";

		[AppResource(AppResourceType.Icon)]
		const string applicationTasktrayDebug = "/Resources/Icon/Tasktray/App-debug.ico";
		[AppResource(AppResourceType.Icon)]
		const string applicationTasktrayBeta = "/Resources/Icon/Tasktray/App-beta.ico";
		[AppResource(AppResourceType.Icon)]
		const string applicationTasktrayRelease = "/Resources/Icon/Tasktray/App-release.ico";
		[AppResource(AppResourceType.Image)]
		const string commonFiltering = "/Resources/Image/Common/CommonFiltering.png";
		[AppResource(AppResourceType.Image)]
		const string commonCopy = "/Resources/Image/Common/CommonCopy.png";
		[AppResource(AppResourceType.Image)]
		const string commonSend = "/Resources/Image/Common/CommonSend.png";
		[AppResource(AppResourceType.Image)]
		const string commonPin = "/Resources/Image/Common/CommonPin.png";
		[AppResource(AppResourceType.Image)]
		const string commonAdd = "/Resources/Image/Common/CommonAdd.png";
		[AppResource(AppResourceType.Image)]
		const string commonRemove = "/Resources/Image/Common/CommonRemove.png";
		[AppResource(AppResourceType.Image)]
		const string commonSave = "/Resources/Image/Common/CommonSave.png";
		[AppResource(AppResourceType.Image)]
		const string commonUsingClipboard = "/Resources/Image/Common/CommonUsingClipboard.png";
		[AppResource(AppResourceType.Image)]
		const string commonUp = "/Resources/Image/Common/CommonUp.png";
		[AppResource(AppResourceType.Image)]
		const string commonDown = "/Resources/Image/Common/CommonDown.png";
		[AppResource(AppResourceType.Image)]
		const string commonFile = "/Resources/Image/Common/CommonFile.png";
		[AppResource(AppResourceType.Image)]
		const string commonFolder = "/Resources/Image/Common/CommonFolder.png";
		[AppResource(AppResourceType.Image)]
		const string commonClear = "/Resources/Image/Common/CommonClear.png";
		[AppResource(AppResourceType.Image)]
		const string commonRefresh = "/Resources/Image/Common/CommonRefresh.png";
		[AppResource(AppResourceType.Image)]
		const string commonFontBold = "/Resources/Image/Common/CommonFontBold.png";
		[AppResource(AppResourceType.Image)]
		const string commonFontItalic = "/Resources/Image/Common/CommonFontItalic.png";


		[AppResource(AppResourceType.Image)]
		const string templateTemplate = "/Resources/Image/Template/Template.png";
		[AppResource(AppResourceType.Image)]
		const string templatePlain = "/Resources/Image/Template/TemplatePlain.png";
		[AppResource(AppResourceType.Image)]
		const string templateReplace = "/Resources/Image/Template/TemplateReplace.png";
		[AppResource(AppResourceType.Image)]
		const string templateProgrammable = "/Resources/Image/Template/TemplateProgrammable.png";

		[AppResource(AppResourceType.Image)]
		const string clipboardClear = "/Resources/Image/Clipboard/Clear.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardTextFormat = "/Resources/Image/Clipboard/ClipboardText.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardHtml = "/Resources/Image/Clipboard/ClipboardHtml.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardRichTextFormat = "/Resources/Image/Clipboard/ClipboardRichTextFormat.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardImage = "/Resources/Image/Clipboard/ClipboardImage.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardFile = "/Resources/Image/Clipboard/ClipboardFile.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardImageFit = "/Resources/Image/Clipboard/ClipboardImageFit.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardImageRaw = "/Resources/Image/Clipboard/ClipboardImageRaw.png";

		[AppResource(AppResourceType.Image)]
		const string streamKill = "/Resources/Image/Stream/StreamKill.png";

		[AppResource(AppResourceType.Image)]
		const string windowList = "/Resources/Image/Window/WindowList.png";
		[AppResource(AppResourceType.Image)]
		const string windowLoad = "/Resources/Image/Window/WindowLoad.png";
		[AppResource(AppResourceType.Image)]
		const string windowSave = "/Resources/Image/Window/WindowSave.png";
	}
}
