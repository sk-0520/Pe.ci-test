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
				if(Bracket == null) {
					return Name;
				} else {
					return Bracket.Item1 + Name + Bracket.Item2;
				}
			}
		}

		public Tuple<string, string> Bracket { get; set; }

		public override void SetLanguage(Language lang)
		{
			Comment = lang["template/replace/program/name/" + Name];
		}
	}

}
