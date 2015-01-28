namespace ContentTypeTextNet.Pe.Library.Utility.DB
{
	using System;
	using System.Collections.Generic;
	using System.Data;
	using System.Data.Common;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using System.Reflection;

	/// <summary>
	/// DBクエリ。
	/// </summary>
	public class DbQuery: IDisposable
	{
		/// <summary>
		/// 生成。
		/// 
		/// 基本的にDBManagerから作成するのでユーザーコードでは使用しない。
		/// </summary>
		/// <param name="dbManager"></param>
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
		/// コマンド。
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

		/// <summary>
		/// 条件式が存在するか。
		/// </summary>
		/// <param name="name">条件式名。</param>
		/// <returns></returns>
		public bool HasExpression(string name)
		{
			return Expression.ContainsKey(name);
		}

		/// <summary>
		/// 条件式取得。
		/// </summary>
		/// <param name="exprName">条件式名。</param>
		/// <returns></returns>
		public CommandExpression GetExpression(string exprName)
		{
			return Expression[exprName];
		}

		/// <summary>
		/// 条件式の設定。
		/// </summary>
		/// <param name="exprName"></param>
		/// <param name="expr"></param>
		/// <returns></returns>
		public CommandExpression SetExpression(string exprName, CommandExpression expr)
		{
			return Expression[exprName] = expr;
		}

		/// <summary>
		/// コマンド条件式を真で設定。
		/// </summary>
		/// <param name="exprName"></param>
		/// <param name="trueCommand"></param>
		/// <returns></returns>
		public CommandExpression SetExpression(string exprName, string trueCommand)
		{
			var expr = new CommandExpression(trueCommand);
			return SetExpression(exprName, expr);
		}

		/// <summary>
		/// 条件式を設定。
		/// 
		/// 偽の場合は空白となる。
		/// </summary>
		/// <param name="exprName"></param>
		/// <param name="condition"></param>
		/// <param name="trueCommand"></param>
		/// <returns></returns>
		public CommandExpression SetExpression(string exprName, bool condition, string trueCommand)
		{
			var expr = new CommandExpression(condition, trueCommand);
			return SetExpression(exprName, expr);
		}

		/// <summary>
		/// 条件式を設定。
		/// </summary>
		/// <param name="exprName"></param>
		/// <param name="condition"></param>
		/// <param name="trueCommand"></param>
		/// <param name="falseCommand"></param>
		/// <returns></returns>
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
		/// 対象Row/Dtoから物理名・プロパティ紐付一覧を取得。
		/// </summary>
		/// <returns></returns>
		private IList<EntityMappingInfo> GetTargetInfoList<T>()
			where T: DbData
		{
			var members = typeof(T).GetMembers();
			var targetList = new List<EntityMappingInfo>(members.Length);
			foreach(var member in members) {
				var tartgetNameAttribute = member.GetCustomAttribute(typeof(EntityMappingAttribute)) as EntityMappingAttribute;
				if(tartgetNameAttribute != null) {
					var propertyInfo = typeof(T).GetProperty(member.Name);
					var targetInfo = new EntityMappingInfo(tartgetNameAttribute, propertyInfo);
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
						var rawValue = reader[targetInfo.EntityMappingAttribute.PhysicalName];
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
		private EntityMappingSet GetEntitySet<T>()
			where T: Row
		{
			var tableAttribute = (EntityMappingAttribute)typeof(T).GetCustomAttribute(typeof(EntityMappingAttribute));
			var tableName = tableAttribute.PhysicalName;
			var columnPropName = GetTargetInfoList<T>();

			return new EntityMappingSet(tableName, columnPropName);
		}



		protected void SetParameterFromEntitySet(Row row, EntityMappingSet entitySet)
		{
			foreach(var targetInfo in entitySet.TargetInfos) {
				Parameter[targetInfo.PropertyInfo.Name] = targetInfo.PropertyInfo.GetValue(row);
			}
		}

		/// <summary>
		/// エンティティに対して処理を実行
		/// 
		/// 呼び出し時にパラメータ・条件式はクリアされる。
		/// </summary>
		/// <param name="rowList"></param>
		/// <param name="func">実行するコマンドを生成する処理</param>
		private void ExecuteEntityCommand<T>(IList<T> rowList, Func<EntityMappingSet, string> func)
			where T: Row
		{
			var entitySet = GetEntitySet<T>();
			var code = func(entitySet);
			foreach(var entity in rowList) {
				SetParameterFromEntitySet(entity, entitySet);
				ExecuteCommand(code);
			}
		}

		/// <summary>
		/// Rowの挿入。
		/// </summary>
		/// <param name="rowList"></param>
		public void ExecuteInsert<T>(IList<T> rowList)
			where T: Row
		{
			ExecuteEntityCommand(rowList, DBManager.CreateInsertCommandCode);
		}

		/// <summary>
		/// Rowの更新。
		/// </summary>
		/// <param name="rowList"></param>
		public void ExecuteUpdate<T>(IList<T> rowList)
			where T: Row
		{
			ExecuteEntityCommand(rowList, DBManager.CreateUpdateCommandCode);
		}

		/// <summary>
		/// Rowの削除。
		/// </summary>
		/// <param name="rowList"></param>
		public void ExecuteDelete<T>(IList<T> rowList)
			where T: Row
		{
			ExecuteEntityCommand(rowList, DBManager.CreateDeleteCommandCode);
		}

		/// <summary>
		/// 指定行から一致するエンティティを取得する。
		/// </summary>
		/// <param name="row"></param>
		/// <returns>対象のデータが設定された行。見つからない場合は null。</returns>
		public T GetRow<T>(T row)
			where T: Row, new()
		{
			var entitySet = GetEntitySet<T>();
			var code = DBManager.CreateSelectCommandCode(entitySet);
			SetParameterFromEntitySet(row, entitySet);

			return GetDtoListImpl<T>(code).SingleOrDefault();
		}

		/// <summary>
		/// 指定行から主キー(将来的には非重複キー)のみのデータを持つ行を作成。
		/// </summary>
		/// <param name="src"></param>
		/// <returns></returns>
		public virtual T CreateKeyRow<T>(T src)
			where T: Row, new()
		{
			var targetInfos = GetTargetInfoList<T>();
			var keyEntity = new T();
			foreach(var targetInfo in targetInfos.Where(t => t.EntityMappingAttribute.PrimaryKey)) {
				var value = targetInfo.PropertyInfo.GetValue(src);
				targetInfo.PropertyInfo.SetValue(keyEntity, value);
			}
			return keyEntity;
		}

		#region IDisposable

		protected virtual void Dispose(bool disposing)
		{
			DbCommand.Dispose();
		}

		public void Dispose()
		{
			Dispose(true);
		}

		#endregion
	}
}
