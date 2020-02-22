using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    public interface ICommandFinder: IDisposable
    {
        #region function

        /// <summary>
        /// 現在状態を更新。
        /// </summary>
        void Refresh();

        IEnumerable<ICommandItem> ListupCommandItems(string inputValue, Regex inputRegex, CancellationToken cancellationToken);

        #endregion
    }
}
