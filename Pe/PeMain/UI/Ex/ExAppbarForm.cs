namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System.Diagnostics;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExAppbarForm: AppbarForm
	{ }

	public class CommonAppbarForm: ExAppbarForm, ISetCommonData
	{
		public CommonAppbarForm()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();

			this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));

			this.ResumeLayout(false);
		}

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
