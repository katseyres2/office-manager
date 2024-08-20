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
    /// Interaction logic for OfficeDetailView.xaml
    /// </summary>
    public partial class OfficeDetailView : Window
    {
        private readonly Office _currentOffice;
        private readonly OwnerViewModel _ownerViewModel;
        private readonly OfficeViewModel _officeViewModel;
        private Owner? _currentOwner;

        public OfficeDetailView(Office office, OfficeViewModel officeViewModel, OwnerViewModel ownerViewModel)
        {
            InitializeComponent();
            _currentOffice = office;
            _officeViewModel = officeViewModel;
            _ownerViewModel = ownerViewModel;

            foreach (Owner o in _ownerViewModel.Owners)
            {
                ComboBoxOwner.Items.Add(o);
                
                if (_currentOffice.Owner != null && _currentOffice.Owner.OwnerId == o.OwnerId)
                {
                    _currentOwner = o;
                }
            }

            ComboBoxOwner.SelectedItem = _currentOwner;
            DataContext = _currentOffice;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _currentOffice.Active = false;
            _officeViewModel.UpdateOffice(_currentOffice);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _currentOffice.Surface = Double.Parse(officeSurface.Text);
            _currentOffice.Rent = Double.Parse(officeRent.Text);
            _currentOffice.Active = officeActive.IsChecked ?? _currentOffice.Active;
            _currentOffice.Type = Int32.Parse(officeType.Text);
            
            _currentOffice.Address.Number = officeAddressNumber.Text;
            _currentOffice.Address.Street = officeStreet.Text;
            _currentOffice.Address.PostCode = officePostCode.Text;
            _currentOffice.Address.City = officeCity.Text;
            _currentOffice.Address.Country = officeCountry.Text;

            _currentOffice.Owner = _currentOwner ?? _currentOffice.Owner;

            _officeViewModel.UpdateOffice(_currentOffice);
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
