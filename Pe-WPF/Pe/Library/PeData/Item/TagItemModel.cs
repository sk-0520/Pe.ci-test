﻿namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// タグを管理。
	/// </summary>
	[Serializable]
	public class TagItemModel: PeDataBase, IDeepClone
	{
		public TagItemModel() 
			: base() 
		{
			Items = new CollectionModel<string>();
		}

		/// <summary>
		/// タグ。
		/// </summary>
		[DataMember, XmlArray("Items"), XmlArrayItem("Item")]
		public CollectionModel<string> Items { get; set; }

		#region IDeepClone
		
		public IDeepClone DeepClone()
		{
			var result = new TagItemModel();

			result.Items = new CollectionModel<string>(Items);

			return result;
		}

		#endregion
	}
}
