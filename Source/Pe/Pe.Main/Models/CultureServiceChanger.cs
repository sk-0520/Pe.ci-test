using System;
using System.Linq;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models
{

    public class CultureServiceChanger
    {
        public CultureServiceChanger(CultureService cultureService, IWindowManager windowManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ApplicationConfiguration applicationConfiguration, ILoggerFactory loggerFactory)
        {
            CultureService = cultureService;
            WindowManager = windowManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            ApplicationConfiguration = applicationConfiguration;
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
        }

        #region property

        private CultureService CultureService { get; }
        private IWindowManager WindowManager { get; }
        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private ApplicationConfiguration ApplicationConfiguration { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        #endregion

        #region function

        private string LoadLanguageName()
        {
            string lang;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new AppGeneralSettingEntityDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                lang = dao.SelectLanguage();
            }

            // もうちょっと柔軟性あってもいいと思うよ
            return ApplicationConfiguration.General.SupportCultures
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

            var language = CultureService.GetXmlLanguage();
            var views = Enum.GetValues<WindowKind>()
                .Select(i => WindowManager.GetWindowItems(i))
                .SelectMany(i => i)
                .Select(i => i.Window)
            ;
            foreach(var view in views) {
                view.Language = language;
            }
        }

        #endregion
    }
}
