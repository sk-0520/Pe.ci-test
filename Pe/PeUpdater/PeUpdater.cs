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
	/// <item>
	/// 	<term>platform</term>
	/// 	<description>CPU種別。</description>
	/// </item>
	/// <item>
	/// 	<term>rc</term>
	/// 	<description>RC版のDL判定。</description>
	/// </item>
	/// <item>
	/// 	<term>checkonly</term>
	/// 	<description>アップデートチェックのみ行う。</description>
	/// </item>
	/// <item>
	/// 	<term>wait</term>
	/// 	<description>キー待ち。</description>
	/// </item>
	/// </list>
	class PeUpdater
	{
		public static void Main(string[] args)
		{
			Update update = null;
			try {
				var commandLine = new CommandLine(args);
				if(commandLine.Length == 0) {
					throw new PeUpdaterException(PeUpdaterCode.NotFoundArgument);
				}
				
				update = new Update(commandLine);
				update.Check();
				if(update.IsVersionUp) {
					if(update.CheckOnly) {
						Console.WriteLine(">> UPDATE:{0} {1}", update.VersionText, update.IsRCVersion ? "RC": "RELEASE");
					} else {
						update.Execute();
						Console.WriteLine(">> SUCCESS");
					}
				} else {
					Console.WriteLine(">> NONE");
				}
			} catch(PeUpdaterException ex) {
				Console.WriteLine(">> ERROR");
				Console.WriteLine(">> {0}", (int)ex.PeUpdaterCode);
				Console.WriteLine(ex);
			} catch(Exception ex) {
				Console.WriteLine(">> ERROR");
				Console.WriteLine(ex);
			}
			if(update != null && update.Wait) {
				Console.WriteLine("Press any key to continue ...");
				Console.ReadKey(false);
			}
		}
	}
}