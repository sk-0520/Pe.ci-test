using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	/// <summary>
	/// コマンドライン引数を分解したりなんやしたり
	/// </summary>
	public class CommandLine
	{
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

		private void Initialize()
		{
			Options = new List<string>();

			KeyValueHeader = "/";
			KeyValueSeparator = "=";
		}

		/// <summary>
		/// オプションヘッダ。
		/// </summary>
		public string KeyValueHeader { get; set; }
		/// <summary>
		/// オプション分割文字。
		/// </summary>
		public string KeyValueSeparator { get; set; }

		/// <summary>
		/// 渡されたコマンドラインを統括。
		/// </summary>
		public List<string> Options { get; private set; }
		/// <summary>
		/// オプション数。
		/// </summary>
		public int Length { get { return Options.Count; } }

		/// <summary>
		/// キー KeyValueSeparator 値を分割。
		/// </summary>
		/// <param name="pair"></param>
		/// <returns></returns>
		private KeyValuePair<string, string> SplitKeyValue(string pair)
		{
			var index = pair.IndexOf(KeyValueSeparator);
			if(index != -1) {
				var key = string.Concat(pair.Take(index));
				var value = string.Concat(pair.Skip(index + KeyValueHeader.Length));
				if(value.Length > "\"\"".Length && value.First() == '"' && value.Last() == '"') {
					value = value.Substring(1, value.Length - 1 - 1);
				}
				return new KeyValuePair<string,string>(key, value);
			}
			
			throw new ArgumentException(string.Format("pair = {0}, header = {1}", pair, KeyValueHeader));
		}
		
		/// <summary>
		/// KeyValueHeader + option 検索。
		/// </summary>
		/// <param name="keyOption"></param>
		/// <returns></returns>
		private IEnumerable<string> Find(string keyOption)
		{
			return Options
				.Where(s => s.Length >= keyOption.Length)
				.Where(s => s.StartsWith(keyOption))
				.Where(s => s == keyOption || s.StartsWith(keyOption + this.KeyValueSeparator))
			;
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

		/// <summary>
		/// 値を持つ KeyValueHeader + option が存在するかを確認。
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		public bool HasValue(string option)
		{
			return HasValue(option, 0);
		}
		public bool HasValue(string option, int index = 0)
		{
			var keyOption = GetKeyOption(option);
			if(HasKeyOption(keyOption)) {
				var pairs = Options.Where(s => s.StartsWith(keyOption));
				return pairs.ElementAt(index).IndexOf(KeyValueSeparator) != -1;
			}

			return false;
		}
		
		private int CountKeyOption(string keyOption)
		{
			return Options.Count(s => s.StartsWith(keyOption));
		}
		
		/// <summary>
		/// オプション数取得。
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		public int CountOption(string option)
		{
			var keyOption = GetKeyOption(option);
			return CountKeyOption(keyOption);
		}
		
		/// <summary>
		/// 値を列挙。
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		public IEnumerable<string> GetValues(string option)
		{
			var keyOption = GetKeyOption(option);
			var optionCount = CountKeyOption(keyOption);
			
			foreach(var i in Enumerable.Range(0, optionCount)) {
				yield return KeyToValue(keyOption, i);
			}
		}

		/// <summary>
		/// 値を取得。
		/// </summary>
		/// <param name="option"></param>
		/// <returns></returns>
		public string GetValue(string option)
		{
			return GetValues(option).First();
		}

		
	}
}
