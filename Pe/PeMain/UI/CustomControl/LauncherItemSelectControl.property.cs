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
using PeMain.Data;
using PeSkin;
using ContentTypeTextNet.Pe.Library.Utility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl_property.
	/// </summary>
	partial class LauncherItemSelectControl
	{
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
		
		public IEnumerable<LauncherItem> ViewItems
		{
			get
			{
				return this._viewItems;
			}
		}
		
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
	}
}
