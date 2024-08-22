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
        private readonly AddressViewModel _addressViewModel;

        /// <summary>
        /// Initializes a new instance of the OfficeDetailView class.
        /// </summary>
        /// <param name="office">The current office to be displayed and edited.</param>
        /// <param name="officeViewModel">The OfficeViewModel used to handle office updates.</param>
        /// <param name="ownerViewModel">The OwnerViewModel used to populate the owner ComboBox.</param>
        /// <param name="contractViewModel">The ContractViewModel used to manage contracts related to the office.</param>
        public OfficeDetailView(Office office, OfficeViewModel officeViewModel, OwnerViewModel ownerViewModel, ContractViewModel contractViewModel, AddressViewModel addressViewModel)
        {
            InitializeComponent();
            _currentOffice = office;
            _officeViewModel = officeViewModel;
            _ownerViewModel = ownerViewModel;
            _contractViewModel = contractViewModel;
            _addressViewModel = addressViewModel;

            // Populate owner ComboBox with owners from OwnerViewModel and select current owner
            foreach (Owner o in _ownerViewModel.Owners)
            {
                ComboBoxOwner.Items.Add(o);
                
                if (_currentOffice.Owner != null && _currentOffice.Owner.OwnerId == o.OwnerId)
                {
                    _currentOwner = o;
                }
            }

            // Clear and repopulate the contracts associated with the current office
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

        /// <summary>
        /// Handles the click event of the Delete button. Marks the office as inactive and updates it.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            _currentOffice.Active = false;
            _officeViewModel.UpdateOffice(_currentOffice);
            Close();
        }

        /// <summary>
        /// Handles the click event of the Cancel button. Closes the window without saving changes.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the click event of the Save button. Validates input and updates the current office.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _currentOffice.Surface = Double.Parse(officeSurface.Text);
                _currentOffice.Rent = Double.Parse(officeRent.Text);
                _currentOffice.Active = officeActive.IsChecked ?? _currentOffice.Active;
                _currentOffice.Type = Int32.Parse(officeType.Text);
                _currentOffice.Description = officeDescription.Text;

                // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
                // This bug that throws a DebugException when updating database took me like 10 hours to resolve
                // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

                _currentOffice.Owner = null!; // Bullshit Bug Resolved Yeah!!! The virtual Owner was not null, I only have to set to null and update the Office.OwnerId!
                _currentOffice.Address = null!;
                _currentOffice.Contracts.Clear();
                
                if (_currentOwner != null)
                {
                    _currentOffice.OwnerId = _currentOwner.OwnerId;
                }

                Address address = _addressViewModel.Addresses.First(a => a.AddressId == _currentOffice.AddressId);
                
                address.Number = officeAddressNumber.Text;
                address.Street = officeStreet.Text;
                address.PostCode = officePostCode.Text;
                address.City = officeCity.Text;
                address.Country = officeCountry.Text;

                _officeViewModel.UpdateOffice(_currentOffice);
                _addressViewModel.UpdateAddress(address);

                // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
                // Yes, this one above!!
                // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑

                Close();
            }
            catch (FormatException ex) { MessageBox.Show(ex.Message); }
            catch (ArgumentNullException ex) { MessageBox.Show(ex.Message); }
            catch (OverflowException ex) { MessageBox.Show(ex.Message); }
            catch (DbUpdateException ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Handles the selection change event for the owner ComboBox. Updates the selected owner.
        /// </summary>
        private void ComboBoxOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Owner selectedOwner = (Owner)combo.SelectedItem;
            _currentOwner = selectedOwner;
        }
    }
}
