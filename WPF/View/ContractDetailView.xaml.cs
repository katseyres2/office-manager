﻿using System;
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
            currentContract = contract;
            this.officeViewModel = officeViewModel;
            this.tenantViewModel = tenantViewModel;
            this.contractViewModel = contractViewModel;

            // Populate tenant ComboBox and select current tenant
            foreach (Tenant tenant in tenantViewModel.Tenants)
            {
                ComboBoxTenant.Items.Add(tenant);

                if (currentContract.Tenant != null && currentContract.Tenant.TenantId == tenant.TenantId)
                {
                    currentTenant = tenant;
                }
            }

            // Populate office ComboBox and select current office
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

        /// <summary>
        /// Handles the selection change event for the tenant ComboBox. Updates the selected tenant.
        /// </summary>
        private void ComboBoxTenant_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Tenant selectedTenant = (Tenant)combo.SelectedItem;
            currentTenant = selectedTenant;
        }

        /// <summary>
        /// Handles the selection change event for the office ComboBox. Updates the selected office.
        /// </summary>
        private void ComboBoxOffice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Office selectedOffice = (Office)combo.SelectedItem;
            currentOffice = selectedOffice;
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

            // Clear current associations to prevent errors
            currentContract.StartDate = startDate;
            currentContract.EndDate = endDate;

            // Assign selected office and tenant
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

            // Update the contract in the view model
            contractViewModel.UpdateContract(currentContract);
            Close();
        }
    }
}
