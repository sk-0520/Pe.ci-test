/**
This file is part of SharedLibrary.

SharedLibrary is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SharedLibrary is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SharedLibrary.  If not, see <http://www.gnu.org/licenses/>.
*/
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
