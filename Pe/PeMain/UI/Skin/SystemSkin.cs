/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/07
 * 時刻: 21:36
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Windows.Forms;

using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SystemSkin.
	/// </summary>
	public class SystemSkin: Skin
	{
		private void SetVisualStyle(Form target)
		{
			Debug.Assert(EnabledVisualStyle);
			
			var margin = new MARGINS();
			margin.leftWidth = -1;
			API.DwmExtendFrameIntoClientArea(target.Handle, ref margin);
		}
		public override void Start(Form target)
		{
			base.Start(target);
			
			if(EnabledVisualStyle) {
				SetVisualStyle(target);
			}
		}
		
		public override void Close(Form target)
		{
			if(EnabledVisualStyle) {
				var margin = new MARGINS();
				margin.leftWidth = 0;
				margin.rightWidth = 0;
				margin.topHeight = 0;
				margin.bottomHeight = 0;
				API.DwmExtendFrameIntoClientArea(target.Handle, ref margin);
			}
		}
		
		

	}
}
