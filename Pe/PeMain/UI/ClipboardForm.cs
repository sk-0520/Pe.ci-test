namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.UI;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	public partial class ClipboardForm: CommonForm
	{
		#region Define

		const string imageText = "image_text";
		const string imageRtf = "image_rtf";
		const string imageHtml = "image_html";
		const string imageImage = "image_image";
		const string imageFile = "image_file";
		const string imageRawTemplate = "image_raw_template";
		const string imageReplaceTemplate = "image_replace_template";

		class ClipboardWebBrowser: ShowWebBrowser
		{
			public ClipboardWebBrowser()
				: base()
			{ }
		}

		enum ImageViewSize
		{
			Fill,
			Raw,
		}

		#endregion ////////////////////////////////////////

		#region Variable

		FlowLayoutPanel _panelClipboradItem = new FlowLayoutPanel();
		Button _commandText = new Button();
		Button _commandRtf = new Button();
		Button _commandHtml = new Button();
		Button _commandImage = new Button();
		Button _commandFile = new Button();

		Button _commandMulti = new Button();

		Button _commandAdd = new Button();
		Button _commandUp = new Button();
		Button _commandDown = new Button();

		IList<ReplaceItem> _replaceCommentList;

		ImageViewSize _imageSize;

		#endregion ////////////////////////////////////////

		public ClipboardForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		//CommonData CommonData { get; set; }
		int HoverItemIndex { get; set; }
		int SelectedItemIndex { get; set; }

		ImageViewSize ImageSize {
			get { return this._imageSize; }
			set {
				this._imageSize = value;
				ChangeImageSize(this._imageSize);
			}
		}

		bool ImageDragging { get; set; }
		Point ImageDragPosition { get; set; }

		#endregion ////////////////////////////////////////

		#region Initialize

		void InitializeCommand()
		{
			var commandButtons = new[] {
				this._commandMulti,
				this._commandText,
				this._commandRtf,
				this._commandHtml,
				this._commandImage,
				this._commandFile,
				this._commandAdd,
				this._commandUp,
				this._commandDown,
			};
			var buttonSize = GetButtonSize();

			foreach(var command in commandButtons) {
				command.Size = buttonSize;
				//command.FlatStyle = FlatStyle.Flat;
				command.Margin = Padding.Empty;
				command.Click += command_Click;
			}
			this._commandMulti.Margin = new Padding(0, 0, NativeMethods.GetSystemMetrics(SM.SM_CXEDGE), 0);
			this._panelClipboradItem.Padding = Padding.Empty;
			this._panelClipboradItem.Margin = Padding.Empty;
			//this._panelClipboradItem.BackColor = Color.Transparent;
			//this._panelClipboradItem.BackColor = Color.FromArgb(50, Color.Black);
			this._panelClipboradItem.Size = Size.Empty;
			this._panelClipboradItem.AutoSize = true;
			//this._panelClipboradItem.Controls.AddRange(commandButtons);
			this.listItemStack.Controls.Add(this._panelClipboradItem);
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
			this.tabPreview_pageRawTemplate.ImageKey = imageRawTemplate;
			this.tabPreview_pageReplaceTemplate.ImageKey = imageReplaceTemplate;
			

			//ChangeCommand(-1);
			//ChangeSelsectedItem(-1);
			WebBrowserUtility.AttachmentNewWindow(this.viewHtml);

			listItemStack.MouseWheel += listClipboard_MouseWheel;

			this._replaceCommentList = TemplateLanguageName.GetMembersList()
				.Concat(AppLanguageName.GetMembersList())
				.Select(m => new ReplaceItem() { Name = m })
				.ToList()
			;
			this.listReplace.DataSource = new BindingList<ReplaceItem>(this._replaceCommentList);

			ChekedReplace();
		}

		void Initialize()
		{
			InitializeUI();
			ImageSize = ImageViewSize.Fill;
		}

		#endregion ////////////////////////////////////////

		#region Language

		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			UIUtility.SetDefaultText(this, CommonData.Language);

			this.toolClipboard_itemEnabled.SetLanguage(CommonData.Language);
			this.toolClipboard_itemTopmost.SetLanguage(CommonData.Language);
			this.toolClipboard_itemSave.SetLanguage(CommonData.Language);
			this.toolClipboard_itemRemove.SetLanguage(CommonData.Language);
			this.toolClipboard_itemClear.SetLanguage(CommonData.Language);
			this.toolClipboard_itemEmpty.SetLanguage(CommonData.Language);

			this.labelTemplateName.SetLanguage(CommonData.Language);
			this.selectTemplateReplace.SetLanguage(CommonData.Language);

			this.tabPreview_pageRawTemplate.SetLanguage(CommonData.Language);
			this.tabPreview_pageReplaceTemplate.SetLanguage(CommonData.Language);

			this.columnName.SetLanguage(CommonData.Language);
			this.columnPath.SetLanguage(CommonData.Language);

			this.labelHtmlUri.SetLanguage(CommonData.Language);

			this.toolImage_itemRaw.SetLanguage(CommonData.Language);
			this.toolImage_itemFill.SetLanguage(CommonData.Language);

			this.toolClipboard_itemType_itemClipboard.Text = ClipboardListType.History.ToText(CommonData.Language);
			this.toolClipboard_itemType_itemTemplate.Text = ClipboardListType.Template.ToText(CommonData.Language);

			this.tabPreview_pageText.Text = ClipboardType.Text.ToText(CommonData.Language);
			this.tabPreview_pageRtf.Text = ClipboardType.Rtf.ToText(CommonData.Language);
			this.tabPreview_pageHtml.Text = ClipboardType.Html.ToText(CommonData.Language);
			this.tabPreview_pageImage.Text = ClipboardType.Image.ToText(CommonData.Language);
			this.tabPreview_pageFile.Text = ClipboardType.File.ToText(CommonData.Language);

			//var templateHtml = File.ReadAllText(Path.Combine(Literal.ApplicationLanguageDirPath, CommonData.Language.TemplateFileName));
			//var acceptMap = new Dictionary<string, string>() {
			//	{"STYLE", File.ReadAllText(Path.Combine(Literal.ApplicationStyleDirPath, "common.css"), Encoding.UTF8) },
			//};
			//var replaced = templateHtml.ReplaceRangeFromDictionary("${", "}", acceptMap);
			//this.webTemplateComment.DocumentText = replaced;
			foreach(var item in this._replaceCommentList) {
				item.SetLanguage(CommonData.Language);
			}
		}

		#endregion ////////////////////////////////////////

		#region skin
		protected override void ApplySkin()
		{
			base.ApplySkin();

			this.toolClipboard_itemEnabled.Image = CommonData.Skin.GetImage(SkinImage.Clipboard);
			this.toolClipboard_itemTopmost.Image = CommonData.Skin.GetImage(SkinImage.Pin);
			this.toolClipboard_itemSave.Image = CommonData.Skin.GetImage(SkinImage.Save);
			this.toolClipboard_itemRemove.Image = CommonData.Skin.GetImage(SkinImage.Remove);
			this.toolClipboard_itemClear.Image = CommonData.Skin.GetImage(SkinImage.Clear);
			this.toolClipboard_itemEmpty.Image = CommonData.Skin.GetImage(SkinImage.Refresh);

			this.toolClipboard_itemType_itemClipboard.Image = CommonData.Skin.GetImage(SkinImage.Clipboard);
			this.toolClipboard_itemType_itemTemplate.Image = CommonData.Skin.GetImage(SkinImage.RawTemplate);

			this.toolImage_itemRaw.Image = CommonData.Skin.GetImage(SkinImage.ImageRaw);
			this.toolImage_itemFill.Image = CommonData.Skin.GetImage(SkinImage.ImageFill);
			this.commandHtmlUri.Image = CommonData.Skin.GetImage(SkinImage.ClipboardCopy);

			var skinItems = new[] {
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardText), Control = this._commandText, Name = imageText },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardRichTextFormat), Control = this._commandRtf, Name = imageRtf },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardHtml), Control = this._commandHtml, Name = imageHtml },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardImage), Control = this._commandImage, Name = imageImage },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardFile), Control = this._commandFile, Name = imageFile },
				new { Image = CommonData.Skin.GetImage(SkinImage.ClipboardCopy), Control = this._commandMulti, Name = string.Empty },
				new { Image = CommonData.Skin.GetImage(SkinImage.RawTemplate), Control = default(Button), Name = imageRawTemplate},
				new { Image = CommonData.Skin.GetImage(SkinImage.ReplaceTemplate), Control = default(Button), Name = imageReplaceTemplate},
				new { Image = CommonData.Skin.GetImage(SkinImage.Add), Control = this._commandAdd, Name = string.Empty },
				new { Image = CommonData.Skin.GetImage(SkinImage.Up), Control = this._commandUp, Name = string.Empty },
				new { Image = CommonData.Skin.GetImage(SkinImage.Down), Control = this._commandDown, Name = string.Empty },
			};
			this.imageTab.Images.Clear();
			foreach(var skinItem in skinItems) {
				if(skinItem.Control != null) {
					skinItem.Control.Image = skinItem.Image;
				}
				if(!string.IsNullOrEmpty(skinItem.Name)) {
					this.imageTab.Images.Add(skinItem.Name, skinItem.Image);
				}
			}
		}
		#endregion ////////////////////////////////////////

		#region Function


		Size GetButtonSize()
		{
			return new Size(
				IconScale.Small.ToWidth() + NativeMethods.GetSystemMetrics(SM.SM_CXEDGE) * 4,
				IconScale.Small.ToHeight() + NativeMethods.GetSystemMetrics(SM.SM_CYEDGE) * 4
			);
		}
		
		//public void SetCommonData(CommonData commonData)
		//{
		//	CommonData = commonData;

		//	ApplySetting();
		//}

		void ApplySettingUI()
		{
			CommonData.MainSetting.Clipboard.TemplateItems.ListChanged += TemplateItems_ListChanged;
			CommonData.MainSetting.Clipboard.HistoryItems.ListChanged += HistoryItems_ListChanged;
			Location = CommonData.MainSetting.Clipboard.Location;
			Size = CommonData.MainSetting.Clipboard.Size;
			ChangeEnabled(CommonData.MainSetting.Clipboard.Enabled);
			ChangeTopmost(CommonData.MainSetting.Clipboard.TopMost);
			var buttonSize = GetButtonSize();
			using(var g = CreateGraphics()) {
				var fontHeight = (int)g.MeasureString("☃", Font).Height;
				var buttonHeight = buttonSize.Height;
				this.listItemStack.ItemHeight = fontHeight + buttonHeight;
			}
			this.viewText.Font = this.CommonData.MainSetting.Clipboard.TextFont.Font;
			this.inputTemplateSource.Font = this.CommonData.MainSetting.Clipboard.TextFont.Font;
			Visible = CommonData.MainSetting.Clipboard.Visible;

			ChangeSelectListType(CommonData.MainSetting.Clipboard.ClipboardListType);
		}

		/// <summary>
		/// BUGS: Formsバインドで描画が変になる。
		/// </summary>
		protected override void ApplySetting()
		{
			base.ApplySetting();

			ApplySettingUI();

			ChangeListItemNumber(this.listItemStack.SelectedIndex, this.listItemStack.Items.Count, CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History);
		}

		void ChangeTopmost(bool topMost)
		{
			CommonData.MainSetting.Clipboard.TopMost = topMost;
			this.toolClipboard_itemTopmost.Checked = topMost;
			TopMost = topMost;
		}

		void ChangeEnabled(bool enabled)
		{
			CommonData.MainSetting.Clipboard.Enabled = enabled;
			this.toolClipboard_itemEnabled.Checked = enabled;
		}

		void ChangeSelectTypeControl(ToolStripItem item)
		{
			this.toolClipboard_itemType.Text = item.Text;
			this.toolClipboard_itemType.Image = item.Image;

			var map = new Dictionary<ToolStripItem, ClipboardListType>() {
				{ this.toolClipboard_itemType_itemClipboard, ClipboardListType.History },
				{ this.toolClipboard_itemType_itemTemplate,  ClipboardListType.Template},
			};

			ChangeSelectType(map[item]);
		}

		/// <summary>
		/// だっさいなぁ。
		/// </summary>
		/// <param name="type"></param>
		void ChangeSelectListType(ClipboardListType type)
		{
			var map = new Dictionary<ClipboardListType, ToolStripItem>() {
				{ ClipboardListType.History, this.toolClipboard_itemType_itemClipboard },
				{ ClipboardListType.Template, this.toolClipboard_itemType_itemTemplate},
			};

			ChangeSelectTypeControl(map[type]);

			ChangeCommand(-1);

			//ChangeSelsectedItem(type == ClipboardListType.Template ? 0 : -1);
			//ChangeSelsectedItem(type == ClipboardListType.Template ? 0 : -1);
			ChangeSelsectedItem(-1);
		}

		void ChangeCommandType(ClipboardListType type)
		{
			Control[] commandList;
			if(type == ClipboardListType.History) {
				commandList = new[] {
					this._commandMulti,
					this._commandText,
					this._commandRtf,
					this._commandHtml,
					this._commandImage,
					this._commandFile,
				};
			} else {
				commandList = new[] {
					this._commandMulti,
					this._commandAdd,
					this._commandUp,
					this._commandDown,
				};
			}
			this._panelClipboradItem.Controls.Clear();
			this._panelClipboradItem.Controls.AddRange(commandList);
			this._panelClipboradItem.Size = Size.Empty;
		}

		/// <summary>
		/// スタックリストに指定リストをバインドする。
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		void BindStackList<T>(IList<T> list)
		{
			var bindingList = new BindingList<T>(list);
			this.listItemStack.DataSource = bindingList;
		}

		void ChangeSelectType(ClipboardListType type)
		{
			CommonData.MainSetting.Clipboard.ClipboardListType = type;

			this.listItemStack.BeginUpdate();
			try {
				SelectedItemIndex = -1;
				HoverItemIndex = -1;
				if(type == ClipboardListType.History) {
					BindStackList(this.CommonData.MainSetting.Clipboard.HistoryItems);
				} else {
					Debug.Assert(type == ClipboardListType.Template);
					if(!this.CommonData.MainSetting.Clipboard.TemplateItems.Any()) {
						// 新規アイテムの生成
						var newItem = CreateTemplate();
						this.CommonData.MainSetting.Clipboard.TemplateItems.Add(newItem);
					}
					BindStackList(this.CommonData.MainSetting.Clipboard.TemplateItems);
				}
				ChangeCommandType(type);
			} finally {
				this.listItemStack.EndUpdate();
			}
		}


		void ChangeListItemNumber(int index, int count, bool showLimit)
		{
			if(index == -1) {
				this.statusClipboard_itemSelectedIndex.Text = "-";
			} else {
				this.statusClipboard_itemSelectedIndex.Text = (index + 1).ToString();
			}

			this.statusClipboard_itemCount.Text = count.ToString();

			this.statusClipboard_itemLimitLeft.Visible = showLimit;
			this.statusClipboard_itemLimitCount.Visible = showLimit;
			this.statusClipboard_itemLimitRight.Visible = showLimit;
			if(showLimit) {
				this.statusClipboard_itemLimitCount.Text = CommonData.MainSetting.Clipboard.Limit.ToString();
			}
		}

		void ChangeCommand(int index)
		{
			//if((index != -1 && HoverItemIndex != index) || (index != -1 && HoverItemIndex == -1)) {
			if((index > -1 && HoverItemIndex != index)) {
				if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
					var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
					var map = new Dictionary<ClipboardType, Control>() {
						{ ClipboardType.Text, this._commandText },
						{ ClipboardType.Rtf, this._commandRtf },
						{ ClipboardType.Html, this._commandHtml },
						{ ClipboardType.Image, this._commandImage },
						{ ClipboardType.File, this._commandFile },
						//{ ClipboardType.All, this._commandMulti},
					};
					foreach(var pair in map.ToArray()) {
						pair.Value.Enabled = (clipboardItem.ClipboardTypes.HasFlag(pair.Key));
					}
				} else {
					Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
					var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
					var buttons = new[] {
						new { Contrl = this._commandMulti, Enbaled = true },
						new { Contrl = this._commandAdd, Enbaled = true },
						new { Contrl = this._commandUp, Enbaled = index != 0 },
						new { Contrl = this._commandDown, Enbaled = index != this.listItemStack.Items.Count - 1 },
					};
					foreach(var button in buttons) {
						button.Contrl.Enabled = button.Enbaled;
					}
				}
			}

			HoverItemIndex = index;
			this._panelClipboradItem.Visible = HoverItemIndex != -1;
		}

		TabPage ChangeSelsectedHistoryItem(ClipboardItem clipboardItem)
		{
			var map = new Dictionary<ClipboardType, TabPage>() {
				{ ClipboardType.Text, this.tabPreview_pageText },
				{ ClipboardType.Rtf, this.tabPreview_pageRtf },
				{ ClipboardType.Html, this.tabPreview_pageHtml},
				{ ClipboardType.Image, this.tabPreview_pageImage },
				{ ClipboardType.File, this.tabPreview_pageFile },
			};

			foreach(var type in clipboardItem.GetClipboardTypeList()) {
				this.tabPreview.TabPages.Add(map[type]);

				switch(type) {
					case ClipboardType.Text: {
							this.viewText.Text = clipboardItem.Text;
						}
						break;

					case ClipboardType.Rtf: {
							this.viewRtf.Rtf = clipboardItem.Rtf;
						}
						break;

					case ClipboardType.Html:
						{
							ClipboardHtmlDataItem html;
							var result = ClipboardUtility.TryConvertHtmlFromClipbordHtml(clipboardItem.Html, out html, CommonData.Logger);
							if(html.SourceURL != null) {
								this.viewHtmlUri.Text = html.SourceURL.ToString();
							} else {
								this.viewHtmlUri.Text = string.Empty;
							}
							if(result) {
								this.viewHtml.DocumentText = html.ToHtml();
							} else {
								var elements = string.Format("<p style='font-weight: bold; color: #f00; background: #fff'>{0}</p><hr />", CommonData.Language["clipboard/html/error"]);
								this.viewHtml.DocumentText = elements + clipboardItem.Html;
							}
						}
						break;

					case ClipboardType.Image: {
							this.viewImage.Image = clipboardItem.Image;
						}
						break;

					case ClipboardType.File: {
							var imageList = new ImageList();
							imageList.ColorDepth = ColorDepth.Depth32Bit;
							imageList.ImageSize = IconScale.Small.ToSize();
							var listItemList = new List<ListViewItem>(clipboardItem.Files.Count());
							var showExtentions = SystemEnvironment.IsExtensionShow();
							Func<string, string> getName;
							if(showExtentions) getName = Path.GetFileName; else getName = Path.GetFileNameWithoutExtension;
							foreach(var path in clipboardItem.Files) {
								var key = path.GetHashCode().ToString();
								var name = getName(path);

								Icon icon;
								if(FileUtility.Exists(path)) {
									icon = IconUtility.Load(path, IconScale.Small, 0);
								} else {
									icon = LauncherItem.notfoundIconMap[IconScale.Small];
								}

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
			}

			return map[clipboardItem.GetSingleClipboardType()];
		}

		TabPage ChangeSelsectedTemplateItem(TemplateItem templateItem)
		{
			this.tabPreview.TabPages.AddRange(new[] {
				this.tabPreview_pageRawTemplate,
				this.tabPreview_pageReplaceTemplate
			});

			// あれやこれやがだるいのでバインドる。
			this.inputTemplateName.DataBindings.Clear();
			var bindName = this.inputTemplateName.DataBindings.Add("Text", templateItem, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
			bindName.Parse += TemplateName_Parse;

			this.inputTemplateSource.DataBindings.Clear();
			this.inputTemplateSource.DataBindings.Add("Text", templateItem, "Source", false, DataSourceUpdateMode.OnPropertyChanged);

			this.selectTemplateReplace.DataBindings.Clear();
			this.selectTemplateReplace.DataBindings.Add("Checked", templateItem, "ReplaceMode", false, DataSourceUpdateMode.OnPropertyChanged);
			

			return this.tabPreview_pageRawTemplate;
		}

		/// <summary>
		/// タブ内の各コントロールを初期化する。
		/// </summary>
		void ResetControlInTabPage()
		{
			this.viewText.ResetText();
			this.viewRtf.ResetText();
			this.viewHtml.DocumentText = null;
			this.viewImage.Image = null;
			this.viewFile.Items.Clear();
		}

		void ChangeSelsectedItem(int index)
		{
			if(Initialized) {
				this.tabPreview.Enabled = index != -1;
			}

			if(index == -1) {
				return;
			}

			//this.tabPreview.SuspendLayout();
			WindowsUtility.SetRedraw(this, false);
			this.tabPreview.TabPages.Clear();
			// タブ内のコントロールを初期化
			ResetControlInTabPage();

			TabPage defaultTabPage;
			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
				defaultTabPage = ChangeSelsectedHistoryItem(clipboardItem);
			} else {
				var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
				defaultTabPage = ChangeSelsectedTemplateItem(templateItem);
			}
			this.tabPreview.SelectedTab = defaultTabPage;
			//this.tabPreview.ResumeLayout();
			WindowsUtility.SetRedraw(this, true);
			Refresh();
		}

		void CopyItem(ClipboardItem clipboardItem, ClipboardType clipboardType)
		{
			var map = new Dictionary<ClipboardType, Action<ClipboardSetting>>() {
				{ ClipboardType.Text, (clipboardSetting) => {
					ClipboardUtility.CopyText(clipboardItem.Text, clipboardSetting);
				} },
				{ ClipboardType.Rtf, (clipboardSetting) => {
					ClipboardUtility.CopyRtf(clipboardItem.Rtf, clipboardSetting);
				} },
				{ ClipboardType.Html, (clipboardSetting) => {
					ClipboardUtility.CopyHtml(clipboardItem.Html, clipboardSetting);
				} },
				{ ClipboardType.Image, (clipboardSetting) => {
					ClipboardUtility.CopyImage(clipboardItem.Image, clipboardSetting);
				} },
				{ ClipboardType.File, (clipboardSetting) => {
					ClipboardUtility.CopyFile(clipboardItem.Files.Where(f => FileUtility.Exists(f)), clipboardSetting);
				} },
				{ ClipboardType.All, (clipboardSetting) => {
					var data = new DataObject();
					var typeFuncs = new Dictionary<ClipboardType, Action>() {
						{ ClipboardType.Text, () => data.SetText(clipboardItem.Text, TextDataFormat.UnicodeText) },
						{ ClipboardType.Rtf, () => data.SetText(clipboardItem.Rtf, TextDataFormat.Rtf) },
						{ ClipboardType.Html, () => data.SetText(clipboardItem.Html, TextDataFormat.Html) },
						{ ClipboardType.Image, () => data.SetImage(clipboardItem.Image) },
						{ ClipboardType.File, () => {
							var sc = new StringCollection();
							sc.AddRange(clipboardItem.Files.ToArray());
							data.SetFileDropList(sc); 
						}},
					};
					foreach(var type in clipboardItem.GetClipboardTypeList()) {
						typeFuncs[type]();
					}
					ClipboardUtility.CopyDataObject(data, clipboardSetting);
				} },
			};
			map[clipboardType](CommonData.MainSetting.Clipboard);
		}

		void CopySingleItem(int index)
		{
			Debug.Assert(index != -1);
			
			var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
			CopyItem(clipboardItem, clipboardItem.GetSingleClipboardType());
		}

		bool SaveClipboardItem(string path, ClipboardItem clipboardItem, ClipboardType type)
		{
			Debug.Assert(type != ClipboardType.File);

			var map = new Dictionary<ClipboardType, Action>() {
				{ ClipboardType.Text, () => File.WriteAllText(path, clipboardItem.Text) },
				{ ClipboardType.Rtf, () => File.WriteAllText(path, clipboardItem.Rtf) },
				{ ClipboardType.Html, () => File.WriteAllText(path, ClipboardUtility.ConvertHtmlFromClipbordHtml(clipboardItem.Html, CommonData.Logger).ToHtml()) },
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

		bool SaveTemplateItem(string path, TemplateItem templateItem)
		{
			try {
				File.WriteAllText(path, TemplateUtility.ToPlainText(templateItem, CommonData.Language));
				return true;
			} catch(Exception ex) {
				CommonData.Logger.Puts(LogType.Error, templateItem.Name, ex);
				return false;
			}
		}

		void OpenClipboardItemSaveDialog(ClipboardItem clipboardItem)
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
				dialog.Attachment(filter);
				dialog.FileName = clipboardItem.Timestamp.ToString(Literal.timestampFileName);
				dialog.FilterIndex = defIndex;
				if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					var item = (DialogFilterValueItem<ClipboardType>)filter.Items[dialog.FilterIndex - 1];
					var path = dialog.FileName;
					var type = item.Value;
					SaveClipboardItem(path, clipboardItem, type);
				}
			}
		}

		void OpenTemplateItemSaveDialog(TemplateItem templateItem)
		{
			using(var dialog = new SaveFileDialog()) {
				var filter = new DialogFilter();
				filter.Items.Add(new DialogFilterItem(CommonData.Language["clipboard/page/raw-template"], "*.txt"));
				dialog.Attachment(filter);
				var fileName = PathUtility.ToSafeName(templateItem.Name);
				if(string.IsNullOrWhiteSpace(fileName)) {
					fileName = Literal.NowTimestampFileName;
				}
				dialog.FileName = fileName;
				if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					var path = dialog.FileName;
					SaveTemplateItem(path, templateItem);
				}
			}
		}

		public void ClearEvent()
		{
			CommonData.MainSetting.Clipboard.TemplateItems.ListChanged -= TemplateItems_ListChanged;
			CommonData.MainSetting.Clipboard.HistoryItems.ListChanged -= HistoryItems_ListChanged;
		}

		void ListChanged<T>(ClipboardListType targetType, EventList<T> itemList, Action action)
		{
			if(CommonData.MainSetting.Clipboard.ClipboardListType != targetType) {
				return;
			}

			try {
				this.listItemStack.BeginUpdate();

				var isActive = Form.ActiveForm == this;
				var selectedIndex = this.listItemStack.SelectedIndex;
				this.listItemStack.DataSource = null;
				this.listItemStack.SelectedIndex = -1;

				if(action != null) {
					action();
				}

				//if(itemList.Any()) {
				//var bindList = new BindingList<T>(itemList);
				//this.listItemStack.DataSource = bindList;
				//}
				BindStackList(itemList);

				if(isActive) {
					if(selectedIndex < this.listItemStack.Items.Count) {
						this.listItemStack.SelectedIndex = selectedIndex;
					} else {
						this.listItemStack.SelectedIndex = this.listItemStack.Items.Count - 1;
					}
				} else if(itemList.Any()) {
					this.listItemStack.SelectedIndex = 0;
				}
				this._panelClipboradItem.Visible = false;
				ChangeCommand(-1);
			} finally {
				this.listItemStack.EndUpdate();
			}
		}

		string GetUniqueTemplateName()
		{
			return TextUtility.ToUniqueDefault(CommonData.Language["new/template-item"], CommonData.MainSetting.Clipboard.TemplateItems.Select(t => t.Name));
		}

		TemplateItem CreateTemplate()
		{
			Debug.Assert(CommonData != null);

			return new TemplateItem() {
				Name = GetUniqueTemplateName(),
			};
		}

		/// <summary>
		/// テンプレートを追加。
		/// </summary>
		/// <param name="templateItem">追加するテンプレート位置のアイテム</param>
		void AddTemplate(TemplateItem templateItem)
		{
			var createdItem = CreateTemplate();
			if(templateItem != null) {
				var index = CommonData.MainSetting.Clipboard.TemplateItems.IndexOf(templateItem);
				CommonData.MainSetting.Clipboard.TemplateItems.Insert(index, createdItem);
				this.listItemStack.SelectedIndex = index;
			}
		}

		void SwapListItem<T>(IList<T> list, int from, int to)
		{
			var swapItem = list[from];
			list[from] = list[to];
			list[to] = swapItem;
			//this.listClipboard.SelectedIndex = to;
			this.listItemStack.Invalidate();
		}

		void UpTemplate(TemplateItem templateItem)
		{
			var index = CommonData.MainSetting.Clipboard.TemplateItems.IndexOf(templateItem);
			Debug.Assert(index != -1);
			if(index != 0) {
				SwapListItem(CommonData.MainSetting.Clipboard.TemplateItems, index, index -1);
			}
		}

		void DownTemplate(TemplateItem templateItem)
		{
			var index = CommonData.MainSetting.Clipboard.TemplateItems.IndexOf(templateItem);
			Debug.Assert(index != -1);
			if(index + 1 < CommonData.MainSetting.Clipboard.TemplateItems.Count) {
				SwapListItem(CommonData.MainSetting.Clipboard.TemplateItems, index, index + 1);
			}
		}

		void CopyTemplate(TemplateItem templateItem)
		{
			var templateText = TemplateUtility.ToPlainText(templateItem, CommonData.Language);
			if(!string.IsNullOrEmpty(templateText)) {
				ClipboardUtility.CopyText(templateText, CommonData.MainSetting.Clipboard);
			}
		}

		void ChekedReplace()
		{
			var check = this.selectTemplateReplace.Checked;
			//this.webTemplateComment.Visible = check;

			this.panelTemplateSource.Panel2Collapsed = !check;
		}

		void InsertReplaceItem(ReplaceItem replaceItem)
		{
			var nowSelectIndex = this.inputTemplateSource.SelectionStart;
			var replaceWord = replaceItem.ReplaceWord;
			this.inputTemplateSource.SelectedText = replaceWord;
			this.inputTemplateSource.Select(nowSelectIndex, replaceWord.Length);
			this.inputTemplateSource.Focus();
		}

		void AttachmentImageDrag()
		{
			ImageDragging = false;
			this.viewImage.MouseDown += viewImage_MouseDown;
			this.viewImage.MouseUp += viewImage_MouseUp;
			this.viewImage.MouseMove += viewImage_MouseMove;
		}

		void DetachmentImageDrag()
		{
			this.viewImage.MouseDown -= viewImage_MouseDown;
			this.viewImage.MouseMove -= viewImage_MouseMove;
			this.viewImage.MouseUp -= viewImage_MouseUp;
		}

		void ChangeImageSize(ImageViewSize imageViewSize)
		{
			var controlMap = new Dictionary<ImageViewSize, ToolStripButton>() {
				{ ImageViewSize.Raw,  this.toolImage_itemRaw },
				{ ImageViewSize.Fill, this.toolImage_itemFill },
			};
			foreach(var control in controlMap.Values) {
				control.Checked = false;
			}
			controlMap[imageViewSize].Checked = true;

			switch(imageViewSize) {
				case ImageViewSize.Fill:
					this.viewImage.Dock = DockStyle.Fill;
					this.viewImage.SizeMode = PictureBoxSizeMode.Zoom;
					DetachmentImageDrag();
					break;

				case ImageViewSize.Raw:
					this.viewImage.Dock = DockStyle.None;
					this.viewImage.Size = Size.Empty;
					this.viewImage.SizeMode = PictureBoxSizeMode.AutoSize;
					AttachmentImageDrag();
					break;

				default:
					throw new NotImplementedException();
			}
		}

		#endregion ////////////////////////////////////////

		#region Draw

		void DrawClipboardItem(Graphics g, int itemIndex, Rectangle bounds, Color foreColor)
		{
			var item = CommonData.MainSetting.Clipboard.HistoryItems[itemIndex];
			var map = new Dictionary<ClipboardType, string>() {
					{ ClipboardType.Text, imageText},
					{ ClipboardType.Rtf, imageRtf},
					{ ClipboardType.Html, imageHtml},
					{ ClipboardType.Image, imageImage},
					{ ClipboardType.File, imageFile},
				};
			var image = this.imageTab.Images[map[item.GetSingleClipboardType()]];

			var drawArea = new Rectangle(bounds.X + this.listItemStack.Margin.Left, bounds.Bottom - image.Height - +this.listItemStack.Margin.Bottom - 1, image.Width, image.Height);

			g.DrawImage(image, drawArea);

			using(var sf = new StringFormat())
			using(var brush = new SolidBrush(foreColor)) {
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Near;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				g.DrawString(item.Name, Font, brush, bounds, sf);

				sf.Alignment = StringAlignment.Far;
				sf.LineAlignment = StringAlignment.Far;
				g.DrawString(item.Timestamp.ToString(), SystemFonts.SmallCaptionFont, brush, bounds, sf);
			}
		}

		void DrawTemplateItem(Graphics g, int itemIndex, Rectangle bounds, Color foreColor)
		{
			var item = CommonData.MainSetting.Clipboard.TemplateItems[itemIndex];
			using(var sf = new StringFormat())
			using(var brush = new SolidBrush(foreColor)) {
				sf.Alignment = StringAlignment.Near;
				sf.LineAlignment = StringAlignment.Near;
				sf.Trimming = StringTrimming.EllipsisCharacter;
				sf.FormatFlags = StringFormatFlags.NoWrap;
				g.DrawString(item.Name, Font, brush, bounds, sf);
			}
		}

		void DrawItem(DrawItemEventArgs e)
		{
			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				DrawClipboardItem(e.Graphics, e.Index, e.Bounds, e.ForeColor);
			} else {
				Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
				DrawTemplateItem(e.Graphics, e.Index, e.Bounds, e.ForeColor);
			}
		}

		StringFormat GetReplaceTitleFormat()
		{
			var sf = new StringFormat();
			return sf;
		}

		StringFormat GetReplaceCommentFormat()
		{
			var sf = new StringFormat();
			return sf;
		}

		int GetReplaceCommentPadding()
		{
			return 20;
		}

		Font GetReplaceTitleFont()
		{
			return new Font(Font.FontFamily, Font.SizeInPoints, FontStyle.Bold);
		}

		Font GetReplaceCommentFont()
		{
			return new Font(Font.FontFamily, Font.SizeInPoints, default(FontStyle));
		}

		void PaintReplaceItem(Graphics g, ReplaceItem replaceItem, Action<Font, Font, StringFormat, StringFormat, int, SizeF> action)
		{
			using(var titleFont = GetReplaceTitleFont())
			using(var commentFont = GetReplaceCommentFont())
			using(var titleFormat = GetReplaceTitleFormat())
			using(var commentFormat = GetReplaceCommentFormat()) {
				var width = this.listReplace.ClientSize.Width - this.listReplace.Margin.Right;
				var titleSize = g.MeasureString(replaceItem.Name, titleFont, width, titleFormat);

				action(titleFont, commentFont, titleFormat, commentFormat, width, titleSize);
			}
		}

		#endregion ////////////////////////////////////////

		void TemplateName_Parse(object sender, ConvertEventArgs e)
		{
			var s = (string)e.Value;
			if(string.IsNullOrWhiteSpace(s)) {
				e.Value = GetUniqueTemplateName();
			}
			this.listItemStack.Invalidate();
		}

		private void toolClipboard_itemType_itemClipboard_Click(object sender, EventArgs e)
		{
			ChangeSelectTypeControl((ToolStripItem)sender);
		}

		void TemplateItems_ListChanged(object sender, EventArgs e)
		{
			ListChanged(ClipboardListType.Template, CommonData.MainSetting.Clipboard.TemplateItems, null);
		}

		void HistoryItems_ListChanged(object sender, EventArgs e)
		{
			ListChanged(ClipboardListType.History, CommonData.MainSetting.Clipboard.HistoryItems, () => {
				if(CommonData.MainSetting.Clipboard.HistoryItems.Count == 0) {
					ResetControlInTabPage();
				}
			});
		}

		private void listClipboard_DrawItem(object sender, DrawItemEventArgs e)
		{
			if(e.Index != -1) {

				e.DrawBackground();

				DrawItem(e);

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
			var index = this.listItemStack.SelectedIndex;
			if(index != SelectedItemIndex) {
				SelectedItemIndex = index;
				ChangeListItemNumber(this.listItemStack.SelectedIndex, this.listItemStack.Items.Count, CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History);
				ChangeSelsectedItem(this.listItemStack.SelectedIndex);
				if(Form.ActiveForm == this) {
					ActiveControl = this.listItemStack;
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
			if(CommonData == null) {
				return;
			}
			CommonData.MainSetting.Clipboard.Size = Size;
		}

		private void toolClipboard_itemTopmost_Click(object sender, EventArgs e)
		{
			var check = !toolClipboard_itemTopmost.Checked;
			ChangeTopmost(check);
		}

		private void listClipboard_MouseMove(object sender, MouseEventArgs e)
		{
			var index = this.listItemStack.IndexFromPoint(e.Location);// -this.listClipboard.TopIndex;
			var showIndex = index - this.listItemStack.TopIndex;
			var top = this.listItemStack.ItemHeight * (showIndex + 1) - GetButtonSize().Height - 1;
			
			if(top != this._panelClipboradItem.Top) {
				this._panelClipboradItem.Top = top;
			}
			ChangeCommand(index);
		}

		private void tabPreview_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if(!Created) {
				e.Cancel = true;
				return;
			}
			//Debug.Assert(SelectedItemIndex != -1);
			var index = this.listItemStack.SelectedIndex;
			if(index == -1) {
				e.Cancel = true;
				return;
			}

			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
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
			} else {
				Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
				if(e.TabPage == this.tabPreview_pageReplaceTemplate) {
					var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
					if(templateItem.ReplaceMode) {
						var rtf = TemplateUtility.ToRtf(templateItem, CommonData.Language, CommonData.MainSetting.Clipboard.TextFont);
						this.viewReplaceTemplate.Rtf = rtf;
					} else {
						// 置き換え処理を行わないのであればプレビューは表示するだけ無駄
						e.Cancel = true;
					}
				}
			}
		}

		void command_Click(object sender, EventArgs e)
		{
			if(0 > HoverItemIndex) {
				return;
			}
			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				try {
					var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[HoverItemIndex];
					var map = new Dictionary<object, ClipboardType>() {
						{ this._commandText, ClipboardType.Text },
						{ this._commandRtf, ClipboardType.Rtf },
						{ this._commandHtml, ClipboardType.Html },
						{ this._commandImage, ClipboardType.Image },
						{ this._commandFile, ClipboardType.File },
						{ this._commandMulti, ClipboardType.All },
					};
					CopyItem(clipboardItem, map[sender]);
				} catch(Exception ex) {
					CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
				}
			} else {
				var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[HoverItemIndex];
				var map = new Dictionary<object, Action<TemplateItem>>() {
					{ this._commandMulti, CopyTemplate },
					{ this._commandAdd,   AddTemplate },
					{ this._commandUp,    UpTemplate },
					{ this._commandDown,  DownTemplate },
				};
				map[sender](templateItem);
			}
		}

		private void listClipboard_DoubleClick(object sender, EventArgs e)
		{
			var index = this.listItemStack.SelectedIndex;
			if(index != -1) {
				if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
					try {
						var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
						CopyItem(clipboardItem, ClipboardType.All);
					} catch(Exception ex) {
						CommonData.Logger.Puts(LogType.Error, ex.Message, ex);
					}
				} else {
					Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
					var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
					CopyTemplate(templateItem);
				}
			}
		}

		private void toolClipboard_itemSave_Click(object sender, EventArgs e)
		{
			var index = this.listItemStack.SelectedIndex;
			if(index != -1) {
				if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
					var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
					OpenClipboardItemSaveDialog(clipboardItem);
				} else {
					var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
					OpenTemplateItemSaveDialog(templateItem);
				}
			}
		}

		private void toolClipboard_itemClear_ButtonClick(object sender, EventArgs e)
		{
			var index = this.listItemStack.SelectedIndex;
			if(index != -1) {
				if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
					var clipboardItem = CommonData.MainSetting.Clipboard.HistoryItems[index];
					CommonData.MainSetting.Clipboard.HistoryItems.RemoveAt(index);
					clipboardItem.ToDispose();
				} else {
					Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
					// 最後の一つを削除するとあまりよろしくない
					if(CommonData.MainSetting.Clipboard.TemplateItems.Count != 1) {
						var templateItem = CommonData.MainSetting.Clipboard.TemplateItems[index];
						CommonData.MainSetting.Clipboard.TemplateItems.RemoveAt(index);
					}
				}
			}
		}

		private void toolClipboard_itemClear_Click(object sender, EventArgs e)
		{
			if(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.History) {
				var items = CommonData.MainSetting.Clipboard.HistoryItems.ToArray();
				CommonData.MainSetting.Clipboard.HistoryItems.Clear();
				ResetControlInTabPage();
				foreach(var item in items) {
					item.ToDispose();
				}
			} else {
				Debug.Assert(CommonData.MainSetting.Clipboard.ClipboardListType == ClipboardListType.Template);
				var lastItem = CommonData.MainSetting.Clipboard.TemplateItems.LastOrDefault();
				if(lastItem != null) {
					CommonData.MainSetting.Clipboard.TemplateItems.Clear();
					CommonData.MainSetting.Clipboard.TemplateItems.Add(lastItem);
				}
			}
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

		private void listClipboard_MouseLeave(object sender, EventArgs e)
		{
			if(this._panelClipboradItem.Visible) {
				var point = this.listItemStack.PointToClient(Cursor.Position);
				this._panelClipboradItem.Visible = this.listItemStack.DisplayRectangle.Contains(point);
			}
		}

		private void toolClipboard_itemEnabled_Click(object sender, EventArgs e)
		{
			var check = !toolClipboard_itemEnabled.Checked;
			ChangeEnabled(check);
		}

		void listClipboard_MouseWheel(object sender, MouseEventArgs e)
		{
			var noDraw = new IntPtr(0);
			var onDraw = new IntPtr(1);

			NativeMethods.SendMessage(Handle, WM.WM_SETREDRAW, noDraw, IntPtr.Zero);
			
			this.listItemStack.Invalidate();

			NativeMethods.SendMessage(Handle, WM.WM_SETREDRAW, onDraw, IntPtr.Zero);

			this._panelClipboradItem.Refresh();
		}

		private void ClipboardForm_VisibleChanged(object sender, EventArgs e)
		{
			if(Visible) {
				UIUtility.ShowFrontActive(this);
			}
		}

		private void selectTemplateReplace_CheckedChanged(object sender, EventArgs e)
		{
			ChekedReplace();
		}


		private void listReplace_MeasureItem(object sender, MeasureItemEventArgs e)
		{
			Debug.Assert(e.Index != -1);
			if(!e.Index.Between(0, this._replaceCommentList.Count - 1)) {
				return;
			}
			var replaceItem = this._replaceCommentList[e.Index];
			PaintReplaceItem(e.Graphics, replaceItem, (titleFont, commentFont, titleFormat, commentFormat, width, titleSize) => {
				var commentSize = e.Graphics.MeasureString(replaceItem.Comment, commentFont, width - GetReplaceCommentPadding(), commentFormat);
				e.ItemHeight = (int)(titleSize.Height + commentSize.Height) + this.listReplace.Margin.Vertical + this.listReplace.Margin.Top;
			});
		}

		private void listReplace_DrawItem(object sender, DrawItemEventArgs e)
		{
			if(e.Index != -1) {
				var replaceItem = this._replaceCommentList[e.Index];
				PaintReplaceItem(e.Graphics, replaceItem, (titleFont, commentFont, titleFormat, commentFormat, width, titleSize) => {
					using(var foreBrush = new SolidBrush(e.ForeColor)) {
						var titleArea = new RectangleF() {
							X = e.Bounds.X + this.listReplace.Margin.Left,
							Y = e.Bounds.Y + this.listReplace.Margin.Top,
							Width = width,
							Height = titleSize.Height
						};
						var commentArea = new RectangleF() {
							X = e.Bounds.X + GetReplaceCommentPadding(),
							Y = e.Bounds.Y + titleSize.Height + this.listReplace.Margin.Top,
							Width = width - GetReplaceCommentPadding(),
							Height = e.Bounds.Height - titleSize.Height
						};
						e.DrawBackground();
						e.Graphics.DrawString(replaceItem.Name, titleFont, foreBrush, titleArea);
						e.Graphics.DrawString(replaceItem.Comment, commentFont, foreBrush, commentArea);
					}
				});
			}
		}

		private void listReplace_Resize(object sender, EventArgs e)
		{
			if(this._replaceCommentList == null) {
				return;
			}
			this.listReplace.BeginUpdate();
			var selectedItem = this.listReplace.SelectedItem;
			try {
				this.listReplace.DataSource = new BindingList<ReplaceItem>(this._replaceCommentList);
				this.listReplace.SelectedItem = selectedItem;
			} finally {
				this.listReplace.EndUpdate();
			}
		}

		private void listReplace_DoubleClick(object sender, EventArgs e)
		{
			var replaceItem = this.listReplace.SelectedItem as ReplaceItem;
			if(replaceItem != null) {
				InsertReplaceItem(replaceItem);
			}
		}

		private void commandHtmlUri_Click(object sender, EventArgs e)
		{
			// 現在URI表示欄に表示されている項目をこぴる
			var uri = this.viewHtmlUri.Text;
			if(!string.IsNullOrWhiteSpace(uri)) {
				ClipboardUtility.CopyText(uri, CommonData.MainSetting.Clipboard);
			}
		}

		private void toolImage_itemRaw_Click(object sender, EventArgs e)
		{
			var map = new Dictionary<object, ImageViewSize>() {
				{ this.toolImage_itemFill, ImageViewSize.Fill },
				{ this.toolImage_itemRaw, ImageViewSize.Raw },
			};
			ImageSize = map[sender];
		}

		void viewImage_MouseDown(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Left) {
				if(this.panelImage.HorizontalScroll.Visible || this.panelImage.VerticalScroll.Visible) {
					ImageDragging = true;
					ImageDragPosition = e.Location;
					Cursor = Cursors.NoMove2D;
				}
			}
		}

		void viewImage_MouseMove(object sender, MouseEventArgs e)
		{
			if(ImageDragging) {
				var movePoint = new Point(
					-this.panelImage.AutoScrollPosition.X - (e.Location.X - ImageDragPosition.X),
					-this.panelImage.AutoScrollPosition.Y - (e.Location.Y - ImageDragPosition.Y)
				);
				this.panelImage.AutoScrollPosition = movePoint;
			}
		}

		void viewImage_MouseUp(object sender, MouseEventArgs e)
		{
			if(ImageDragging) {
				ImageDragging = false;
				Cursor = Cursors.Default;
			}
		}
	}
}
