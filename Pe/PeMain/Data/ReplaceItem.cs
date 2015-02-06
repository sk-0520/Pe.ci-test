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

		public string Comment { get; private set; }

		public string ReplaceWord { get { return string.Format("@[{0}]", Name); } }

		#region ISetLanguage

		public void SetLanguage(Language lang)
		{
			Comment = lang["template/replace/name/" + Name];
		}

		#endregion
	}

}
