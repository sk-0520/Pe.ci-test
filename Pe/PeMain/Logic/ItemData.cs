/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 22:03
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using PeMain.Data;
using PeUtility;

namespace PeMain.Logic
{
	/// <summary>
	/// UIに対して項目と名称を設定する。
	/// </summary>
	public class ItemData<T>
	{
		private T _value;
		
		public ItemData(T value)
		{
			this._value = value;
		}
		/// <summary>
		/// 表示文字列
		/// </summary>
		public virtual string Display
		{
			get
			{
				return this._value.ToString();
			}
		}
		/// <summary>
		/// 設定データ
		/// </summary>
		public T Value
		{
			get
			{
				return this._value;
			}
		}
	}
	
	public static class ItemDataUtility
	{
		private static void SetValueAndDisplay(ListControl control)
		{
			control.ValueMember = "Value";
			control.DisplayMember  = "Display";
		}
		
		public static void Attachment<T>(this ComboBox control, IEnumerable<ItemData<T>> itemDatas, T defaultData)
		{
			SetValueAndDisplay(control);
			control.DataSource = itemDatas;
			control.SelectedValue = defaultData;
		}
		public static void Attachment<T>(this ComboBox control, IEnumerable<ItemData<T>> itemDatas)
		{
			control.Attachment(itemDatas, itemDatas.DefaultIfEmpty().First().Value);
		}
		
		public static void Attachment<T>(this ToolStripComboBox control, IEnumerable<ItemData<T>> itemDatas, T defaultData)
		{
			control.ComboBox.Attachment(itemDatas, defaultData);
		}
		public static void Attachment<T>(this ToolStripComboBox control, IEnumerable<ItemData<T>> itemDatas)
		{
			control.ComboBox.Attachment(itemDatas);
		}
		
	}
	
	public class UseLanguageItemData<T>: ItemData<T>
	{
		public UseLanguageItemData(T value): base(value) { }
		public UseLanguageItemData(T value, Language language): base(value)
		{
			Language = language;
		}
		public Language Language { get; set; }
	}
	
	
	public class ToolbarPositionItem: UseLanguageItemData<ToolbarPosition>
	{
		public ToolbarPositionItem(ToolbarPosition value): base(value) { }
		public ToolbarPositionItem(ToolbarPosition value, Language lang): base(value, lang) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}
	
	public class IconScaleItemData: UseLanguageItemData<IconScale>
	{
		public IconScaleItemData(IconScale value): base(value) { }
		public IconScaleItemData(IconScale value, Language language): base(value, language) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}

	class ColorData: ItemData<Color>, ISetLanguage
	{
		private string _displayTitle;
		private string _displayValue;
		Language Language { get; set; }
		
		public ColorData(Color value, string displayTitle, string displayValue): base(value)
		{
			this._displayTitle = displayTitle;
			this._displayValue = displayValue;
		}
		
		public override string Display {
			get
			{
				var title = this._displayTitle;
				var value = this._displayValue;
				if(Language != null) {
					title = Language[title];
					value = Language[value];
				}
				return string.Format("{0}: {1}", title, value);
			}
		}
		
		public void SetLanguage(Language language)
		{
			Language = language;
		}
	}
	
		
	/// <summary>
	/// ランチャ種別のUI用ラッパ。
	/// </summary>
	public class LauncherTypeItem: UseLanguageItemData<LauncherType>
	{
		public LauncherTypeItem(LauncherType value): base(value) { }
		public LauncherTypeItem(LauncherType value, Language lang): base(value, lang) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}
	
}
