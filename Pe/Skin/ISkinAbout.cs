namespace ContentTypeTextNet.Pe.Library.Skin
{
	using System;

	public interface ISkinAbout
	{
		string Name { get; }
		string Author { get; }
		Uri Website { get; }
		bool Setting { get; }
	}
}
