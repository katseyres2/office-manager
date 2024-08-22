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
    /// Interaction logic for ContractCreationView.xaml
    /// </summary>
    public partial class ContractCreationView : Window
    {
        private readonly ContractViewModel _contractViewModel;
        private readonly TenantViewModel _tenantViewModel;
        private readonly OfficeViewModel _officeViewModel;
        private Office? _currentOffice;
        private Tenant? _currentTenant;

        /// <summary>
        /// Initializes a new instance of the ContractCreationView class.
        /// </summary>
        /// <param name="officeViewModel">The OfficeViewModel used to populate the office ComboBox.</param>
        /// <param name="tenantViewModel">The TenantViewModel used to populate the tenant ComboBox.</param>
        /// <param name="contractViewModel">The ContractViewModel used to handle contract creation.</param>
        public ContractCreationView(OfficeViewModel officeViewModel, TenantViewModel tenantViewModel, ContractViewModel contractViewModel)
        {
            InitializeComponent();

            _contractViewModel = contractViewModel;
            _tenantViewModel = tenantViewModel;
            _officeViewModel = officeViewModel;

            PopulateTenantComboBox();
            PopulateOfficeComboBox();
        }

        private void PopulateTenantComboBox()
        {
            List<Tenant> tenants = _tenantViewModel.Tenants.OrderBy(t => t.FirstName).ToList();

            // Populate tenant ComboBox with tenants from TenantViewModel
            foreach (Tenant tenant in tenants)
            {
                ComboBoxTenant.Items.Add(tenant);
            }
        }

        private void PopulateOfficeComboBox()
        {
            List<Office> offices = _officeViewModel.Offices.OrderBy(o => o.OfficeId).ToList();

            // Populate office ComboBox with offices from OfficeViewModel
            foreach (Office office in offices)
            {
                ComboBoxOffice.Items.Add(office);
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
            TenantAddressStreet.Content = _currentTenant.Address.Street;
            TenantAddressPostCode.Content = _currentTenant.Address.PostCode;
            TenantAddressCity.Content = _currentTenant.Address.City;
            TenantAddressCountry.Content = _currentTenant.Address.Country;
        }

        /// <summary>
        /// Handles the click event of the Cancel button. Closes the window without saving.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the click event of the Save button. Validates input and creates a new contract.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTenant == null || _currentOffice == null)
            {
                MessageBox.Show("Tenant and office must be not null.");
                return;
            }

            DateTime startDate;
            DateTime endDate;

            try
            {
                 startDate = DateTime.Parse(contractStartDate.Text);
                 endDate = DateTime.Parse(contractEndDate.Text);
            } catch (FormatException)
            {
                MessageBox.Show("Invalid start or end date format.");
                return;
            }

            if (startDate > endDate)
            {
                MessageBox.Show("End date must be older than start date.");
                return;
            }

            try
            {
                _contractViewModel.CreateContract(startDate, endDate, _currentOffice, _currentTenant);
                Close();
            }
            catch (ReservationOverrideException ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        /// Handles the selection change event for the tenant ComboBox. Updates the selected tenant.
        /// </summary>
        private void ComboBoxTenant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Tenant selectedTenant = (Tenant)combo.SelectedItem;
            _currentTenant= selectedTenant;

            RefreshTenantInformation();
        }
    }
}
