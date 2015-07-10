using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	[DataContract, Serializable]
	public class NoteIndexSettingModel: SettingModelBase
	{
		public NoteIndexSettingModel()
			:base()
		{
			Items = new NoteItemCollectionModel();
		}

		[DataMember]
		public NoteItemCollectionModel Items { get; set; }
	}
}
