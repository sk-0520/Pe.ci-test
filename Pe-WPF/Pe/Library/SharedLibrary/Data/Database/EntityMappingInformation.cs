namespace ContentTypeTextNet.Library.SharedLibrary.Data.Database
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Attribute;

	/// <summary>
	/// 物理名・プロパティ紐付。
	/// 
	/// TargetNameAttributeに紐付く物理名とプロパティ情報。
	/// </summary>
	public sealed class EntityMappingInformation
	{
		public EntityMappingInformation(EntityMappingAttribute attribute, PropertyInfo propertyInfo)
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
}
