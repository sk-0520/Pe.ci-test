namespace ContentTypeTextNet.Library.SharedLibrary.Data.Database
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;
	using ContentTypeTextNet.Library.SharedLibrary.Data.Database;

	/// <summary>
	/// エンティティ一覧情報
	/// 
	/// エンティティとして必要な物理名とエンティティオブジェクトのプロパティ一覧。
	/// </summary>
	public sealed class EntityMappingSet
	{
		public EntityMappingSet(string tableName, IList<EntityMappingInformation> targetInfos)
		{
			TableName = tableName;
			TargetInfos = targetInfos;
		}
		/// <summary>
		/// テーブル名。
		/// </summary>
		public string TableName { get; private set; }
		/// <summary>
		/// 対象TargetInfoの集合
		/// </summary>
		public IList<EntityMappingInformation> TargetInfos { get; private set; }
	}
}
