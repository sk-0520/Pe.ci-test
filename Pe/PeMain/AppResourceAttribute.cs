namespace ContentTypeTextNet.Pe.PeMain
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class AppResourceAttribute: Attribute
	{
		public AppResourceAttribute(AppResourceType appResourceType)
		{
			AppResourceType = appResourceType;
		}

		#region property

		public AppResourceType AppResourceType { get; private set; }

		#endregion
	}
}
