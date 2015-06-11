namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class EventDisposer: DisposeFinalizeBase
	{
		public EventDisposer()
			: base()
		{ }

		#region property

		protected Delegate EventHandler { get; private set; }
		protected Action<Delegate> ReleaseEvent { get; private set; }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				ReleaseEvent(EventHandler);
			}

			base.Dispose(disposing);
		}
		#endregion

		#region function

		public Delegate Handle(Delegate eventHandler, Action<Delegate> releaseEvent)
		{
			if(EventHandler != null) {
				throw new InvalidOperationException("EventHandler");
			}
			if(ReleaseEvent != null) {
				throw new InvalidOperationException("ReleaseEvent");
			}

			if(eventHandler == null) {
				throw new ArgumentNullException("eventHandler");
			}
			if(releaseEvent == null) {
				throw new ArgumentNullException("releaseEvent");
			}

			ReleaseEvent = releaseEvent;
			return EventHandler = eventHandler;
		}

		#endregion
	}

	public class EventHandlerDisposer: DisposeFinalizeBase
	{
		public EventHandlerDisposer()
			: base()
		{ }

		#region property

		protected EventHandler EventHandler {get; private set;}
		protected Action<EventHandler> ReleaseEvent { get; private set; }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				ReleaseEvent(EventHandler);
			}

			base.Dispose(disposing);
		}
		#endregion

		#region function

		public EventHandler Handle(EventHandler eventHandler, Action<EventHandler> releaseEvent)
		{
			if(EventHandler != null) {
				throw new InvalidOperationException("EventHandler");
			}
			if(ReleaseEvent != null) {
				throw new InvalidOperationException("ReleaseEvent");
			}

			if(eventHandler == null) {
				throw new ArgumentNullException("eventHandler");
			}
			if(releaseEvent == null) {
				throw new ArgumentNullException("releaseEvent");
			}

			ReleaseEvent = releaseEvent;
			return EventHandler = eventHandler;
		}

		#endregion
	}

	public class EventHandlerDisposer<TEventArgs>: DisposeFinalizeBase
		where TEventArgs: EventArgs
	{
		public EventHandlerDisposer()
			: base()
		{ }

		#region property

		protected EventHandler<TEventArgs> EventHandler { get; private set; }
		protected Action<EventHandler<TEventArgs>> ReleaseEvent { get; private set; }

		#endregion

		#region DisposeFinalizeBase

		protected override void Dispose(bool disposing)
		{
			if(!IsDisposed) {
				ReleaseEvent(EventHandler);
			}

			base.Dispose(disposing);
		}
		#endregion

		#region function

		public EventHandler<TEventArgs> Handle(EventHandler<TEventArgs> eventHandler, Action<EventHandler<TEventArgs>> releaseEvent)
		{
			if(EventHandler != null) {
				throw new InvalidOperationException("EventHandler");
			}
			if(ReleaseEvent != null) {
				throw new InvalidOperationException("ReleaseEvent");
			}

			if(eventHandler == null) {
				throw new ArgumentNullException("eventHandler");
			}
			if(releaseEvent == null) {
				throw new ArgumentNullException("releaseEvent");
			}

			ReleaseEvent = releaseEvent;
			return EventHandler = eventHandler;
		}

		#endregion
	}

}
