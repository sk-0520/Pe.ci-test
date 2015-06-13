namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class EventUtility
	{
		public static Delegate CreateEvent(Delegate handler, Action<Delegate> releaseEvent, out EventDisposer eventDisposer)
		{
			eventDisposer = new EventDisposer();
			return eventDisposer.Handle(handler, releaseEvent);
		}
		public static Delegate Auto(Delegate handler, Action<Delegate> releaseEvent)
		{
			EventDisposer eventDisposer;
			return CreateEvent(handler, releaseEvent, out eventDisposer);
		}

		public static TEventHandler Create<TEventHandler>(TEventHandler handler, Action<TEventHandler> releaseEvent, out EventDisposer<TEventHandler> eventDisposer)
		{
			eventDisposer = new EventDisposer<TEventHandler>();
			return eventDisposer.Handle(handler, releaseEvent);
		}
		public static TEventHandler Auto<TEventHandler>(TEventHandler handler, Action<TEventHandler> releaseEvent)
		{
			EventDisposer<TEventHandler> eventDisposer;
			return Create(handler, releaseEvent, out eventDisposer);
		}
	}
}
