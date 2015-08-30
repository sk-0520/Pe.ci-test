namespace ContentTypeTextNet.Pe.Library.Utility.DB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Dto, Row で使用するカラム名。
	/// 
	/// 行の場合はテーブル名まで指定する。
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Class | AttributeTargets.Property,
		AllowMultiple = true,
		Inherited = true
	)]
	public sealed class EntityMappingAttribute: Attribute
	{
		public EntityMappingAttribute(string physicalName, bool primaryKey)
		{
			PhysicalName = physicalName;
			PrimaryKey = primaryKey;
		}

		public EntityMappingAttribute(string physicalName)
			: this(physicalName, false)
		{ }
		/// <summary>
		/// 物理名
		/// </summary>
		public string PhysicalName { get; private set; }
		/// <summary>
		/// 主キー
		/// </summary>
		public bool PrimaryKey { get; private set; }
	}

	/// <summary>
	/// 物理名・プロパティ紐付。
	/// 
	/// TargetNameAttributeに紐付く物理名とプロパティ情報。
	/// </summary>
	public sealed class EntityMappingInfo
	{
		public EntityMappingInfo(EntityMappingAttribute attribute, PropertyInfo propertyInfo)
		{
			EntityMappingAttribute = attribute;
			PropertyInfo = propertyInfo;
		}
		public EntityMappingAttribute EntityMappingAttribute { get; private set; }
		/// <summary>
		/// TargetNameAttributeで紐付くプロパティ。
		/// </summary>
		public PropertyInfo PropertyInfo { get; private set; }
	}

	/// <summary>
	/// エンティティ一覧情報
	/// 
	/// エンティティとして必要な物理名とエンティティオブジェクトのプロパティ一覧。
	/// </summary>
	public sealed class EntityMappingSet
	{
		public EntityMappingSet(string tableName, IList<EntityMappingInfo> targetInfos)
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
		public IList<EntityMappingInfo> TargetInfos { get; private set; }
	}
	


}
