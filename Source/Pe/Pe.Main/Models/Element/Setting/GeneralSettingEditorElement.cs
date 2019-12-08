using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public abstract class GeneralSettingEditorElementBase : ElementBase
    {
        public GeneralSettingEditorElementBase(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
        }

        #region property

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IFileDatabaseBarrier FileDatabaseBarrier { get; }
        #endregion
    }

    public class AppExecuteSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppExecuteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
        }

        #endregion
    }


    public class AppGeneralSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppGeneralSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        #endregion
    }


    public class AppUpdateSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppUpdateSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        #endregion
    }



    public class AppCommandSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppCommandSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        #endregion
    }



    public class AppNoteSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppNoteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        #endregion
    }



    public class AppStandardInputOutputSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppStandardInputOutputSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        #endregion
    }


    public class AppWindowSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppWindowSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        #endregion
    }

}
