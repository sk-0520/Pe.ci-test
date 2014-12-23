using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	[Serializable]
	public class ApplicationFile: NameItem
	{
		[XmlAttribute]
		public string Directory { get; set; }
	}

	[Serializable]
	public class ApplicationItem: NameItem
	{
		public ApplicationFile File { get; set; }
	}

	[Serializable]
	public class Applications
	{
		public Applications()
		{
			Items = new List<ApplicationItem>();
		}

		public List<ApplicationItem> Items { get; set; }
	}
}
