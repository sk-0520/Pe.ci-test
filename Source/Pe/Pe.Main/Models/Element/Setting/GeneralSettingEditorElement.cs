using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public abstract class GeneralSettingEditorElementBase : ElementBase
    {
        public GeneralSettingEditorElementBase(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IFileDatabaseBarrier FileDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        #endregion

        #region function

        public void Save(DatabaseCommandPack commandPack)
        {
            SaveImpl(commandPack);
        }

        protected abstract void SaveImpl(DatabaseCommandPack commandPack);

        #endregion
    }

    public class AppExecuteSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppExecuteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public string UserId { get; set; } = string.Empty;
        public bool SendUsageStatistics { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppExecuteSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();
            }

            UserId = setting.UserId;
            SendUsageStatistics = setting.SendUsageStatistics;

            var userIdManager = new UserIdManager(LoggerFactory);
            if(!userIdManager.IsValidUserId(UserId)) {
                UserId = userIdManager.CreateFromEnvironment();
            }
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(commandPack.Main.Commander, StatementLoader, commandPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppExecuteSettingData() {
                SendUsageStatistics = SendUsageStatistics,
                UserId = UserId,
            };
            appExecuteSettingEntityDao.UpdateSettingExecuteSetting(data, commandPack.CommonStatus);
        }

        #endregion
    }


    public class AppGeneralSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppGeneralSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public CultureInfo CultureInfo { get; set; } = CultureInfo.CurrentCulture;

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppGeneralSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appGeneralSettingEntityDao = new AppGeneralSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appGeneralSettingEntityDao.SelectSettingGeneralSetting();
            }

            CultureInfo = CultureInfo.GetCultureInfo(setting.Language);
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var appGeneralSettingEntityDao = new AppGeneralSettingEntityDao(commandPack.Main.Commander, StatementLoader, commandPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppGeneralSettingData() {
                Language = CultureInfo.Name,
            };
            appGeneralSettingEntityDao.UpdateSettingGeneralSetting(data, commandPack.CommonStatus);
        }

        #endregion
    }


    public class AppUpdateSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppUpdateSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public bool IsCheckReleaseVersion { get; set; }
        public bool IsCheckRcVersion { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppUpdateSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appUpdateSettingEntityDao = new AppUpdateSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appUpdateSettingEntityDao.SelectSettingUpdateSetting();
            }

            IsCheckReleaseVersion = setting.IsCheckReleaseVersion;
            IsCheckRcVersion = setting.IsCheckRcVersion;
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var appUpdateSettingEntityDao = new AppUpdateSettingEntityDao(commandPack.Main.Commander, StatementLoader, commandPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppUpdateSettingData() {
                IsCheckReleaseVersion = IsCheckReleaseVersion,
                IsCheckRcVersion = IsCheckRcVersion,
            };
            appUpdateSettingEntityDao.UpdateSettingUpdateSetting(data, commandPack.CommonStatus);
        }

        #endregion
    }



    public class AppCommandSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppCommandSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }



    public class AppNoteSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppNoteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }



    public class AppStandardInputOutputSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppStandardInputOutputSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }


    public class AppWindowSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppWindowSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public bool IsEnabled { get; set; }
        public int Count { get; set; }
        public TimeSpan Interval { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            SettingAppWindowSettingData setting;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appWindowSettingEntityDao = new AppWindowSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                setting = appWindowSettingEntityDao.SelectSettingWindowSetting();
            }

            IsEnabled = setting.IsEnabled;
            Count = setting.Count;
            Interval = setting.Interval;
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var appWindowSettingEntityDao = new AppWindowSettingEntityDao(commandPack.Main.Commander, StatementLoader, commandPack.Main.Implementation, LoggerFactory);
            var data = new SettingAppWindowSettingData() {
                IsEnabled = IsEnabled,
                Count = Count,
                Interval = Interval,
            };
            appWindowSettingEntityDao.UpdateSettinWindowSetting(data, commandPack.CommonStatus);
        }

        #endregion
    }

}
