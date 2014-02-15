/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/22
 * 時刻: 1:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PeMain.Data;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl_functions.
	/// </summary>
	public partial class LauncherItemSelectControl
	{
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
			var itemheight = Math.Max(iconHeight, fontHeight) + this.listLauncherItems.Padding.Vertical + 1 * 2;
			this.listLauncherItems.ItemHeight = itemheight;
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
		
		public void SetLanguage(Language language)
		{
			ApplyLanguage(language);
			this._language = language;
		}
		
		public void SetItems(IEnumerable<LauncherItem> items)
		{
			this._items.Clear();
			this.listLauncherItems.Items.Clear();
			
			if(items != null) {
				var list = items.ToArray();
				this._items.AddRange(list);
				this.listLauncherItems.Items.AddRange(list);
			}
		}
		
		void CreateLauncherItem()
		{
			var itemName = this._language["control/launcher-selecter/new-item"];
			if(_items.Count > 0) {
				itemName = itemName.ToUnique(_items.Select(i => i.Name));
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
		
		List<LauncherItem> ApplyFilter()
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
			
			return list.Distinct().ToList();
		}
		
	}
}
