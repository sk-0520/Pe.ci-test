namespace ContentTypeTextNet.Pe.PeMain.ViewModel
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
		public TemplateKeywordViewModel(string key, bool isProgram, INonProcess nonProcess)
		{
			Key = key;
			IsProgram = isProgram;
			NonProcess = nonProcess;
		}

		#region property

		bool IsProgram { get; set; }

		public string Key { get; private set; }

		public string KeywordTitle { get { return Key; } }
		public string KeywordComment { 
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

		/// <summary>
		/// Nameの示す型。
		/// </summary>
		public Type Type { get; set; }
		/// <summary>
		/// 置き換え文字列を挿入時にスペース内部にキャレットを移動させるか。
		/// </summary>
		public bool CaretInSpace { get; set; }


		#endregion

		#region function
		/// <summary>
		/// Typeをキャストとして適応する。
		/// </summary>
		/// <param name="word"></param>
		/// <returns></returns>
		string GetReplaceWord(string word)
		{
			if(Type != null) {
				return string.Format("({0}){1}", Type.Name, word);
			}
			return word;
		}
		#endregion

		#region IHavingNonProcess

		public INonProcess NonProcess { get; private set; }

		#endregion
	}
}
