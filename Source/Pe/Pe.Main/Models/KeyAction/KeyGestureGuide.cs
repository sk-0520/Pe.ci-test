using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Domain;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.KeyAction
{
    public interface IKeyGestureGuide
    {
        #region property

        #endregion

        #region function

        string GetCommandKey();
        string GetNoteKey(KeyActionContentNote keyActionContentNote);
        IEnumerable<string> GetLauncherItemKeys(LauncherItemId launcherItemId);

        #endregion
    }

    internal class KeyGestureGuide: IKeyGestureGuide
    {
        public KeyGestureGuide(IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, ICultureService cultureService, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            MainDatabaseBarrier = mainDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            CultureService = cultureService;
        }

        #region property

        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private ICultureService CultureService { get; }
        #endregion

        #region function

        private string ConvertKeyText(KeyGestureItem keyGestureItem)
        {
            var factory = new KeyMappingFactory();

            var keyMessages = keyGestureItem.Mappings.Select(i => factory.ToString(CultureService, i, Properties.Resources.String_Hook_Keyboard_Join));
            var keyMessage = string.Join(Properties.Resources.String_Hook_Keyboard_Separator, keyMessages);

            return keyMessage;
        }

        private string ConvertFirstKeyText(KeyGestureSetting setting)
        {
            if(setting.Items.Count == 0) {
                return string.Empty;
            }

            var keyMessage = ConvertKeyText(setting.Items[0]);

            return keyMessage;
        }

        private string GetKeyMappingSting(KeyActionKind keyActionKind, string parameter)
        {
            KeyGestureSetting? setting = null;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new KeyGestureGuideDomainDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = dao.SelectKeyMappings(keyActionKind, parameter);
            }

            return ConvertFirstKeyText(setting);
        }

        private IEnumerable<string> GetLauncherItemKeyMappingStings(LauncherItemId launcherItemId)
        {
            KeyGestureSetting? setting = null;
            using(var context = MainDatabaseBarrier.WaitRead()) {
                var dao = new KeyGestureGuideDomainDao(context, DatabaseStatementLoader, context.Implementation, LoggerFactory);
                setting = dao.SelectLauncherKeyMappings(launcherItemId);
            }

            if(setting.Items.Count == 0) {
                return Enumerable.Empty<string>();
            }

            return setting.Items.Select(i => ConvertKeyText(i));
        }

        #endregion

        #region IKeyGestureGuide

        /// <inheritdoc cref="IKeyGestureGuide.GetCommandKey"/>
        public string GetCommandKey()
        {
            return GetKeyMappingSting(KeyActionKind.Command, string.Empty);
        }

        /// <inheritdoc cref="IKeyGestureGuide.GetNoteKey(KeyActionContentNote)"/>
        public string GetNoteKey(KeyActionContentNote keyActionContentNote)
        {
            var converter = new NoteContentConverter();
            var parameter = converter.ToContent(keyActionContentNote);
            return GetKeyMappingSting(KeyActionKind.Note, parameter);
        }

        /// <inheritdoc cref="IKeyGestureGuide.GetLauncherItemKeys(Guid)"/>
        public IEnumerable<string> GetLauncherItemKeys(LauncherItemId launcherItemId)
        {
            return GetLauncherItemKeyMappingStings(launcherItemId);
        }

        #endregion
    }
}
