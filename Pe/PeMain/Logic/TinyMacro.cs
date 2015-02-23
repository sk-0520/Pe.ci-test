namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

	/// <summary>
	/// ネストすら許されない簡易マクロ。
	/// </summary>
	public class TinyMacro
	{
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

		public string Name { get; set; }
		public string RawParameter { get; set; }
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
