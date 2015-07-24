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
		const string applicationTasktray
#if DEBUG
				= "/Resources/Icon/Tasktray/App-debug.ico";
#elif BETA
				= "/Resources/Icon/Tasktray/App-beta.ico";
#else
				= "/Resources/Icon/Tasktray/App-release.ico";
#endif
		[AppResource(AppResourceType.Image)]
		const string commonFiltering = "/Resources/Image/Common/Filtering.png";
		[AppResource(AppResourceType.Image)]
		const string commonTemplate = "/Resources/Image/Common/Template.png";
		[AppResource(AppResourceType.Image)]
		const string commonClipboard = "/Resources/Image/Common/Clipboard.png";
		[AppResource(AppResourceType.Image)]
		const string commonSend = "/Resources/Image/Common/Send.png";
		[AppResource(AppResourceType.Image)]
		const string commonPin = "/Resources/Image/Common/Pin.png";
		[AppResource(AppResourceType.Image)]
		const string commonWindowList = "/Resources/Image/Common/Pin.png";
		[AppResource(AppResourceType.Image)]
		const string commonAdd = "/Resources/Image/Common/Add.png";
		[AppResource(AppResourceType.Image)]
		const string commonRemove = "/Resources/Image/Common/Remove.png";
		[AppResource(AppResourceType.Image)]
		const string commonSave = "/Resources/Image/Common/Save.png";
		[AppResource(AppResourceType.Image)]
		const string commonUsingClipboard = "/Resources/Image/Common/UsingClipboard.png";
		[AppResource(AppResourceType.Image)]
		const string commonUp = "/Resources/Image/Common/Up.png";
		[AppResource(AppResourceType.Image)]
		const string commonDown = "/Resources/Image/Common/Down.png";


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
		const string clipboardImageFit = "/Resources/Image/Clipboard/ImageFit.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardImageRaw = "/Resources/Image/Clipboard/ImageRaw.png";

		[AppResource(AppResourceType.Image)]
		const string windowLoad = "/Resources/Image/Window/WindowLoad.png";
		[AppResource(AppResourceType.Image)]
		const string windowSave = "/Resources/Image/Window/WindowSave.png";
	}
}
