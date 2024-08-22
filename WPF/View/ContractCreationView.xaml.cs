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
        private readonly ContractViewModel contractViewModel;
        private readonly TenantViewModel tenantViewModel;
        private readonly OfficeViewModel officeViewModel;
        private Office? currentOffice;
        private Tenant? currentTenant;

        /// <summary>
        /// Initializes a new instance of the ContractCreationView class.
        /// </summary>
        /// <param name="officeViewModel">The OfficeViewModel used to populate the office ComboBox.</param>
        /// <param name="tenantViewModel">The TenantViewModel used to populate the tenant ComboBox.</param>
        /// <param name="contractViewModel">The ContractViewModel used to handle contract creation.</param>
        public ContractCreationView(OfficeViewModel officeViewModel, TenantViewModel tenantViewModel, ContractViewModel contractViewModel)
        {
            InitializeComponent();
            this.contractViewModel = contractViewModel;
            this.tenantViewModel = tenantViewModel;
            this.officeViewModel = officeViewModel;

            // Populate tenant ComboBox with tenants from TenantViewModel
            foreach (Tenant tenant in tenantViewModel.Tenants)
            {
                ComboBoxTenant.Items.Add(tenant);
            }

            // Populate office ComboBox with offices from OfficeViewModel
            foreach (Office office in officeViewModel.Offices)
            {
                ComboBoxOffice.Items.Add(office);
            }
        }

        private void RefreshOfficeInformation()
        {
            if (currentOffice == null) return;

            OfficeOwnerLabel.Content = currentOffice.Owner.Label;
            OfficeOwnerVAT.Content = currentOffice.Owner.Tva;

            OfficeAddressNumber.Content = currentOffice.Address.Number;
            OfficeAddressStreet.Content = currentOffice.Address.Street;
            OfficeAddressPostCode.Content = currentOffice.Address.PostCode;
            OfficeAddressCity.Content = currentOffice.Address.City;
            OfficeAddressCountry.Content = currentOffice.Address.Country;

            OfficeRent.Content = $"{currentOffice.Rent} €";
            OfficeSurface.Content = $"{currentOffice.Surface} m²";
            OfficeType.Content = currentOffice.Type;
        }

        private void RefreshTenantInformation()
        {
            if (currentTenant == null) return;

            TenantFirstName.Content = currentTenant.FirstName;
            TenantLastName.Content = currentTenant.LastName;
            TenantEmail.Content = currentTenant.Email;
            TenantPhone.Content = currentTenant.Phone;

            TenantAddressNumber.Content = currentTenant.Address.Number;
            TenantAddressStreet.Content = currentTenant.Address.Street;
            TenantAddressPostCode.Content = currentTenant.Address.PostCode;
            TenantAddressCity.Content = currentTenant.Address.City;
            TenantAddressCountry.Content = currentTenant.Address.Country;
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
            if (currentTenant == null || currentOffice == null)
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
                contractViewModel.CreateContract(startDate, endDate, currentOffice, currentTenant);
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
            currentOffice = selectedOffice;
            
            RefreshOfficeInformation();
        }

        /// <summary>
        /// Handles the selection change event for the tenant ComboBox. Updates the selected tenant.
        /// </summary>
        private void ComboBoxTenant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Tenant selectedTenant = (Tenant)combo.SelectedItem;
            currentTenant= selectedTenant;

            RefreshTenantInformation();
        }
    }
}
