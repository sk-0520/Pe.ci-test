namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

	public abstract class ExNumericUpDown: NumericUpDown
	{ }

	public class DefaultValueNumericUpDown:ExNumericUpDown
	{
		public decimal DefaultValue { get; set; }
	}

	public class RevertDefaultValueNumericUpDown: DefaultValueNumericUpDown, ISetLanguage, ILanguage
	{
		public Language Language { get; private set; }

		public void SetLanguage(Language language)
		{
			Language = language;
		}
	}
}
