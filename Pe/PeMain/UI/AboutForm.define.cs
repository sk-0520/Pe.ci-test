using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PeMain.UI
{
	partial class AboutForm
	{
		class ComponentInfo
		{
			public ComponentInfo(XElement x)
			{
				Name = x.Attribute("name").Value;
				Type = x.Attribute("type").Value;
				URI = x.Attribute("uri").Value;
				License = x.Attribute("license").Value;
			}

			public string Name { get; private set; }
			public string Type { get; private set; }
			public string URI { get; private set; }
			public string License { get; private set; }
		}
	}
}
