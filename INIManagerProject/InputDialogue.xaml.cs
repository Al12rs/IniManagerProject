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
using System.Windows.Shapes;

namespace INIManagerProject
{
    /// <summary>
    /// Logica di interazione per InputDialogue.xaml
    /// </summary>
    public partial class InputDialogue : Window
    {
        public InputDialogue(string question, string defaultAnswer="")
        {
            InitializeComponent();
            QuestionLabel.Text = question;
            TextAnswer.Text = defaultAnswer;
            TextAnswer.Focus();
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            TextAnswer.SelectAll();
            TextAnswer.Focus();
        }

        public string Answer
        {
            get { return TextAnswer.Text; }
        }

    }
}
