using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    internal static class PersistentHelper
    {
        #region define

        public class PersistentCommandsPack: ApplicationDatabaseCommandsPack
        {
            public PersistentCommandsPack(IDatabaseTransaction mainTransaction, IDatabaseTransaction fileTransaction, IDatabaseTransaction temporaryTransaction, IDatabaseCommonStatus commonStatus)
                : base(
                    new DatabaseCommands(mainTransaction, mainTransaction.Implementation),
                    new DatabaseCommands(fileTransaction, mainTransaction.Implementation),
                    new DatabaseCommands(temporaryTransaction, mainTransaction.Implementation),
                    commonStatus
                )
            {
                MainTransaction = mainTransaction;
                FileTransaction = fileTransaction;
                TemporaryTransaction = temporaryTransaction;
            }

            #region property

            IDatabaseTransaction MainTransaction { get; }
            IDatabaseTransaction FileTransaction { get; }
            IDatabaseTransaction TemporaryTransaction { get; }

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

            #region ApplicationDatabaseCommandsPack

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

        static PersistentCommandsPack WaitPack(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus, bool isReadOnly)
        {
            static IDatabaseTransaction Do(IDatabaseBarrier databaseBarrier, bool isReadOnly)
            {
                if(isReadOnly) {
                    return databaseBarrier.WaitRead();
                }
                return databaseBarrier.WaitWrite();
            }

            var main = Do(mainDatabaseBarrier, isReadOnly);
            var file = Do(fileDatabaseBarrier, isReadOnly);
            var temp = Do(temporaryDatabaseBarrier, isReadOnly);

            var result = new PersistentCommandsPack(main, file, temp, databaseCommonStatus);

            return result;
        }

        public static PersistentCommandsPack WaitWritePack(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus)
        {
            return WaitPack(mainDatabaseBarrier, fileDatabaseBarrier, temporaryDatabaseBarrier, databaseCommonStatus, false);
        }

        public static PersistentCommandsPack WaitReadPack(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, ITemporaryDatabaseBarrier temporaryDatabaseBarrier, IDatabaseCommonStatus databaseCommonStatus)
        {
            return WaitPack(mainDatabaseBarrier, fileDatabaseBarrier, temporaryDatabaseBarrier, databaseCommonStatus, true);
        }

        #endregion
    }
}
