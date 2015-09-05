namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	public class TemplateKeywordViewModel: ViewModelBase, IHavingNonProcess
	{
		public TemplateKeywordViewModel(string key, TemplateReplaceMode replaceMode, Tuple<string, string> bracket, INonProcess nonProcess)
		{
			CheckUtility.DebugEnforce(replaceMode != TemplateReplaceMode.None);

			Key = key;
			TemplateReplaceMode = replaceMode;
			Bracket = bracket;
			NonProcess = nonProcess;
		}

		#region property

		TemplateReplaceMode TemplateReplaceMode { get; set; }

		string Key { get; set; }

		public string Title { get { return Key; } }
		public string Comment { 
			get{
				string langKey;
				if(TemplateReplaceMode == TemplateReplaceMode.Program) {
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
				var result = new StringBuilder();

				if(Type != null) {
					result.Append("(");
					result.Append(Type.Name);
					result.Append(")");
				}
				if(Bracket != null) {
					result.Append(Bracket.Item1);
				}
				result.Append(Key);
				if(Bracket != null) {
					result.Append(Bracket.Item2);
				}

				return result.ToString();
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
