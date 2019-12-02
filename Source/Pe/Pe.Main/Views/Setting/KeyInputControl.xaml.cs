using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Views.Setting
{
    /// <summary>
    /// KeyInputControl.xaml の相互作用ロジック
    /// </summary>
    public partial class KeyInputControl : UserControl
    {
        public KeyInputControl()
        {
            InitializeComponent();
        }

        #region KeyProperty

        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            nameof(Key),
            typeof(Key),
            typeof(KeyInputControl),
            new FrameworkPropertyMetadata(
                Key.None,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnKeyChanged)
            )
        );

        public Key Key
        {
            get { return (Key)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as KeyInputControl;
            if(ctrl != null) {
                ctrl.Key = (Key)e.NewValue;
            }
        }

        #endregion

        #region IsEnabledModifierKeyProperty

        public static readonly DependencyProperty IsEnabledModifierKeyProperty = DependencyProperty.Register(
            nameof(IsEnabledModifierKey),
            typeof(bool),
            typeof(KeyInputControl),
            new FrameworkPropertyMetadata(
                true,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnIsEnabledModifierKeyChanged)
            )
        );

        public bool IsEnabledModifierKey
        {
            get { return (bool)GetValue(IsEnabledModifierKeyProperty); }
            set { SetValue(IsEnabledModifierKeyProperty, value); }
        }

        static void OnIsEnabledModifierKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as KeyInputControl;
            if(ctrl != null) {
                ctrl.IsEnabledModifierKey = (bool)e.NewValue;
            }
        }

        #endregion

        #region ShiftProperty

        public static readonly DependencyProperty ShiftProperty = DependencyProperty.Register(
            nameof(Shift),
            typeof(ModifierKey),
            typeof(KeyInputControl),
            new FrameworkPropertyMetadata(
                ModifierKey.None,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnShiftChanged)
            )
        );

        public ModifierKey Shift
        {
            get { return (ModifierKey)GetValue(ShiftProperty); }
            set { SetValue(ShiftProperty, value); }
        }

        static void OnShiftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as KeyInputControl;
            if(ctrl != null) {
                ctrl.Shift = (ModifierKey)e.NewValue;
            }
        }

        #endregion

        #region ControlProperty

        public static readonly DependencyProperty ControlProperty = DependencyProperty.Register(
            nameof(Control),
            typeof(ModifierKey),
            typeof(KeyInputControl),
            new FrameworkPropertyMetadata(
                ModifierKey.None,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnControlChanged)
            )
        );

        public ModifierKey Control
        {
            get { return (ModifierKey)GetValue(ControlProperty); }
            set { SetValue(ControlProperty, value); }
        }

        static void OnControlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as KeyInputControl;
            if(ctrl != null) {
                ctrl.Control = (ModifierKey)e.NewValue;
            }
        }

        #endregion

        #region AltProperty

        public static readonly DependencyProperty AltProperty = DependencyProperty.Register(
            nameof(Alt),
            typeof(ModifierKey),
            typeof(KeyInputControl),
            new FrameworkPropertyMetadata(
                ModifierKey.None,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnAltChanged)
            )
        );

        public ModifierKey Alt
        {
            get { return (ModifierKey)GetValue(AltProperty); }
            set { SetValue(AltProperty, value); }
        }

        static void OnAltChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as KeyInputControl;
            if(ctrl != null) {
                ctrl.Alt = (ModifierKey)e.NewValue;
            }
        }

        #endregion

        #region SuperProperty

        public static readonly DependencyProperty SuperProperty = DependencyProperty.Register(
            nameof(Super),
            typeof(ModifierKey),
            typeof(KeyInputControl),
            new FrameworkPropertyMetadata(
                ModifierKey.None,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSuperChanged)
            )
        );

        public ModifierKey Super
        {
            get { return (ModifierKey)GetValue(SuperProperty); }
            set { SetValue(SuperProperty, value); }
        }

        static void OnSuperChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as KeyInputControl;
            if(ctrl != null) {
                ctrl.Super = (ModifierKey)e.NewValue;
            }
        }

        #endregion

        #region ModifierKeyItemsSource

        public static readonly DependencyProperty ModifierKeyItemsSourceProperty = DependencyProperty.Register(
            nameof(ModifierKeyItemsSource),
            typeof(IEnumerable),
            typeof(KeyInputControl),
            new PropertyMetadata(
                new[] {
                    ModifierKey.None,
                    ModifierKey.Any,
                    ModifierKey.Left,
                    ModifierKey.Right,
                    ModifierKey.All,
                },
                new PropertyChangedCallback(OnModifierKeyItemsSourcePropertyChanged)
            )
        );

        public IEnumerable ModifierKeyItemsSource
        {
            get { return (IEnumerable)GetValue(ModifierKeyItemsSourceProperty); }
            set { SetValue(ModifierKeyItemsSourceProperty, value); }
        }

        private static void OnModifierKeyItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is KeyInputControl control) {
                control.OnModifierKeyItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
            }
        }

        private void OnModifierKeyItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(ModifierKeyItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }

            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(ModifierKeyItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }
        }

        void ModifierKeyItemsSourceValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        { }

        #endregion

        #region KeyItemsSource

        public static readonly DependencyProperty KeyItemsSourceProperty = DependencyProperty.Register(
            nameof(KeyItemsSource),
            typeof(IEnumerable),
            typeof(KeyInputControl),
            new PropertyMetadata(
                EnumUtility.GetMembers<Key>().OrderBy(i => i).ToList(),
                new PropertyChangedCallback(OnKeyItemsSourcePropertyChanged)
            )
        );

        public IEnumerable KeyItemsSource
        {
            get { return (IEnumerable)GetValue(KeyItemsSourceProperty); }
            set { SetValue(KeyItemsSourceProperty, value); }
        }

        private static void OnKeyItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if(sender is KeyInputControl control) {
                control.OnKeyItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
            }
        }

        private void OnKeyItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            var oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

            if(null != oldValueINotifyCollectionChanged) {
                oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(KeyItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }

            var newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
            if(null != newValueINotifyCollectionChanged) {
                newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(KeyItemsSourceValueINotifyCollectionChanged_CollectionChanged);
            }
        }

        void KeyItemsSourceValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        { }

        #endregion

    }
}
