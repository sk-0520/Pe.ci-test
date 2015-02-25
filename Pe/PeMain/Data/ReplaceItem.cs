namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class ReplaceItem: NameItem, ISetLanguage
	{
		public Language Language { get; private set; }

		public string Comment { get; protected set; }

		public virtual string ReplaceWord { get { return string.Format("@[{0}]", Name); } }

		#region ISetLanguage

		public virtual void SetLanguage(Language lang)
		{
			Comment = lang["template/replace/text/name/" + Name];
		}

		#endregion
	}

	public class ProgramReplaceItem : ReplaceItem
	{
		public override string ReplaceWord
		{
			get
			{
				var name = Bracket == null
					? Name
					: Bracket.Item1 + Name + Bracket.Item2;
				;

				return GetReplaceWord(name);
			}
		}

		/// <summary>
		/// Nameの前後に設定する文字列。
		/// </summary>
		public Tuple<string, string> Bracket { get; set; }
		/// <summary>
		/// Nameの示す型。
		/// </summary>
		public Type Type { get; set; }
		/// <summary>
		/// 置き換え文字列を挿入時にスペース内部にキャレットを移動させるか。
		/// </summary>
		public bool CaretInSpace { get; set; }

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

		public override void SetLanguage(Language lang)
		{
			Comment = lang["template/replace/program/name/" + Name];
		}
	}

}
