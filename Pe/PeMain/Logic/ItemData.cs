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
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeUtility;

namespace PeMain.Logic
{
	/// <summary>
	/// Description of DataMember.
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
	
	public class UseLanguageItemData<T>: ItemData<T>
	{
		public UseLanguageItemData(T value): base(value) { }
		public UseLanguageItemData(T value, Language lang): base(value) 
		{
			Language = lang;
		}
		public Language Language { get; set; }
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
			control.DataSource = itemDatas;
			SetValueAndDisplay(control);
			control.SelectedValue = defaultData;
		}
		public static void Attachment<T>(this ComboBox control, IEnumerable<ItemData<T>> itemDatas)
		{
			control.Attachment(itemDatas, itemDatas.DefaultIfEmpty().First().Value);
		}
	}
	
	public class IconSizeItem: UseLanguageItemData<IconSize>
	{
		public IconSizeItem(IconSize value): base(value) { }
		public IconSizeItem(IconSize value, Language lang): base(value, lang) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}
	

	
}
