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

namespace Envelope.Views
{
    /// <summary>
    /// Interaction logic for AddFolderView.xaml
    /// </summary>
    public partial class AddFolderView : Window
    {
        public AddFolderView()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
         
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
