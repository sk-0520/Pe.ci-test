/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 14:06
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PeUtility
{
	public partial class AppbarForm
	{
		/// <summary>
		/// ドッキングするディスプレイ
		/// </summary>
		public Screen DockScreen { get; set; }
		/// <summary>
		/// ドッキングタイプ
		/// </summary>
		public virtual DesktopDockType DesktopDockType
		{
			get
			{
				return this._desktopDockType;
			}
			set
			{
				Docking(value);
				this._desktopDockType = value;
			}
		}
		
		public bool AutoHide { get; set; }
		
		/// <summary>
		/// ドッキングしているか
		/// </summary>
		public bool IsDocking { get; private set; }
		/// <summary>
		/// ドッキング状態でのバーサイズ
		/// 
		/// 左右: Width
		/// 上下: Height
		/// </summary>
		public Size BarSize { get; set; }
		/// <summary>
		/// 自動的に隠れた状態から復帰する際に使用する領域
		/// </summary>
		public Size ShowBarSize { get; private set; }
		/// <summary>
		/// 自動的に隠れた際のサイズ。
		/// 各片がドッキング位置に対応。
		/// </summary>
		public Padding HiddenSize { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string MessageString { get; set; }
	}
}
