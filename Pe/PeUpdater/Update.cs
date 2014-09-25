/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/25
 * 時刻: 23:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using PeUtility;

namespace PeUpdater
{
	/// <summary>
	/// 更新処理もろもろ
	/// </summary>
	public class Update
	{
		private CommandLine _commandLine;
		
		/*
		uint _pid;
		
		ushort _majorVersion;
		ushort _minorVersion;
		ushort _revisionVersion;
		ushort _buildVersion;
		
		string _uri;
		string _downloadDir;
		string _expandDir;
		*/
		
		public Update(CommandLine commandLine)
		{
			Debug.Assert(commandLine.Length > 0);
			
			this._commandLine = commandLine;
			
		}
	}
}
