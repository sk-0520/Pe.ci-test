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
using System.Data.Common;

namespace PeUtility
{
	/// <summary>
	/// DB接続・操作の一元化
	/// </summary>
	public class DBManager
	{
		public DBManager(DbConnection connection, bool isOpened, bool sharedCommand)
		{
			Connection = connection;
			
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
		
		protected DbParameter[] MakeParameter(DbCommand command, Dictionary<string, object> parameter)
		{
			var list = new List<DbParameter>();
			foreach(var pair in parameter) {
				var param = command.CreateParameter();
				param.ParameterName = pair.Key;
				param.Value = pair.Value;
				
				list.Add(param);
			}
			
			return list.ToArray();
		}
		
		public DbDataReader ExecuteReader(string code, Dictionary<string, object> parameter = null)
		{
			var command = UseCommand();
			command.CommandText = code;
			if(parameter != null && parameter.Count > 0) {
				var paramList = MakeParameter(command, parameter);
				command.Parameters.AddRange(paramList);
				command.Prepare();
			}
			var reader = command.ExecuteReader();
			UnuseCommand(command);
			return reader;
		}
		
		public void Close()
		{
			Connection.Close();
		}
	}
}
