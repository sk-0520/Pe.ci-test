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
	public class DisplayValue<T>
	{
		private T _value;
		
		public DisplayValue(T value)
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
	
	public static class DisplayValueUtility
	{
		private static void SetValueAndDisplay(ListControl control)
		{
			control.ValueMember = "Value";
			control.DisplayMember  = "Display";
		}
		
		public static void Attachment<T>(this ComboBox control, IEnumerable<DisplayValue<T>> itemDatas, T defaultData)
		{
			SetValueAndDisplay(control);
			control.DataSource = itemDatas;
			control.SelectedValue = defaultData;
		}
		public static void Attachment<T>(this ComboBox control, IEnumerable<DisplayValue<T>> itemDatas)
		{
			control.Attachment(itemDatas, itemDatas.DefaultIfEmpty().First().Value);
		}
		
		public static void Attachment<T>(this ToolStripComboBox control, IEnumerable<DisplayValue<T>> itemDatas, T defaultData)
		{
			control.ComboBox.Attachment(itemDatas, defaultData);
		}
		public static void Attachment<T>(this ToolStripComboBox control, IEnumerable<DisplayValue<T>> itemDatas)
		{
			control.ComboBox.Attachment(itemDatas);
		}
		
	}
	
	public class UseLanguageDisplayValue<T>: DisplayValue<T>
	{
		public UseLanguageDisplayValue(T value): base(value) { }
		public UseLanguageDisplayValue(T value, Language language): base(value)
		{
			Language = language;
		}
		public Language Language { get; set; }
	}
	
	
	public class ToolbarPositionDisplayValue: UseLanguageDisplayValue<ToolbarPosition>
	{
		public ToolbarPositionDisplayValue(ToolbarPosition value): base(value) { }
		public ToolbarPositionDisplayValue(ToolbarPosition value, Language lang): base(value, lang) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}
	
	public class IconScaleDisplayValue: UseLanguageDisplayValue<IconScale>
	{
		public IconScaleDisplayValue(IconScale value): base(value) { }
		public IconScaleDisplayValue(IconScale value, Language language): base(value, language) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}

	class ColorDisplayValue: DisplayValue<Color>, ISetLanguage
	{
		private string _displayTitle;
		private string _displayValue;
		Language Language { get; set; }
		
		public ColorDisplayValue(Color value, string displayTitle, string displayValue): base(value)
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
	public class LauncherTypeDisplayValue: UseLanguageDisplayValue<LauncherType>
	{
		public LauncherTypeDisplayValue(LauncherType value): base(value) { }
		public LauncherTypeDisplayValue(LauncherType value, Language lang): base(value, lang) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}
	
}
