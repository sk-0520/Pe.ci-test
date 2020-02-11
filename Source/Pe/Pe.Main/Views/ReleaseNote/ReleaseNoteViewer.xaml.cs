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
using ContentTypeTextNet.Pe.Main.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Views.ReleaseNote
{
    /// <summary>
    /// ReleaseNoteViewer.xaml の相互作用ロジック
    /// </summary>
    public partial class ReleaseNoteViewer : UserControl
    {
        public ReleaseNoteViewer()
        {
            InitializeComponent();
        }

        #region Item

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(
            nameof(Item),
            typeof(ReleaseNoteItemData),
            typeof(ReleaseNoteViewer),
            new FrameworkPropertyMetadata(
                default(ReleaseNoteItemData),
                new PropertyChangedCallback(OnItemChanged)
            )
        );

        public ReleaseNoteItemData Item
        {
            get { return (ReleaseNoteItemData)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        private static void OnItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ReleaseNoteViewer control) {
            }
        }

        #endregion

    }
}
