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

using PGBD_Project.DB;

using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for OwnerDetailView.xaml
    /// </summary>
    public partial class OwnerDetailView : Window
    {
        public Owner CurrentOwner;
        private OwnerViewModel OwnerViewModel;

        public OwnerDetailView(Owner owner, OwnerViewModel ownerViewModel)
        {
            InitializeComponent();
            CurrentOwner = owner;
            OwnerViewModel = ownerViewModel;
            DataContext = CurrentOwner;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentOwner.Active = ownerActive.IsChecked ?? CurrentOwner.Active;
            CurrentOwner.Tva = ownerVAT.Text;
            CurrentOwner.Label = ownerLabel.Text;
            CurrentOwner.UpdatedAt = DateTime.Now;

            OwnerViewModel.UpdateOwner(CurrentOwner);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentOwner.Active = false;
            OwnerViewModel.UpdateOwner(CurrentOwner);
            Close();
        }
    }
}
