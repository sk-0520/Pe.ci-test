using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
	public class InformationGroup
	{
		public InformationGroup(string title)
		{
			Title = title;
			Items = new Dictionary<string, object>();
		}

		public string Title { get; private set; }
		public Dictionary<string, object> Items { get; private set; }

		public override string ToString()
		{
			var stream = new StringWriter();
			stream.WriteLine("{0} =================", Title);
			foreach (var pair in Items) {
				stream.WriteLine("{0}: {1}", pair.Key, pair.Value);
			}
			return stream.ToString();
		}
	}
}
