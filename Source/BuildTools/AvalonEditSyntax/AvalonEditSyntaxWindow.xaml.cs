using System;
using System.Collections.Generic;
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

        #region Window

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.editSyntax.Text = @"
<?xml version=""1.0"" encoding =""utf-8"" ?>
<SyntaxDefinition xmlns = ""http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008"">
</SyntaxDefinition>
";
        }

        #endregion

        #region function

        void ClearError()
        {
            this.showError.Text = string.Empty;
        }

        void AddError(string message)
        {
            this.showError.Text += message + Environment.NewLine;
        }

        void Apply(string rawXml)
        {
            ClearError();

            if(string.IsNullOrWhiteSpace(rawXml)) {
                AddError("XML が空っぽ");
                return;
            }

            //new System.Xml.XmlDocument()
        }


        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Apply(this.editSyntax.Text);
        }
    }
}
