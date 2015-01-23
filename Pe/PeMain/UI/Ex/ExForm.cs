namespace ContentTypeTextNet.Pe.PeMain.UI
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
		/// <summary>
		/// 共通データ。
		/// </summary>
		protected CommonData CommonData { get; set; }
		/// <summary>
		/// 初期化済みであるか。
		/// 
		/// ここで言う初期化は SetCommonData を実施済みであるかという意味。
		/// </summary>
		protected bool Initialized { get; set; }

		public void SetCommonData(CommonData commonData)
		{
			Initialized = false;

			CommonData = commonData;
			ApplySetting();

			Initialized = true;
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
