namespace ContentTypeTextNet.Pe.Library.Skin
{
	using System;

	public class SkinAbout: ISkinAbout
	{
		protected string _name;
		protected string _author;
		protected Uri _website;
		protected bool _setting;

		public SkinAbout(string name, string author, Uri website, bool setting)
		{
			this._name = name;
			this._author = author;
			this._website = website;
			this._setting = setting;
		}

		public string Name { get { return this._name; } }
		public string Author { get { return this._author; } }
		public Uri Website { get { return this._website; } }
		public bool Setting { get { return this._setting; } }
	}
}
