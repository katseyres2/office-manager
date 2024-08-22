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

using WPF.Exception;
using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for ContractDetailView.xaml
    /// </summary>
    public partial class ContractDetailView : Window
    {
        private readonly Contract _currentContract;
        private readonly ContractViewModel _contractViewModel;
        private readonly OfficeViewModel _officeViewModel;
        private readonly TenantViewModel _tenantViewModel;
        private Tenant? _currentTenant;
        private Office? _currentOffice;

        /// <summary>
        /// Initializes a new instance of the ContractDetailView class.
        /// </summary>
        /// <param name="contract">The current contract to be displayed and edited.</param>
        /// <param name="officeViewModel">The OfficeViewModel used to populate the office ComboBox.</param>
        /// <param name="tenantViewModel">The TenantViewModel used to populate the tenant ComboBox.</param>
        /// <param name="contractViewModel">The ContractViewModel used to handle contract updates.</param>
        public ContractDetailView(Contract contract, OfficeViewModel officeViewModel, TenantViewModel tenantViewModel, ContractViewModel contractViewModel)
        {
            InitializeComponent();
            
            _currentContract = contract;

            _officeViewModel = officeViewModel;
            _tenantViewModel = tenantViewModel;
            _contractViewModel = contractViewModel;

            PopulateComboBoxTenant();
            PopulateComboBoxOffice();

            ComboBoxTenant.SelectedItem = _currentTenant;
            ComboBoxOffice.SelectedItem = _currentOffice;
            DataContext = _currentContract;

            RefreshOfficeInformation();
            RefreshTenantInformation();
        }

        private void PopulateComboBoxTenant()
        {
            List<Tenant> tenants = _tenantViewModel.Tenants.OrderBy(t => t.FirstName).ToList();

            // Populate tenant ComboBox and select current tenant
            foreach (Tenant tenant in tenants)
            {
                ComboBoxTenant.Items.Add(tenant);

                if (_currentContract.Tenant != null && _currentContract.Tenant.TenantId == tenant.TenantId)
                {
                    _currentTenant = tenant;
                }
            }
        }

        private void PopulateComboBoxOffice()
        {
            // Populate office ComboBox and select current office
            foreach (Office office in _officeViewModel.Offices)
            {
                ComboBoxOffice.Items.Add(office);

                if (_currentContract.Office != null && _currentContract.Office.OfficeId == office.OfficeId)
                {
                    _currentOffice = office;
                }
            }
        }

        private void RefreshOfficeInformation()
        {
            if (_currentOffice == null) return;

            OfficeOwnerLabel.Content = _currentOffice.Owner.Label;
            OfficeOwnerVAT.Content = _currentOffice.Owner.Tva;
            
            OfficeAddressNumber.Content = _currentOffice.Address.Number;
            OfficeAddressStreet.Content = _currentOffice.Address.Street;
            OfficeAddressPostCode.Content = _currentOffice.Address.PostCode;
            OfficeAddressCity.Content = _currentOffice.Address.City;
            OfficeAddressCountry.Content = _currentOffice.Address.Country;

            OfficeRent.Content = $"{_currentOffice.Rent} €";
            OfficeSurface.Content = $"{_currentOffice.Surface} m²";
            OfficeType.Content = _currentOffice.Type;
        }

        private void RefreshTenantInformation()
        {
            if (_currentTenant == null) return;

            TenantFirstName.Content = _currentTenant.FirstName;
            TenantLastName.Content = _currentTenant.LastName;
            TenantEmail.Content = _currentTenant.Email;
            TenantPhone.Content = _currentTenant.Phone;

            TenantAddressNumber.Content = _currentTenant.Address.Number;
            TenantAddressStreet.Content= _currentTenant.Address.Street;
            TenantAddressPostCode.Content= _currentTenant.Address.PostCode;
            TenantAddressCity.Content= _currentTenant.Address.City;
            TenantAddressCountry.Content= _currentTenant.Address.Country;
        }

        /// <summary>
        /// Handles the selection change event for the tenant ComboBox. Updates the selected tenant.
        /// </summary>
        private void ComboBoxTenant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Tenant selectedTenant = (Tenant)combo.SelectedItem;
            _currentTenant = selectedTenant;

            RefreshTenantInformation();
        }

        /// <summary>
        /// Handles the selection change event for the office ComboBox. Updates the selected office.
        /// </summary>
        private void ComboBoxOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Office selectedOffice = (Office)combo.SelectedItem;
            _currentOffice = selectedOffice;
            
            RefreshOfficeInformation();
        }

        /// <summary>
        /// Handles the click event of the Delete button. Currently not implemented.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Not implemented.
        }

        /// <summary>
        /// Handles the click event of the Cancel button. Closes the window without saving changes.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the click event of the Save button. Validates input and updates the current contract.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTenant == null || _currentOffice == null)
            {
                MessageBox.Show("Tenant and office must be not null.");
                return;
            }

            DateTime startDate = DateTime.Parse(contractStartDate.Text);
            DateTime endDate = DateTime.Parse(contractEndDate.Text);

            if (startDate > endDate) 
            {
                MessageBox.Show("End date must be older than start date.");
                return;
            }

            // Clear current associations to prevent errors
            _currentContract.StartDate = startDate;
            _currentContract.EndDate = endDate;

            // Assign selected office and tenant
            _currentContract.Office = null!;
            _currentContract.Tenant = null!;

            _currentContract.OfficeId = _currentOffice.OfficeId;
            _currentContract.TenantId = _currentTenant.TenantId;
            
            try
            {
                // Update the contract in the view model
                _contractViewModel.UpdateContract(_currentContract);
                Close();
            } catch (ReservationOverrideException ex) { MessageBox.Show(ex.Message);}
        }
    }
}
