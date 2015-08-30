namespace ContentTypeTextNet.Pe.PeMain.IF
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public interface ICommonData: IHavingCommonData
	{
		void SetCommonData(CommonData commonData, object extensionData);
	}
}
