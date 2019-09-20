/*
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Data.Database;
using System.Reflection;
using System.Diagnostics;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;

namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Database
{
    /// <summary>
    /// DBクエリ。
    /// </summary>
    public class DatabaseQuery: DisposeFinalizeBase
    {
        /// <summary>
        /// 生成。
        /// <para>基本的に<see cref="DatabaseManager"/>から作成するのでユーザーコードでは使用しない。</para>
        /// </summary>
        /// <param name="dbManager"></param>
        public DatabaseQuery(DatabaseManager dbManager)
        {
            DBManager = dbManager;
            DbCommand = dbManager.Connection.CreateCommand();

            Parameter = new Dictionary<string, object>();
            Expression = new Dictionary<string, CommandExpression>();

            ConditionPattern = @"\{(\w+)\}";
        }

        #region property

        /// <summary>
        /// 生成元。
        /// </summary>
        public DatabaseManager DBManager { get; private set; }
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

        #endregion

        #region function

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
        /// 対象<see cref="ContentTypeTextNet.Library.SharedLibrary.Data.Database.DataTransferObject"/>/<see cref="ContentTypeTextNet.Library.SharedLibrary.Data.Database.DatabaseRow"/>から物理名・プロパティ紐付一覧を取得。
        /// </summary>
        /// <returns></returns>
        private IList<EntityMappingInformation> GetTargetInfoList<T>()
            where T : DatabaseObjectBase
        {
            var members = typeof(T).GetMembers();
            var targetList = new List<EntityMappingInformation>(members.Length);
            foreach(var member in members) {
                var tartgetNameAttribute = member.GetCustomAttribute(typeof(EntityMappingAttribute)) as EntityMappingAttribute;
                if(tartgetNameAttribute != null) {
                    var propertyInfo = typeof(T).GetProperty(member.Name);
                    var targetInfo = new EntityMappingInformation(tartgetNameAttribute, propertyInfo);
                    targetList.Add(targetInfo);
                }
            }

            return targetList;
        }

        /// <summary>
        /// 指定値からコマンドを実行
        /// </summary>
        /// <param name="code"></param>
        /// <returns><typeparam name="TDto" />の集合</returns>
        private IEnumerable<TDto> GetDtoListImpl<TDto>(string code)
            where TDto : DatabaseObjectBase, new()
        {
            var targetInfos = GetTargetInfoList<TDto>();
            using(var reader = ExecuteReader(code)) {
                while(reader.Read()) {
                    var dto = new TDto();
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
        /// <returns><typeparam name="TDto" />単体</returns>
        public TDto GetResultSingle<TDto>(string code)
            where TDto : DataTransferObject, new()
        {
            return GetDtoListImpl<TDto>(code).Single();
        }

        /// <summary>
        /// 指定値からコマンドを実行
        /// </summary>
        /// <param name="code"></param>
        /// <returns><typeparam name="TDto" />集合</returns>
        public IEnumerable<TDto> GetResultList<TDto>(string code)
            where TDto : DataTransferObject, new()
        {
            return GetDtoListImpl<TDto>(code);
        }

        /// <summary>
        /// 対象のエンティティからエンティティ一覧情報を取得
        /// </summary>
        /// <returns></returns>
        private EntityMappingSet GetEntitySet<TRow>()
            where TRow : DatabaseRow
        {
            var tableAttribute = (EntityMappingAttribute)typeof(TRow).GetCustomAttribute(typeof(EntityMappingAttribute));
            var tableName = tableAttribute.PhysicalName;
            var columnPropName = GetTargetInfoList<TRow>();

            return new EntityMappingSet(tableName, columnPropName);
        }

        protected void SetParameterFromEntitySet(DatabaseRow row, EntityMappingSet entitySet)
        {
            foreach(var targetInfo in entitySet.TargetInfos) {
                Parameter[targetInfo.PropertyInfo.Name] = targetInfo.PropertyInfo.GetValue(row);
            }
        }

        /// <summary>
        /// エンティティに対して処理を実行
        /// <para>呼び出し時にパラメータ・条件式はクリアされる。</para>
        /// </summary>
        /// <param name="rowList"></param>
        /// <param name="func">実行するコマンドを生成する処理</param>
        private void ExecuteEntityCommand<TRow>(IList<TRow> rowList, Func<EntityMappingSet, string> func)
            where TRow : DatabaseRow
        {
            var entitySet = GetEntitySet<TRow>();
            var code = func(entitySet);
            foreach(var entity in rowList) {
                SetParameterFromEntitySet(entity, entitySet);
                ExecuteCommand(code);
            }
        }

        /// <summary>
        /// <see cref="DatabaseRow"/>の挿入。
        /// </summary>
        /// <param name="rowList"></param>
        public void ExecuteInsert<TRow>(IList<TRow> rowList)
            where TRow : DatabaseRow
        {
            ExecuteEntityCommand(rowList, DBManager.CreateInsertCommandCode);
        }

        /// <summary>
        /// <see cref="DatabaseRow"/>の更新。
        /// </summary>
        /// <param name="rowList"></param>
        public void ExecuteUpdate<TRow>(IList<TRow> rowList)
            where TRow : DatabaseRow
        {
            ExecuteEntityCommand(rowList, DBManager.CreateUpdateCommandCode);
        }

        /// <summary>
        /// <see cref="DatabaseRow"/>の削除。
        /// </summary>
        /// <param name="rowList"></param>
        public void ExecuteDelete<TRow>(IList<TRow> rowList)
            where TRow : DatabaseRow
        {
            ExecuteEntityCommand(rowList, DBManager.CreateDeleteCommandCode);
        }

        /// <summary>
        /// 指定行から一致するエンティティを取得する。
        /// </summary>
        /// <param name="row"></param>
        /// <returns>対象のデータが設定された行。見つからない場合は null。</returns>
        public TRow GetRow<TRow>(TRow row)
            where TRow : DatabaseRow, new()
        {
            var entitySet = GetEntitySet<TRow>();
            var code = DBManager.CreateSelectCommandCode(entitySet);
            SetParameterFromEntitySet(row, entitySet);

            return GetDtoListImpl<TRow>(code).SingleOrDefault();
        }

        /// <summary>
        /// 指定行から主キー(将来的には非重複キー)のみのデータを持つ行を作成。
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public virtual TRow CreateKeyRow<TRow>(TRow src)
            where TRow : DatabaseRow, new()
        {
            var targetInfos = GetTargetInfoList<TRow>();
            var keyEntity = new TRow();
            foreach(var targetInfo in targetInfos.Where(t => t.EntityMappingAttribute.PrimaryKey)) {
                var value = targetInfo.PropertyInfo.GetValue(src);
                targetInfo.PropertyInfo.SetValue(keyEntity, value);
            }
            return keyEntity;
        }

        #endregion

        #region DisposeFinalizeBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Debug.Assert(DbCommand != null);

                DbCommand.Dispose();
                DbCommand = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
