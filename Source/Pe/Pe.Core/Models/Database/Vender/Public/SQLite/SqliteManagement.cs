using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Database;

namespace ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite
{
    public class SqliteManagement: DatabaseManagementWithContext
    {
        public SqliteManagement(IDatabaseContext context, IDatabaseImplementation implementation)
            : base(context, implementation)
        { }

        #region property

        private static Regex TypeRegex = new Regex(
            @"
            (?<TYPE>\w+)
            \s*
            \(
                \s*
                (?<PRECISION>\d+)
                \s*
                (
                    ,
                    \s*
                    (?<SCALE>\d+)
                    \s*
                )?
                \s*
            \)
            ",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture,
            Timeout.InfiniteTimeSpan
        );
        #endregion

        #region function

        internal static DatabaseColumnType ToDatabaseColumnType(string rawType)
        {
            int precision = 0, scale = 0;
            Type cliType = typeof(void);

            rawType = rawType.ToUpperInvariant();

            switch(rawType) {
                case "INTEGER":
                case "NUMERIC":
                    cliType = typeof(long);
                    break;

                case "REAL":
                    cliType = typeof(double);
                    break;

                case "TEXT":
                    cliType = typeof(string);
                    break;

                case "BLOB":
                    cliType = typeof(byte[]);
                    break;

                default:
                    var match = TypeRegex.Match(rawType);
                    if(match.Success) {
                        var t = match.Groups["TYPE"];

                        var column = ToDatabaseColumnType(t.Value);
                        cliType = column.CliType;

                        var p = match.Groups["PRECISION"];
                        precision = int.Parse(p.Value, CultureInfo.InvariantCulture);

                        var s = match.Groups["SCALE"];
                        if(!string.IsNullOrWhiteSpace(s.Value)) {
                            scale = int.Parse(s.Value, CultureInfo.InvariantCulture);
                        }
                    }
                    break;
            }

            return new DatabaseColumnType(
                rawType,
                precision,
                scale,
                cliType
            );
        }

        #endregion

        #region DatabaseManagementWithContext

        public override ISet<DatabaseInformationItem> GetDatabaseItems()
        {
            var rows = Context.Query<string>(@"
                select
                    pragma_database_list.name
                from
                    pragma_database_list
            ");

            return new HashSet<DatabaseInformationItem>(rows.Select(a => new DatabaseInformationItem(a)));
        }

        public override ISet<DatabaseSchemaItem> GetSchemaItems(DatabaseInformationItem informationItem)
        {
            return new HashSet<DatabaseSchemaItem>([
                new DatabaseSchemaItem(informationItem, informationItem.Name, informationItem.Name == "main")
            ]);
        }

        public override ISet<DatabaseResourceItem> GetResourceItems(DatabaseSchemaItem schemaItem, DatabaseResourceKinds kinds)
        {
            var kindItems = new List<string>();
            if(kinds.HasFlag(DatabaseResourceKinds.Table)) {
                kindItems.Add("table");
            }
            if(kinds.HasFlag(DatabaseResourceKinds.View)) {
                kindItems.Add("view");
            }
            if(kinds.HasFlag(DatabaseResourceKinds.Index)) {
                kindItems.Add("index");
            }

            var parameter = new {
                Kinds = kindItems
            };
            var rows = Context.Query(@"
                select
                    sqlite_master.type,
                    sqlite_master.name,
                    sqlite_master.sql
                from
                    sqlite_master
                where
                    sqlite_master.type in @Kinds
            ", parameter);

            var items = rows.Select(a => new DatabaseResourceItem(
                schemaItem,
                a.name,
                a.type switch {
                    "table" => DatabaseResourceKinds.Table,
                    "view" => DatabaseResourceKinds.View,
                    "index" => DatabaseResourceKinds.Index,
                    _ => throw new NotSupportedException(),
                },
                a.sql
            ));

            return new HashSet<DatabaseResourceItem>(items);
        }

        public override IList<DatabaseColumnItem> GetColumns(DatabaseResourceItem tableResource)
        {
            if(tableResource.Kind != DatabaseResourceKinds.Table) {
                throw new ArgumentException($"{tableResource.Kind}", nameof(tableResource));
            }

            var rows = Context.Query($@"
                PRAGMA table_info('{Implementation.Escape(tableResource.Name)}')
            ");

            var items = rows
                .Select(a => new DatabaseColumnItem(
                    tableResource,
                    a.name,
                    (int)a.cid,
                    (int)a.pk == 1,
                    (int)a.notnull != 1,
                    a.dflt_value ?? string.Empty,
                    ToDatabaseColumnType(a.type)
                ))
                .OrderBy(a => a.Position)
                .ToList()
            ;

            return items;
        }

        #endregion
    }
}
