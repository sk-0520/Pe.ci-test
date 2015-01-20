namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Windows.Forms;

	public static class Startup
	{
		/// <summary>
		/// TODO: ちょっと分けたい^^;
		/// </summary>
		/// <param name="args"></param>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			ContentTypeTextNet.Pe.PeMain.Logic.ApplicationRoot.Execute(args);
		}
	}
}
