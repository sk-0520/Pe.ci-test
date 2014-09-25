/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/09/25
 * 時刻: 22:43
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeUtility;

namespace PeUpdater
{
	/// <summary>
	/// 
	/// </summary>
	/// <list type="table">
	/// <listheader>
	/// 	<term>オプション名</term>
	/// 	<description>内容</description>
	/// </listheader>
	/// <item>
	/// 	<term>pid</term>
	/// 	<description>PeMainのプロセスID</description>
	/// </item>
	/// <item>
	/// 	<term>version</term>
	/// 	<description>PeMainのバージョン(*.*.*.*)</description>
	/// </item>
	/// <item>
	/// 	<term>uri</term>
	/// 	<description>バージョン情報定義URI。</description>
	/// </item>
	/// <item>
	/// 	<term>download</term>
	/// 	<description>ダウンロードディレクトリ。</description>
	/// </item>
	/// <item>
	/// 	<term>expand</term>
	/// 	<description>展開ディレクトリ。</description>
	/// </item>
	/// </list>
	class PeUpdater
	{
		public static void Main(string[] args)
		{
			var commandLine = new CommandLine(args);
			if(commandLine.Length == 0) {
				throw new PeUpdaterException(PeUpdaterCode.NotFoundArgument);
			}
			
			var update = new Update(commandLine);
		}
	}
}