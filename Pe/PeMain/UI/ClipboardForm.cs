using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.PeMain.UI;
using ContentTypeTextNet.Pe.PeMain.UI.Ex;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	public partial class ClipboardForm: Form, ISetCommonData
	{
		#region Define

		const string imageText = "image_text";
		const string imageRtf = "image_rtf";
		const string imageHtml = "image_html";
		const string imageImage = "image_image";
		const string imageFile = "image_file";

		class ClipboardWebBrowser: ShowWebBrowser
		{
			public ClipboardWebBrowser()
				: base()
			{ }
		}

		#endregion ////////////////////////////////////////

		#region Variable

		FlowLayoutPanel _panelClipboradItem = new FlowLayoutPanel();
		Button _commandText = new Button();
		Button _commandRtf = new Button();
		Button _commandHtml = new Button();
		Button _commandImage = new Button();
		Button _commandFile = new Button();

		#endregion ////////////////////////////////////////

		public ClipboardForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		CommonData CommonData { get; set; }
		int HoverItemIndex { get; set; }
		int SelectedItemIndex { get; set; }

		#endregion ////////////////////////////////////////

		#region Initialize

		void InitializeCommand()
		{
			var commandButtons = GetButtonList().ToArray();
			this._commandText.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardText;
			this._commandRtf.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardRichTextFormat;
			this._commandHtml.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardHtml;
			this._commandImage.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardImage;
			this._commandFile.Image = global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardFile;
			var buttonSize = GetButtonSize();

			foreach(var command in commandButtons) {
				command.Size = buttonSize;
				//command.FlatStyle = FlatStyle.Flat;
				command.Margin = Padding.Empty;
				command.Click += command_Click;
			}
			this._panelClipboradItem.Padding = Padding.Empty;
			this._panelClipboradItem.Margin = Padding.Empty;
			//this._panelClipboradItem.BackColor = Color.Transparent;
			//this._panelClipboradItem.BackColor = Color.FromArgb(50, Color.Black);
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
			this.tabPreview_pageHtml.ImageKey = imageHtml;
			this.tabPreview_pageImage.ImageKey = imageImage;
			this.tabPreview_pageFile.ImageKey = imageFile;

			ChangeCommand(-1);
			ChangeSelsectedItem(-1);
			WebBrowserUtility.AttachmentNewWindow(this.viewHtml);
		}

		void Initialize()
		{
			SelectedItemIndex = -1;
			HoverItemIndex = -1;

			this.imageTab.Images.Add(imageText, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardText);
			this.imageTab.Images.Add(imageRtf, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardRichTextFormat);
			this.imageTab.Images.Add(imageHtml, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardHtml);
			this.imageTab.Images.Add(imageImage, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardImage);
			this.imageTab.Images.Add(imageFile, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardFile);

			InitializeUI();
		}

		#endregion ////////////////////////////////////////

		#region Language

		private void ApplyLanguage()
		{
			UIUtility.SetDefaultText(this, CommonData.Language);

			this.toolClipboard_itemTopmost.SetLanguage(CommonData.Language);
			this.toolClipboard_itemSave.SetLanguage(CommonData.Language);
			this.toolClipboard_itemRemove.SetLanguage(CommonData.Language);
			this.toolClipboard_itemClear.SetLanguage(CommonData.Language);
			this.toolClipboard_itemEmpty.SetLanguage(CommonData.Language);
			this.toolClipboard_itemType_itemClipboard.SetLanguage(CommonData.Language);
			this.toolClipboard_itemType_itemTemplate.SetLanguage(CommonData.Language);

			this.columnName.SetLanguage(CommonData.Language);
			this.columnPath.SetLanguage(CommonData.Language);

			this.tabPreview_pageText.Text = ClipboardType.Text.ToText(CommonData.Language);
			this.tabPreview_pageRtf.Text = ClipboardType.Rtf.ToText(CommonData.Language);
			this.tabPreview_pageHtml.Text = ClipboardType.Html.ToText(CommonData.Language);
			this.tabPreview_pageImage.Text = ClipboardType.Image.ToText(CommonData.Language);
			this.tabPreview_pageFile.Text = ClipboardType.File.ToText(CommonData.Language);

		}

		#endregion ////////////////////////////////////////

		#region Function

		/// <summary>
		/// ボタン一覧
		/// </summary>
		/// <returns></returns>
		IEnumerable<Button> GetButtonList()
		{
			return new[] {
				this._commandText,
				this._commandRtf,
				this._commandHtml,
				this._commandImage,
				this._commandFile,
			};
		}

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
			this.statusClipboard_itemLimit.Text = CommonData.MainSetting.Clipboard.Limit.ToString();
		}

		void ChangeCommand(int index)
		{
			//if((index != -1 && HoverItemIndex != index) || (index != -1 && HoverItemIndex == -1)) {
			if((index > -1 && HoverItemIndex != index)) {
				var clipboardItem = CommonData.MainSetting.Clipboard.Items[index];
				var map = new Dictionary<ClipboardType, Control>() {
					{ ClipboardType.Text, this._commandText },
					{ ClipboardType.Rtf, this._commandRtf },
					{ ClipboardType.Html, this._commandHtml },
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
			//SelectedItemIndex = index;
			this.tabPreview.Enabled = index != -1;

			if(index == -1) {
				return;
			}

			var map = new Dictionary<ClipboardType, TabPage>() {
				{ ClipboardType.Text, this.tabPreview_pageText },
				{ ClipboardType.Rtf, this.tabPreview_pageRtf },
				{ ClipboardType.Html, this.tabPreview_pageHtml},
				{ ClipboardType.Image, this.tabPreview_pageImage },
				{ ClipboardType.File, this.tabPreview_pageFile },
			};

			this.tabPreview.SuspendLayout();
			this.tabPreview.TabPages.Clear();

			var clipboardItem = CommonData.MainSetting.Clipboard.Items[index];

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

					case ClipboardType.Html:
						{
							string html;
							var result = ClipboardUtility.TryConvertHtmlFromClipbordHtml(clipboardItem.Html, out html);
							if(result) {
								this.viewHtml.DocumentText = html;
							} else {
								var elements = string.Format("<p style='font-weight: bold; color: #f00; background: #fff'>{0}</p><hr />", CommonData.Language["clipboard/html/error"]);
								this.viewHtml.DocumentText = elements + clipboardItem.Html;
							}
						}
						break;

					case ClipboardType.Image:
						{
							this.viewImage.Image = clipboardItem.Image;
						}
						break;

					case ClipboardType.File:
						{
							var imageList = new ImageList();
							imageList.ColorDepth = ColorDepth.Depth32Bit;
							imageList.ImageSize = IconScale.Small.ToSize();
							var listItemList = new List<ListViewItem>(clipboardItem.Files.Count());
							var showExtentions = SystemEnvironment.IsExtensionShow();
							Func<string,string> getName;
							if(showExtentions) getName = Path.GetFileName; else getName = Path.GetFileNameWithoutExtension;
							foreach(var path in clipboardItem.Files) {
								var key = path.GetHashCode().ToString();
								var name = getName(path);

								var icon = IconUtility.Load(path, IconScale.Small, 0);
								imageList.Images.Add(key, icon);

								var listItem = new ListViewItem();
								listItem.Text = name;
								listItem.ImageKey = key;

								listItem.SubItems.Add(path);

								listItemList.Add(listItem);
							}
							this.viewFile.Items.Clear();
							this.viewFile.SmallImageList.ToDispose();
							this.viewFile.SmallImageList = imageList;
							this.viewFile.Items.AddRange(listItemList.ToArray());

							this.viewFile.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
						}
						break;

					default:
						throw new NotImplementedException();
				}
				this.tabPreview.TabPages.Add(map[type]);
			}
			this.tabPreview.SelectedTab = map[clipboardItem.GetSingleClipboardType()];
			this.tabPreview.ResumeLayout();
		}

		void CopyItem(ClipboardItem clipboardItem, ClipboardType clipboardType)
		{
			var map = new Dictionary<ClipboardType, Action<CommonData>>() {
				{ ClipboardType.Text, (setting) => {
					ClipboardUtility.CopyText(clipboardItem.Text, setting);
				} },
				{ ClipboardType.Rtf, (setting) => {
					ClipboardUtility.CopyRtf(clipboardItem.Rtf, setting);
				} },
				{ ClipboardType.Html, (setting) => {
					ClipboardUtility.CopyHtml(clipboardItem.Html, setting);
				} },
				{ ClipboardType.Image, (setting) => {
					ClipboardUtility.CopyImage(clipboardItem.Image, setting);
				} },
				{ ClipboardType.File, (setting) => {
					ClipboardUtility.CopyFile(clipboardItem.Files, setting);
				} },
			};
			map[clipboardType](CommonData);
		}

		void CopySingleItem(int index)
		{
			Debug.Assert(index != -1);
			
			var clipboardItem = CommonData.MainSetting.Clipboard.Items[index];
			CopyItem(clipboardItem, clipboardItem.GetSingleClipboardType());
		}

		bool SaveItem(string path, ClipboardItem clipboardItem, ClipboardType type)
		{
			Debug.Assert(type != ClipboardType.File);

			var map = new Dictionary<ClipboardType, Action>() {
				{ ClipboardType.Text, () => File.WriteAllText(path, clipboardItem.Text) },
				{ ClipboardType.Rtf, () => File.WriteAllText(path, clipboardItem.Rtf) },
				{ ClipboardType.Html, () => File.WriteAllText(path, clipboardItem.Html) },
				{ ClipboardType.Image, () => clipboardItem.Image.Save(path, ImageFormat.Png) },
			};

			try {
				map[type]();
				return true;
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, clipboardItem.Name, ex);
				return false;
			}
		}

		void OpenSaveDialog(ClipboardItem clipboardItem)
		{
			var filter = new DialogFilter();
			var map = new[] {
				new { ClipboardType = ClipboardType.Text, Wildcard = new [] {"*.txt"} },
				new { ClipboardType = ClipboardType.Rtf, Wildcard = new [] {"*.rtf"} },
				new { ClipboardType = ClipboardType.Html, Wildcard = new [] {"*.html"} },
				new { ClipboardType = ClipboardType.Image, Wildcard = new [] {"*.png"} },
			}.Select(v => new {
				ClipboardType = v.ClipboardType,
				DisplayText = v.ClipboardType.ToText(CommonData.Language),
				Wildcard = v.Wildcard,
			}).ToDictionary(k => k.ClipboardType, v => new DialogFilterValueItem<ClipboardType>(v.ClipboardType, v.DisplayText, v.Wildcard));

			var defType = clipboardItem.GetSingleClipboardType();
			var defIndex = 0;
			var tempIndex = 0;
			foreach(var type in clipboardItem.GetClipboardTypeList().Where(t => t != ClipboardType.File)) {
				filter.Items.Add(map[type]);
				tempIndex += 1;
				if(defType == type) {
					defIndex = tempIndex;
				}
			}

			using(var dialog = new SaveFileDialog()) {
				filter.Attachment(dialog);
				dialog.FileName = clipboardItem.Timestamp.ToString(Literal.timestampFileName);
				dialog.FilterIndex = defIndex;
				if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					var item = (DialogFilterValueItem<ClipboardType>)filter.Items[dialog.FilterIndex - 1];
					var path = dialog.FileName;
					var type = item.Value;
					SaveItem(path, clipboardItem, type);
				}
			}
		}

		public void ClearEvent()
		{
			CommonData.MainSetting.Clipboard.Items.ListChanged -= Items_ListChanged;
		}

		#endregion ////////////////////////////////////////

		private void toolClipboard_itemType_itemClipboard_Click(object sender, EventArgs e)
		{
			ChangeSelectType((ToolStripItem)sender);
		}

		void Items_ListChanged(object sender, EventArgs e)
		{
			this.listClipboard.SuspendLayout();
			var isActive = Form.ActiveForm == this;
			var selectedIndex = this.listClipboard.SelectedIndex;
			this.listClipboard.DataSource = null;
			if(CommonData.MainSetting.Clipboard.Items.Count == 0) {
				this.viewText.ResetText();
				this.viewRtf.ResetText();
				this.viewHtml.DocumentText = null;
				this.viewImage.Image = null;
				this.viewFile.Items.Clear();
			}
			this.listClipboard.DataSource = this.CommonData.MainSetting.Clipboard.Items;
			if(isActive) {
				if(selectedIndex + 1 < this.listClipboard.Items.Count) {
					this.listClipboard.SelectedIndex = selectedIndex + 1;
				}
			} else if(this.CommonData.MainSetting.Clipboard.Items.Any()) {
				this.listClipboard.SelectedIndex = 0;
			}
			this._panelClipboradItem.Visible = false;
			ChangeCommand(-1);
			this.listClipboard.ResumeLayout();
		}

		private void listClipboard_DrawItem(object sender, DrawItemEventArgs e)
		{
			if(e.Index != -1) {
				var item = CommonData.MainSetting.Clipboard.Items[e.Index];

				e.DrawBackground();

				var map = new Dictionary<ClipboardType, string>() {
					{ ClipboardType.Text, imageText},
					{ ClipboardType.Rtf, imageRtf},
					{ ClipboardType.Html, imageHtml},
					{ ClipboardType.Image, imageImage},
					{ ClipboardType.File, imageFile},
				};
				var image = this.imageTab.Images[map[item.GetSingleClipboardType()]];

				var drawArea = new Rectangle(e.Bounds.X + this.listClipboard.Margin.Left, e.Bounds.Bottom - image.Height - +this.listClipboard.Margin.Bottom - 1, image.Width, image.Height);
				
				e.Graphics.DrawImage(image, drawArea);
				
				using(var sf = new StringFormat())
				using(var brush = new SolidBrush(e.ForeColor)) {
					sf.Alignment = StringAlignment.Near;
					sf.LineAlignment = StringAlignment.Near;
					sf.Trimming = StringTrimming.EllipsisCharacter;
					sf.FormatFlags = StringFormatFlags.NoWrap;
					e.Graphics.DrawString(item.Name, Font, brush, e.Bounds, sf);

					sf.Alignment = StringAlignment.Far;
					sf.LineAlignment = StringAlignment.Far;
					e.Graphics.DrawString(item.Timestamp.ToString(), SystemFonts.SmallCaptionFont, brush, e.Bounds, sf);
				}
				using(var pen = new Pen(Color.FromArgb(128, e.ForeColor))) {
					var bottom = e.Bounds.Bottom - 1;
					pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
					e.Graphics.DrawLine(pen, e.Bounds.X, bottom, e.Bounds.Right - 1, bottom);
				}
				e.DrawFocusRectangle();
			}
		}

		private void listClipboard_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Debug.WriteLine(this.listClipboard.SelectedIndex.ToString());
			//Debug.WriteLine(ActiveControl);
			//var isActive = ActiveControl == this.listClipboard;
			var index = this.listClipboard.SelectedIndex;
			if(index != SelectedItemIndex) {
				SelectedItemIndex = index;
				ChangeListItemNumber(this.listClipboard.SelectedIndex, this.listClipboard.Items.Count);
				ChangeSelsectedItem(this.listClipboard.SelectedIndex);
				if(Form.ActiveForm == this) {
					ActiveControl = this.listClipboard;
				}
				//	this.listClipboard.Select();
			}
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
			var top = this.listClipboard.ItemHeight * (index + 1) - GetButtonSize().Height - 1;
			if(top != this._panelClipboradItem.Top) {
				this._panelClipboradItem.Top = top;
			}
			ChangeCommand(index);
		}

		private void tabPreview_Selecting(object sender, TabControlCancelEventArgs e)
		{
			//Debug.Assert(SelectedItemIndex != -1);
			var index = this.listClipboard.SelectedIndex;
			Debug.Assert(index != -1);

			var clipboardItem = CommonData.MainSetting.Clipboard.Items[index];
			var typeList = clipboardItem.GetClipboardTypeList();
			var list = new[] {
				new { TabPage = this.tabPreview_pageText, ClipboardType = ClipboardType.Text },
				new { TabPage = this.tabPreview_pageRtf, ClipboardType = ClipboardType.Rtf },
				new { TabPage = this.tabPreview_pageHtml, ClipboardType = ClipboardType.Html },
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

		void command_Click(object sender, EventArgs e)
		{
			if(0 > HoverItemIndex) {
				return;
			}
			try {
				var clipboardItem = CommonData.MainSetting.Clipboard.Items[HoverItemIndex];
				var map = new Dictionary<object, ClipboardType>() {
					{ this._commandText, ClipboardType.Text },
					{ this._commandRtf, ClipboardType.Rtf },
					{ this._commandHtml, ClipboardType.Html },
					{ this._commandImage, ClipboardType.Image },
					{ this._commandFile, ClipboardType.File },
				};
				CopyItem(clipboardItem, map[sender]);
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
			}
		}

		private void listClipboard_DoubleClick(object sender, EventArgs e)
		{
			var index = this.listClipboard.SelectedIndex;
			if(index != -1) {
				try {
					CopySingleItem(index);
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
				}
			}
		}

		private void toolClipboard_itemSave_Click(object sender, EventArgs e)
		{
			var index = this.listClipboard.SelectedIndex;
			if(index != -1) {
				var clipboardItem = CommonData.MainSetting.Clipboard.Items[index];
				OpenSaveDialog(clipboardItem);
			}
		}

		private void toolClipboard_itemClear_ButtonClick(object sender, EventArgs e)
		{
			var index = this.listClipboard.SelectedIndex;
			if(index != -1) {
				CommonData.MainSetting.Clipboard.Items.RemoveAt(index);
			}
		}

		private void toolClipboard_itemClear_Click(object sender, EventArgs e)
		{
			CommonData.MainSetting.Clipboard.Items.Clear();
		}

		private void toolClipboard_itemEmpty_Click(object sender, EventArgs e)
		{
			Clipboard.Clear();
		}

		private void viewHtml_ShowMessage(object sender, ShowMessageEventArgs e)
		{
			e.Result = 0;
			e.Handled = false;
		}
	}
}
