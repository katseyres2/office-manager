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

using Microsoft.EntityFrameworkCore;

using PGBD_Project.DB;

using WPF.Helper;
using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for OwnerCreationView.xaml
    /// </summary>
    public partial class OwnerCreationView : Window
    {
        private OwnerViewModel OwnerViewModel;

        public OwnerCreationView(OwnerViewModel ownerViewModel)
        {
            InitializeComponent();
            OwnerViewModel = ownerViewModel;
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OwnerViewModel.CreateOwner(ownerLabel.Text, ownerVAT.Text);
                Close();
            } catch (DbUpdateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
