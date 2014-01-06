﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:16
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// Description of ToolbarForm_initialize.
	/// </summary>
	public partial class ToolbarForm
	{
		void InitializeUI()
		{
			bool isAero;
			API.DwmIsCompositionEnabled(out isAero);
			if(isAero) {
				var margin = new MARGINS();
				margin.leftWidth = -1;
				//API.DwmExtendFrameIntoClientArea(Handle, ref margin);
			}
			var frame = SystemInformation.Border3DSize;
			frame.Height = 0;
			Padding = new Padding(frame.Width, frame.Height, frame.Width, frame.Height);
		}
		
		void Initialize()
		{
			InitializeUI();
		}
	}
}
