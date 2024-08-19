using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using PGBD_Project.BU;
using PGBD_Project.DB;

using WPF.ViewModel;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly OwnerViewModel OwnerViewModel;

        public MainWindow()
        {
            InitializeComponent();
            OwnerViewModel = new();
            DataContext = OwnerViewModel;
        }

        private void DataGridCell_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = (DataGridCell)sender;
            int row = DataGridRow.GetRowContainingElement(cell).GetIndex();
            //int column = cell.Column.DisplayIndex;
            Owner owner = (Owner)dataGridOwners.Items.GetItemAt(row);
            OwnerViewModel.OpenOwnerDetailWindow(owner);
        }

        private void CreateOwner_Click(object sender, RoutedEventArgs e)
        {
            OwnerViewModel.OpenOwnerCreationWindow();
        }
    }
}
