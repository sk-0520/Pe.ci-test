using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    public interface IDatabaseCommands
    {
        #region property

        IDatabaseCommander Commander { get; }
        IDatabaseImplementation Implementation { get; }

        #endregion
    }

    /// <inheritdoc cref="IDatabaseCommands"/>
    public class DatabaseCommands: IDatabaseCommands
    {
        public DatabaseCommands(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Commander = commander;
            Implementation = implementation;
        }

        #region IDatabaseCommands

        /// <inheritdoc cref="IDatabaseCommands.Commander"/>
        public IDatabaseCommander Commander { get; }

        /// <inheritdoc cref="IDatabaseCommands.Implementation"/>
        public IDatabaseImplementation Implementation { get; }

        #endregion
    }

}
