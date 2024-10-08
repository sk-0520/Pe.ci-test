using System;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Library.Database;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    internal static class PersistenceHelper
    {
        #region define

        public class PersistenceContextsPack: ApplicationDatabaseContextsPack
        {
            public PersistenceContextsPack(IDatabaseTransaction mainTransaction, IDatabaseTransaction fileTransaction, IDatabaseTransaction temporaryTransaction, IDatabaseCommonStatus commonStatus)
                : base(
                    new DatabaseContexts(mainTransaction, mainTransaction.Implementation),
                    new DatabaseContexts(fileTransaction, mainTransaction.Implementation),
                    new DatabaseContexts(temporaryTransaction, mainTransaction.Implementation),
                    commonStatus
                )
            {
                MainTransaction = mainTransaction;
                FileTransaction = fileTransaction;
                TemporaryTransaction = temporaryTransaction;
            }

            #region property

            private IDatabaseTransaction MainTransaction { get; }
            private IDatabaseTransaction FileTransaction { get; }
            private IDatabaseTransaction TemporaryTransaction { get; }

            #endregion

            #region function

            public void Commit()
            {
                MainTransaction.Commit();
                FileTransaction.Commit();
                TemporaryTransaction.Commit();
            }

            public void Rollback()
            {
                MainTransaction.Rollback();
                FileTransaction.Rollback();
                TemporaryTransaction.Rollback();
            }

            #endregion

            #region ApplicationDatabaseContextsPack

            protected override void Dispose(bool disposing)
            {
                if(!IsDisposed) {
                    if(disposing) {
                        MainTransaction.Dispose();
                        FileTransaction.Dispose();
                        TemporaryTransaction.Dispose();
                    }
                }

                base.Dispose(disposing);
            }

            #endregion
        }

        #endregion

        #region function

        private static PersistenceContextsPack WaitPack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus, bool isReadOnly)
        {
            static IDatabaseTransaction Do(IDatabaseBarrier databaseBarrier, bool isReadOnly)
            {
                if(isReadOnly) {
                    return databaseBarrier.WaitRead();
                }
                return databaseBarrier.WaitWrite();
            }

            var main = Do(mainDatabaseBarrier, isReadOnly);
            var file = Do(largeDatabaseBarrier, isReadOnly);
            var temp = Do(temporaryDatabaseBarrier, isReadOnly);

            var result = new PersistenceContextsPack(main, file, temp, databaseCommonStatus);

            return result;
        }

        public static PersistenceContextsPack WaitWritePack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus)
        {
            return WaitPack(mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseCommonStatus, false);
        }

        public static PersistenceContextsPack WaitReadPack(IMainDatabaseBarrier mainDatabaseBarrier, ILargeDatabaseBarrier largeDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus)
        {
            return WaitPack(mainDatabaseBarrier, largeDatabaseBarrier, temporaryDatabaseBarrier, databaseCommonStatus, true);
        }

        /// <summary>
        /// データベース内データを複製。
        /// </summary>
        /// <remarks>
        /// <para>DB実装依存。Sqliteべったり。</para>
        /// </remarks>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        public static void Copy(IDatabaseAccessor source, IDatabaseAccessor destination)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            if(source == destination) {
                throw new ArgumentException($"{nameof(source)} == {nameof(destination)}");
            }

            //--------------------------------
            // SQLite しか知らん
            var src = (SqliteAccessor)source;
            var dst = (SqliteAccessor)destination;

            var dbNames = new[] { "main", "temp" };
            foreach(var dbName in dbNames) {
                src.CopyTo(dbName, dst, dbName);
            }
        }

        #endregion
    }
}
