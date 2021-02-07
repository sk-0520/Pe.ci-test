using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    internal class CommandExecuteParameter: ICommandExecuteParameter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="screen">コマンド実行スクリーン。</param>
        /// <param name="isExtend">拡張機能を用いるか。</param>
        public CommandExecuteParameter(IScreen screen, bool isExtend)
        {
            Screen = screen ?? throw new ArgumentNullException(nameof(screen));
            IsExtend = isExtend;
        }

        #region ICommandExecuteParameter

        /// <inheritdoc cref="ICommandExecuteParameter.Screen"/>
        public IScreen Screen { get; }

        /// <inheritdoc cref="ICommandExecuteParameter.IsExtend"/>
        public bool IsExtend { get; }

        #endregion
    }
}
