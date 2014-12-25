using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	public class HashModel
	{
		public HashModel()
		{
			HashSetting = new HashSetting();
		}

		public HashSetting HashSetting { get; set; }
	}
}
