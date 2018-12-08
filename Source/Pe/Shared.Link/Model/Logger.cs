
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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

        All = Trace | Debug | Information | Warning | Error | Fatal,
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
        public LogItem(DateTime timestamp, LogKind kind, string header, string message, string detail, Caller caller, int skipFrame)
        {
            Timestamp = timestamp;
            Kind = kind;
            Header = header;
            Message = message;
            Detail = detail;
            Caller = caller;

            StackTrace = new StackTrace(skipFrame + 1, true);
            Thread = Thread.CurrentThread;
            Assembly = Assembly.GetCallingAssembly();
        }

        #region property

        public static int ShortFileIndex { get; set; }

        public DateTime Timestamp { get; }
        public LogKind Kind { get; }
        public string Header { get; }
        public string Message { get; }
        public string Detail { get; }
        public Caller Caller { get; }

        public StackTrace StackTrace { get; }
        public Thread Thread  { get; }
        public Assembly Assembly { get; }

        public bool HasDetail => Detail != null;

        //TODO
        public string ShortFilePath => Caller.filePath.Substring(ShortFileIndex);

        #endregion
    }


    public interface ILogFactory
    {
        ILogger CreateLogger(string header);
    }

    public interface ILoggerBase
    {

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

    public interface ILogger: ILoggerBase, ILogFactory
    {
        string Header { get; }

        bool IsEnabledTrace { get; }
        bool IsEnabledDebug { get; }
        bool IsEnabledInformation { get; }
        bool IsEnabledWarning { get; }
        bool IsEnabledError { get; }
        bool IsEnabledFatal { get; }
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

        #region property

        protected LoggerBase ParentLogger { get; }
        protected string CurrentHeader { get; }

        #endregion

        #region function

        public virtual void SetEnabled(LogKind logKinds)
        {
            if(logKinds.HasFlag(LogKind.Trace)) {
                IsEnabledTrace = true;
            }
            if(logKinds.HasFlag(LogKind.Debug)) {
                IsEnabledDebug = true;
            }
            if(logKinds.HasFlag(LogKind.Information)) {
                IsEnabledInformation = true;
            }
            if(logKinds.HasFlag(LogKind.Warning)) {
                IsEnabledWarning = true;
            }
            if(logKinds.HasFlag(LogKind.Error)) {
                IsEnabledError = true;
            }
            if(logKinds.HasFlag(LogKind.Fatal)) {
                IsEnabledFatal = true;
            }
        }
        public virtual void SetDisabled(LogKind logKinds)
        {
            if(logKinds.HasFlag(LogKind.Trace)) {
                IsEnabledTrace = false;
            }
            if(logKinds.HasFlag(LogKind.Debug)) {
                IsEnabledDebug = false;
            }
            if(logKinds.HasFlag(LogKind.Information)) {
                IsEnabledInformation = false;
            }
            if(logKinds.HasFlag(LogKind.Warning)) {
                IsEnabledWarning = false;
            }
            if(logKinds.HasFlag(LogKind.Error)) {
                IsEnabledError = false;
            }
            if(logKinds.HasFlag(LogKind.Fatal)) {
                IsEnabledFatal = false;
            }
        }

        protected abstract void PutCore(LogItem logItem);

        #endregion

        #region ILoggerBase

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
        public virtual bool IsEnabledTrace { get; protected set; }
        public virtual bool IsEnabledDebug { get; protected set; }
        public virtual bool IsEnabledInformation { get; protected set; }
        public virtual bool IsEnabledWarning { get; protected set; }
        public virtual bool IsEnabledError { get; protected set; }
        public virtual bool IsEnabledFatal { get; protected set; }

        protected void Put(LogItem logItem)
        {
            if(ParentLogger != null) {
                ParentLogger.Put(logItem);
            } else {
                switch(logItem.Kind) {
                    case LogKind.Trace:
                        if(!IsEnabledTrace) {
                            return;
                        }
                        break;
                    case LogKind.Debug:
                        if(!IsEnabledDebug) {
                            return;
                        }
                        break;
                    case LogKind.Information:
                        if(!IsEnabledInformation) {
                            return;
                        }
                        break;
                    case LogKind.Warning:
                        if(!IsEnabledWarning) {
                            return;
                        }
                        break;
                    case LogKind.Error:
                        if(!IsEnabledError) {
                            return;
                        }
                        break;
                    case LogKind.Fatal:
                        if(!IsEnabledFatal) {
                            return;
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
                PutCore(logItem);
            }
        }
        protected abstract ILogger CreateLoggerCore(string header);

        public void Trace(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Trace, Header, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        //public void Trace(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Trace, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Trace(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Trace, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Trace(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Trace, Header, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));

        public void Debug(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Debug, Header, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        //public void Debug(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Debug, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Debug(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Debug, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Debug(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Debug, Header, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));

        public void Information(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Information, Header, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        //public void Information(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Information, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Information(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Information, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Information(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Information, Header, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));

        public void Warning(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Warning, Header, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        //public void Warning(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Warning, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Warning(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Warning, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Warning(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Warning, Header, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));

        public void Error(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Error, Header, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        //public void Error(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Error, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Error(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Error, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Error(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Error, Header, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));

        public void Fatal(string message, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Fatal, Header, message, null, new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        //public void Fatal(string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Fatal, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Fatal(string message, object detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Fatal, Header, message, detail?.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));
        public void Fatal(Exception exception, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, LogKind.Fatal, Header, exception.Message, exception.ToString(), new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));

        public void Put(LogKind kind, string message, string detail, [CallerLineNumber] int callerLineNumber = 0, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "") => Put(new LogItem(DateTime.Now, kind, Header, message, detail, new Caller(callerLineNumber, callerFilePath, callerMemberName), 1));

        #endregion

        #region ILogFactory

        public ILogger CreateLogger(string header) => CreateLoggerCore(header);

        #endregion
    }

    public sealed class NullLogger: LoggerBase
    {
        protected override void PutCore(LogItem logItem)
        { }

        protected override ILogger CreateLoggerCore(string header) => new NullLogger();
    }

    public sealed class ChildLogger: LoggerBase
    {
        public ChildLogger(string header, LoggerBase parentLogger)
            : base(header, parentLogger)
        { }

        protected override void PutCore(LogItem logItem)
        {
            throw new NotSupportedException();
        }

        protected override ILogger CreateLoggerCore(string header) => new ChildLogger(header, this);
    }

    public class TestLogger : LoggerBase
    {
        public TestLogger()
            : base()
        {
            IsEnabledTrace = true;
            IsEnabledDebug = true;
            IsEnabledInformation = true;
            IsEnabledWarning = true;
            IsEnabledError = true;
            IsEnabledFatal = true;
        }

        protected override void PutCore(LogItem logItem)
        {
            var header = $"{DateTime.Now:yyyy-MM-ddTHH:mm:ss} {Header}[{logItem.Kind}] ";
            var headerIndent = new string(' ', header.Length);
            Console.WriteLine($"{header}{logItem.Message} <{logItem.Caller.memberName}>");
            if(logItem.HasDetail) {
                Console.WriteLine($"{headerIndent}{logItem.Detail}");
            }
            Console.WriteLine($"{headerIndent}{logItem.Caller.filePath}({logItem.Caller.lineNumber})");
            Console.WriteLine($"{headerIndent}{nameof(logItem.Assembly)}: {logItem.Assembly}, {nameof(logItem.Thread)}: {logItem.Thread}");
        }

        protected override ILogger CreateLoggerCore(string header) => new ChildLogger(header, this);
    }

    public static class ILogFactoryExtensions
    {
        #region function

        public static ILogger CreateCurrentClass(this ILogFactory logFactory)
        {
            return logFactory.CreateLogger(new StackFrame(1).GetMethod().DeclaringType.Name);
        }

        public static ILogger CreateCurrentMethod(this ILogFactory logFactory)
        {
            var method = new StackFrame(1).GetMethod();
            return logFactory.CreateLogger($"{method.DeclaringType.Name}.{method.Name}");
        }

        #endregion
    }

}

