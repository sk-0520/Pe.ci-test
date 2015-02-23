namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Text.RegularExpressions;
	using ContentTypeTextNet.Pe.Library.Utility;

	/// <summary>
	/// 簡易マクロ。
	/// 
	/// 指定する値は 1 始まりとする。
	/// </summary>
	public class TinyMacro
	{
		const string errorHead = "#";
		const string errorTail = "#";

		string PutError(string s)
		{
			return string.Format("{0} {1} {2}", errorHead, s, errorTail);
		}

		/// <summary>
		/// マクロ展開。
		/// <para>
		/// =MACRO(params,...)を展開する。
		/// </para>
		/// </summary>
		/// <param name="source">置き換え前文字列。</param>
		/// <returns>置き換え後文字列。</returns>
		public static string Convert(string source)
		{
			var regex = new Regex(@"(?'OPEN'=(?<MACRO>\w+)\()(?<PARAMS>([^*]|\*[^/])*)?(?'CLOSE-OPEN'\))", RegexOptions.ExplicitCapture | RegexOptions.Multiline);
			return ConvertImpl(source, regex);
		}

		/// <summary>
		/// 指定正規表現を使用してマクロ展開。
		/// </summary>
		/// <param name="source">置き換え前文字列。</param>
		/// <param name="regex">MACRO, PARAMSが定義された正規表現。</param>
		/// <returns>置き換え後文字列。</returns>
		static string ConvertImpl(string source, Regex regex)
		{
			Debug.Assert(regex != null);

			var result = regex.Replace(source, (Match m) => {
				var macro = new TinyMacro(
					m.Groups["MACRO"].Value,
					m.Success ? ConvertImpl(m.Groups["PARAMS"].Value, regex) : string.Empty
				);
				return macro.Execute();
			});

			return result;
		}

		/// <summary>
		/// 生成。
		/// </summary>
		/// <param name="name">マクロ名。</param>
		/// <param name="rawParam">パラメータ。</param>
		public TinyMacro(string name, string rawParam)
		{
			Name = name;
			RawParameter = rawParam;
			if(string.IsNullOrWhiteSpace(rawParam)) {
				ParameterList = new string[0];
			} else {
				ParameterList = rawParam.Split(',').Select(s => s.Trim()).ToArray();
			}
		}

		/// <summary>
		/// マクロ名。
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// 渡された生のパラメータ。
		/// </summary>
		public string RawParameter { get; private set; }
		/// <summary>
		/// 生パラメータをいい感じに分割したパラメータリスト。
		/// </summary>
		public IReadOnlyList<string> ParameterList { get; private set; }

		/// <summary>
		/// 生パラメータの文字数取得。
		/// </summary>
		/// <returns></returns>
		protected virtual string ExecuteLength()
		{
			return RawParameter.Length.ToString();
		}

		/// <summary>
		/// 生パラメータをトリムする。
		/// <para>改行を含むならtimeLines。</para>
		/// </summary>
		/// <returns></returns>
		protected virtual string ExecuteTrim()
		{
			return RawParameter.Trim();
		}

		/// <summary>
		/// 生パラメータを行毎にトリムる。
		/// </summary>
		/// <returns></returns>
		protected virtual string ExecuteTrimLines()
		{
			return string.Join(
				Environment.NewLine, 
				RawParameter
					.SplitLines()
					.Select(s => s.Trim())
			);
		}

		protected virtual string ExecuteLine()
		{
			if(ParameterList.Count == 1) {
				return PutError("Parameter: single");
			}
			var number = ParameterList[1];
			int lineNumber;
			if(!int.TryParse(number, out lineNumber)) {
				return PutError("Line number: " + number);
			}

			var lines = ParameterList.First().SplitLines().ToArray();
			if(lineNumber.Between(1, lines.Length)) {
				return lines[lineNumber - 1];
			} else {
				return PutError("Out range: line");
			}
		}

		/// <summary>
		/// 現在のマクロ名から処理実行。
		/// </summary>
		/// <returns></returns>
		public string Execute()
		{
			var map = new Dictionary<string, Func<string>>() {
				{ MacroName.length, ExecuteLength },
				{ MacroName.trim, ExecuteTrim },
				{ MacroName.trimLines, ExecuteTrimLines },
				{ MacroName.line, ExecuteLine},
			}.ToDictionary(p => p.Key.ToLower(), p => p.Value);

			Func<string> fn;
			if(map.TryGetValue(Name.ToLower(CultureInfo.InvariantCulture), out fn)) {
				return fn();
			}

			return PutError(Name);
		}


	}
}
