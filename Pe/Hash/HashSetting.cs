using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentTypeTextNet.Pe.Applications.Hash
{
	public class HashSetting
	{
		public HashSetting()
		{
			X = 100;
			Y = 100;

			Width = 150;
			Height = 150;
		}

		public int X { get; set; }
		public int Y { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
	}
}
