using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// 条件が偽の場合にコマンドと条件式のどちらを使用するか。
	/// </summary>
	public enum FalseCondition
	{
		/// <summary>
		/// コマンドを使用する。
		/// </summary>
		Command,
		/// <summary>
		/// 条件式を使用する。
		/// </summary>
		Expression,
	}
	
	/// <summary>
	/// 条件式からコマンドの構築。
	/// 
	/// DBManagerに渡すコマンドに対してさらに実装側で条件分岐を行う。
	/// </summary>
	public class CommandExpression
	{
		/// <summary>
		/// 条件式のデフォルト値生成。
		/// </summary>
		public CommandExpression()
		{
			Condition = false;
			TrueCommand = string.Empty;
			FalseCondition = FalseCondition.Command;
			FalseCommand = string.Empty;
			FalseExpression = null;
		}
		
		public CommandExpression(string trueCommand): this()
		{
			Condition = true;
			TrueCommand = trueCommand;
		}
		
		/// <summary>
		/// 条件式を指定値で生成。
		/// 
		/// 偽の場合は空文字列となる。
		/// </summary>
		/// <param name="condition">条件</param>
		/// <param name="trueCommand">真の場合のコマンド。</param>
		public CommandExpression(bool condition, string trueCommand): this()
		{
			Condition = condition;
			TrueCommand = trueCommand;
			FalseCondition = FalseCondition.Command;
		}
		
		/// <summary>
		/// 条件式を指定値で生成。
		/// </summary>
		/// <param name="condition">条件</param>
		/// <param name="trueCommand">真の場合のコマンド</param>
		/// <param name="falseCommand">偽の場合のコマンド</param>
		public CommandExpression(bool condition, string trueCommand, string falseCommand): this()
		{
			Condition = condition;
			TrueCommand = trueCommand;
			FalseCondition = FalseCondition.Command;
			FalseCommand = falseCommand;
		}
		
		/// <summary>
		/// 条件
		/// </summary>
		public bool Condition { get; private set; }
		/// <summary>
		/// 条件が真の場合のコマンド
		/// </summary>
		public string TrueCommand { get; private set; }
		/// <summary>
		/// 条件が偽の場合にコマンドと式のどちらを使用するか
		/// </summary>
		public FalseCondition FalseCondition { get; private set; }
		/// <summary>
		/// 条件が偽の場合のコマンド
		/// </summary>
		public string FalseCommand { get; private set; }
		/// <summary>
		/// 条件が偽の場合の式
		/// </summary>
		public CommandExpression FalseExpression { get; private set; }
		
		/// <summary>
		/// 条件をコマンドに落とし込む。
		/// </summary>
		/// <returns></returns>
		public string ToCode()
		{
			if(Condition) {
				return TrueCommand;
			}
			
			if(FalseCondition == FalseCondition.Command) {
				// 文字列
				return FalseCommand;
			} else {
				Debug.Assert(FalseCondition == FalseCondition.Expression);
				// 式
				return FalseExpression.ToCode();
			}
		}
	}
	
	
	/// <summary>
	/// DTO, Entity で使用するカラム名。
	/// 
	/// Entity の場合はテーブル名まで指定する。
	/// </summary>
	[AttributeUsage(
		AttributeTargets.Class  | AttributeTargets.Property,
		AllowMultiple = true,
		Inherited = true
	)]
	public sealed class TargetNameAttribute: Attribute
	{
		public TargetNameAttribute(string name, bool primary)
		{
			TargetName = name;
			PrimaryKey = primary;
		}
		
		public TargetNameAttribute(string name): this(name, false)
		{ }
		/// <summary>
		/// 物理名
		/// </summary>
		public string TargetName { get; private set; }
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
	public sealed class TargetInfo
	{
		public TargetInfo(TargetNameAttribute attribute, PropertyInfo propertyInfo)
		{
			TargetNameAttribute = attribute;
			PropertyInfo = propertyInfo;
		}
		public TargetNameAttribute TargetNameAttribute { get; private set; }
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
	public sealed class EntitySet
	{
		public EntitySet(string tableName, IList<TargetInfo> targetInfos)
		{
			TableName = tableName;
			TargetInfos = targetInfos;
		}
		/// <summary>
		/// テーブル名。
		/// </summary>
		public string TableName { get ; private set; }
		/// <summary>
		/// 対象TargetInfoの集合
		/// </summary>
		public IList<TargetInfo> TargetInfos { get; private set; }
	}
	
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

	public class DbQuery: IDisposable
	{
		public DbQuery(DBManager dbManager)
		{
			DBManager = dbManager;
			DbCommand = dbManager.Connection.CreateCommand();

			Parameter = new Dictionary<string, object>();
			Expression = new Dictionary<string, CommandExpression>();

			ConditionPattern = @"\{(\w+)\}";
		}
		/// <summary>
		/// 生成元。
		/// </summary>
		public DBManager DBManager { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		protected DbCommand DbCommand { get; set; }

		/// <summary>
		/// パラメータ。
		/// </summary>
		public IDictionary<string, object> Parameter { get; private set; }
		/// <summary>
		/// 条件式。
		/// </summary>
		protected IDictionary<string, CommandExpression> Expression { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		private Regex CompiledPattern { get; set; }
		/// <summary>
		/// 条件式パターン。
		/// </summary>
		public string ConditionPattern
		{
			get { return CompiledPattern.ToString(); }
			set { CompiledPattern = new Regex(value, RegexOptions.Singleline | RegexOptions.Compiled); }
		}

		public bool HasExpression(string name)
		{
			return Expression.ContainsKey(name);
		}

		public CommandExpression GetExpression(string exprName)
		{
			return Expression[exprName];
		}

		public CommandExpression SetExpression(string exprName, CommandExpression expr)
		{
			return Expression[exprName] = expr;
		}

		public CommandExpression SetExpression(string exprName, string trueCommand)
		{
			var expr = new CommandExpression(trueCommand);
			return SetExpression(exprName, expr);
		}

		public CommandExpression SetExpression(string exprName, bool condition, string trueCommand)
		{
			var expr = new CommandExpression(condition, trueCommand);
			return SetExpression(exprName, expr);
		}

		public CommandExpression SetExpression(string exprName, bool condition, string trueCommand, string falseCommand)
		{
			var expr = new CommandExpression(condition, trueCommand, falseCommand);
			return SetExpression(exprName, expr);
		}

		/// <summary>
		/// パラメータの生成。
		/// </summary>
		/// <param name="command">コマンド</param>
		/// <param name="name">パラメータ名</param>
		/// <param name="value">パラメータ値</param>
		/// <returns>パラメータ</returns>
		protected DbParameter MakeParameter(DbCommand command, string name, object value)
		{
			var param = command.CreateParameter();

			param.ParameterName = name;
			if(value != null) {
				var type = value.GetType();
				param.Value = DBManager.DbValueFromValue(value, type);
				param.DbType = DBManager.DbTypeFromType(type);
			}
			return param;
		}
		/// <summary>
		/// 現在設定されているパラメータの配列を作成
		/// </summary>
		/// <param name="command">コマンド</param>
		/// <returns>パラメータ一覧</returns>
		protected DbParameter[] MakeParameterList(DbCommand command)
		{
			var list = new List<DbParameter>(Parameter.Count);
			foreach(var pair in Parameter) {
				var param = MakeParameter(command, pair.Key, pair.Value);
				list.Add(param);
			}

			return list.ToArray();
		}
		/// <summary>
		/// 現在設定されているパラメータをコマンドに設定。
		/// </summary>
		/// <param name="command">コマンド</param>
		/// <returns>設定の有無</returns>
		protected bool SetParameter(DbCommand command)
		{
			if(Parameter.Count > 0) {
				var paramList = MakeParameterList(command);
				command.Parameters.Clear();
				command.Parameters.AddRange(paramList);
				command.Prepare();

				return true;
			}

			return false;
		}
		/// <summary>
		/// 条件式をパターンから置き換え。
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		protected virtual string ExpressionReplace(string code)
		{
			if(Expression.Count == 0) {
				return code;
			}

			//var pattern = ConditionPattern;
			var replacedCode = CompiledPattern.Replace(code, (Match m) => Expression[m.Groups[1].Value].ToCode());

			return replacedCode;
		}

		/// <summary>
		/// 現在の指定値からコマンド実行。
		/// </summary>
		/// <param name="func">実行を担当する処理</param>
		/// <param name="code">コマンド</param>
		/// <returns></returns>
		private T Executer<T>(Func<DbCommand, T> func, string code)
		{
			DbCommand.CommandText = ExpressionReplace(code);
			SetParameter(DbCommand);
			return func(DbCommand);
		}
		/// <summary>
		/// 現在の指定値からコマンド実行。
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		public DbDataReader ExecuteReader(string code)
		{
			return Executer(command => command.ExecuteReader(), code);
		}
		/// <summary>
		/// 現在の指定値からコマンド実行。
		/// </summary>
		/// <param name="code"></param>
		/// <returns>影響を受けた行数</returns>
		public int ExecuteCommand(string code)
		{
			return Executer(command => command.ExecuteNonQuery(), code);
		}

		/// <summary>
		/// 対象Entity/DTOから物理名・プロパティ紐付一覧を取得。
		/// </summary>
		/// <returns></returns>
		private IList<TargetInfo> GetTargetInfoList<T>()
			where T: DbData
		{
			var members = typeof(T).GetMembers();
			var targetList = new List<TargetInfo>(members.Length);
			foreach(var member in members) {
				var tartgetNameAttribute = member.GetCustomAttribute(typeof(TargetNameAttribute)) as TargetNameAttribute;
				if(tartgetNameAttribute != null) {
					var propertyInfo = typeof(T).GetProperty(member.Name);
					var targetInfo = new TargetInfo(tartgetNameAttribute, propertyInfo);
					targetList.Add(targetInfo);
				}
			}

			return targetList;
		}

		/// <summary>
		/// 指定値からコマンドを実行
		/// </summary>
		/// <param name="code"></param>
		/// <returns>Tの集合</returns>
		private IEnumerable<T> GetDtoListImpl<T>(string code)
			where T: DbData, new()
		{
			var targetInfos = GetTargetInfoList<T>();
			using(var reader = ExecuteReader(code)) {
				while(reader.Read()) {
					var dto = new T();
					foreach(var targetInfo in targetInfos) {
						var rawValue = reader[targetInfo.TargetNameAttribute.TargetName];
						var property = targetInfo.PropertyInfo;
						var convedValue = DBManager.To(rawValue, property.PropertyType);
						property.SetValue(dto, convedValue);
					}
					yield return dto;
				}
			}
		}

		/// <summary>
		/// 指定値からコマンドを実行
		/// </summary>
		/// <param name="code"></param>
		/// <returns>T単体</returns>
		public T GetResultSingle<T>(string code)
			where T: Dto, new()
		{
			return GetDtoListImpl<T>(code).Single();
		}
		/// <summary>
		/// 指定値からコマンドを実行
		/// </summary>
		/// <param name="code"></param>
		/// <returns>T集合</returns>
		public IEnumerable<T> GetResultList<T>(string code)
			where T: Dto, new()
		{
			return GetDtoListImpl<T>(code);
		}

		/// <summary>
		/// 対象のエンティティからエンティティ一覧情報を取得
		/// </summary>
		/// <returns></returns>
		private EntitySet GetEntitySet<T>()
			where T: Entity
		{
			var tableAttribute = (TargetNameAttribute)typeof(T).GetCustomAttribute(typeof(TargetNameAttribute));
			var tableName = tableAttribute.TargetName;
			var columnPropName = GetTargetInfoList<T>();

			return new EntitySet(tableName, columnPropName);
		}

		/// <summary>
		/// エンティティ取得用コードの生成。
		/// </summary>
		/// <param name="entitySet"></param>
		/// <returns></returns>
		protected virtual string CreateSelectCommandCode(EntitySet entitySet)
		{
			// 主キー
			var primary = entitySet.TargetInfos.Where(t => t.TargetNameAttribute.PrimaryKey);

			var code = string.Format(
				"select * from {0} where {1}",
				entitySet.TableName,
				string.Join("and ", primary.Select(t => string.Format("{0} = :{1}", t.TargetNameAttribute.TargetName, t.PropertyInfo.Name)))
			);

			return code;
		}

		/// <summary>
		/// エンティティ挿入用コードの生成。
		/// </summary>
		/// <param name="entitySet"></param>
		/// <returns></returns>
		protected virtual string CreateInsertCommandCode(EntitySet entitySet)
		{
			var code = string.Format(
				"insert into {0} ({1}) values({2})",
				entitySet.TableName,
				string.Join(", ", entitySet.TargetInfos.Select(t => t.TargetNameAttribute.TargetName)),
				string.Join(", ", entitySet.TargetInfos.Select(t => ":" + t.PropertyInfo.Name))
			);

			return code;
		}
		/// <summary>
		/// エンティティ更新用コードの生成。
		/// </summary>
		/// <param name="entitySet"></param>
		/// <returns></returns>
		protected virtual string CreateUpdateCommandCode(EntitySet entitySet)
		{
			// 主キー
			var primary = entitySet.TargetInfos.Where(t => t.TargetNameAttribute.PrimaryKey);
			// 変更データ
			var data = entitySet.TargetInfos.Where(t => !t.TargetNameAttribute.PrimaryKey);

			var code = string.Format(
				"update {0} set {1} where {2}",
				entitySet.TableName,
				string.Join(", ", data.Select(t => string.Format("{0} = :{1}", t.TargetNameAttribute.TargetName, t.PropertyInfo.Name))),
				string.Join(" and ", primary.Select(t => string.Format("{0} = :{1}", t.TargetNameAttribute.TargetName, t.PropertyInfo.Name)))
			);

			return code;
		}

		/// <summary>
		/// エンティティ削除用コードの生成。
		/// </summary>
		/// <param name="entitySet"></param>
		/// <returns></returns>
		protected virtual string CreateDeleteCommandCode(EntitySet entitySet)
		{
			// 主キー
			var primary = entitySet.TargetInfos.Where(t => t.TargetNameAttribute.PrimaryKey);

			var code = string.Format(
				"delete from {0} where {1}",
				entitySet.TableName,
				string.Join("and ", primary.Select(t => string.Format("{0} = :{1}", t.TargetNameAttribute.TargetName, t.PropertyInfo.Name)))
			);

			return code;
		}

		protected void SetParameterFromEntitySet(Entity entity, EntitySet entitySet)
		{
			foreach(var targetInfo in entitySet.TargetInfos) {
				Parameter[targetInfo.PropertyInfo.Name] = targetInfo.PropertyInfo.GetValue(entity);
			}
		}

		/// <summary>
		/// エンティティに対して処理を実行
		/// 
		/// 呼び出し時にパラメータ・条件式はクリアされる。
		/// </summary>
		/// <param name="entityList"></param>
		/// <param name="func">実行するコマンドを生成する処理</param>
		private void ExecuteEntityCommand<T>(IList<T> entityList, Func<EntitySet, string> func)
			where T: Entity
		{
			var entitySet = GetEntitySet<T>();
			var code = func(entitySet);
			foreach(var entity in entityList) {
				SetParameterFromEntitySet(entity, entitySet);
				ExecuteCommand(code);
			}
		}
		/// <summary>
		/// Entityの挿入。
		/// </summary>
		/// <param name="entityList"></param>
		public void ExecuteInsert<T>(IList<T> entityList)
			where T: Entity
		{
			ExecuteEntityCommand(entityList, CreateInsertCommandCode);
		}
		/// <summary>
		/// Entityの更新。
		/// </summary>
		/// <param name="entityList"></param>
		public void ExecuteUpdate<T>(IList<T> entityList)
			where T: Entity
		{
			ExecuteEntityCommand(entityList, CreateUpdateCommandCode);
		}
		/// <summary>
		/// Entityの削除。
		/// </summary>
		/// <param name="entityList"></param>
		public void ExecuteDelete<T>(IList<T> entityList)
			where T: Entity
		{
			ExecuteEntityCommand(entityList, CreateDeleteCommandCode);
		}

		/// <summary>
		/// 指定エンティティから一致するエンティティを取得する。
		/// </summary>
		/// <param name="entity"></param>
		/// <returns>対象のデータが設定されたエンティティ。見つからない場合は null。</returns>
		public T GetEntity<T>(T entity)
			where T: Entity, new()
		{
			var entitySet = GetEntitySet<T>();
			var code = CreateSelectCommandCode(entitySet);
			SetParameterFromEntitySet(entity, entitySet);

			return GetDtoListImpl<T>(code).SingleOrDefault();
		}
		/// <summary>
		/// 指定エンティティから主キー(将来的には非重複キー)のみのデータを持つエンティティを作成。
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public virtual T CreateKeyEntity<T>(T src)
			where T: Entity, new()
		{
			var targetInfos = GetTargetInfoList<T>();
			var keyEntity = new T();
			foreach(var targetInfo in targetInfos.Where(t => t.TargetNameAttribute.PrimaryKey)) {
				var value = targetInfo.PropertyInfo.GetValue(src);
				targetInfo.PropertyInfo.SetValue(keyEntity, value);
			}
			return keyEntity;
		}

		protected virtual void Dispose(bool disposing)
		{
			DbCommand.Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
		}
	}

	/// <summary>
	/// DB接続・操作の一元化
	/// 
	/// すんごいとろくない限り処理速度は考えない。
	/// </summary>
	public abstract class DBManager: IDisposable
	{
		/// <summary>
		/// 生成。
		/// 
		/// </summary>
		/// <param name="connection">コネクション</param>
		/// <param name="isOpened">コネクションは開いているか。閉じている場合は開く。</param>
		public DBManager(DbConnection connection, bool isOpened)
		{
			///Parameter = new Dictionary<string, object>();
			///Expression = new Dictionary<string, CommandExpression>();
			
			Connection = connection;
			
			
			if(!isOpened) {
				Connection.Open();
			}
		}
		/// <summary>
		/// DB接続。
		/// </summary>
		public DbConnection Connection { get; private set; }
		/// <summary>
		/// トランザクションの開始
		/// </summary>
		/// <returns></returns>
		public DbTransaction BeginTransaction()
		{
			var tran = Connection.BeginTransaction();
			
			return tran;
		}
		
		/// <summary>
		/// コマンド生成。
		/// 
		/// ユーザーコードでは多分出番ない、はず。
		/// </summary>
		/// <returns></returns>
		public virtual DbQuery CreateQuery()
		{
			return new DbQuery(this);
		}
		
		/// <summary>
		/// 型変換。
		/// 
		/// キャストでなく実際の変換処理も担当する
		/// </summary>
		/// <param name="value"></param>
		/// <param name="toType"></param>
		/// <returns></returns>
		public virtual object To(object value, Type toType)
		{
			return value;
		}
		/// <summary>
		/// 型変換。
		/// 
		/// 実際の型変換には object To(object, Type) を使用する。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public T To<T>(object value)
		{
			return (T)(To(value, typeof(T)));
		}
		
		/// <summary>
		/// DBに合わせてデータ調整
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public virtual object DbValueFromValue(object value, Type type)
		{
			return value;
		}
		
		/// <summary>
		/// DBに合わせて型調整
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual DbType DbTypeFromType(Type type)
		{
			var map = new Dictionary<Type, DbType>() {
				{ typeof(byte), DbType.Byte },
				{ typeof(sbyte), DbType.SByte },
				{ typeof(short), DbType.Int16 },
				{ typeof(ushort), DbType.UInt16 },
				{ typeof(int), DbType.Int32 },
				{ typeof(uint), DbType.UInt32 },
				{ typeof(long), DbType.Int64 },
				{ typeof(ulong), DbType.UInt64 },
				{ typeof(float), DbType.Single },
				{ typeof(double), DbType.Double },
				{ typeof(decimal), DbType.Decimal },
				{ typeof(bool), DbType.Boolean },
				{ typeof(string), DbType.String },
				{ typeof(char), DbType.StringFixedLength },
				{ typeof(Guid), DbType.Guid },
				{ typeof(DateTime), DbType.DateTime },
				{ typeof(DateTimeOffset), DbType.DateTimeOffset },
				{ typeof(byte[]), DbType.Binary },
				{ typeof(byte?), DbType.Byte },
				{ typeof(sbyte?), DbType.SByte },
				{ typeof(short?), DbType.Int16 },
				{ typeof(ushort?), DbType.UInt16 },
				{ typeof(int?), DbType.Int32 },
				{ typeof(uint?), DbType.UInt32 },
				{ typeof(long?), DbType.Int64 },
				{ typeof(ulong?), DbType.UInt64 },
				{ typeof(float?), DbType.Single },
				{ typeof(double?), DbType.Double },
				{ typeof(decimal?), DbType.Decimal },
				{ typeof(bool?), DbType.Boolean },
				{ typeof(char?), DbType.StringFixedLength },
				{ typeof(Guid?), DbType.Guid },
				{ typeof(DateTime?), DbType.DateTime },
			};
			
			#if DEBUG
			if(map.ContainsKey(type)) {
				#endif
				return map[type];
				#if DEBUG
			}
			throw new ArgumentException(type.ToString());
			#endif
		}


		protected virtual void Dispose(bool disposing)
		{
			Connection.Dispose();
		}


		/// <summary>
		/// とじるん。
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
		
		
	}
}
