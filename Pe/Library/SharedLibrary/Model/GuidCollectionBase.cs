﻿namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	[Serializable]
	public abstract class GuidCollectionBase<TValue>: TIdCollectionModel<Guid, TValue>
		where TValue: GuidModelBase
	{
		public GuidCollectionBase()
			: base()
		{ }
	}
}
