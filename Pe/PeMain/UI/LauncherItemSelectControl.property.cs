/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/22
 * 時刻: 1:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Setting;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl_property.
	/// </summary>
	public partial class LauncherItemSelectControl
	{
		public Language Language
		{
			get 
			{
				return this._language;
			}
			set
			{
				ApplyLanguage(value);
				this._language = value;
			}
		}
		
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
	}
}
