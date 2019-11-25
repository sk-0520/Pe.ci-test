using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Core.Views.Attached
{
    public class DragAndDropBehavior : Behavior<UIElement>
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
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PreviewMouseDown -= AssociatedObject_MouseDown;
            AssociatedObject.MouseMove -= AssociatedObject_MouseMove;
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            AssociatedObject.DragOver -= AssociatedObject_DragOver;
            AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
            AssociatedObject.Drop -= AssociatedObject_Drop;

            base.OnDetaching();
        }

        #endregion

        void AssociatedObject_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragAndDrop?.MouseDown((UIElement)sender, e);
        }

        void AssociatedObject_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DragAndDrop?.MouseMove((UIElement)sender, e);
        }

        void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            DragAndDrop?.DragEnter((UIElement)sender, e);
        }

        void AssociatedObject_DragOver(object sender, DragEventArgs e)
        {
            DragAndDrop?.DragOver((UIElement)sender, e);
        }

        void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            DragAndDrop?.DragLeave((UIElement)sender, e);
        }

        void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            DragAndDrop?.Drop((UIElement)sender, e);
        }
    }
}
