namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Data;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public class DialogFilterList: List<DialogFilterItem>
	{
		public string FilterText
		{
			get {
				return string.Join("|", this.Select(i => i.ToString()));
			}
		}
	}
}
