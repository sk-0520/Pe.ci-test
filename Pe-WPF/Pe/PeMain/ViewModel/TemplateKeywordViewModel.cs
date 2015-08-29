﻿namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	public class TemplateKeywordViewModel: ViewModelBase, IHavingNonProcess
	{
		public TemplateKeywordViewModel(string key, bool isProgram, Tuple<string,string> bracket, INonProcess nonProcess)
		{
			Key = key;
			IsProgram = isProgram;
			Bracket = bracket;
			NonProcess = nonProcess;
		}

		#region property

		bool IsProgram { get; set; }

		string Key { get; set; }

		public string Title { get { return Key; } }
		public string Comment { 
			get{
				string langKey;
				if(IsProgram) {
					langKey = "template/keyword/program";
				} else {
					langKey = "template/keyword/text";
				}

				return NonProcess.Language[langKey + "/" + Key];
			} 
		}
		public string Keyword
		{
			get
			{
				return Key;
			}
		}

		Tuple<string,string> Bracket { get; set; }

		/// <summary>
		/// Nameの示す型。
		/// </summary>
		public Type Type { get; set; }
		/// <summary>
		/// 置き換え文字列を挿入時にスペース内部にキャレットを移動させるか。
		/// </summary>
		public bool CaretInSpace { get; set; }


		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion
	}
}
