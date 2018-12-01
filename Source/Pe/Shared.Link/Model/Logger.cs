
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Link.Model
{

    [Flags]
    public enum LogKind
    {
        Trace,
        Debug,
        Information,
        Warning,
        Error,
        Fatal,
    }

    public struct Caller
    {
        #region variable

        public readonly string memberName;
        public readonly string filePath;
        public readonly int lineNumber;

        #endregion

        public Caller([CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            this.memberName = callerMemberName;
            this.filePath = callerFilePath;
            this.lineNumber = callerLineNumber;
        }
    }

    public delegate void PutDelegate(LogKind kind, string message, string detail, Caller caller);

    public interface ILogger
    {
        string Header { get; }

        void Trace(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Trace(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Trace(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        void Debug(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Debug(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Debug(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        void Information(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Information(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Information(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        void Warning(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Warning(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Warning(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        void Error(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Error(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Error(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        void Fatal(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Fatal(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void Fatal(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        void Put(LogKind kind, string message, string detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0);

        ILogger CreateChild(string header);
    }

    public abstract class LoggerBase : ILogger
    {
        public LoggerBase()
        { }

        public LoggerBase(string header)
        {
            Header = header;
        }

        protected LoggerBase(string header, LoggerBase parentLogger)
        {
            Header = header;
            ParentLogger = parentLogger;
        }

        public string Header { get; }
        protected LoggerBase ParentLogger { get; }

        protected abstract void PutCore(LogKind kind, string message, string detail, Caller caller);
        protected abstract ILogger CreateChildCore(string header);

        public void Trace(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Trace, message, null, new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Trace(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Trace, message, detail?.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Trace(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Trace, ex.Message, ex.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));

        public void Debug(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Debug, message, null, new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Debug(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Debug, message, detail?.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Debug(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Debug, ex.Message, ex.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));

        public void Information(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Information, message, null, new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Information(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Information, message, detail?.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Information(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Information, ex.Message, ex.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));

        public void Warning(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Warning, message, null, new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Warning(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Warning, message, detail?.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Warning(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Warning, ex.Message, ex.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));

        public void Error(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Error, message, null, new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Error(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Error, message, detail?.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Error(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Error, ex.Message, ex.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));

        public void Fatal(string message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Fatal, message, null, new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Fatal(string message, object detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Fatal, message, detail?.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public void Fatal(Exception ex, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(LogKind.Fatal, ex.Message, ex.ToString(), new Caller(callerMemberName, callerFilePath, callerLineNumber));

        public void Put(LogKind kind, string message, string detail, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0) => PutCore(kind, message, detail, new Caller(callerMemberName, callerFilePath, callerLineNumber));
        public ILogger CreateChild(string header) => CreateChildCore(header);
    }

    public class NullLogger: LoggerBase
    {
        protected override void PutCore(LogKind kind, string message, string detail, Caller caller)
        { }

        protected override ILogger CreateChildCore(string header) => new NullLogger();
    }

    public class TestLogger : LoggerBase
    {
        public TestLogger()
            : base()
        { }

        public TestLogger(string header)
            : base(header)
        { }

        protected TestLogger(string header, LoggerBase parentLogger)
            : base(header, parentLogger)
        { }

        protected override void PutCore(LogKind kind, string message, string detail, Caller caller)
        {
            var header = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss} {Header}[{kind}] ";
            var headerIndent = new string(' ', header.Length);
            Console.WriteLine($"{header} {message} <{caller.memberName}>");
            if(detail != null) {
                Console.WriteLine($"{headerIndent}{detail}");
            }
            Console.WriteLine($"{headerIndent}{caller.filePath}({caller.lineNumber})");
        }

        protected override ILogger CreateChildCore(string header)
        {
            return new TestLogger(header, this);
        }

    }
}

