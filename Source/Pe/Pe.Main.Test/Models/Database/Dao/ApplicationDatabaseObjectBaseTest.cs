using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Database.Dao
{
    public class ApplicationDatabaseObjectBaseTest
    {
        #region define

        class Dc: IDatabaseContext
        {
            public IDataReader GetDataReader(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public Task<IDataReader> GetDataReaderAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public DataTable GetDataTable(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public Task<DataTable> GetDataTableAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public TResult? GetScalar<TResult>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public Task<TResult?> GetScalarAsync<TResult>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<T> Query<T>(string statement, object? parameter = null, bool buffered = true)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<T>> QueryAsync<T>(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<dynamic> Query(string statement, object? parameter = null, bool buffered = true)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<dynamic>> QueryAsync(string statement, object? parameter = null, bool buffered = true, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public T QueryFirst<T>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public Task<T> QueryFirstAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public T QueryFirstOrDefault<T>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public Task<T?> QueryFirstOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public T QuerySingle<T>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public Task<T> QuerySingleAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public T QuerySingleOrDefault<T>(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }

            public Task<T?> QuerySingleOrDefaultAsync<T>(string statement, object? parameter = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }

            public int Execute(string statement, object? parameter = null)
            {
                throw new NotImplementedException();
            }
            public Task<int> ExecuteAsync(string statement, object? parameter = null, CancellationToken cancellationToken = default)
            {
                throw new NotImplementedException();
            }
        }

        class Dsl: IDatabaseStatementLoader
        {
            public string LoadStatement(string key)
            {
                throw new NotImplementedException();
            }

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S927:Parameter names should match base declaration and other partial definitions", Justification = "<保留中>")]
            public string LoadStatementByCurrent(Type caller, [CallerMemberName] string callerMemberName = "")
            {
                throw new NotImplementedException();
            }
        }

        class Di: IDatabaseImplementation
        {
            public string NewLine { get; set; } = Environment.NewLine;

            public bool SupportedTransactionDDL => throw new NotSupportedException();

            public bool SupportedTransactionDML => throw new NotSupportedException();
            public bool SupportedTransactionTruncate => throw new NotSupportedException();

            public bool SupportedLineComment => throw new NotSupportedException();
            public bool SupportedBlockComment => throw new NotSupportedException();

            public IEnumerable<string> LineComments => throw new NotSupportedException();
            public IEnumerable<DatabaseBlockComment> BlockComments => throw new NotSupportedException();
            public DatabaseBlockComment ProcessBodyRange => throw new NotSupportedException();
            public string PreFormatStatement(string statement)
            {
                throw new NotSupportedException();
            }

            public string ToStatementColumnName(string columnName)
            {
                throw new NotSupportedException();
            }

            public string ToStatementParameterName(string parameterName, int index)
            {
                throw new NotSupportedException();
            }

            public string ToStatementTableName(string tableName)
            {
                throw new NotSupportedException();
            }

            public string ToLineComment(string statement) => throw new NotSupportedException();
            public string ToBlockComment(string statement) => throw new NotSupportedException();


            public string Escape(string word)
            {
                throw new NotSupportedException();
            }

            public string EscapeLike(string word) => throw new NotSupportedException();

            public IDatabaseManagement CreateManagement(IDatabaseContext context)
            {
                return new DatabaseManagement();
            }
        }

        class Adao: ApplicationDatabaseObjectBase
        {
            public Adao()
                : base(new Dc(), new Dsl(), new Di(), new LoggerFactory())
            { }

            public int ToInt_Public(long v) => base.ToInt(v);
        }

        #endregion

        #region property

        private Test Test { get; } = Test.Create();

        #endregion

        #region function

        [Theory]
        [InlineData(0, 0)]
        [InlineData(int.MaxValue, int.MaxValue)]
        [InlineData(int.MaxValue, (long)int.MaxValue + 1)]
        [InlineData(int.MinValue, (long)int.MinValue - 1)]
        [InlineData(int.MinValue, int.MinValue)]
        public void ToIntTest(int expected, long value)
        {
            var adao = new Adao();
            var actual = adao.ToInt_Public(value);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
