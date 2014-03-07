/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 03/07/2014
 * 時刻: 00:34
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace PeUtility
{
	public struct InformationGroup
	{
		private string _title;
		private Dictionary<string, string> _items;
		public InformationGroup(string title, Dictionary<string, string> items)
		{
			this._title = title;
			this._items = items;
		}
		
		public string Title { get { return this._title; } }
		public Dictionary<string, string> Items { get { return this._items; } }
	}
	/// <summary>
	/// Description of Information.
	/// </summary>
	public class Information
	{
		public Information()
		{
			Groups = new List<InformationGroup>();
		}
		
		public List<InformationGroup> Groups { get; private set; }
		
		/// <summary>
		/// メモリ情報取得
		/// </summary>
		public virtual void GatheringMemory()
		{
			var map = new Dictionary<string, string>();
			
			Groups.Add(new InformationGroup("Memory", map));
		}
		
		/// <summary>
		/// ドライブ情報取得
		/// </summary>
		public virtual void GatheringDrive()
		{
			var map = new Dictionary<string, string>();
			
			Groups.Add(new InformationGroup("Drive", map));
		}
		
		/// <summary>
		/// CPU情報取得
		/// </summary>
		public virtual void GatheringCPU()
		{
			var map = new Dictionary<string, string>();
			
			Groups.Add(new InformationGroup("CPU", map));
		}
		
		/// <summary>
		/// OS情報取得
		/// </summary>
		public virtual void GatheringOS()
		{
			var map = new Dictionary<string, string>();
			
			Groups.Add(new InformationGroup("OS", map));
		}
		
		/// <summary>
		/// ディスプレイ情報取得
		/// </summary>
		public virtual void GatheringDisplay()
		{
			var map = new Dictionary<string, string>();
			
			Groups.Add(new InformationGroup("Display", map));
		}
		
		public virtual void GatheringAll()
		{
			GatheringCPU();
			GatheringDisplay();
			GatheringDrive();
			GatheringMemory();
			GatheringOS();
		}
		
	}
}
