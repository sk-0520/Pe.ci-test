namespace ContentTypeTextNet.Library.SharedLibrary.View.Attached
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Input;
	using ContentTypeTextNet.Library.SharedLibrary.Data;

	public class DragTarget
	{
		#region UsingDragTarget

		public static readonly DependencyProperty UsingDragTargetProperty = DependencyProperty.RegisterAttached(
			"UsingDragTarget",
			typeof(bool),
			typeof(DragTarget),
			new FrameworkPropertyMetadata(OnUsingDragTargetChanged)
		);

		private static void OnUsingDragTargetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = d as UIElement;
			if (element != null) {
				SetUsingDragTarget(element, (bool)e.NewValue);
			}
		}

		public static bool GetUsingDragTarget(UIElement dependencyObject)
		{
			return (bool)dependencyObject.GetValue(UsingDragTargetProperty);
		}
		public static void SetUsingDragTarget(UIElement dependencyObject, bool value)
		{
			var oldValue = GetUsingDragTarget(dependencyObject);
			if (oldValue && oldValue != value) {
				dependencyObject.DragEnter -= element_DragEnter;
				dependencyObject.DragLeave -= element_DragLeave;
				dependencyObject.DragOver -= element_DragOver;
				dependencyObject.Drop -= element_DragDrop;
				dependencyObject.AllowDrop = false;
			} else {
				dependencyObject.DragEnter += element_DragEnter;
				dependencyObject.DragLeave += element_DragLeave;
				dependencyObject.DragOver += element_DragOver;
				dependencyObject.Drop += element_DragDrop;
				dependencyObject.AllowDrop = true;
			}
			dependencyObject.SetValue(UsingDragTargetProperty, value);
		}

		static UIElement GetEnabledElement(object obj)
		{
			var element = obj as UIElement;
			if (element != null && GetUsingDragTarget(element)) {
				return element;
			} else {
				return null;
			}
		}

		static void CallEvent(object sender, DragEventArgs e, Func<UIElement, ICommand> getCommand)
		{
			var element = GetEnabledElement(sender);
			if (element != null) {
				var eventdata = new EventData<DragEventArgs>(element, e);
				var command = getCommand(element);
				if (command != null) {
					if (command.CanExecute(eventdata)) {
						command.Execute(eventdata);
					}
				}
			}
		}

		static void element_DragEnter(object sender, DragEventArgs e)
		{
			CallEvent(sender, e, GetDragEnterCommand);
		}

		static void element_DragLeave(object sender, DragEventArgs e)
		{
			CallEvent(sender, e, GetDragLeaveCommand);
		}

		static void element_DragOver(object sender, DragEventArgs e)
		{
			CallEvent(sender, e, GetDragOverCommand);
		}

		static void element_DragDrop(object sender, DragEventArgs e)
		{
			CallEvent(sender, e, GetDragDropCommand);
		}

		#endregion

		#region DragEnterCommand

		public static readonly DependencyProperty DragEnterCommandProperty = DependencyProperty.RegisterAttached(
			"DragEnterCommand",
			typeof(ICommand),
			typeof(DragTarget),
			new PropertyMetadata(null, OnDragEnterCommandChanged)
		);

		private static void OnDragEnterCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = d as UIElement;
			if (element != null) {
				SetDragEnterCommand(element, e.NewValue as ICommand);
			}
		}

		public static ICommand GetDragEnterCommand(UIElement dependencyObject)
		{
			return dependencyObject.GetValue(DragEnterCommandProperty) as ICommand;
		}
		public static void SetDragEnterCommand(UIElement dependencyObject, ICommand value)
		{
			dependencyObject.SetValue(DragEnterCommandProperty, value);
		}

		#endregion

		#region DragLeaveCommand

		public static readonly DependencyProperty DragLeaveCommandProperty = DependencyProperty.RegisterAttached(
			"DragLeaveCommand",
			typeof(ICommand),
			typeof(DragTarget),
			new PropertyMetadata(null, OnDragLeaveCommandChanged)
		);

		private static void OnDragLeaveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = d as UIElement;
			if (element != null) {
				SetDragLeaveCommand(element, e.NewValue as ICommand);
			}
		}

		public static ICommand GetDragLeaveCommand(UIElement dependencyObject)
		{
			return dependencyObject.GetValue(DragLeaveCommandProperty) as ICommand;
		}
		public static void SetDragLeaveCommand(UIElement dependencyObject, ICommand value)
		{
			dependencyObject.SetValue(DragLeaveCommandProperty, value);
		}

		#endregion

		#region DragOverCommand

		public static readonly DependencyProperty DragOverCommandProperty = DependencyProperty.RegisterAttached(
			"DragOverCommand",
			typeof(ICommand),
			typeof(DragTarget),
			new PropertyMetadata(null, OnDragOverCommandChanged)
		);

		private static void OnDragOverCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = d as UIElement;
			if (element != null) {
				SetDragOverCommand(element, e.NewValue as ICommand);
			}
		}

		public static ICommand GetDragOverCommand(UIElement dependencyObject)
		{
			return dependencyObject.GetValue(DragOverCommandProperty) as ICommand;
		}
		public static void SetDragOverCommand(UIElement dependencyObject, ICommand value)
		{
			dependencyObject.SetValue(DragOverCommandProperty, value);
		}

		#endregion

		#region DragDropCommand

		public static readonly DependencyProperty DragDropCommandProperty = DependencyProperty.RegisterAttached(
			"DragDropCommand",
			typeof(ICommand),
			typeof(DragTarget),
			new PropertyMetadata(null, OnDragDropCommandChanged)
		);

		private static void OnDragDropCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var element = d as UIElement;
			if (element != null) {
				SetDragDropCommand(element, e.NewValue as ICommand);
			}
		}

		public static ICommand GetDragDropCommand(UIElement dependencyObject)
		{
			return dependencyObject.GetValue(DragDropCommandProperty) as ICommand;
		}
		public static void SetDragDropCommand(UIElement dependencyObject, ICommand value)
		{
			dependencyObject.SetValue(DragDropCommandProperty, value);
		}

		#endregion
	}
}
