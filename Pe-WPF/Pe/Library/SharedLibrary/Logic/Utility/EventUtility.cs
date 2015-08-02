namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading.Tasks;

	public static class EventUtility
	{
		public static Delegate CreateEvent(Delegate handler, Action<Delegate> releaseEvent, out EventDisposer eventDisposer, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			eventDisposer = new EventDisposer();
			return eventDisposer.Handling(handler, releaseEvent, callerFile, callerLine, callerMember);
		}
		public static Delegate Auto(Delegate handler, Action<Delegate> releaseEvent, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			EventDisposer eventDisposer;
			return CreateEvent(handler, releaseEvent, out eventDisposer, callerFile, callerLine, callerMember);
		}

		public static TEventHandler Create<TEventHandler>(TEventHandler handler, Action<TEventHandler> releaseEvent, out EventDisposer<TEventHandler> eventDisposer, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			eventDisposer = new EventDisposer<TEventHandler>();
			return eventDisposer.Handling(handler, releaseEvent, callerFile, callerLine, callerMember);
		}
		public static TEventHandler Auto<TEventHandler>(TEventHandler handler, Action<TEventHandler> releaseEvent, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
		{
			EventDisposer<TEventHandler> eventDisposer;
			return Create(handler, releaseEvent, out eventDisposer, callerFile, callerLine, callerMember);
		}
	}
}
