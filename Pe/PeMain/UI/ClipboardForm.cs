using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	public partial class ClipboardForm: Form, ISetCommonData
	{
		#region Variable
		#endregion

		public ClipboardForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region Property

		CommonData CommonData { get; set; }

		#endregion

		#region Initialize

		void Initialize()
		{ }

		#endregion

		#region Language

		private void ApplyLanguage()
		{
			throw new NotImplementedException();
		}
		
		#endregion

		#region Function

		public void SetCommonData(CommonData commonData)
		{
			CommonData = commonData;

			ApplySetting();
		}

		void ApplySetting()
		{
			ApplyLanguage();
		}


		#endregion
	}
}
