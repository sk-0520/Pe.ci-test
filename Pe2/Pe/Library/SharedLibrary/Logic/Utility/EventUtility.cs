namespace ContentTypeTextNet.Library.SharedLibrary.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public static class EventUtility
	{
		public static Delegate Create(Delegate handler, Action<Delegate> releaseEvent, out EventDisposer eventDisposer)
		{
			eventDisposer = new EventDisposer();
			return eventDisposer.Handle(handler, releaseEvent);
		}
		public static Delegate Auto(Delegate handler, Action<Delegate> releaseEvent)
		{
			EventDisposer eventDisposer;
			return Create(handler, releaseEvent, out eventDisposer);
		}

		public static EventHandler Create(EventHandler handler, Action<EventHandler> releaseEvent, out EventHandlerDisposer eventDisposer)
		{
			eventDisposer = new EventHandlerDisposer();
			return eventDisposer.Handle(handler, releaseEvent);
		}
		public static EventHandler Auto(EventHandler handler, Action<EventHandler> releaseEvent)
		{
			EventHandlerDisposer eventDisposer;
			return Create(handler, releaseEvent, out eventDisposer);
		}

		public static EventHandler<TEventArgs> Create<TEventArgs>(EventHandler<TEventArgs> handler, Action<EventHandler<TEventArgs>> releaseEvent, out EventHandlerDisposer<TEventArgs> eventDisposer)
		{
			eventDisposer = new EventHandlerDisposer<TEventArgs>();
			return eventDisposer.Handle(handler, releaseEvent);
		}
		public static EventHandler<TEventArgs> Auto<TEventArgs>(EventHandler<TEventArgs> handler, Action<EventHandler<TEventArgs>> releaseEvent)
		{
			EventHandlerDisposer<TEventArgs> eventDisposer;
			return Create(handler, releaseEvent, out eventDisposer);
		}
	}
}
