using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ContentTypeTextNet.Pe.Core.Views
{
    /// <summary>
    /// LockButton.xaml の相互作用ロジック
    /// </summary>
    public partial class LockButton : UserControl
    {
        public LockButton()
        {
            InitializeComponent();
        }

        #region LockContentProperty

        public static readonly DependencyProperty LockContentProperty = DependencyProperty.Register(
            nameof(LockContent),
            typeof(UIElement),
            typeof(LockButton),
            new FrameworkPropertyMetadata(
                null,
                new PropertyChangedCallback(OnContentChanged)
            )
        );

        public UIElement LockContent
        {
            get { return (UIElement)GetValue(LockContentProperty); }
            set { SetValue(LockContentProperty, value); }
        }

        static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as LockButton;
            if(ctrl != null) {
                ctrl.LockContent = (UIElement)e.NewValue;
            }
        }

        #endregion

        //#region RestoreTimeProperty

        //public static readonly DependencyProperty RestoreTimeProperty = DependencyProperty.Register(
        //    nameof(RestoreTime),
        //    typeof(TimeSpan),
        //    typeof(LockButton),
        //    new FrameworkPropertyMetadata(
        //        TimeSpan.FromSeconds(10),
        //        new PropertyChangedCallback(OnRestoreTimeChanged)
        //    )
        //);

        //public object RestoreTime
        //{
        //    get { return (object)GetValue(RestoreTimeProperty); }
        //    set { SetValue(RestoreTimeProperty, value); }
        //}

        //static void OnRestoreTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var ctrl = d as LockButton;
        //    if(ctrl != null) {
        //        ctrl.RestoreTime = (TimeSpan)e.NewValue;
        //    }
        //}

        //#endregion

    }
}
