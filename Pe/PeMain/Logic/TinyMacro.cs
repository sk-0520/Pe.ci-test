namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Text.RegularExpressions;

	/// <summary>
	/// 簡易マクロ。
	/// </summary>
	public class TinyMacro
	{
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
			var regex = new Regex(@"(?'OPEN'=(?<MACRO>\w+)\()(?<PARAMS>.+)?(?'CLOSE-OPEN'\))");
			return ConvertImpl(source, regex);
		}

		/// <summary>
		/// 指定正規表現を使用してマクロ展開。
		/// </summary>
		/// <param name="source">置き換え前文字列。</param>
		/// <param name="regex">正規表現。</param>
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
		string ExecuteLength()
		{
			return RawParameter.Length.ToString();
		}

		/// <summary>
		/// 現在のマクロ名から処理実行。
		/// </summary>
		/// <returns></returns>
		public string Execute()
		{
			var map = new Dictionary<string, Func<string>>() {
					{ MacroName.length, ExecuteLength },
				};
			Func<string> fn;
			if(map.TryGetValue(Name.ToLower(CultureInfo.InvariantCulture), out fn)) {
				return fn();
			}

			return "#" + Name + "#";
		}


	}
}
