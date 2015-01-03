/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 22:03
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.Library.Skin;

namespace ContentTypeTextNet.Pe.PeMain.Logic
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
	
	/// <summary>
	/// DisplayValue共通処理。
	/// </summary>
	public static class DisplayValueUtility
	{
		private static void SetValueAndDisplay(ListControl control)
		{
			control.ValueMember = "Value";
			control.DisplayMember  = "Display";
		}
		
		/// <summary>
		/// 接続。
		/// </summary>
		/// <param name="control"></param>
		/// <param name = "values"></param>
		/// <param name="defaultData"></param>
		public static void Attachment<T>(this ComboBox control, IEnumerable<DisplayValue<T>> values, T defaultData)
		{
			SetValueAndDisplay(control);
			control.DataSource = values.ToArray();
			control.SelectedValue = defaultData;
		}
		/// <summary>
		/// 接続。
		/// </summary>
		/// <param name="control"></param>
		/// <param name="values"></param>
		public static void Attachment<T>(this ComboBox control, IEnumerable<DisplayValue<T>> values)
		{
			control.Attachment(values, values.DefaultIfEmpty().First().Value);
		}
		
		/// <summary>
		/// 接続。
		/// </summary>
		/// <param name="control"></param>
		/// <param name="values"></param>
		/// <param name="defaultData"></param>
		public static void Attachment<T>(this ToolStripComboBox control, IEnumerable<DisplayValue<T>> values, T defaultData)
		{
			control.ComboBox.Attachment(values, defaultData);
		}
		/// <summary>
		/// 接続。
		/// </summary>
		/// <param name="control"></param>
		/// <param name="values"></param>
		public static void Attachment<T>(this ToolStripComboBox control, IEnumerable<DisplayValue<T>> values)
		{
			control.ComboBox.Attachment(values);
		}
		
	}
	
	public abstract class UseLanguageDisplayValue<T>: DisplayValue<T>, ISetLanguage
	{
		public UseLanguageDisplayValue(T value): base(value) { }
		
		public Language Language { get; private set; }
		
		public void SetLanguage(Language language)
		{
			Language = language;
		}
	}
	
	
	public class ToolbarPositionDisplayValue: UseLanguageDisplayValue<ToolbarPosition>
	{
		public ToolbarPositionDisplayValue(ToolbarPosition value): base(value) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}
	
	public class IconScaleDisplayValue: UseLanguageDisplayValue<IconScale>
	{
		public IconScaleDisplayValue(IconScale value): base(value) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}

	class ColorDisplayValue: UseLanguageDisplayValue<Color>
	{
		private string _displayTitle;
		private string _displayValue;
		
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
	}

	public class ApplicationDisplayValue: UseLanguageDisplayValue<ApplicationItem>
	{
		public ApplicationDisplayValue(ApplicationItem value) : base(value) { }
		public override string Display
		{
			get
			{
				var key = string.Format("applications/{0}", Value.LanguageKey);
				return Language[key];
			}
		}
	}

	/// <summary>
	/// ランチャ種別のUI用ラッパ。
	/// </summary>
	public class LauncherTypeDisplayValue: UseLanguageDisplayValue<LauncherType>
	{
		public LauncherTypeDisplayValue(LauncherType value): base(value) { }
		
		public override string Display { get { return Value.ToText(Language); } }
	}
	
	public class ToolbarDisplayValue: DisplayValue<ToolbarItem>
	{
		public ToolbarDisplayValue(ToolbarItem value): base(value) { }
		
		public override string Display { get { return ScreenUtility.GetScreenName(Value.Name, null); } }
	}
	
	public class ToolbarGroupNameDisplayValue: DisplayValue<string>
	{
		public ToolbarGroupNameDisplayValue(string value): base(value) { }
	}
	
	public class LanguageDisplayValue: DisplayValue<Language>
	{
		public LanguageDisplayValue(Language value): base(value) { }
		
		public override string Display 
		{ 
			get 
			{ 
				return string.Format("{0}({1})", Value.Name, Value.Code);
			}
		}
	}

	
	

}
