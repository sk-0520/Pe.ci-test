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
    public class ApplicationDatabaseFactory: SqliteFactory
    {
        #region define
        #endregion

        public ApplicationDatabaseFactory(bool foreignKeys, bool isReadOnly)
        {
            var builder = CreateConnectionBuilder();
            builder.DataSource = ":memory:";
            builder.ForeignKeys = foreignKeys;
            if(isReadOnly) {
                builder.ReadOnly = isReadOnly;
            }

            ConnectionString = builder.ToString();
        }

        public ApplicationDatabaseFactory(FileInfo file, bool foreignKeys, bool isReadOnly)
        {
            var builder = CreateConnectionBuilder();
            builder.DataSource = ToSafeFile(file).FullName;
            builder.ForeignKeys = foreignKeys;
            if(isReadOnly) {
                builder.ReadOnly = isReadOnly;
            }

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


    public class ApplicationDatabaseImplementation: SqliteImplementation
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
    }

    public class ApplicationDatabaseAccessor: SqliteAccessor
    {
        public ApplicationDatabaseAccessor(IDatabaseFactory connectionCreator, ILogger logger)
            : base(connectionCreator, logger)
        { }

        public ApplicationDatabaseAccessor(IDatabaseFactory connectionCreator, ILoggerFactory loggerFactory)
            : base(connectionCreator, loggerFactory)
        { }

        #region function

        internal void CopyTo(string sourceName, ApplicationDatabaseAccessor destination, string destinationName)
        {
            Connection.BackupDatabase(destination.Connection, destinationName, sourceName, -1, null, -1);
        }

        #endregion

        #region DatabaseAccessor

        protected override void LoggingStatement(string statement, object? parameter)
        {
#if DEBUG
            ThrowIfDisposed();

            if(!Logger.IsEnabled(LogLevel.Trace)) {
                return;
            }

            var indent = "    ";

            var lines = TextUtility.ReadLines(statement).ToList();

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
#endif
        }

        #endregion
    }

    internal readonly struct StatementAccessorParameter
    {
        public StatementAccessorParameter(string fullName)
        {
            if(fullName == null) {
                throw new ArgumentNullException(fullName);
            }

            var values = fullName.Split('.', StringSplitOptions.None);

            if(values.Length < 2) {
                throw new ArgumentException(fullName);
            }

            if(values.Any(i => string.IsNullOrWhiteSpace(i))) {
                throw new ArgumentException(fullName);
            }

            if(values.Length == 2) {
                Namespace = string.Empty;
                ClassName = values[0];
                MethodName = values[1];
            } else {
                Namespace = string.Join('.', values[0..^2]);
                ClassName = values[^2];
                MethodName = values[^1];
            }
        }

        #region property

        public string Namespace { get; }
        public string ClassName { get; }
        public string MethodName { get; }

        #endregion
    }


    public class ApplicationDatabaseStatementLoader: DatabaseStatementLoaderBase, IDisposable
    {
        #region define

        public const string IgnoreNamespace = "ContentTypeTextNet.Pe.Main";
        const string SelectStatement = @"
select
    Statements.Statement
from
    Statements
where
    Statements.Namespace = @Namespace
    and
    Statements.ClassName = @ClassName
    and
    Statements.MethodName = @MethodName
limit
    1
                ";

        #endregion

        public ApplicationDatabaseStatementLoader(DirectoryInfo baseDirectory, TimeSpan timelimit, DatabaseAccessor? statementAccessor, bool givePriorityToFile, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            BaseDirectory = baseDirectory;
            StatementCache = new ReferencePool<string, string>(TimeSpan.FromMinutes(10), timelimit, false, loggerFactory);
            GivePriorityToFile = givePriorityToFile;
            StatementAccessor = statementAccessor;

            if(StatementAccessor == null) {
                Logger.LogInformation("SQL文読み込み方法 -> ファイル: {0}", BaseDirectory.FullName);
            } else {
                if(GivePriorityToFile) {
                    Logger.LogInformation("SQL文読み込み方法 -> 存在ファイル優先のsqlite: {0} -> {1}", BaseDirectory.FullName, StatementAccessor.BaseConnection.ConnectionString);
                } else {
                    Logger.LogInformation("SQL文読み込み方法 -> sqlite: {0}", StatementAccessor.BaseConnection.ConnectionString);
                }
            }
        }

        #region property

        DirectoryInfo BaseDirectory { get; }
        ReferencePool<string, string> StatementCache { get; }

        /// <summary>
        /// SQLファイルを優先して読み込む。
        /// </summary>
        bool GivePriorityToFile { get; }
        DatabaseAccessor? StatementAccessor { get; }

        public int SqlFileBufferSize { get; set; } = 4096;
        public Encoding SqlFileEncoding { get; set; } = Encoding.UTF8;

        #endregion

        #region function

        string ConvertFileName(string key)
        {
            var keyPath = key.Substring(IgnoreNamespace.Length + 1).Replace('.', Path.DirectorySeparatorChar) + ".sql";
            var filePath = Path.Combine(BaseDirectory.FullName, keyPath);

            return filePath;
        }

        string CreateCacheFromFile(string filePath)
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, SqlFileBufferSize);
            using var reader = new StreamReader(stream, SqlFileEncoding);
            return reader.ReadToEnd();
        }

        string CreateCacheFromAccessor(string key)
        {
            Debug.Assert(StatementAccessor != null);

            var statementAccessorParameter = new StatementAccessorParameter(key);
            return StatementAccessor.QueryFirst<string>(SelectStatement, statementAccessorParameter);
        }

        string CreateCache(string key)
        {
#if DEBUG
            using var x = ActionDisposerHelper.Create((d, sw) => Logger.LogTrace("SQL読み込み時間: {0}, {1}", sw.Elapsed, key), Stopwatch.StartNew());
#endif
            if(StatementAccessor == null) {
                return CreateCacheFromFile(ConvertFileName(key));
            }

            if(GivePriorityToFile) {
                var filePath = ConvertFileName(key);
                if(File.Exists(filePath)) {
                    Logger.LogDebug("{0} に該当するファイルが存在するため優先実行: {1}", key, filePath);
                    return CreateCacheFromFile(filePath);
                }
            }

            return CreateCacheFromAccessor(key);
        }

        string LoadStatementCore(string key)
        {
            Debug.Assert(0 < key.Length);

            return StatementCache.Get(key, CreateCache);
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

            var key = callerType.FullName + "." + callerMemberName;
            return LoadStatement(key);
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposedValue) {
                if(disposing) {
                    StatementAccessor?.Dispose();
                    StatementCache.Dispose();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

    }
}
