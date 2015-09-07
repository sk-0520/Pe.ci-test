namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.Define;

	public interface IToolbarNode
	{
		ToolbarNodeKind ToolbarNodeKind { get; }
		bool IsExpanded { get; }
		bool IsSelected { get; set; }
		string Name { get; set; }
		bool CanEdit { get; }
	}
}
