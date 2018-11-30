
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

        readonly string memberName;
        readonly string filePath;
        readonly int lineNumber;

        #endregion

        public Caller([CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            this.memberName = callerMemberName;
            this.filePath = callerFilePath;
            this.lineNumber = callerLineNumber;
        }
    }

    public interface ILogger
    {
        void Trace(string message, Caller caller = new Caller());
        void Trace(string message, object detail, Caller caller = new Caller());
        void Trace(Exception ex, Caller caller = new Caller());

        void Debug(string message, Caller caller = new Caller());
        void Debug(string message, object detail, Caller caller = new Caller());
        void Debug(Exception ex, Caller caller = new Caller());

        void Information(string message, Caller caller = new Caller());
        void Information(string message, object detail, Caller caller = new Caller());
        void Information(Exception ex, Caller caller = new Caller());

        void Warning(string message, Caller caller = new Caller());
        void Warning(string message, object detail, Caller caller = new Caller());
        void Warning(Exception ex, Caller caller = new Caller());

        void Error(string message, Caller caller = new Caller());
        void Error(string message, object detail, Caller caller = new Caller());
        void Error(Exception ex, Caller caller = new Caller());

        void Fatal(string message, Caller caller = new Caller());
        void Fatal(string message, object detail, Caller caller = new Caller());
        void Fatal(Exception ex, Caller caller = new Caller());

        void Put(LogKind kind, string message, string detail, Caller caller = new Caller());
    }

    public abstract class LoggerBase : ILogger
    {
        protected abstract void PutCore(LogKind kind, string message, string detail, ref Caller caller);

        public void Trace(string message, Caller caller = new Caller()) => PutCore(LogKind.Trace, message, null, ref caller);
        public void Trace(string message, object detail, Caller caller = new Caller()) => PutCore(LogKind.Trace, message, detail?.ToString(), ref caller);
        public void Trace(Exception ex, Caller caller = new Caller()) => PutCore(LogKind.Trace, ex.Message, ex.ToString(), ref caller);

        public void Debug(string message, Caller caller = new Caller()) => PutCore(LogKind.Debug, message, null, ref caller);
        public void Debug(string message, object detail, Caller caller = new Caller()) => PutCore(LogKind.Debug, message, detail?.ToString(), ref caller);
        public void Debug(Exception ex, Caller caller = new Caller()) => PutCore(LogKind.Debug, ex.Message, ex.ToString(), ref caller);

        public void Information(string message, Caller caller = new Caller()) => PutCore(LogKind.Information, message, null, ref caller);
        public void Information(string message, object detail, Caller caller = new Caller()) => PutCore(LogKind.Information, message, detail?.ToString(), ref caller);
        public void Information(Exception ex, Caller caller = new Caller()) => PutCore(LogKind.Information, ex.Message, ex.ToString(), ref caller);

        public void Warning(string message, Caller caller = new Caller()) => PutCore(LogKind.Warning, message, null, ref caller);
        public void Warning(string message, object detail, Caller caller = new Caller()) => PutCore(LogKind.Warning, message, detail?.ToString(), ref caller);
        public void Warning(Exception ex, Caller caller = new Caller()) => PutCore(LogKind.Warning, ex.Message, ex.ToString(), ref caller);

        public void Error(string message, Caller caller = new Caller()) => PutCore(LogKind.Error, message, null, ref caller);
        public void Error(string message, object detail, Caller caller = new Caller()) => PutCore(LogKind.Error, message, detail?.ToString(), ref caller);
        public void Error(Exception ex, Caller caller = new Caller()) => PutCore(LogKind.Error, ex.Message, ex.ToString(), ref caller);

        public void Fatal(string message, Caller caller = new Caller()) => PutCore(LogKind.Fatal, message, null, ref caller);
        public void Fatal(string message, object detail, Caller caller = new Caller()) => PutCore(LogKind.Fatal, message, detail?.ToString(), ref caller);
        public void Fatal(Exception ex, Caller caller = new Caller()) => PutCore(LogKind.Fatal, ex.Message, ex.ToString(), ref caller);

        public void Put(LogKind kind, string message, string detail, Caller caller) => PutCore(kind, message, detail, ref caller);
    }

    public class NullLogger: LoggerBase
    {
        protected override void PutCore(LogKind kind, string message, string detail, ref Caller caller)
        { }
    }
}

