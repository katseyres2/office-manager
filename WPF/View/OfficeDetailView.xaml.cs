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
        private Owner? _currentOwner;
        private readonly OwnerViewModel _ownerViewModel;
        private readonly OfficeViewModel _officeViewModel;
        private readonly ContractViewModel _contractViewModel;

        public OfficeDetailView(Office office, OfficeViewModel officeViewModel, OwnerViewModel ownerViewModel, ContractViewModel contractViewModel)
        {
            InitializeComponent();
            _currentOffice = office;
            _officeViewModel = officeViewModel;
            _ownerViewModel = ownerViewModel;
            _contractViewModel = contractViewModel;

            foreach (Owner o in _ownerViewModel.Owners)
            {
                ComboBoxOwner.Items.Add(o);
                
                if (_currentOffice.Owner != null && _currentOffice.Owner.OwnerId == o.OwnerId)
                {
                    _currentOwner = o;
                }
            }

            _currentOffice.Contracts.Clear();

            foreach (Contract contract in _contractViewModel.Contracts)
            {
                if (contract.OfficeId == _currentOffice.OfficeId)
                {
                    _currentOffice.Contracts.Add(contract);
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
            try
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

                // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                // This bug that throws a DebugException when updating database took me like 10 hours to resolve
                // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

                _currentOffice.Owner = null!; // Bullshit Bug Resolved Yeah!!! The virtual Owner was not null, I only have to set to null and update the Office.OwnerId!
                
                if (_currentOwner != null)
                {
                    _currentOffice.OwnerId = _currentOwner.OwnerId;
                }

                _officeViewModel.UpdateOffice(_currentOffice);

                // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
                // Yes, this one above!!
                // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



                Close();
            }
            catch (FormatException ex) { MessageBox.Show(ex.Message); }
            catch (ArgumentNullException ex) { MessageBox.Show(ex.Message); }
            catch (OverflowException ex) { MessageBox.Show(ex.Message); }
        }

        private void ComboBoxOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Owner selectedOwner = (Owner)combo.SelectedItem;
            _currentOwner = selectedOwner;
        }
    }
}
