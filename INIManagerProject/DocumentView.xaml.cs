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

namespace INIManagerProject
{
    /// <summary>
    /// Logica di interazione per DocumentView.xaml
    /// </summary>
    public partial class DocumentView : UserControl
    {

        public DocumentView()
        {
            InitializeComponent();
            List<Edit> items = new List<Edit>();
            items.Add(new Edit() { EditName = "BASE FILE",EditPriority="", IsEnabled=false});
            items.Add(new Edit() { EditName = "Edit 1", EditPriority = "1", IsEnabled = true }) ;
            items.Add(new Edit() { EditName = "Edit 2", EditPriority="2", IsEnabled = true});
            items.Add(new Edit() { EditName = "Edit 3", EditPriority="3", IsEnabled = true});
            lvEdit.ItemsSource = items;
          
        }
    }

    public class Edit
    {
        public string EditName { get; set;}
        public String EditPriority { get; set;}
        public bool IsEnabled { get; set; }
    }


}
