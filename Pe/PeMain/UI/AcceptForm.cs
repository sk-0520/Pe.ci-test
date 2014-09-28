﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 09/21/2014
 * 時刻: 10:08
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of AcceptForm.
	/// </summary>
	public partial class AcceptForm : Form, ISetCommonData
	{
		public AcceptForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			Initialize();
		}
		
		void CommandAccept_Click(object sender, EventArgs e)
		{
			CommonData.MainSetting.RunningInfo.CheckUpdate = this.selectUpdateCheck.Checked;
			CommonData.MainSetting.RunningInfo.CheckUpdateRC = this.selectUpdateCheckRC.Checked;
			
			DialogResult = DialogResult.OK;
		}
	}
}
