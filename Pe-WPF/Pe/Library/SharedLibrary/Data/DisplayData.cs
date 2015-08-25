namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	/// <summary>
	/// 表示文字列と値を持つデータ。
	/// </summary>
	/// <typeparam name="TValue">値の型。</typeparam>
	public class DisplayData<TValue>: IDisplayText
	{
		public DisplayData(string displayText, TValue value)
		{
			DisplayText = displayText;
			Value = value;
		}

		#region property

		public TValue Value { get; private set; }

		#endregion

		#region IDisplayText

		public string DisplayText { get; private set; }

		#endregion
	}

	/// <summary>
	/// ヘルパ
	/// </summary>
	public static class DisplayData
	{
		public static DisplayData<TValue> Create<TValue>(string displayText, TValue value)
		{
			var result = new DisplayData<TValue>(displayText, value);
			return result;
		}
	}

}
