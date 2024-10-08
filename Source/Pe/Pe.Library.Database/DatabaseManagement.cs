using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Database
{
    public class DatabaseManagement: IDatabaseManagement
    {
        #region IDatabaseManagement

        public virtual ISet<DatabaseInformationItem> GetDatabaseItems()
        {
            throw new NotImplementedException();
        }

        public virtual ISet<DatabaseSchemaItem> GetSchemaItems(DatabaseInformationItem informationItem)
        {
            throw new NotImplementedException();
        }

        public virtual ISet<DatabaseResourceItem> GetResourceItems(DatabaseSchemaItem schemaItem, DatabaseResourceKinds kinds)
        {
            throw new NotImplementedException();
        }

        public virtual IList<DatabaseColumnItem> GetColumns(DatabaseResourceItem tableResource)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class DatabaseManagementWithContext: DatabaseManagement
    {
        public DatabaseManagementWithContext(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            Context = context;
            Implementation = implementation;
        }

        #region property

        protected IDatabaseContext Context { get; }
        protected IDatabaseImplementation Implementation { get; }

        #endregion
    }
}
