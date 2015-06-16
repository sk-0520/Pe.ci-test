namespace ContentTypeTextNet.Library.SharedLibrary.IF
{
	using System;
	using System.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public interface IApplicationDesktopToolbar
	{
		void Docking(DockType dockType);
	}
}
