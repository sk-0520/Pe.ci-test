using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PeMain.UI
{
	public abstract class ExToolTip: ToolTip
	{
		public ExToolTip(): base()
		{ }

		public ExToolTip(IContainer cont): base(cont)
		{ }
	}
}
