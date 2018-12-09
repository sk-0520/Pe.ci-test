using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Database
{
    public class ApplicationDatabaseFactory : SqliteFactory
    {
        public ApplicationDatabaseFactory()
        {
            var builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = ":memory:";

            ConnectionString = builder.ToString();
        }

        public ApplicationDatabaseFactory(FileInfo file)
        {
            var builder = new SQLiteConnectionStringBuilder();
            builder.DataSource = file.FullName;

            ConnectionString = builder.ToString();
        }

        #region property

        string ConnectionString { get; }

        #endregion

        #region function

        public static FileInfo ToSafeFile(FileInfo fileInfo)
        {
            // #66 を考慮
            if(fileInfo.FullName.StartsWith(@"\\")) {
                return new FileInfo(@"\\" + fileInfo.FullName);
            }

            return fileInfo;
        }

        #endregion

        #region IDatabaseFactory

        public override IDbConnection CreateConnection() => new SQLiteConnection(ConnectionString);

        public override IDatabaseImplementation CreateImplementation() => new ApplicationDatabaseImplementation();

        #endregion
    }

    public class ApplicationDatabaseImplementation : DatabaseImplementation
    {
        #region property

        Dictionary<Type, object> NullMapping { get; } = new Dictionary<Type, object>() {
            [typeof(string)] = string.Empty,
            [typeof(DateTime)] = DateTime.MinValue,
        };

        #endregion

        #region function

        bool IsIgnoreStatement(string sqlLine)
        {
            if(string.IsNullOrWhiteSpace(sqlLine)) {
                return true;
            }

            if(sqlLine.StartsWith("--", StringComparison.Ordinal)) {
                return true;
            }

            if(sqlLine.StartsWith("DECLARE", StringComparison.InvariantCultureIgnoreCase)) {
                return true;
            }
            if(sqlLine.StartsWith("DEC", StringComparison.InvariantCultureIgnoreCase)) {
                return true;
            }
            if(sqlLine.StartsWith("SET", StringComparison.InvariantCultureIgnoreCase)) {
                return true;
            }

            return false;
        }

        #endregion

        #region DatabaseChecker

        public override object GetNullValue(Type type)
        {
            if(NullMapping.TryGetValue(type, out var value)) {
                return value;
            }

            return base.GetNullValue(type);
        }

        public override bool IsNull(object value)
        {
            if(base.IsNull(value)) {
                return true;
            }

            return value.Equals(GetNullValue(value.GetType()));
        }

        public override string PreFormatSql(string sql)
        {
            return string.Join(
                Environment.NewLine,
                TextUtility.ReadLines(sql)
                    .Select(s => s.Trim())
                    .SkipWhile(s => IsIgnoreStatement(s))
            );
        }

        #endregion
    }

    public class ApplicationDatabaseAccessor : DatabaseAccessor<SQLiteConnection>
    {
        public ApplicationDatabaseAccessor(IDatabaseFactory connectionCreator, ILogger logger)
            : base(connectionCreator, logger)
        { }
        [Injection]
        public ApplicationDatabaseAccessor(IDatabaseFactory connectionCreator, ILogFactory logFactory)
            : this(connectionCreator, logFactory.CreateCurrentClass())
        { }

        #region function

        #endregion

    }

    public class ApplicationDatabaseStatementLoader : DatabaseStatementLoaderBase
    {
        #region define

        public const string IgnoreNamespace = "ContentTypeTextNet.Pe.Main";

        #endregion

        public ApplicationDatabaseStatementLoader(DirectoryInfo baseDirectory, TimeSpan lifeTime, ILogger logger)
            : base(logger)
        {
            BaseDirectory = baseDirectory;
            StatementCache = new CachePool<string, string>(lifeTime);
        }

        [Injection]
        public ApplicationDatabaseStatementLoader(DirectoryInfo baseDirectory, TimeSpan lifeTime, ILogFactory logFactory)
            : this(baseDirectory, lifeTime, logFactory.CreateCurrentClass())
        { }

        #region property

        DirectoryInfo BaseDirectory { get; }
        CachePool<string, string> StatementCache { get; }

        public int SqlFileBufferSize { get; set; } = 4096;
        public Encoding SqlFileEncoding { get; set; } = Encoding.Unicode;

        #endregion

        #region function

        string CreateCache(string key)
        {
            var keyPath = key.Replace('.', Path.DirectorySeparatorChar) + ".sql";
            var filePath = Path.Combine(BaseDirectory.FullName, keyPath);

            using(var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, SqlFileBufferSize)) {
                using(var reader = new StreamReader(stream, SqlFileEncoding)) {
                    return reader.ReadToEnd();
                }
            }
        }

        string LoadStatementCore(string key)
        {
            Debug.Assert(0 < key.Length);

            return StatementCache.GetOrAdd(key, CreateCache);
        }

        #endregion

        #region DatabaseStatementLoaderBase

        public override string LoadStatement(string key)
        {
            if(string.IsNullOrEmpty(key)) {
                throw new ArgumentNullException(nameof(key));
            }

            return LoadStatementCore(key);
        }

        public override string LoadStatementByCurrent()
        {
            var member = GetCurrentMember();
            var baseNamespace = member.DeclaringType.FullName.Substring(IgnoreNamespace.Length + 1);
            var key = baseNamespace + "." + member.Name;
            return LoadStatement(key);
        }

        #endregion
    }
}
