using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public abstract class KeyboardJobSettingEditorElementBase : ElementBase
    {
        public KeyboardJobSettingEditorElementBase(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }

        #endregion
    }

    public sealed class KeyboardReplaceJobSettingEditorElement : KeyboardJobSettingEditorElementBase
    {
        public KeyboardReplaceJobSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, statementLoader, loggerFactory)
        { }


        #region KeyboardJobSettingEditorElementBase

        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
