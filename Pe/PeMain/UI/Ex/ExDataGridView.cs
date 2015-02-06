namespace ContentTypeTextNet.Pe.PeMain.UI.Ex
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;

	#region abstract

	public abstract class ExDataGridView: DataGridView
	{ }

	public abstract class ExDataGridViewButtonColumn: DataGridViewButtonColumn
	{ }

	public abstract class ExDataGridViewButtonCell: DataGridViewButtonCell
	{ }

	#endregion

	#region ExDataGridViewButtonColumn

	public abstract class NoteDataGridViewButtonColumn: ExDataGridViewButtonColumn
	{ }

	public class NoteFontDataGridViewButtonColumn: NoteDataGridViewButtonColumn
	{
		public override DataGridViewCell CellTemplate
		{
			get
			{
				return new NoteFontDataGridViewButtonCell();
			}
			set
			{
				if(value is NoteFontDataGridViewButtonCell) {
					base.CellTemplate = value;
				} else {
					throw new ArgumentException();
				}
			}
		}
	}

	public class NoteColorDataGridViewButtonColumn: NoteDataGridViewButtonColumn
	{ }

	#endregion

	#region ExDataGridViewButtonCell

	public abstract class NoteDataGridViewButtonCell: ExDataGridViewButtonCell
	{ }

	public class NoteFontDataGridViewButtonCell: NoteDataGridViewButtonCell
	{ }

	public class NoteColorDataGridViewButtonCell: NoteDataGridViewButtonCell
	{ }

	#endregion
}
