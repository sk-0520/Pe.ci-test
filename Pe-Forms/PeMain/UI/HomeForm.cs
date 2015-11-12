namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using System.Xml.Linq;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	/// <summary>
	/// Description of HomeForm.
	/// </summary>
	public partial class HomeForm: CommonForm
	{
		#region define
		#endregion ////////////////////////////////////

		#region variable

		List<LogItem> _logList = new List<LogItem>();

		#endregion ////////////////////////////////////

		public HomeForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		//CommonData CommonData { get; set; }

		public bool ItemFound { get; private set; }
		public IReadOnlyList<LogItem> LogList { get { return this._logList; } }
		#endregion ////////////////////////////////////

		#region ISetCommonData
		//public void SetCommonData(CommonData commonData)
		//{
		//	CommonData = commonData;

		//	ApplySetting();
		//}
		#endregion ////////////////////////////////////

		#region initialize
		void InitializeUI()
		{
			UIUtility.ShowCenterInPrimaryScreen(this);
		}

		void Initialize()
		{
			InitializeUI();
		}
		#endregion ////////////////////////////////////

		#region language
		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			UIUtility.SetDefaultText(this, CommonData.Language);
			AcceptButton = this.commandClose;
			CancelButton = this.commandClose;
			this.commandClose.SetLanguage(CommonData.Language);

			var controls = new Control[] {
				this.commandLauncher,
				this.labelLauncher,
				this.commandNotify,
				this.labelNotify,
				this.commandStartup,
				this.labelStartup,
			};
			foreach(var control in controls) {
				control.SetLanguage(CommonData.Language);
			}

		}
		#endregion ////////////////////////////////////

		#region skin
		protected override void ApplySkin()
		{
			base.ApplySkin();

			this.commandLauncher.Image = CommonData.Skin.GetImage(SkinImage.Finder);
			this.commandNotify.Image = CommonData.Skin.GetImage(SkinImage.Flag);
			this.commandStartup.Image = CommonData.Skin.GetImage(SkinImage.Windows);
		}
		#endregion ////////////////////////////////////

		#region function
		protected override void ApplySetting()
		{
			base.ApplySetting();
		}

		IEnumerable<string> SplitLauncherItemText(string text)
		{
			return text.SplitLines().Select(s => s.Trim()).Where(s => !s.StartsWith("#", StringComparison.Ordinal));
		}

		/// <summary>
		/// なっがいなこれ。
		/// 
		/// TODO: default-launcher.xml の定義が必要
		/// </summary>
		void MakeDefaultLauncherItem()
		{
			var path = Literal.ApplicationDefaultLauncherItemPath;
			var srcBuffer = File.ReadAllText(path);
			var lngBuffer = CommonData.Language.ReplaceAllLanguage(srcBuffer);
			var xml = XElement.Parse(lngBuffer);

			// アイテム取得
			var defaultItemList = new List<LauncherItem>();
			var itemElements = xml.Elements("items").Elements("LauncherItem");
			foreach(var itemElement in itemElements) {
				var item = Serializer.LoadXMLString<LauncherItem>(itemElement.ToString());
				var isAdd = false;
				// データの補正
				if(item.LauncherType.IsIn(LauncherType.File, LauncherType.Directory)) {
					// コマンド修正
					foreach(var command in SplitLauncherItemText(item.Command)) {
						var expandPath = Environment.ExpandEnvironmentVariables(command);
						if(item.LauncherType == LauncherType.File) {
							if(File.Exists(expandPath)) {
								item.Command = command;
								isAdd = true;
								break;
							}
						} else {
							Debug.Assert(item.LauncherType == LauncherType.Directory);
							if(Directory.Exists(expandPath)) {
								item.Command = command;
								isAdd = true;
								break;
							}
						}
					}
					// アイコン修正
					foreach(var iconPath in SplitLauncherItemText(item.IconItem.Path)) {
						var expandPath = Environment.ExpandEnvironmentVariables(iconPath);
						if(File.Exists(expandPath)) {
							item.IconItem.Path = iconPath;
						}
					}
				}

				if(isAdd) {
					defaultItemList.Add(item);
				}
			}

			// ユーザーのシステムから取得
			var allUsersFiles =
				Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu))
				.Concat(Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms)))
				;
			var nowUserFiles =
				Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.StartMenu))
				.Concat(Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Programs)))
				;
			var mergeFiles = nowUserFiles
				.Concat(allUsersFiles)
				.Where(s => PathUtility.IsShortcutPath(s) || PathUtility.IsExecutePath(s))
				.Distinct()
				;

			var userItemList = new List<LauncherItem>(mergeFiles.Count());
			foreach(var mergeFile in mergeFiles) {
				try {
					var item = LauncherItemUtility.LoadFile(mergeFile, false);
					userItemList.Add(item);
				} catch(Exception ex) {
					// #68 暫定回避
					this._logList.Add(new LogItem(LogType.Warning, ex.Message, new ExceptionMessage(mergeFile, ex)));
				}
			}

			// アイテムマージ
			// TODO: マージに何も考えてない
			var mergedItemList = defaultItemList.Union(userItemList).ToArray();

			// グループ作成
			var toolbarGroupItemList = new List<ToolbarGroupItem>();
			var groupElements = xml.Elements("groups").Elements("group");
			foreach(var groupElement in groupElements) {
				var groupName = groupElement.Attribute("Name").Value;
				var toolbarGroupItem = new ToolbarGroupItem();
				toolbarGroupItem.Name = groupName;
				var useGroup = false;
				foreach(var itemName in groupElement.Elements("item").Select(node => node.Attribute("Name").Value)) {
					if(mergedItemList.Any(item => item.Name == itemName)) {
						toolbarGroupItem.ItemNames.Add(itemName);
						useGroup = true;
					}
				}
				if(useGroup) {
					toolbarGroupItemList.Add(toolbarGroupItem);
				}
			}
			// ユーザーのシステムからグループ構築
			if(userItemList.Any()) {
				var toolbarGroupItem = new ToolbarGroupItem();
				toolbarGroupItem.Name = CommonData.Language["default/group/programs"];
				foreach(var name in userItemList.Select(item => item.Name)) {
					toolbarGroupItem.ItemNames.Add(name);
				}
				toolbarGroupItemList.Insert(0, toolbarGroupItem);
			}

			// 登録処理をマージで実行
			// アイテムマージ, TODO: Linqでできそう
			foreach(var item in mergedItemList) {
				if(CommonData.MainSetting.Launcher.Items.Any(i => i.IsNameEqual(item.Name))) {
					continue;
				}
				CommonData.MainSetting.Launcher.Items.Add(item);
			}
			// グループマージ
			foreach(var groupItem in toolbarGroupItemList) {
				var aliveGroup = CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.FirstOrDefault(g => g.Name == groupItem.Name);
				if(aliveGroup != null) {
					// マージ
					var unionItems = aliveGroup.ItemNames.Union(groupItem.ItemNames).ToArray();
					aliveGroup.ItemNames.Clear();
					aliveGroup.ItemNames.AddRange(unionItems);
				} else {
					// 追加
					CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.Add(groupItem);
				}
			}

			// 既存グループのうち「新規グループ」に何も格納されていなければ消しちゃう
			var removeGroupItem = CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.SingleOrDefault(g => g.Name == CommonData.Language["new/group-item"]);
			if(removeGroupItem != null && !removeGroupItem.ItemNames.Any()) {
				CommonData.MainSetting.Toolbar.ToolbarGroup.Groups.Remove(removeGroupItem);
			}


			//Serializer.SaveFile(mergeItemList, "Z:\\a.xml");
			//Serializer.SaveFile(toolbarGroupItemList, "Z:\\b.xml");

			ItemFound = true;
		}
		#endregion ////////////////////////////////////

		void CommandNotify_Click(object sender, EventArgs e)
		{
			SystemExecutor.OpenNotificationAreaHistory(CommonData);
		}
		
		void CommandStartup_Click(object sender, EventArgs e)
		{
			var path = Literal.StartupShortcutPath;
			var icon = MessageBoxIcon.Information;
			string message;
			if(!File.Exists(path)) {
				try {
					AppUtility.MakeAppShortcut(path);
					message = CommonData.Language["home/startup/dialog/message"];
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
					message = ex.Message;
					icon = MessageBoxIcon.Error;
				}
			} else {
				message = CommonData.Language["home/startup/exists"];
				CommonData.Logger.Puts(LogType.Information, message, path);
			}
			MessageBox.Show(message, CommonData.Language["home/startup/dialog/caption"], MessageBoxButtons.OK, icon);
		}
		
		void CommandLauncher_Click(object sender, EventArgs e)
		{
			if(CommonData.MainSetting.Launcher.Items.Count != 0) {
				var dialogResult = MessageBox.Show(CommonData.Language["home/launcher/dialog/merge/message"], CommonData.Language["home/launcher/dialog/merge/caption"], MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				if(dialogResult != DialogResult.OK) {
					return;
				}
				AppUtility.SaveSetting(CommonData);
			}
			MakeDefaultLauncherItem();
			if(ItemFound) {
				MessageBox.Show(CommonData.Language["home/launcher/dialog/found/message"], CommonData.Language["home/launcher/dialog/found/caption"], MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
		
		void HomeForm_Shown(object sender, EventArgs e)
		{
			UIUtility.ShowFrontActive(this);
		}
	}
}
