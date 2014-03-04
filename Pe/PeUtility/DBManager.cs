﻿/*
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
using System.Text.RegularExpressions;

namespace PeUtility
{
	public enum FalseCondition
	{
		Text,
		Expression,
	}
	
	public class CommandExpression
	{
		public CommandExpression()
		{
			Condition = false;
			TrueText = string.Empty;
			FalseCondition = FalseCondition.Text;
			FalseText = string.Empty;
			FalseExpression = null;
		}
		
		
		public CommandExpression(bool condition, string trueText): this()
		{
			Condition = condition;
			TrueText = trueText;
			FalseCondition = FalseCondition.Text;
		}
		
		public CommandExpression(bool condition, string trueText, string falseText): this()
		{
			Condition = condition;
			TrueText = trueText;
			FalseCondition = FalseCondition.Text;
			FalseText = falseText;
		}
		
		public CommandExpression(bool condition, string trueText, CommandExpression commandExpression): this()
		{
			Condition = condition;
			TrueText = trueText;
			FalseCondition = FalseCondition.Expression;
			FalseExpression = commandExpression;
		}
		
		public bool Condition { get; set; }
		public string TrueText { get; set; }
		public FalseCondition FalseCondition { get; set; }
		public string FalseText { get; set; }
		public CommandExpression FalseExpression { get; set; }
		
		public string ToCode()
		{
			if(Condition) {
				return TrueText;
			}
			
			if(FalseCondition == FalseCondition.Text) {
				// 文字列
				return FalseText;
			} else {
				Debug.Assert(FalseCondition == FalseCondition.Expression);
				// 式
				return FalseExpression.ToCode();
			}
		}
	}
	
	/// <summary>
	/// DB接続・操作の一元化
	/// </summary>
	public abstract class DBManager
	{
		public DBManager(DbConnection connection, bool isOpened, bool sharedCommand)
		{
			Parameter = new Dictionary<string, object>();
			Expression = new Dictionary<string, CommandExpression>();
			
			Connection = connection;
			
			ConditionPattern = @"\[\[\w+\]\]";
			
			if(!isOpened) {
				Connection.Open();
			}
			
			SharedCommand = sharedCommand;
			if(SharedCommand) {
				Command = CreateCommand();
			}
		}
		public DbConnection Connection { get; private set; }
		public bool SharedCommand { get; private set; }
		public DbCommand Command { get; private set; }
		public string ConditionPattern { get; set; }
		
		public Dictionary<string, object> Parameter { get; private set; }
		public Dictionary<string, CommandExpression> Expression { get; private set; }
		
		public void Clear()
		{
			Parameter.Clear();
			Expression.Clear();
		}
		
		private DbCommand UseCommand()
		{
			if(SharedCommand) {
				return Command;
			} else {
				return CreateCommand();
			}
		}
		private void UnuseCommand(DbCommand command)
		{
			if(Command != command) {
				command.Dispose();
			}
		}
		
		public DbCommand CreateCommand()
		{
			return Connection.CreateCommand();
		}
		
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
		
		protected virtual DbParameter MakeParameter(DbCommand command, string name, object value)
		{
			var param = command.CreateParameter();
			
			param.ParameterName = name;
			param.Value = value;
			param.DbType = DbTypeFromType(value.GetType());
			
			return param;
		}
		
		protected DbParameter[] MakeParameterList(DbCommand command)
		{
			var list = new List<DbParameter>(Parameter.Count);
			foreach(var pair in Parameter) {
				var param = MakeParameter(command, pair.Key, pair.Value);
				list.Add(param);
			}
			
			return list.ToArray();
		}
		
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
		
		protected virtual string ExpressionReplace(string code)
		{
			if(Expression.Count == 0) {
				return code;
			}
			
			var pattern = ConditionPattern;
			var replacedCode = Regex.Replace(code, pattern, (Match m) => Expression[m.Groups[1].Value].ToCode());
			
			return replacedCode;
		}
		
		private T Executer<T>(Func<DbCommand,T> func, string code)
		{
			var command = UseCommand();
			try {
				command.CommandText = ExpressionReplace(code);
				SetParameter(command);
				return func(command);
			} finally {
				UnuseCommand(command);
			}
		}
		
		public DbDataReader ExecuteReader(string code)
		{
			return Executer(command => command.ExecuteReader(), code);
		}
		
		public int ExecuteCommand(string code)
		{
			return Executer(command => command.ExecuteNonQuery(), code);
		}
		
		public void Close()
		{
			Connection.Close();
		}
	}
}
