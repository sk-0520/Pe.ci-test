using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	public partial class ClipboardForm: Form, ISetCommonData
	{
		#region Define

		const string imageText = "image_text";
		const string imageRtf = "image_rtf";
		const string imageImage = "image_image";
		const string imageFile = "image_file";

		#endregion

		#region Variable

		FlowLayoutPanel _panelClipboradItem = new FlowLayoutPanel();
		Button _commandText = new Button();
		Button _commandRtf = new Button();
		Button _commandImage = new Button();
		Button _commandFile = new Button();

		#endregion

		public ClipboardForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		CommonData CommonData { get; set; }
		int HoverItemIndex { get; set; }
		int SelectedItemIndex { get; set; }

		#endregion

		#region Initialize
		
		void InitializeCommand()
		{
			var commandButtons = new[] {
				this._commandText,
				this._commandRtf,
				this._commandImage,
				this._commandFile,
			};
			this._commandText.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardText;
			this._commandRtf.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardRichTextFormat;
			this._commandImage.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardImage;
			this._commandFile.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardFile;
			var buttonSize = GetButtonSize();

			foreach(var command in commandButtons) {
				command.Size = buttonSize;
				//command.FlatStyle = FlatStyle.Flat;
				command.Margin = Padding.Empty;
			}
			this._panelClipboradItem.Padding = Padding.Empty;
			this._panelClipboradItem.Margin = Padding.Empty;
			this._panelClipboradItem.BackColor = Color.Transparent;
			this._panelClipboradItem.BackColor = Color.FromArgb(25, Color.Black);
			this._panelClipboradItem.Size = Size.Empty;
			this._panelClipboradItem.AutoSize = true;
			this._panelClipboradItem.Controls.AddRange(commandButtons);
			this.listClipboard.Controls.Add(this._panelClipboradItem);
			this._panelClipboradItem.Visible = false;
		}

		void InitializeUI()
		{
			InitializeCommand();

			this.tabPreview_pageText.ImageKey = imageText;
			this.tabPreview_pageRtf.ImageKey = imageRtf;
			this.tabPreview_pageImage.ImageKey = imageImage;
			this.tabPreview_pageFile.ImageKey = imageFile;

			ChangeSelsectedItem(-1);
		}

		void Initialize()
		{
			this.imageTab.Images.Add(imageText, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardText);
			this.imageTab.Images.Add(imageRtf, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardRichTextFormat);
			this.imageTab.Images.Add(imageImage, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardImage);
			this.imageTab.Images.Add(imageFile, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardFile);

			InitializeUI();
		}

		#endregion

		#region Language

		private void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);

			this.toolClipboard_itemTopmost.SetLanguage(CommonData.Language);
			this.toolClipboard_itemSave.SetLanguage(CommonData.Language);
			this.toolClipboard_itemClear.SetLanguage(CommonData.Language);
			this.toolClipboard_itemEmpty.SetLanguage(CommonData.Language);
			this.toolClipboard_itemType_itemClipboard.SetLanguage(CommonData.Language);
			this.toolClipboard_itemType_itemTemplate.SetLanguage(CommonData.Language);
			
			this.tabPreview_pageText.Text = ClipboardType.Text.ToText(CommonData.Language);
			this.tabPreview_pageRtf.Text = ClipboardType.Rtf.ToText(CommonData.Language);
			this.tabPreview_pageImage.Text = ClipboardType.Image.ToText(CommonData.Language);
			this.tabPreview_pageFile.Text = ClipboardType.File.ToText(CommonData.Language);
		}
		
		#endregion

		#region Function

		Size GetButtonSize()
		{
			return new Size(
				IconScale.Small.ToWidth() + NativeMethods.GetSystemMetrics(SM.SM_CXEDGE) * 4,
				IconScale.Small.ToHeight() + NativeMethods.GetSystemMetrics(SM.SM_CYEDGE) * 4
			);
		}
		
		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}

		void ApplySettingUI()
		{
			this.listClipboard.DataSource = this.CommonData.MainSetting.Clipboard.Items;
			CommonData.MainSetting.Clipboard.Items.ListChanged += Items_ListChanged;

			Location = CommonData.MainSetting.Clipboard.Location;
			Size = CommonData.MainSetting.Clipboard.Size;
			ChangeTopmost(CommonData.MainSetting.Clipboard.TopMost);
			var buttonSize = GetButtonSize();
			using(var g = CreateGraphics()) {
				var fontHeight = (int)g.MeasureString("☃", Font).Height;
				var buttonHeight = buttonSize.Height;
				this.listClipboard.ItemHeight = fontHeight + buttonHeight;
			}
			this.viewText.Font = this.CommonData.MainSetting.Clipboard.TextFont.Font;
			Visible = CommonData.MainSetting.Clipboard.Visible;
		}

		/// <summary>
		/// BUGS: Formsバインドで描画が変になる。
		/// </summary>
		void ApplySetting()
		{
			ApplyLanguage();
			ApplySettingUI();

			ChangeSelectType(this.toolClipboard_itemType_itemClipboard);

			ChangeListItemNumber(this.listClipboard.SelectedIndex, this.listClipboard.Items.Count);
		}

		void ChangeTopmost(bool topMost)
		{
			CommonData.MainSetting.Clipboard.TopMost = topMost;
			this.toolClipboard_itemTopmost.Checked = topMost;
			TopMost = topMost;
		}

		void ChangeSelectType(ToolStripItem item)
		{
			this.toolClipboard_itemType.Text = item.Text;
			this.toolClipboard_itemType.Image = item.Image;
		}

		void ChangeListItemNumber(int index, int count)
		{
			if(index == -1) {
				this.statusClipboard_itemSelectedIndex.Text = "-";
			} else {
				this.statusClipboard_itemSelectedIndex.Text = (index + 1).ToString();
			}

			this.statusClipboard_itemCount.Text = count.ToString();
		}

		void ChangeCommand(int index)
		{
			if(index != -1 && HoverItemIndex != index) {
				var clipboardItem = CommonData.MainSetting.Clipboard.Items[index];
				var map = new Dictionary<ClipboardType, Control>() {
					{ ClipboardType.Text, this._commandText },
					{ ClipboardType.Rtf, this._commandRtf },
					{ ClipboardType.Image, this._commandImage },
					{ ClipboardType.File, this._commandFile },
				};
				foreach(var pair in map.ToArray()) {
					pair.Value.Enabled = (clipboardItem.ClipboardTypes.HasFlag(pair.Key));
				}
			}

			HoverItemIndex = index;
			this._panelClipboradItem.Visible = HoverItemIndex != -1;
		}

		void ChangeSelsectedItem(int index)
		{
			SelectedItemIndex = index;
			this.tabPreview.Enabled = SelectedItemIndex != -1;

			if(SelectedItemIndex == -1) {
				return;
			}

			var map = new Dictionary<ClipboardType, TabPage>() {
				{ ClipboardType.Text, this.tabPreview_pageText },
				{ ClipboardType.Rtf, this.tabPreview_pageRtf },
				{ ClipboardType.Image, this.tabPreview_pageImage },
				{ ClipboardType.File, this.tabPreview_pageFile },
			};
			var clipboardItem = CommonData.MainSetting.Clipboard.Items[SelectedItemIndex];
			this.tabPreview.SelectedTab = map[clipboardItem.GetSingleClipboardType()];

			foreach(var type in clipboardItem.GetClipboardTypeList()) {
				switch(type) {
					case ClipboardType.Text: 
						{
							this.viewText.Text = clipboardItem.Text;
						}
						break;

					case ClipboardType.Rtf:
						{
							this.viewRtf.Rtf = clipboardItem.Rtf;
						}
						break;

					case ClipboardType.Image:
						{
							this.viewImage.Image = clipboardItem.Image;
						}
						break;

					case ClipboardType.File:
						{
							// TODO
						}
						break;

					default:
						throw new NotImplementedException();
				}
			}
		}

		#endregion

		private void toolClipboard_itemType_itemClipboard_Click(object sender, EventArgs e)
		{
			ChangeSelectType((ToolStripItem)sender);
		}

		void Items_ListChanged(object sender, EventArgs e)
		{
			this.listClipboard.SuspendLayout();
			var selectedIndex = this.listClipboard.SelectedIndex;
			this.listClipboard.DataSource = null;
			this.listClipboard.DataSource = this.CommonData.MainSetting.Clipboard.Items;
			if(selectedIndex + 1 < this.listClipboard.Items.Count) {
				this.listClipboard.SelectedIndex = selectedIndex + 1;
			}
			//ChangeListItemNumber(this.listClipboard.SelectedIndex, this.listClipboard.Items.Count);
			this.listClipboard.ResumeLayout();
		}

		private void listClipboard_DrawItem(object sender, DrawItemEventArgs e)
		{
			if(e.Index != -1) {
				var item = CommonData.MainSetting.Clipboard.Items[e.Index];

				e.DrawBackground();
				using(var brush = new SolidBrush(e.ForeColor)) {
					var displayText = LanguageUtility.ClipboardItemToDisplayText(CommonData.Language, item);
					e.Graphics.DrawString(displayText, Font, brush, e.Bounds.Location);
				}
				e.DrawFocusRectangle();
			}
		}

		private void listClipboard_SelectedIndexChanged(object sender, EventArgs e)
		{
			ChangeListItemNumber(this.listClipboard.SelectedIndex, this.listClipboard.Items.Count);
			ChangeSelsectedItem(this.listClipboard.SelectedIndex);
		}

		private void ClipboardForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if(e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Visible = false;
				CommonData.MainSetting.Clipboard.Visible = false;
			}
		}

		private void ClipboardForm_LocationChanged(object sender, EventArgs e)
		{
			CommonData.MainSetting.Clipboard.Location = Location;
		}

		private void ClipboardForm_SizeChanged(object sender, EventArgs e)
		{
			CommonData.MainSetting.Clipboard.Size = Size;
		}

		private void toolClipboard_itemTopmost_Click(object sender, EventArgs e)
		{
			var check = !toolClipboard_itemTopmost.Checked;
			ChangeTopmost(check);
		}

		private void listClipboard_MouseMove(object sender, MouseEventArgs e)
		{
			var index = this.listClipboard.IndexFromPoint(e.Location) - this.listClipboard.TopIndex;
			var top = this.listClipboard.ItemHeight * (index + 1) - GetButtonSize().Height;
			if(top != this._panelClipboradItem.Top) {
				this._panelClipboradItem.Top = top;
			}
			ChangeCommand(index);
		}

		private void tabPreview_Selecting(object sender, TabControlCancelEventArgs e)
		{
			Debug.Assert(SelectedItemIndex != -1);

			var clipboardItem = CommonData.MainSetting.Clipboard.Items[SelectedItemIndex];
			var typeList = clipboardItem.GetClipboardTypeList();
			var list = new[] {
				new { TabPage = this.tabPreview_pageText, ClipboardType = ClipboardType.Text },
				new { TabPage = this.tabPreview_pageRtf, ClipboardType = ClipboardType.Rtf },
				new { TabPage = this.tabPreview_pageImage, ClipboardType = ClipboardType.Image },
				new { TabPage = this.tabPreview_pageFile, ClipboardType = ClipboardType.File },
			};
			foreach(var item in list) {
				if(e.TabPage == item.TabPage && typeList.Any(t => t.HasFlag(item.ClipboardType))) {
					return;
				}
			}

			e.Cancel = true;
		}
	}
}
