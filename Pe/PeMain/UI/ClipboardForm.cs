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
		#endregion

		public ClipboardForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		CommonData CommonData { get; set; }

		#endregion

		#region Initialize
		
		void InitializeUI()
		{
			this.tabPreview_pageText.ImageKey = imageText;
			this.tabPreview_pageRichTextFormat.ImageKey = imageRtf;
			this.tabPreview_pageImage.ImageKey = imageImage;
			this.tabPreview_pageFile.ImageKey = imageFile;
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
			this.toolClipboard_itemType_itemClipboard.SetLanguage(CommonData.Language);
			this.toolClipboard_itemType_itemTemplate.SetLanguage(CommonData.Language);

			this.tabPreview_pageText.Text = ClipboardType.Text.ToText(CommonData.Language);
			this.tabPreview_pageRichTextFormat.Text = ClipboardType.RichTextFormat.ToText(CommonData.Language);
			this.tabPreview_pageImage.Text = ClipboardType.Image.ToText(CommonData.Language);
			this.tabPreview_pageFile.Text = ClipboardType.File.ToText(CommonData.Language);
		}
		
		#endregion

		#region Function

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}

		void ApplySettingUI()
		{
			this.listClipboard.DataSource = this.CommonData.MainSetting.Clipboard.Items;
			CommonData.MainSetting.Clipboard.Items.ListChanged += Items_ListChanged;

			Visible = CommonData.MainSetting.Clipboard.Visible;
			Location = CommonData.MainSetting.Clipboard.Location;
			Size = CommonData.MainSetting.Clipboard.Size;

			using(var g = CreateGraphics()) {
				var fontHeight = (int)g.MeasureString("☃", this.CommonData.MainSetting.Clipboard.TextFont.Font).Height;
				var buttonHeight = IconScale.Small.ToHeight();
				this.listClipboard.ItemHeight = fontHeight + buttonHeight;
			}
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
					e.Graphics.DrawString(item.ToString(), CommonData.MainSetting.Clipboard.TextFont.Font, brush, e.Bounds.Location);
				}
				e.DrawFocusRectangle();
			}
		}

		private void listClipboard_SelectedIndexChanged(object sender, EventArgs e)
		{
			ChangeListItemNumber(this.listClipboard.SelectedIndex, this.listClipboard.Items.Count);
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
	}
}
