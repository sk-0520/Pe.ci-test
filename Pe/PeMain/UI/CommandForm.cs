namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;
	using System.Text.RegularExpressions;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using System.Diagnostics;
	using System.IO;
	using ContentTypeTextNet.Pe.Library.Skin;

	public partial class CommandForm: CommonForm
	{
		public CommandForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region property

		IReadOnlyList<LauncherItem> LauncherList { get; set; }
		bool CallUpdateEvent { get; set; }

		#endregion

		#region override

		#endregion

		#region CommonForm

		protected override void ApplySetting()
		{
			base.ApplySetting();
			SetLauncherItems();
		}

		#endregion

		#region initialize
		void Initialize()
		{
			CallUpdateEvent = true;
		}
		#endregion

		#region functino

		void SetLauncherItems()
		{
			//LauncherList = CommonData.MainSetting.Launcher.Items.ToList();
			//this.imputCommand
			//this.inputCommand.DataBindings.Add("Items", LauncherList, "");
			LauncherList = CommonData.MainSetting.Launcher.Items.OrderBy(i => i.Name).ToList();
			//this.inputCommand.DataSource = LauncherList;
			ChangeLauncherItems(string.Empty);
		}

		void ChangeLauncherItems(string s)
		{
			IEnumerable<CommandDisplayValue> list = null;
			if(!string.IsNullOrWhiteSpace(s)) {
				var reg = new Regex(TextUtility.RegexPatternToWildcard(s), RegexOptions.IgnoreCase);
				
//				var tempList = LauncherList.Where(i => reg.IsMatch(i.Name) || i.Tag.Any(t => reg.IsMatch(t)));
				var nameList = LauncherList
					.Where(i => reg.IsMatch(i.Name))
					.Select(i => new CommandDisplayValue(i, i.Name, LauncherCommandType.Name))
				;
				var tagList = LauncherList
					.SelectMany(
						(i, index) => i.Tag,
						(i, tag) => new {
							Item = i,
							Tag = tag
						}
					)
					.Where(pair => reg.IsMatch(pair.Tag))
					.Select(pair => new CommandDisplayValue(pair.Item, pair.Tag, LauncherCommandType.Tag))
				;
				list = nameList.Concat(tagList);
			}
			if(list == null || !list.Any()) {
				list = LauncherList.Select(i => new CommandDisplayValue(i, i.Name, LauncherCommandType.Name));
			}

			var nowCommandValue = new CommandDisplayValue(new LauncherItem(), s, LauncherCommandType.None);
			if(!string.IsNullOrWhiteSpace(s)) {
				nowCommandValue = list.FirstOrDefault(i => i.Display.StartsWith(s, StringComparison.OrdinalIgnoreCase)) ?? nowCommandValue;
			}
			if(nowCommandValue.LauncherCommandType == LauncherCommandType.None) {
				list = new[] { nowCommandValue }.Concat(list);
			}
			try {
				this.inputCommand.TextUpdate -= inputCommand_TextUpdate;

				this.inputCommand.Attachment(list, nowCommandValue.Value);

				//this.inputCommand.SelectionStart = ;
				this.inputCommand.Select(s.Length, nowCommandValue.Display.Length);
				ChangeIcon();
			} finally {
				this.inputCommand.TextUpdate += inputCommand_TextUpdate;
			}
		}

		CommandKind GetCommandKindFromText(string s)
		{
			if(string.IsNullOrWhiteSpace(s)) {
				return CommandKind.None;
			}
			var uri = new[] {
				"http://",
				"https://",
			};
			if(uri.Any(t => s.StartsWith(t))) {
				return CommandKind.Uri;
			}

			return CommandKind.FilePath;
		}

		CommandKind GetCommandKind()
		{
			var dv = this.inputCommand.SelectedItem as CommandDisplayValue;
			if(dv != null && dv.LauncherCommandType != LauncherCommandType.None) {
				return CommandKind.LauncherItem;
			} else {
				return GetCommandKindFromText(this.inputCommand.Text);
			}
		}

		void ChangeIcon()
		{
			var oldImage = this.imageIcon.Image;
			this.imageIcon.Image = null;
			oldImage.ToDispose();

			var kind = GetCommandKind();
			switch(kind) {
				case CommandKind.LauncherItem:
					{
						var item = this.inputCommand.SelectedValue as LauncherItem;
						var icon = item.GetIcon(IconScale.Normal, item.IconItem.Index, CommonData.ApplicationSetting, CommonData.Logger);
						this.imageIcon.Image = IconUtility.ImageFromIcon(icon, IconScale.Normal);
					}
					break;

				case CommandKind.FilePath: 
					{
						var path = Environment.ExpandEnvironmentVariables(this.inputCommand.Text);
						if(FileUtility.Exists(path)) {
							this.imageIcon.Image = IconUtility.GetThumbnailImage(path, IconScale.Normal);
						}
					}
					break;

				case CommandKind.Uri:
					{
						this.imageIcon.Image = (Image)CommonData.Skin.GetImage(SkinImage.Web).Clone();
					}
					break;

				case CommandKind.None:
					break;

				default:
					throw new NotImplementedException();
			}
		}

		public void SetCurrentLocation()
		{
			Location = Cursor.Position;
		}

		void ExecuteCommand()
		{}

		#endregion

		private void CommandForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
			}
		}

		private void commandExecute_Click(object sender, EventArgs e)
		{
			ExecuteCommand();
		}

		private void inputCommand_TextUpdate(object sender, EventArgs e)
		{
			if(CallUpdateEvent) {
				ChangeLauncherItems(this.inputCommand.Text);
				if(!this.inputCommand.DroppedDown) {
				}
			}
		}

		private void inputCommand_KeyDown(object sender, KeyEventArgs e)
		{
			var noUpdateKeys = new[] {
				Keys.Delete,
				Keys.Back,
			};
			if(noUpdateKeys.Any(k => k == e.KeyCode)) {
				CallUpdateEvent = false;
			}
		}

		private void inputCommand_KeyUp(object sender, KeyEventArgs e)
		{
			if(!CallUpdateEvent) {
				CallUpdateEvent = true;
				if(this.inputCommand.Text.Length == 0) {
					ChangeLauncherItems(string.Empty);
				} else {
					ChangeIcon();
				}
			}
		}

	}
}
