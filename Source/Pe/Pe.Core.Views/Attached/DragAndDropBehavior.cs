using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Xaml.Behaviors;

namespace ContentTypeTextNet.Pe.Core.Views.Attached
{
    public class DragAndDropBehavior: Behavior<FrameworkElement>
    {
        #region DragAndDropProperty

        public static readonly DependencyProperty DragAndDropProperty = DependencyProperty.Register(
            nameof(DragAndDrop),
            typeof(IDragAndDrop),
            typeof(DragAndDropBehavior),
            new FrameworkPropertyMetadata(default(IDragAndDrop), new PropertyChangedCallback(OnDragAndDropChanged))
        );

        private static void OnDragAndDropChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as DragAndDropBehavior;
            if(control != null) {
                control.DragAndDrop = (IDragAndDrop)e.NewValue;
            }
        }

        public IDragAndDrop? DragAndDrop
        {
            get { return (IDragAndDrop)GetValue(DragAndDropProperty); }
            set { SetValue(DragAndDropProperty, value); }
        }

        #endregion

        #region Behavior

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewMouseDown += AssociatedObject_MouseDown;

            AssociatedObject.MouseMove += AssociatedObject_MouseMove;
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
            AssociatedObject.DragOver += AssociatedObject_DragOver;
            AssociatedObject.DragLeave += AssociatedObject_DragLeave;
            AssociatedObject.Drop += AssociatedObject_Drop;

            AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
            AssociatedObject.PreviewDragEnter += AssociatedObject_PreviewDragEnter;
            AssociatedObject.PreviewDragOver += AssociatedObject_PreviewDragOver;
            AssociatedObject.PreviewDragLeave += AssociatedObject_PreviewDragLeave;
            AssociatedObject.PreviewDrop += AssociatedObject_PreviewDrop;

            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;

            AssociatedObject.PreviewMouseDown -= AssociatedObject_MouseDown;

            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            AssociatedObject.DragOver -= AssociatedObject_DragOver;
            AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
            AssociatedObject.Drop -= AssociatedObject_Drop;

            AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
            AssociatedObject.PreviewDragEnter -= AssociatedObject_PreviewDragEnter;
            AssociatedObject.PreviewDragOver -= AssociatedObject_PreviewDragOver;
            AssociatedObject.PreviewDragLeave -= AssociatedObject_PreviewDragLeave;
            AssociatedObject.PreviewDrop -= AssociatedObject_PreviewDrop;

            base.OnDetaching();
        }

        #endregion

        private void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && !dragAndDrop.UsePreviewEvent) {
                dragAndDrop.MouseDown((UIElement)sender, e);
            }
        }

        private void AssociatedObject_MouseMove(object sender, MouseEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && !dragAndDrop.UsePreviewEvent) {
                dragAndDrop.MouseMove((UIElement)sender, e);
            }
        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && !dragAndDrop.UsePreviewEvent) {
                dragAndDrop.DragEnter((UIElement)sender, e);
            }
        }

        private void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && !dragAndDrop.UsePreviewEvent) {
                dragAndDrop.DragOver((UIElement)sender, e);
            }
        }

        private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && !dragAndDrop.UsePreviewEvent) {
                dragAndDrop.DragLeave((UIElement)sender, e);
            }
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && !dragAndDrop.UsePreviewEvent) {
                dragAndDrop.Drop((UIElement)sender, e);
            }
        }

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && dragAndDrop.UsePreviewEvent) {
                dragAndDrop.MouseMove((UIElement)sender, e);
            }
        }

        private void AssociatedObject_PreviewDragEnter(object sender, DragEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && dragAndDrop.UsePreviewEvent) {
                dragAndDrop.DragEnter((UIElement)sender, e);
            }
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && dragAndDrop.UsePreviewEvent) {
                dragAndDrop.DragOver((UIElement)sender, e);
            }
        }

        private void AssociatedObject_PreviewDragLeave(object sender, DragEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && dragAndDrop.UsePreviewEvent) {
                dragAndDrop.DragLeave((UIElement)sender, e);
            }
        }

        private void AssociatedObject_PreviewDrop(object sender, DragEventArgs e)
        {
            var dragAndDrop = DragAndDrop;
            if(dragAndDrop != null && dragAndDrop.UsePreviewEvent) {
                dragAndDrop.Drop((UIElement)sender, e);
            }
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            OnDetaching();
        }
    }
}
