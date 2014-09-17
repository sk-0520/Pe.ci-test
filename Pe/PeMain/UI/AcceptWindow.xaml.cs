/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/17
 * 時刻: 22:44
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
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Interaction logic for AcceptWindow.xaml
	/// </summary>
	public partial class AcceptWindow : Window, ISetCommonData
	{
		public AcceptWindow()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// このクラスには不完全なCommonDataが流れ込んでくることに注意。
		/// </summary>
		/// <param name="commonData"></param>
		public void SetCommonData(CommonData commonData)
		{
			
		}
		
	}
}