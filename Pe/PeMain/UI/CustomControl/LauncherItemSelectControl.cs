using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{

	/// <summary>
	/// Description of LauncherItemSelectControl.
	/// </summary>
	public partial class LauncherItemSelectControl : UserControl, ISetLanguage
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		private Language _language;
		private bool _itemEdit;
		private LauncherItemSelecterType _filterType;
		private List<LauncherItem> _items;
		private IEnumerable<LauncherItem> _viewItems;
		private bool _filtering;
		#endregion ////////////////////////////////////

		#region event
		public event EventHandler<CreateItemEventArg> CreateItem;
		public event EventHandler<RemovedItemEventArg> RemovedItem;
		public event EventHandler<SelectedItemEventArg> SelectChangedItem;
		#endregion ////////////////////////////////////

		public LauncherItemSelectControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		public bool ItemEdit
		{
			get 
			{
				return this._itemEdit;
			}
			set
			{
				foreach(var item in GetEditToolItem()) {
					item.Visible = value;
				}
				this._itemEdit = value;
			}
		}
		
		public LauncherItemSelecterType FilterType
		{
			get 
			{
				return this._filterType;
			}
			set
			{
				SetFilterType(value);
				this._filterType = value;
			}
		}
		
		public LauncherItem SelectedItem
		{
			get
			{
				var item = this.listLauncherItems.SelectedItem;
				return item as LauncherItem;
			}
			set
			{
				this.listLauncherItems.SelectedItem = value;
			}
		}
		
		public IconScale IconScale { get; set; }
		
		public IList<LauncherItem> Items { get { return this._items; } }
		
		public IEnumerable<LauncherItem> ViewItems { get { return this._viewItems; } }
		
		public bool Filtering
		{
			get 
			{
				return this._filtering;
			}
			set
			{
				//TODO: ぼたんやなんや
				this._filtering = value;
				this.toolLauncherItems_filter.Checked = this._filtering;
				if(this._filtering) {
					var list = ApplyFilter();
					this._viewItems = list;
				} else {
					this._viewItems = this._items;
				}
				this.listLauncherItems.Items.Clear();
				this.listLauncherItems.Items.AddRange(this._viewItems.ToArray());
			}
		}

		public ApplicationSetting ApplicationSetting { get; set; }
		#endregion ////////////////////////////////////

		#region ISetCommonData
		#endregion ////////////////////////////////////

		#region ISetLanguage
		public void SetLanguage(Language language)
		{
			ApplyLanguage(language);
			this._language = language;
		}
		#endregion ////////////////////////////////////

		#region override
		#endregion ////////////////////////////////////

		#region initialize
		void InitializeUI()
		{
		}

		void Initialize()
		{
			this._items = new List<LauncherItem>();

			ItemEdit = true;
			FilterType = LauncherItemSelecterType.Full;
			IconScale = IconScale.Normal;

			InitializeUI();
			TuneItemHeight();
			ResizeInputArea();
		}
		#endregion ////////////////////////////////////

		#region language
		void ApplyLanguage(Language language)
		{
			if(language == null) {
				return;
			}

			this.toolLauncherItems_create.SetLanguage(language);
			this.toolLauncherItems_remove.SetLanguage(language);
			this.toolLauncherItems_filter.SetLanguage(language);
			this.toolLauncherItems_type_full.SetLanguage(language);
			this.toolLauncherItems_type_name.SetLanguage(language);
			this.toolLauncherItems_type_tag.SetLanguage(language);

			ToolLauncherItems_type_Click(this.toolLauncherItems_type_full, null);
		}
		#endregion ////////////////////////////////////

		#region function
		IEnumerable<ToolStripItem> GetEditToolItem()
		{
			return new ToolStripItem[] {
				this.toolLauncherItems_create,
				this.toolLauncherItems_remove,
				this.toolLauncherItems_editSeparator
			};
		}
		
		void ResizeInputArea()
		{
			int totalWidth = this.toolLauncherItems.Margin.Horizontal + this.toolLauncherItems.Padding.Horizontal;
			foreach(ToolStripItem item in this.toolLauncherItems.Items) {
				if(item == this.toolLauncherItems_input) {
					continue;
				} else if(item.Visible) {
					totalWidth += item.Width + item.Padding.Horizontal + item.Margin.Horizontal;
				}
			}
			var parentWidth = this.toolLauncherItems.Width - this.toolLauncherItems.Padding.Horizontal - this.toolLauncherItems.Margin.Horizontal;
			var inputWidth = parentWidth - totalWidth - this.toolLauncherItems.Margin.Horizontal - this.toolLauncherItems.Padding.Horizontal;
			var size = new Size(inputWidth, this.toolLauncherItems_input.Size.Height);
			this.toolLauncherItems_input.Size = size;
		}
		
		void TuneItemHeight()
		{
			var iconHeight = IconScale.ToHeight();
			var fontHeight = Font.Height;
			var itemHeight = Math.Max(iconHeight, fontHeight) + this.listLauncherItems.Padding.Vertical + 1 * 2;
			this.listLauncherItems.ItemHeight = itemHeight;
		}
		
		void SetFilterType(LauncherItemSelecterType type)
		{
			var toolItem = new Dictionary<LauncherItemSelecterType, ToolStripItem>() {
				{LauncherItemSelecterType.Full, this.toolLauncherItems_type_full},
				{LauncherItemSelecterType.Name, this.toolLauncherItems_type_name},
				{LauncherItemSelecterType.Tag, this.toolLauncherItems_type_tag},
			}[type];
			
			this.toolLauncherItems_type.Text = toolItem.Text;
			this.toolLauncherItems_type.ToolTipText = toolItem.ToolTipText;
			this.toolLauncherItems_type.Image = toolItem.Image;
		}

		public void SetItems(IEnumerable<LauncherItem> items, ApplicationSetting applicationSetting)
		{
			this._items.Clear();
			this.listLauncherItems.Items.Clear();

			ApplicationSetting = applicationSetting;

			if(items != null) {
				var list = items.ToArray();
				this._items.AddRange(list);
				this.listLauncherItems.Items.AddRange(list);
			}
		}
		
		void CreateLauncherItem()
		{
			var itemName = this._language["control/launcher-selecter/new-item"];
			if(this._items.Count > 0) {
				itemName = TextUtility.ToUniqueDefault(itemName, this._items.Select(i => i.Name));
			}
			var item = new LauncherItem();
			item.Name = itemName;
			this._items.Add(item);
			Filtering = false;
			
			this.listLauncherItems.SelectedItem = item;
			if(CreateItem != null)  {
				var e = new CreateItemEventArg();
				e.Item = item;
				CreateItem(this, e);
			}
		}
		
		void RemoveLauncherItem(LauncherItem item)
		{
			this._items.Remove(item);
			var index = this.listLauncherItems.Items.IndexOf(item);
			this.listLauncherItems.Items.Remove(item);
			int newIndex = -1;
			if(this.listLauncherItems.Items.Count > 0) {
				newIndex = index.Rounding(0, this.listLauncherItems.Items.Count - 1);
			}
			if(RemovedItem != null) {
				var e = new RemovedItemEventArg();
				e.Item = item;
				RemovedItem(this, e);
			}
			this.listLauncherItems.SelectedIndex = newIndex;
		}
		
		public bool HasItem(LauncherItem item)
		{
			return this._items.Any(i => i.Name == item.Name);
		}
		
		public void AddItem(LauncherItem item)
		{
			if(Filtering) {
				Filtering = false;
			}
			
			this._items.Add(item);
			this.listLauncherItems.Items.Add(item);
			this.listLauncherItems.SelectedItem = item;
		}

		IEnumerable<LauncherItem> ApplyFilter()
		{
			var srcPattern = this.toolLauncherItems_input.Text;
			var wldPattern = TextUtility.RegexPatternToWildcard(srcPattern);
			var reg = new Regex(wldPattern);
			var type = FilterType;
			
			var nameSeq = this._items.Where(item => reg.IsMatch(item.Name));
			var tagSeq = this._items.Where(item => item.Tag.Any(s => reg.IsMatch(s)));
			
			var list = new List<LauncherItem>();
			
			if(type == LauncherItemSelecterType.Full || type == LauncherItemSelecterType.Name) {
				list.AddRange(nameSeq);
			}
			if(type == LauncherItemSelecterType.Full || type == LauncherItemSelecterType.Tag) {
				list.AddRange(tagSeq);
			}

			return list.Distinct();
		}
		#endregion ////////////////////////////////////

		void LauncherItemSelectControlResize(object sender, EventArgs e)
		{
			ResizeInputArea();
		}
		
		void LauncherItemSelectControlLoad(object sender, EventArgs e)
		{
			TuneItemHeight();
			ResizeInputArea();
		}
		
		void ToolLauncherItems_type_Click(object sender, EventArgs e)
		{
			var type = new Dictionary<ToolStripItem, LauncherItemSelecterType>() {
				{this.toolLauncherItems_type_full, LauncherItemSelecterType.Full},
				{this.toolLauncherItems_type_name, LauncherItemSelecterType.Name},
				{this.toolLauncherItems_type_tag, LauncherItemSelecterType.Tag},
			}[(ToolStripItem)sender];
			
			FilterType = type;
		}
		
		void ToolLauncherItems_filter_Click(object sender, EventArgs e)
		{
			if(this.toolLauncherItems_filter.Checked) {
				Filtering = false;
			}
		}
		
		void ToolLauncherItems_createClick(object sender, EventArgs e)
		{
			CreateLauncherItem();
		}
		
		void ToolLauncherItems_removeClick(object sender, EventArgs e)
		{
			var item = this.listLauncherItems.SelectedItem;
			if(item != null) {
				RemoveLauncherItem((LauncherItem)item);
			}
		}
		
		void ListLauncherItemsSelectedIndexChanged(object sender, EventArgs e)
		{
			if(SelectChangedItem != null) {
				var index = this.listLauncherItems.SelectedIndex;
				var ev = new SelectedItemEventArg();
				if(index != -1) {
					ev.Item = (LauncherItem)this.listLauncherItems.Items[index];
				}
				SelectChangedItem(this, ev);
			}
		}
		
		void ListLauncherItems_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			
			var g = e.Graphics;
			if(e.Index != -1) {
				// TODO: アイコン位置と文字列位置の補正が必要
				var item = (LauncherItem)this.listLauncherItems.Items[e.Index];
				var icon = item.GetIcon(IconScale, item.IconItem.Index, ApplicationSetting);
				if(icon != null) {
					var padding = e.Bounds.Height / 2 - IconScale.ToHeight() / 2;
					g.DrawIcon(icon, e.Bounds.X + padding, e.Bounds.Y + padding);
				}
				var textArea = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
				textArea.X += this.listLauncherItems.ItemHeight;
				textArea.Width -= this.listLauncherItems.ItemHeight;
				using(var brush = new SolidBrush(e.ForeColor)) {
					using(var format = new StringFormat()) {
						format.Alignment = StringAlignment.Near;
						format.LineAlignment = StringAlignment.Center;
						g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
						g.DrawString(item.Name, e.Font, brush, textArea, format);
					}
				}
			}
			
			e.DrawFocusRectangle();
		}
		
		void ToolLauncherItems_input_TextChanged(object sender, EventArgs e)
		{
			Filtering = this.toolLauncherItems_input.TextLength > 0;
		}
	}
}
