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

using PGBD_Project.BU;
using PGBD_Project.DB;

using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for OfficeCreationView.xaml
    /// </summary>
    public partial class OfficeCreationView : Window
    {
        private readonly OfficeViewModel _officeViewModel;
        private readonly OwnerViewModel _ownerViewModel;
        private Owner? _currentOwner;

        public OfficeCreationView(OfficeViewModel officeViewModel, OwnerViewModel ownerViewModel)
        {
            InitializeComponent();
            _officeViewModel = officeViewModel;
            _ownerViewModel = ownerViewModel;

            foreach (Owner o in _ownerViewModel.Owners)
            {
                ComboBoxOwner.Items.Add(o);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOwner == null) 
            {
                MessageBox.Show("Please select an owner.");
                return;
            }

            _officeViewModel.CreateOffice(
                Double.Parse(officeSurface.Text),
                officeDescription.Text,
                Double.Parse(officeRent.Text),
                Int32.Parse(officeType.Text),
                _currentOwner,
                officeAddressNumber.Text,
                officeStreet.Text,
                officePostCode.Text,
                officeCity.Text,
                officeCountry.Text
             );

            Close();
        }

        private void ComboBoxOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Owner selectedOwner = (Owner)combo.SelectedItem;
            _currentOwner = selectedOwner;
        }
    }
}
