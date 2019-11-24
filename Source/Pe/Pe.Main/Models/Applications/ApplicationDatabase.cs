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

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class ApplicationDatabaseFactory : SqliteFactory
    {
        #region define
        #endregion

        public ApplicationDatabaseFactory()
        {
            var builder = CreateConnectionBuilder();
            builder.DataSource = ":memory:";
            builder.ForeignKeys = true;

            ConnectionString = builder.ToString();
        }

        public ApplicationDatabaseFactory(FileInfo file)
        {
            var builder = CreateConnectionBuilder();
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

        public override bool IsNull(object? value)
        {
            if(base.IsNull(value)) {
                return true;
            }
            if(value == null) {
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
            var skip = !true;
            if(skip) {
                return;
            }

            if(!Logger.IsEnabled(LogLevel.Trace)) {
                return;
            }

            var indent = "    ";

            var lines = TextUtility.ReadLines(statement).ToList();
            var width = (1 + (int)Math.Log10(lines.Count)).ToString();

            var sb = new StringBuilder(lines.Sum(s => s.Length) * 2);

            void Logging(ObjectDumpItem dumpItem, int nest)
            {
                var ns = new string('-', (int)(nest * 1.5)) + "->";
                sb.Append(indent);
                sb.Append(ns);
                sb.Append(' ');
                sb.Append(dumpItem.MemberInfo);
                sb.Append('=');
                sb.Append(dumpItem.Value);
                sb.Append(" [");
                sb.Append(dumpItem.MemberInfo.DeclaringType);
                sb.Append(']');
                sb.AppendLine();

                foreach(var childItem in dumpItem.Children) {
                    Logging(childItem, nest + 1);
                }
            }

            var method = new StackTrace(4)?.GetFrame(0)?.GetMethod();
            if(method != null) {
                sb.AppendLine(method.ReflectedType!.Name + "." + method.Name);
            } else {
                sb.AppendLine(nameof(LoggingStatement));
            }


            sb.Append(indent);
            sb.AppendLine("[SQL]");
            foreach(var line in lines.Counting(1)) {
                sb.Append(indent);
                sb.AppendFormat("{0," + width + "}: ", line.Number);
                sb.AppendLine(line.Value);
            }
            if(parameter != null) {
                sb.Append(indent);
                sb.AppendLine("[PARAM]");

                var od = new ObjectDumper();
                var dumpItems = od.Dump(parameter);
                foreach(var dumpItem in dumpItems) {
                    Logging(dumpItem, 0);
                }
            }

            using(Logger.BeginScope(nameof(LoggingStatement))) {
                Logger.LogTrace(sb.ToString());
            }

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
        public Encoding SqlFileEncoding { get; set; } = Encoding.UTF8;

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

        public override string LoadStatementByCurrent(Type callerType, [CallerMemberName] string callerMemberName = "")
        {
            Debug.Assert(callerType.FullName != null);

            var baseNamespace = callerType.FullName.Substring(IgnoreNamespace.Length + 1);
            var key = baseNamespace + "." + callerMemberName;
            return LoadStatement(key);
        }

        #endregion
    }
}
