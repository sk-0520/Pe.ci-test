namespace ContentTypeTextNet.Library.SharedLibrary.Attribute
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
	public class PixelKindAttribute : System.Attribute
	{
		public PixelKindAttribute(Px px)
		{
			Px = px;
		}

		#region property

		public Px Px { get; private set; }

		#endregion
	}
}
