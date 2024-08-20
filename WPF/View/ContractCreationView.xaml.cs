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

        public ContractCreationView(OfficeViewModel officeViewModel, TenantViewModel tenantViewModel, ContractViewModel contractViewModel)
        {
            InitializeComponent();
            this.contractViewModel = contractViewModel;
            this.tenantViewModel = tenantViewModel;
            this.officeViewModel = officeViewModel;

            foreach (Tenant tenant in tenantViewModel.Tenants)
            {
                ComboBoxTenant.Items.Add(tenant);
            }

            foreach (Office office in officeViewModel.Offices)
            {
                ComboBoxOffice.Items.Add(office);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

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

        private void ComboBoxOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Office selectedOffice = (Office)combo.SelectedItem;
            currentOffice = selectedOffice;
        }

        private void ComboBoxTenant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Tenant selectedTenant = (Tenant)combo.SelectedItem;
            currentTenant= selectedTenant;
        }
    }
}
