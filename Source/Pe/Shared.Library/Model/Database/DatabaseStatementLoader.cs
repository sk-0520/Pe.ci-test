using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database
{
    /// <summary>
    /// DBに対する実行文を取得する。
    /// <para>SQLだわな。</para>
    /// </summary>
    public interface IDatabaseStatementLoader
    {
        #region function

        /// <summary>
        /// キーから文を取得。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string LoadStatement(string key);
        /// <summary>
        /// 呼び出しクラス情報から文を取得する。
        /// </summary>
        /// <returns></returns>
        string LoadStatementByCurrent();

        #endregion
    }

    public abstract class DatabaseStatementLoaderBase: IDatabaseStatementLoader
    {
        public DatabaseStatementLoaderBase(ILogger logger)
        {
            Logger = logger;
        }

        public DatabaseStatementLoaderBase(ILogFactory logFactory)
            : this(logFactory.CreateCurrentClass())
        { }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        protected MethodBase GetCurrentMember(int skipFrames = 1)
        {
            var stackFrame = new StackFrame(skipFrames - 1);
            return stackFrame.GetMethod();
        }

        #endregion

        #region IDatabaseStatementLoader

        public abstract string LoadStatement(string key);

        public abstract string LoadStatementByCurrent();

        #endregion
    }

    /// <summary>
    /// たぶん一番使うであろうファイルからの読み込みサポート。
    /// </summary>
    public class DatabaseStatementFileLoader : DatabaseStatementLoaderBase
    {
        public DatabaseStatementFileLoader(ILogger logger)
            : base(logger)
        { }

        [Injection]
        public DatabaseStatementFileLoader(ILogFactory logFactory)
            : this(logFactory.CreateCurrentClass())
        { }

        #region function
        #endregion

        #region DatabaseStatementLoaderBase

        public override string LoadStatement(string key)
        {
            throw new NotImplementedException();
        }

        public override string LoadStatementByCurrent()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
