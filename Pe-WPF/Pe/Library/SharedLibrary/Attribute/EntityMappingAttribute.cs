namespace ContentTypeTextNet.Library.SharedLibrary.Attribute
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
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
	public sealed class EntityMappingAttribute: System.Attribute
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
}
