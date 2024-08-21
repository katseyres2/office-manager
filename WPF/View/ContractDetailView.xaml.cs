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
    /// Interaction logic for ContractDetailView.xaml
    /// </summary>
    public partial class ContractDetailView : Window
    {
        private readonly Contract currentContract;
        private readonly ContractViewModel contractViewModel;
        private readonly OfficeViewModel officeViewModel;
        private readonly TenantViewModel tenantViewModel;
        private Tenant? currentTenant;
        private Office? currentOffice;

        public ContractDetailView(Contract contract, OfficeViewModel officeViewModel, TenantViewModel tenantViewModel, ContractViewModel contractViewModel)
        {
            InitializeComponent();
            currentContract = contract;
            this.officeViewModel = officeViewModel;
            this.tenantViewModel = tenantViewModel;
            this.contractViewModel = contractViewModel;

            foreach (Tenant tenant in tenantViewModel.Tenants)
            {
                ComboBoxTenant.Items.Add(tenant);

                if (currentContract.Tenant != null && currentContract.Tenant.TenantId == tenant.TenantId)
                {
                    currentTenant = tenant;
                }
            }

            foreach (Office office in officeViewModel.Offices)
            {
                ComboBoxOffice.Items.Add(office);

                if (currentContract.Office != null && currentContract.Office.OfficeId == office.OfficeId)
                {
                    currentOffice = office;
                }
            }

            ComboBoxTenant.SelectedItem = currentTenant;
            ComboBoxOffice.SelectedItem = currentOffice;
            DataContext = currentContract;
        }

        private void ComboBoxTenant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Tenant selectedTenant = (Tenant)combo.SelectedItem;
            currentTenant = selectedTenant;
        }

        private void ComboBoxOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Office selectedOffice = (Office)combo.SelectedItem;
            currentOffice = selectedOffice;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Not implemented.
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

            DateTime startDate = DateTime.Parse(contractStartDate.Text);
            DateTime endDate = DateTime.Parse(contractEndDate.Text);

            if (startDate > endDate) 
            {
                MessageBox.Show("End date must be older than start date.");
                return;
            }

            currentContract.StartDate = startDate;
            currentContract.EndDate = endDate;

            currentContract.Office = null!;
            currentContract.Tenant = null!;

            if (currentOffice != null)
            {
                currentContract.OfficeId = currentOffice.OfficeId;
            }

            if (currentTenant != null)
            {
                currentContract.TenantId = currentTenant.TenantId;
            }

            contractViewModel.UpdateContract(currentContract);
            Close();
        }
    }
}
