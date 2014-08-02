/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/23
 * 時刻: 22:22
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace PeUtility
{
	/// <summary>
	/// コマンドライン引数を分解したりなんやしたり
	/// </summary>
	public class CommandLine
	{
		public List<string> Options { get; private set; }
		
		public string KeyValueHeader { get; set; }
		public string KeyValueSeparator { get; set; }
		
		private void Initialize()
		{
			Options = new List<string>();
			
			KeyValueHeader = "/";
			KeyValueSeparator = "=";
		}
		
		/// <summary>
		/// 起動時のオプションから呼び出されることを想定
		/// </summary>
		public CommandLine()
		{
			Initialize();
			
			Options.AddRange(Environment.GetCommandLineArgs().Skip(1));
		}
		/// <summary>
		/// スタートアップ関数から呼び出されることを想定
		/// </summary>
		/// <param name="args"></param>
		public CommandLine(string[] args)
		{
			Initialize();
			
			Options.AddRange(args);
		}
		
		private KeyValuePair<string, string> SplitKeyValue(string pair)
		{
			var index = pair.IndexOf(KeyValueSeparator);
			if(index != -1) {
				return new KeyValuePair<string,string>(string.Concat(pair.Take(index)), string.Concat(pair.Skip(index + KeyValueHeader.Length)));
			}
			
			throw new ArgumentException(string.Format("pair = {0}, header = {1}", pair, KeyValueHeader));
		}
		
		private string KeyToValue(string keyOption, int index)
		{
			var pairs = Options.Where(s => s.StartsWith(keyOption));
			var pair = SplitKeyValue(pairs.ElementAt(index));
			return pair.Value;
		}
		
		private string GetKeyOption(string option) 
		{
			return KeyValueHeader + option;
		}
		
		private bool HasKeyOption(string keyOption)
		{
			return Options.Any(s => s.StartsWith(keyOption));
		}
		
		/// <summary>
		/// KeyValueHeader + option が存在するかを確認。
		/// 
		/// データが単独かペアかは問はない。
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		public bool HasOption(string option)
		{
			var keyOption = GetKeyOption(option);
			return HasKeyOption(keyOption);
		}
		
		public bool HasValue(string option, int index = 0)
		{
			var keyOption = GetKeyOption(option);
			if(HasKeyOption(keyOption)) {
				var pairs = Options.Where(s => s.StartsWith(keyOption));
				return pairs.ElementAt(index).IndexOf(KeyValueSeparator) != -1;
			}
			
			throw new ArgumentException(keyOption);
		}
		
		private int CountKeyOption(string keyOption)
		{
			return Options.Count(s => s.StartsWith(keyOption));
		}
		
		public int CountOption(string option)
		{
			var keyOption = GetKeyOption(option);
			return CountKeyOption(keyOption);
		}
		
		public string GetValue(string option)
		{
			var keyOption = GetKeyOption(option);
			if(HasKeyOption(keyOption)) {
				return KeyToValue(keyOption, 0);
			}
			
			throw new ArgumentException(option);
			
		}
		
		
	}
}
