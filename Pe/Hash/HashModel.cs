using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	public enum HashType
	{
		SHA1,
		MD5,
		CRC32,
	}

	public class HashModel
	{
		public HashModel()
		{
			HashSetting = new HashSetting();
		}

		public HashSetting HashSetting { get; set; }

		public string FilePath { get; set; }
		public HashType HashType { get; set; }
		public string SHA1 { get; set; }
		public string MD5 { get; set; }
		public string CRC32 { get; set; }
		public string Compare { get; set; }
	}
}
