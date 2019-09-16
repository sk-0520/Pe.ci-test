using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Core.Models.Database.Vender.Public.SQLite;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Database
{
    public class ApplicationDatabaseFactory : SqliteFactory
    {
        #region define
        #endregion

        public ApplicationDatabaseFactory()
        {
            var builder = CreateCommonBuilder();
            builder.DataSource = ":memory:";
            builder.ForeignKeys = true;

            ConnectionString = builder.ToString();
        }

        public ApplicationDatabaseFactory(FileInfo file)
        {
            var builder = CreateCommonBuilder();
            builder.DataSource = ToSafeFile(file).FullName;
            builder.ForeignKeys = true;

            ConnectionString = builder.ToString();
        }

        #region property

        string ConnectionString { get; }

        #endregion

        #region IDatabaseFactory

        public override IDbConnection CreateConnection() => new SQLiteConnection(ConnectionString);

        public override IDatabaseImplementation CreateImplementation() => new ApplicationDatabaseImplementation();

        #endregion
    }

    public class ApplicationDatabaseImplementation : SqliteImplementation
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

        #region DatabaseImplementation

        public override object GetNullValue(Type type)
        {
            if(NullMapping.TryGetValue(type, out var value)) {
                return value;
            }

#pragma warning disable CS8603 // Null 参照戻り値である可能性があります。
            return base.GetNullValue(type);
#pragma warning restore CS8603 // Null 参照戻り値である可能性があります。
        }

        public override bool IsNull(object value)
        {
            if(base.IsNull(value)) {
                return true;
            }

            return value.Equals(GetNullValue(value.GetType()));
        }

        public override string PreFormatStatement(string statement)
        {
            return string.Join(
                Environment.NewLine,
                TextUtility.ReadLines(statement)
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

        public ApplicationDatabaseAccessor(IDatabaseFactory connectionCreator, ILoggerFactory loggerFactory)
            : base(connectionCreator, loggerFactory)
        { }

        #region function

        #endregion

        #region DatabaseAccessor

        protected override void LoggingStatement(string statement, object? parameter)
        {
            Logger.LogTrace(statement, ObjectDumper.GetDumpString(parameter));
        }

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

        public ApplicationDatabaseStatementLoader(DirectoryInfo baseDirectory, TimeSpan lifeTime, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            BaseDirectory = baseDirectory;
            StatementCache = new CachePool<string, string>(lifeTime);
        }

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

        [MethodImpl(MethodImplOptions.NoInlining)]
        public override string LoadStatementByCurrent()
        {
            var member = GetCurrentMember();
#pragma warning disable CS8602 // null 参照の可能性があるものの逆参照です。
            var baseNamespace = member.DeclaringType.FullName.Substring(IgnoreNamespace.Length + 1);
#pragma warning restore CS8602 // null 参照の可能性があるものの逆参照です。
            var key = baseNamespace + "." + member.Name;
            return LoadStatement(key);
        }

        #endregion
    }
}
