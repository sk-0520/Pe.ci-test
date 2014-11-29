using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeMain.UI
{
	partial class AboutForm
	{
		class ComponentInfo
		{
			public ComponentInfo(string name, string type, string uri)
			{
				Name = name;
				Type = type;
				URI = uri;
			}

			public string Name { get; private set; }
			public string Type { get; private set; }
			public string URI { get; private set; }
		}
	}
}
