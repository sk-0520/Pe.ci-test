
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public Caller([CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            this.memberName = callerMemberName;
            this.filePath = callerFilePath;
            this.lineNumber = callerLineNumber;
        }
    }

    public class LogItem
    {
        public LogItem(DateTime timestamp, LogKind kind, string message, string detail, Caller caller)
        {
            Timestamp = timestamp;
            Kind = kind;
            Message = message;
            Detail = detail;
            Caller = caller;
        }

        #region property

        public DateTime Timestamp { get; }
        public LogKind Kind { get; }
        public string Message { get; }
        public string Detail { get; }
        public Caller Caller { get; }

        public bool HasDetail => Detail != null;

        //TODO
        public string ShortFilePath => Caller.filePath;

        #endregion
    }

    public interface ILogFactory
    {
        ILogger CreateLogger(string header);
    }

    public interface ILogger: ILogFactory
    {
        string Header { get; }

        void Trace(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        //void Trace(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Trace(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Trace(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");

        void Debug(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        //void Debug(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Debug(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Debug(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");

        void Information(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        //void Information(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Information(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Information(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");

        void Warning(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        //void Warning(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Warning(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Warning(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");

        void Error(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        //void Error(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Error(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Error(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");

        void Fatal(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        //void Fatal(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Fatal(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
        void Fatal(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");

        void Put(LogKind kind, string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "");
    }

    public abstract class LoggerBase : ILogger
    {
        public LoggerBase()
        { }

        public LoggerBase(string header)
        {
            CurrentHeader = header;
        }

        protected LoggerBase(string header, LoggerBase parentLogger)
        {
            CurrentHeader = header;
            ParentLogger = parentLogger;
        }

        public string Header
        {
            get
            {
                string separator = "/";

                if(ParentLogger == null) {
                    return separator;
                }

                if(ParentLogger.Header == separator) {
                    return separator + CurrentHeader;
                }

                return ParentLogger.Header + separator + CurrentHeader;
            }
        }
        protected LoggerBase ParentLogger { get; }
        protected string CurrentHeader { get; }

        protected abstract void PutCore(LogItem logItem);
        protected abstract ILogger CreateLoggerCore(string header);

        public void Trace(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Trace, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        //public void Trace(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Trace, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Trace(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Trace, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Trace(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Trace, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));

        public void Debug(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Debug, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        //public void Debug(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Debug, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Debug(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Debug, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Debug(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Debug, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));

        public void Information(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Information, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        //public void Information(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Information, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Information(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Information, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Information(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Information, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));

        public void Warning(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Warning, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        //public void Warning(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Warning, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Warning(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Warning, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Warning(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Warning, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));

        public void Error(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Error, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        //public void Error(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Error, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Error(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Error, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Error(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Error, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));

        public void Fatal(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Fatal, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        //public void Fatal(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Fatal, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Fatal(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Fatal, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));
        public void Fatal(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, LogKind.Fatal, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName)));

        public void Put(LogKind kind, string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => PutCore(new LogItem(DateTime.Now, kind, message, detail, new Caller(callerLineNumber, callerFilePath, callerMemberName)));

        #region ILogFactory

        public ILogger CreateLogger(string header) => CreateLoggerCore(header);

        #endregion
    }

    public class NullLogger: LoggerBase
    {
        protected override void PutCore(LogItem logItem)
        { }

        protected override ILogger CreateLoggerCore(string header) => new NullLogger();
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

        protected override void PutCore(LogItem logItem)
        {
            var header = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss} {Header}[{logItem.Kind}] ";
            var headerIndent = new string(' ', header.Length);
            Console.WriteLine($"{header} {logItem.Message} <{logItem.Caller.memberName}>");
            if(logItem.HasDetail) {
                Console.WriteLine($"{headerIndent}{logItem.Detail}");
            }
            Console.WriteLine($"{headerIndent}{logItem.Caller.filePath}({logItem.Caller.lineNumber})");
        }

        protected override ILogger CreateLoggerCore(string header)
        {
            return new TestLogger(header, this);
        }

    }

    public static class ILogFactoryExtensions
    {
        #region function

        public static ILogger CreateCurrentClass(this ILogFactory logFactory)
        {
            return logFactory.CreateLogger(new StackFrame(1).GetMethod().DeclaringType.Name);
        }

        #endregion
    }

}

