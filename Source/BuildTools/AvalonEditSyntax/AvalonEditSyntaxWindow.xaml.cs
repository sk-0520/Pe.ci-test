using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Views;
using ContentTypeTextNet.Pe.Standard.Base;

namespace AvalonEditSyntax
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region property

        IReadOnlyDictionary<string, string> Defines { get; } = new Dictionary<string, string>() {
            #region docs
            ["env-merge"] =
@"KEY=VALUE
KEY=
=VALUE
=
TEXT
KEY==VALUE
KEY= VALUE
KEY = VALUE
KEY =VALUE
",
            ["env-remove"] =
@"KEY=VALUE
KEY=
=VALUE
=
TEXT
KEY==VALUE
KEY= VALUE
KEY = VALUE
KEY =VALUE
",

            #endregion
        };

        #endregion

        #region function

        void LoadSelectedDefine()
        {
            var key = (string)((ListBoxItem)this.listDefines.SelectedItem).Tag;
            LoadDefine(key);
        }

        void LoadDefine(string key)
        {
            var value = Defines[key];
            this.inputPreview.Text = value;
        }

        void ClearError()
        {
            this.showError.Text = string.Empty;
        }

        void AddError(string message)
        {
            this.showError.Text += message + Environment.NewLine;
        }

        void ApplyCurrent()
        {
            Apply(this.inputSyntax.Text);
        }

        void Apply(string rawXml)
        {
            ClearError();

            if(string.IsNullOrWhiteSpace(rawXml)) {
                AddError("XML が空っぽ");
                return;
            }

            var xmlDoc = new XmlDocument();
            try {
                xmlDoc.LoadXml(rawXml);
            } catch(Exception ex) {
                AddError(ex.ToString());
                return;
            }

            var stream = new MemoryStream(rawXml.Length);
            using(var keep = new KeepStream(stream)) {
                xmlDoc.Save(keep);
            }
            try {
                AvalonEditHelper.SetSyntaxHighlightingDefault(this.inputPreview, stream);
            } catch(Exception ex) {
                AddError(ex.ToString());
                return;
            }
        }


        #endregion

        #region Window

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            LoadSelectedDefine();

            this.inputSyntax.Text =
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<SyntaxDefinition xmlns=""http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008"">
</SyntaxDefinition>
";
            ApplyCurrent();
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Apply(this.inputSyntax.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadSelectedDefine();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.inputSyntax.Text);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var text = Clipboard.GetText();
            if(!string.IsNullOrEmpty(text)) {
                this.inputSyntax.Text = text;
                ApplyCurrent();
            }
        }
    }
}
