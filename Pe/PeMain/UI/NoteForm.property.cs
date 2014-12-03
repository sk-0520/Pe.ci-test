/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/12
 * 時刻: 4:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class NoteForm
	{
		CommonData CommonData { get; set; }
		
		/// <summary>
		/// データそのものが削除された際に真。
		/// </summary>
		public bool Removed { get; private set; }
		
		public NoteItem NoteItem 
		{ 
			get
			{
				return this._bindItem.NoteItem;
			}
			set
			{
				this._bindItem = new NoteBindItem(value);
			}
		}
		bool Changed
		{
			get 
			{
				return this._changed;
			}
			set
			{
				if(this._initialized) {
					this._changed = value;
				}
			}
		}
	}
}
