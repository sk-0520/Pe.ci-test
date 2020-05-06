using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Command
{
    internal class CommandExecuteParameter: ICommandExecuteParameter
    {
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
