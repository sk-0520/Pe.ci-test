using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Note
{
    public class NoteFileElement: ElementBase
    {
        public NoteFileElement(NoteFileData data, IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, IDatabaseStatementLoader databaseStatementLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            NoteId = data.NoteId;
            NoteFileId = data.NoteFileId;
            NoteFileKind = data.NoteFileKind;
            NoteFilePath = data.NoteFilePath;
            Sequence = data.Sequence;

            MainDatabaseBarrier = mainDatabaseBarrier;
            LargeDatabaseBarrier = largeDatabaseBarrier;
            DatabaseStatementLoader = databaseStatementLoader;
            DispatcherWrapper = dispatcherWrapper;

            IconImageLoader = new IconImageLoader(
                new Data.IconData() {
                    Path = NoteFilePath
                },
                DispatcherWrapper,
                LoggerFactory
            );
        }

        #region property

        public NoteId NoteId { get; }
        public NoteFileId NoteFileId { get; }

        private IMainDatabaseBarrier MainDatabaseBarrier { get; }
        private ILargeDatabaseBarrier LargeDatabaseBarrier { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        public NoteFileKind NoteFileKind { get; private set; }
        public string NoteFilePath { get; private set; } = string.Empty;
        public int Sequence { get; private set; }

        /// <summary>
        /// ファイルアイコン表示用。
        /// </summary>
        /// <remarks>
        /// <para>一応持ってるけど使わない方針。</para>
        /// </remarks>
        public IconImageLoader IconImageLoader { get; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}
