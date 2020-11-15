using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using ContentTypeTextNet.Pe.Main.Models;
using ICSharpCode.AvalonEdit.Document;

namespace ContentTypeTextNet.Pe.Main.Views.LauncherItemCustomize
{
    /// <summary>
    /// EnvironmentValieableEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class EnvironmentValieableEditor : UserControl
    {
        public EnvironmentValieableEditor()
        {
            InitializeComponent();
        }

        #region MergeTextDocumentProperty

        public static readonly DependencyProperty MergeTextDocumentProperty = DependencyProperty.Register(
            nameof(MergeTextDocument),
            typeof(TextDocument),
            typeof(EnvironmentValieableEditor),
            new PropertyMetadata(
                new TextDocument(),
                OnMergeTextDocumentChanged
            )
        );

        public TextDocument MergeTextDocument
        {
            get { return (TextDocument)GetValue(MergeTextDocumentProperty); }
            set { SetValue(MergeTextDocumentProperty, value); }
        }

        private static void OnMergeTextDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is EnvironmentValieableEditor control) {
                control.MergeTextDocument = (TextDocument)e.NewValue;
            }
        }

        #endregion

        #region RemoveTextDocumentProperty

        public static readonly DependencyProperty RemoveTextDocumentProperty = DependencyProperty.Register(
            nameof(RemoveTextDocument),
            typeof(TextDocument),
            typeof(EnvironmentValieableEditor),
            new PropertyMetadata(
                new TextDocument(),
                OnRemoveTextDocumentChanged
            )
        );

        public TextDocument RemoveTextDocument
        {
            get { return (TextDocument)GetValue(RemoveTextDocumentProperty); }
            set { SetValue(RemoveTextDocumentProperty, value); }
        }

        private static void OnRemoveTextDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is EnvironmentValieableEditor control) {
                control.RemoveTextDocument = (TextDocument)e.NewValue;
            }
        }

        #endregion

        #region MergeErrorItemsSourceProperty

        public static readonly DependencyProperty MergeErrorItemsSourceProperty = DependencyProperty.Register(
            nameof(MergeErrorItemsSource),
            typeof(IEnumerable),
            typeof(EnvironmentValieableEditor),
            new PropertyMetadata(
                new PropertyChangedCallback(OnMergeErrorItemsSourcePropertyChanged)
            )
        );

        public IEnumerable MergeErrorItemsSource
        {
            get { return (IEnumerable)GetValue(MergeErrorItemsSourceProperty); }
            set { SetValue(MergeErrorItemsSourceProperty, value); }
        }

        private static void OnMergeErrorItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is EnvironmentValieableEditor control) {
                control.OnMergeErrorItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
            }
        }

        private void OnMergeErrorItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(MergeErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }

            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(MergeErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }
        }

        void MergeErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        { }

        #endregion

        #region RemoveErrorItemsSourceProperty

        public static readonly DependencyProperty RemoveErrorItemsSourceProperty = DependencyProperty.Register(
            nameof(RemoveErrorItemsSource),
            typeof(IEnumerable),
            typeof(EnvironmentValieableEditor),
            new PropertyMetadata(
                new PropertyChangedCallback(OnRemoveErrorItemsSourcePropertyChanged)
            )
        );

        public IEnumerable RemoveErrorItemsSource
        {
            get { return (IEnumerable)GetValue(RemoveErrorItemsSourceProperty); }
            set { SetValue(RemoveErrorItemsSourceProperty, value); }
        }

        private static void OnRemoveErrorItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is EnvironmentValieableEditor control) {
                control.OnRemoveErrorItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
            }
        }

        private void OnRemoveErrorItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(RemoveErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }

            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(RemoveErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }
        }

        void RemoveErrorItemsSourceValueINotifyCollectionChanged_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        { }

        #endregion

        private void EnvMergeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = ResourceUtility.OpenSyntaxStream(Properties.Resources.File_Highlighting_EnvironmentVariable_Merge)) {
                AvalonEditHelper.SetSyntaxHighlightingDefault((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }

        private void EnvRemoveEditor_Loaded(object sender, RoutedEventArgs e)
        {
            using(var stream = ResourceUtility.OpenSyntaxStream(Properties.Resources.File_Highlighting_EnvironmentVariable_Remove)) {
                AvalonEditHelper.SetSyntaxHighlightingDefault((ICSharpCode.AvalonEdit.TextEditor)sender, stream);
            }
        }

    }
}
