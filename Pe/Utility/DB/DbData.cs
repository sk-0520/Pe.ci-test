namespace ContentTypeTextNet.Pe.Library.Utility.DB
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Entity, DTO の共通クラス。
	/// 
	/// 特に何もしないが継承クラスではTargetNameAttributeを当てて使用する。
	/// </summary>
	public abstract class DbData
	{ }

	/// <summary>
	/// テーブル行に対応
	/// TargetNameAttributeを当てて使用する。
	/// </summary>
	public abstract class Entity: DbData
	{ }

	/// <summary>
	/// データ取得単位に対応
	/// TargetNameAttributeを当てて使用する。
	/// </summary>
	public abstract class Dto: DbData
	{ }

}
