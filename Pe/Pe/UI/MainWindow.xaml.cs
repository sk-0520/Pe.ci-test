/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/08/29
 * 時刻: 22:57
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Pe.UI
{
	/// <summary>
	/// メインウィンドウ。
	/// 
	/// メインウィンドウはPe内のタスクManagerとして使用され、
	/// 基本的に表には出てこないものとする。
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			
			Initialize();
		}
	}
}