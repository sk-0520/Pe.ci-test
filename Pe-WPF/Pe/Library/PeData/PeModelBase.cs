namespace ContentTypeTextNet.Pe.Library.PeData
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	/// <summary>
	/// Peで使用するデータ。
	/// </summary>
	[DataContract, Serializable]
	public abstract class PeDataBase: DisposeFinalizeModelBase, IPeData
	{ }
}
