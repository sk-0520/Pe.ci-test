namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using ContentTypeTextNet.Pe.Library.Utility;

	#region exception
	
	/// <summary>
	/// TinyMacroで明示的に投げられる例外の親。
	/// </summary>
	public abstract class TinyMacroException: Exception
	{
		const string errorHead = "#";
		const string errorTail = "!";

		public TinyMacroException(TinyMacro tinyMacro)
			: base()
		{
			TinyMacro = tinyMacro;
		}

		public TinyMacroException(TinyMacro tinyMacro, string message)
			: base(message)
		{
			TinyMacro = tinyMacro;
		}

		public TinyMacro TinyMacro { get; private set; }

		public virtual string ErrorMessage
		{
			get { return MakeErrorMessage(Message); }
		}

		protected virtual string MakeErrorMessage(string message)
		{
			return string.Format("{0}{1}{2}", errorHead, message, errorTail);
		}
	}

	/// <summary>
	/// マクロ実行時に投げられる例外。
	/// </summary>
	public class TinyMacroExecuteException: TinyMacroException
	{
		public TinyMacroExecuteException(TinyMacro tinyMacro, string message)
			: base(tinyMacro, message)
		{ }
	}

	/// <summary>
	/// マクロのパラメーター解析中に投げられる例外。
	/// </summary>
	public class TinyMacroParseException: TinyMacroException
	{
		public TinyMacroParseException(TinyMacro tinyMacro, string message, params object[] infos)
			: base(tinyMacro, message)
		{
			if(infos.Length > 0) {
				ErrorInfos = infos;
			} else {
				ErrorInfos = new object[0];
			}
		}

		public IReadOnlyList<object> ErrorInfos { get; private set; }
	}

	#endregion

	/// <summary>
	/// 簡易マクロ。
	/// 
	/// 指定する値は 1 始まりとする。
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
			/*
			var regex = new Regex(@"
				(?'OPEN' =(?<MACRO> \w+)\( )
				(?<PARAMS> ([^*]|\*[^/])*)?
				(?'CLOSE-OPEN' \) )
				(?(OPEN)(?!))
				",
				RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace
			);
			*/
			/*
			var regex = new Regex(@"
				^(?! ( =\w+\( )|( \) ) )*
				(
				((?'OPEN' =(?<MACRO> \w+)\( ) )+
#				(?<PARAMS> ([^*]|\*[^/])+)?
				(?<PARAMS> (?! ( =\w+\( )|( \) ) ))+ )?
				((?'CLOSE-OPEN' \) )(?! ( =\w+\( )|( \) ) )* )+
				)*
				(?(OPEN)(?!))$
				",//#(?(OPEN)(?!))
				RegexOptions.ExplicitCapture | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace
			);
			*/
			/*
			var regex = new Regex(@"
=(?<MACRO>\w+)\( (
	(?:
	(?<PARAMS> ([^*]|\*[^/])*)
	|
	(?<open> =(\w+)\( )
	|
	(?<-open> \) )
	)+
	(?(open)(!?))
) \)
			", RegexOptions.IgnorePatternWhitespace);
			*/
			var result = ConvertImpl(source);
			Debug.WriteLine("[ {0} ] -> [ {1} ]", source, result);
			return result;
		}

		static string ConvertImpl(string source)
		{
			var result = new StringBuilder((int)(source.Length * 1.5));

			return result.ToString();
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
					m.Success ? (m.Groups["PARAMS"].Value/*, regex*/) : string.Empty
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

		#region property

		/// <summary>
		/// マクロ名。
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// 渡された生のパラメーター。
		/// </summary>
		public string RawParameter { get; private set; }
		/// <summary>
		/// 生パラメーターをいい感じに分割したパラメーターリスト。
		/// </summary>
		public IReadOnlyList<string> ParameterList { get; private set; }

		#endregion

		#region check

		/// <summary>
		/// パラメーター数は指定値でなければならない。
		/// </summary>
		/// <param name="count"></param>
		void EnforceParameterCount(int count)
		{
			if(ParameterList.Count != count) {
				throw new TinyMacroParseException(this, "parameter count", ParameterList.Count, count);
			}
		}
		/// <summary>
		/// パラメーター数は指定値以上でなければならない。
		/// </summary>
		/// <param name="count"></param>
		void EnforceParameterCountMoreThan(int count)
		{
			if(ParameterList.Count < count) {
				throw new TinyMacroParseException(this, "parameter count", ParameterList.Count, count);
			}
		}

		/// <summary>
		/// 値をintへ変換する。
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		int EnforceConvertInteger(string value)
		{
			int result;
			if(!int.TryParse(value, out result)) {
				throw new TinyMacroParseException(this, "convert", typeof(int).ToString(), value);
			}
			return result;
		}

		/// <summary>
		/// パラメーターをintへ変換する。
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		int EnforceConvertIntegerFromParameter(int index)
		{
			var rawParam = ParameterList.ElementAtOrDefault(index);
			return EnforceConvertInteger(rawParam);
		}

		#endregion

		#region Macro

		/// <summary>
		/// 生パラメーターの文字数取得。
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
		/// 生パラメーターを行毎にトリムる。
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

		/// <summary>
		/// 変換パラメータ1の各行から変換パラメータ2で指定された行数のみを取得する。
		/// </summary>
		/// <returns></returns>
		protected virtual string ExecuteLine()
		{
			EnforceParameterCountMoreThan(2);
			/*
			var number = ParameterList[1];
			int lineNumber;
			if(!int.TryParse(number, out lineNumber)) {
				throw new TinyMacroParseException(this, "line number", number);
			}
			*/
			var lineNumber = EnforceConvertIntegerFromParameter(1);

			var lines = ParameterList.First().SplitLines().ToArray();
			if(lineNumber.Between(1, lines.Length)) {
				return lines[lineNumber - 1];
			} else {
				throw new TinyMacroParseException(this, "out range", lineNumber);
			}
		}

		/// <summary>
		/// 生パラメーターから環境変数(%xx%)を展開する。
		/// </summary>
		/// <returns></returns>
		protected virtual string ExecuteEnvironment()
		{
			return Environment.ExpandEnvironmentVariables(RawParameter);
		}

		string ExecuteSubstring_Impl(string target, int start, int length, bool leftToRight)
		{
			if(length == 0) {
				return string.Empty;
			}
			Debug.Assert(start >= 0);
			Debug.Assert(length >= 0);

			if(leftToRight) {
				return string.Concat(target.Skip(start).Take(length).ToArray());
			} else {
				return string.Concat(target.Reverse().Skip(start).Take(length).Reverse().ToArray());
			}
		}

		int ExecuteSubstring_GetLength(int index, int defaultValue)
		{
			var rawLength = ParameterList.ElementAtOrDefault(index);
			int safeLength;
			if(!int.TryParse(rawLength, out safeLength)) {
				safeLength = defaultValue;
			}
			if(safeLength < 0) {
				throw new TinyMacroParseException(this, "length", safeLength);
			}

			return safeLength;
		}

		protected virtual string ExecuteLeft()
		{
			var safeLength = ExecuteSubstring_GetLength(1, 1);

			return ExecuteSubstring_Impl(ParameterList[0], 0, safeLength, true);
		}

		protected virtual string ExecuteRight()
		{
			var safeLength = ExecuteSubstring_GetLength(1, 1);

			return ExecuteSubstring_Impl(ParameterList[0], 0, safeLength, false);
		}

		protected virtual string ExecuteSubstring()
		{
			EnforceParameterCountMoreThan(2);

			var rawStart = ParameterList[1];
			int safeStart = EnforceConvertIntegerFromParameter(1);
			if(safeStart < 1) {
				throw new TinyMacroParseException(this, "start", safeStart);
			}
			// マクロ引数から内部用に位置を補正
			safeStart -= 1;

			var safeLength = ExecuteSubstring_GetLength(2, 0);

			if(ParameterList[0].Length < safeStart) {
				return string.Empty;
			}

			return ExecuteSubstring_Impl(ParameterList[0], safeStart, safeLength, true);
		}

		protected string ExecuteRepeat()
		{
			EnforceParameterCount(2);
			var count = EnforceConvertIntegerFromParameter(1);
			return string.Concat(Enumerable.Repeat(ParameterList[0], count));
		}

		#endregion

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
				{ MacroName.environment, ExecuteEnvironment },
				{ MacroName.left, ExecuteLeft },
				{ MacroName.right, ExecuteRight },
				{ MacroName.substring, ExecuteSubstring },
				{ MacroName.repeat, ExecuteRepeat },
			}.ToDictionary(p => p.Key.ToLower(), p => p.Value);

			try {
				Func<string> fn;
				if(map.TryGetValue(Name.ToLower(CultureInfo.InvariantCulture), out fn)) {
					return fn();
				} else {
					throw new TinyMacroExecuteException(this, "undefined");
				}
			} catch(TinyMacroException ex) {
				Debug.WriteLine(ex);
				return ex.ErrorMessage;
			}
		}


	}
}
