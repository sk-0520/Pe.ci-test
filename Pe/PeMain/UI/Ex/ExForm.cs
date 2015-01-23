namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExForm: Form
	{ }

	/// <summary>
	/// アプリケーションで使用する基本Form。
	/// </summary>
	public class CommonForm: ExForm, ISetCommonData
	{
		protected CommonData CommonData { get; set; }
		protected bool CommonDataSetting { get; set; }

		public void SetCommonData(CommonData commonData)
		{
			CommonDataSetting = true;

			CommonData = commonData;
			ApplySetting();

			CommonDataSetting = false;
		}

		protected virtual void ApplySetting()
		{
			Debug.Assert(CommonData != null);

			ApplyLanguage();
			ApplySkin();
		}

		protected virtual void ApplyLanguage()
		{
			Debug.Assert(CommonData.Language != null);
		}

		protected virtual void ApplySkin()
		{
			Debug.Assert(CommonData.Skin != null);
		}
	}
}
