﻿namespace ContentTypeTextNet.Pe.PeMain
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
		const string commonRun = "/Resources/Image/Common/CommonRun.png";
		[AppResource(AppResourceType.Image)]
		const string commonCreate = "/Resources/Image/Common/CommonCreate.png";
		[AppResource(AppResourceType.Image)]
		const string commonConfig = "/Resources/Image/Common/CommonConfig.png";
		[AppResource(AppResourceType.Image)]
		const string commonClose = "/Resources/Image/Common/CommonClose.png";
		[AppResource(AppResourceType.Image)]
		const string commonUpdate = "/Resources/Image/Common/CommonUpdate.png";
		[AppResource(AppResourceType.Image)]
		const string commonOperatingSystem = "/Resources/Image/Common/CommonOperatingSystem.png";

		[AppResource(AppResourceType.Image)]
		const string toolbarToolbar = "/Resources/Image/Toolbar/Toolbar.png";

		[AppResource(AppResourceType.Image)]
		const string streamKill = "/Resources/Image/Stream/StreamKill.png";

		[AppResource(AppResourceType.Image)]
		const string noteNote = "/Resources/Image/Note/Note.png";
		[AppResource(AppResourceType.Image)]
		const string noteHide = "/Resources/Image/Note/NoteHide.png";
		[AppResource(AppResourceType.Image)]
		const string noteFront = "/Resources/Image/Note/NoteFront.png";

		[AppResource(AppResourceType.Image)]
		const string templateTemplate = "/Resources/Image/Template/Template.png";
		[AppResource(AppResourceType.Image)]
		const string templatePlain = "/Resources/Image/Template/TemplatePlain.png";
		[AppResource(AppResourceType.Image)]
		const string templateReplace = "/Resources/Image/Template/TemplateReplace.png";
		[AppResource(AppResourceType.Image)]
		const string templateProgrammable = "/Resources/Image/Template/TemplateProgrammable.png";

		[AppResource(AppResourceType.Image)]
		const string clipboardClipboard = "/Resources/Image/Clipboard/Clipboard.png";
		[AppResource(AppResourceType.Image)]
		const string clipboardClear = "/Resources/Image/Clipboard/ClipboardClear.png";
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
		const string commandCommand = "/Resources/Image/Command/Command.png";

		[AppResource(AppResourceType.Image)]
		const string windowListSystem = "/Resources/Image/Window/WindowListSystem.png";
		[AppResource(AppResourceType.Image)]
		const string windowListTimer = "/Resources/Image/Window/WindowListTimer.png";
		[AppResource(AppResourceType.Image)]
		const string windowLoad = "/Resources/Image/Window/WindowLoad.png";
		[AppResource(AppResourceType.Image)]
		const string windowSave = "/Resources/Image/Window/WindowSave.png";

		[AppResource(AppResourceType.Image)]
		const string logLog = "/Resources/Image/Log/Log.png";
		[AppResource(AppResourceType.Image)]
		const string logDebug = "/Resources/Image/Log/LogDebug.png";
		[AppResource(AppResourceType.Image)]
		const string logTrace = "/Resources/Image/Log/LogTrace.png";
		[AppResource(AppResourceType.Image)]
		const string logInformation = "/Resources/Image/Log/LogInformation.png";
		[AppResource(AppResourceType.Image)]
		const string logWarning = "/Resources/Image/Log/LogWarning.png";
		[AppResource(AppResourceType.Image)]
		const string logError = "/Resources/Image/Log/LogError.png";
		[AppResource(AppResourceType.Image)]
		const string logFatal = "/Resources/Image/Log/LogFatal.png";

		[AppResource(AppResourceType.Image)]
		const string settingSetting = "/Resources/Image/Setting/Setting.png";
		[AppResource(AppResourceType.Image)]
		const string settingLauncherGroupItem = "/Resources/Image/Setting/SettingLauncherGroupItem.png";
		[AppResource(AppResourceType.Image)]
		const string settingLauncherGroupParent = "/Resources/Image/Setting/SettingLauncherGroupParent.png";
	}
}
