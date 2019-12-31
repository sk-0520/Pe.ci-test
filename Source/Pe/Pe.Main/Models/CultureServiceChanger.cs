using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public class CultureServiceChanger
    {
        public CultureServiceChanger(CultureService cultureService, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, Configuration configuration, ILoggerFactory loggerFactory)
        {
            CultureService = cultureService;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;
            Configuration = configuration;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        CultureService CultureService { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        Configuration Configuration { get; }
        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        string LoadLanguageName()
        {
            string lang;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new AppGeneralSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                lang = dao.SelectLanguage();
            }

            // もうちょっと柔軟性あってもいいと思うよ
            return Configuration.General.SupportCultures
                .FirstOrDefault(i => i.Equals(lang, StringComparison.OrdinalIgnoreCase))
                ?? string.Empty
            ;
        }

        public void ChangeCulture()
        {
            var languageName = LoadLanguageName();
            if(string.IsNullOrWhiteSpace(languageName)) {
                CultureService.ChangeAutoCulture();
            } else {
                CultureService.ChangeCulture(languageName);
            }
        }

        #endregion
    }
}
