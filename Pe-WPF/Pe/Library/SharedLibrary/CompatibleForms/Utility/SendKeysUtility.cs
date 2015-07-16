namespace ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Forms = System.Windows.Forms;

	public static class SendKeysUtility
	{
		/// <summary>
		/// なんだかなぁ。
		/// </summary>
		/// <param name="keys"></param>
		public static void Send(string keys)
		{
			Task.Run(() => {
				Forms.SendKeys.SendWait(keys);
			});
		}
	}
}
