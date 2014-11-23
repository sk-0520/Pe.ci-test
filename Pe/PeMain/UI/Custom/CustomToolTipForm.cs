using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.IF;

namespace PeMain.UI
{
	public partial class CustomToolTipForm: Form, ISetCommonData
	{
		CommonData CommonData { get; set; }

		public CustomToolTipForm()
		{
			InitializeComponent();
		}

		void ApplyLanguage()
		{ }
		void ApplySetting()
		{
			ApplyLanguage();
		}

		public void SetCommonData(CommonData commonData)
		{
			CommonData = CommonData;

			ApplySetting();
		}
		
 
	}
}
