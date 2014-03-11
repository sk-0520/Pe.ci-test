/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/27
 * 時刻: 23:43
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace PeUtility
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
		/// 条件式を指定値で生成。
		/// </summary>
		/// <param name="condition">条件</param>
		/// <param name="trueCommand">真の場合のコマンド</param>
		/// <param name="commandExpression">偽の場合の条件式</param>
		public CommandExpression(bool condition, string trueCommand, CommandExpression commandExpression): this()
		{
			Condition = condition;
			TrueCommand = trueCommand;
			FalseCondition = FalseCondition.Expression;
			FalseExpression = commandExpression;
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
	
	public sealed class TargetInfo
	{
		string _targetName;
		bool _primaryKey;
		PropertyInfo _propertyInfo;
		
		public TargetInfo(TargetNameAttribute attribute, PropertyInfo propertyInfo)
		{
			this._targetName = attribute.TargetName;
			this._primaryKey = attribute.PrimaryKey;
			this._propertyInfo = propertyInfo;
		}
		/// <summary>
		/// 物理名
		/// </summary>
		public string TargetName { get { return this._targetName; } }
		/// <summary>
		/// 主キー化
		/// </summary>
		public bool PrimaryKey { get { return this._primaryKey; } }
		/// <summary>
		/// TargetNameAttributeで紐付くプロパティ。
		/// </summary>
		public PropertyInfo PropertyInfo { get { return this._propertyInfo; } }
	}
	
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
		public IList<TargetInfo> TargetInfos { get; private set; }
	}
	
	/// <summary>
	/// Entity, DTO の共通クラス。
	/// 
	/// 特に何もしない。
	/// </summary>
	public abstract class DbData
	{ }
	
	/// <summary>
	/// テーブル行に対応
	/// TargetNameAttribute
	/// </summary>
	public abstract class Entity: DbData
	{ }
	
	/// <summary>
	/// データ取得単位に対応
	/// </summary>
	public abstract class Dto: DbData
	{
		public Dto()
		{ }
	}

	/// <summary>
	/// DB接続・操作の一元化
	/// </summary>
	public abstract class DBManager
	{
		/// <summary>
		/// 生成。
		/// 
		/// </summary>
		/// <param name="connection">コネクション</param>
		/// <param name="isOpened">コネクションは開いているか。閉じている場合は開く。</param>
		public DBManager(DbConnection connection, bool isOpened)
		{
			Parameter = new Dictionary<string, object>();
			Expression = new Dictionary<string, CommandExpression>();
			
			Connection = connection;
			
			ConditionPattern = @"\[\[\w+\]\]";
			
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
		/// 条件式パターン。
		/// </summary>
		public string ConditionPattern { get; set; }
		/// <summary>
		/// パラメータ。
		/// </summary>
		public Dictionary<string, object> Parameter { get; private set; }
		/// <summary>
		/// 条件式。
		/// </summary>
		public Dictionary<string, CommandExpression> Expression { get; private set; }
		/// <summary>
		/// 条件式の生成。
		/// </summary>
		/// <returns></returns>
		public virtual CommandExpression CreateExpresstion()
		{
			return new CommandExpression();
		}
		/// <summary>
		/// パラメータ・条件式のクリア。
		/// </summary>
		public void Clear()
		{
			Parameter.Clear();
			Expression.Clear();
		}
		/// <summary>
		/// コマンド生成。
		/// 
		/// ユーザーコードでは多分出番ない、はず。
		/// </summary>
		/// <returns></returns>
		protected virtual DbCommand CreateCommand()
		{
			return Connection.CreateCommand();
		}
		
		/// <summary>
		/// 型変換。
		/// 
		/// キャストでなく実際の変換処理も担当する
		/// </summary>
		/// <param name="value"></param>
		/// <param name="toType"></param>
		/// <returns></returns>
		protected virtual object To(object value, Type toType)
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
		protected virtual object DbValueFromValue(object value, Type type)
		{
			return value;
		}
		
		/// <summary>
		/// DBに合わせて型調整
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected virtual DbType DbTypeFromType(Type type)
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
			
			if(map.ContainsKey(type)) {
				return map[type];
			}
			
			throw new ArgumentException(type.ToString());
		}
		
		/// <summary>
		/// パラメータの生成。
		/// </summary>
		/// <param name="command">コマンド</param>
		/// <param name="name">パラメータ名</param>
		/// <param name="value">パラメータ値</param>
		/// <returns>パラメータ</returns>
		protected virtual DbParameter MakeParameter(DbCommand command, string name, object value)
		{
			var param = command.CreateParameter();
			
			param.ParameterName = name;
			if(value != null) {
				var type = value.GetType();
				param.Value = DbValueFromValue(value, type);
				param.DbType = DbTypeFromType(type);
			}
			return param;
		}
		/// <summary>
		/// 現在設定されているパラメータを作成。
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
			
			var pattern = ConditionPattern;
			var replacedCode = Regex.Replace(code, pattern, (Match m) => Expression[m.Groups[1].Value].ToCode());
			
			return replacedCode;
		}
		
		/// <summary>
		/// 現在の指定値からコマンド実行。
		/// </summary>
		/// <param name="func"></param>
		/// <param name="code">コマンド</param>
		/// <returns></returns>
		private T Executer<T>(Func<DbCommand,T> func, string code)
		{
			using(var command = CreateCommand()) {
				command.CommandText = ExpressionReplace(code);
				SetParameter(command);
				return func(command);
			}
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
		
		private IList<TargetInfo> GetTargetInfoList<T>()
			where T: DbData
		{
			var targetList = new List<TargetInfo>();
			foreach(var member in typeof(T).GetMembers()) {
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
			where T: Dto, new()
		{
			var targetInfoList = GetTargetInfoList<T>();
			//GetPropertyMap<T>(nameTargetMap);
			using(var reader = ExecuteReader(code)) {
				while(reader.Read()) {
					var dto = new T();
					foreach(var targetInfo in targetInfoList) {
						var rawValue = reader[targetInfo.TargetName];
						var property = targetInfo.PropertyInfo;
						var convedValue = To(rawValue, property.PropertyType);
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
		public T GetDtoSingle<T>(string code)
			where T: Dto, new()
		{
			return GetDtoListImpl<T>(code).Single();
		}
		/// <summary>
		/// 指定値からコマンドを実行
		/// </summary>
		/// <param name="code"></param>
		/// <returns>T集合</returns>
		public IEnumerable<T> GetDtoList<T>(string code)
			where T: Dto, new()
		{
			return GetDtoListImpl<T>(code);
		}
		
		private EntitySet GetEntitySet<T>()
			where T: Entity
		{
			var tableAttribute = (TargetNameAttribute)typeof(T).GetCustomAttribute(typeof(TargetNameAttribute));
			var tableName = tableAttribute.TargetName;
			var columnPropName = GetTargetInfoList<T>();
			
			return new EntitySet(tableName, columnPropName);
		}
		
		protected virtual string CreateInsertCommandCode(EntitySet entitySet)
		{
			var code = string.Format(
				"insert into {0} ({1}) values({2})",
				entitySet.TableName,
				string.Join(", ", entitySet.TargetInfos.Select(t => t.TargetName)),
				string.Join(", ", entitySet.TargetInfos.Select(t => ":" + t.PropertyInfo.Name))
			);
			
			return code;
		}
		
		protected virtual string CreateUpdateCommandCode(EntitySet entitySet)
		{
			return null;
		}
		
		private void ExecuteEntityCommand<T>(IList<T> entityList, Func<EntitySet, string> func)
			where T: Entity
		{
			Parameter.Clear();
			
			var entitySet = GetEntitySet<T>();
			var code = func(entitySet);
			//var targetInfos = GetTargetInfoList<T>();
			foreach(var entity in entityList) {
				foreach(var targetInfo in entitySet.TargetInfos) {
					Parameter[targetInfo.PropertyInfo.Name] = targetInfo.PropertyInfo.GetValue(entity);
				}
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
		/// とじるん。
		/// </summary>
		public void Close()
		{
			Connection.Close();
		}
	}
}
