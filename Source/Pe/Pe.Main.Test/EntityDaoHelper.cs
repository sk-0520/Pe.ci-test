using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Database;

namespace ContentTypeTextNet.Pe.Main.Test
{
    public class EntityDaoHelper
    {
        public EntityDaoHelper(IDatabaseContext context, IDatabaseImplementation implementation)
        {
            Context = context;
            Implementation = implementation;
        }

        #region property

        private IDatabaseContext Context { get; }
        private IDatabaseImplementation Implementation { get; }

        #endregion

        #region function

        private DatabaseResourceItem GetTableResource(IDatabaseManagement management, string tableName)
        {
            var database = management.GetDatabaseItems().First(a => a.Name == "main");
            var schema = management.GetSchemaItems(database).First();
            return management.GetResourceItems(schema, DatabaseResourceKinds.Table).First(a => a.Name == tableName);
        }

        public void CloneRecord(string tableName, IReadOnlyDictionary<string, string> dynamicItems, IReadOnlyDictionary<string, string> source)
        {
            var management = Implementation.CreateManagement(Context);
            var tableResource = GetTableResource(management, tableName);
            var columns = management.GetColumns(tableResource);

            var dynamicList = dynamicItems.ToArray();
            var keys = dynamicList.Select(a => a.Key).ToArray();

            var sb = new StringBuilder();
            sb.AppendLine("insert into");
            sb.AppendLine(tableName);
            sb.AppendLine("(");
            sb.AppendLine(
                string.Join(
                    ",",
                    keys.Concat(
                        columns
                            .Where(a => !keys.Contains(a.Name))
                            .Select(a => a.Name)
                    )
                )
            );
            sb.AppendLine(")");
            sb.AppendLine("select");
            foreach(var pair in dynamicList) {
                sb.Append(pair.Value);
                sb.AppendLine(",");
            }
            var first = true;
            foreach(var column in columns.Where(a => !dynamicItems.ContainsKey(a.Name))) {
                if(first) {
                    first = false;
                } else {
                    sb.Append(",");
                }
                sb.Append(column.Name);
            }
            sb.AppendLine();
            sb.AppendLine("from");
            sb.AppendLine(tableName);

            sb.AppendLine("where");
            sb.AppendLine(String.Join(" and ", source.Select(a => $"{a.Key} = {a.Value}")));

            var sql = sb.ToString();
            Context.InsertSingle(sql);
        }

        #endregion
    }
}
