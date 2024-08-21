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

using PGBD_Project.DB;

using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for TenantDetailView.xaml
    /// </summary>
    public partial class TenantDetailView : Window
    {
        private Tenant CurrentTenant;
        private TenantViewModel TenantViewModel;

        /// <summary>
        /// Initializes a new instance of the TenantDetailView class.
        /// </summary>
        /// <param name="tenant">The current tenant to be displayed and edited.</param>
        /// <param name="tenantViewModel">The TenantViewModel used to handle tenant updates.</param>
        public TenantDetailView(Tenant tenant, TenantViewModel tenantViewModel)
        {
            InitializeComponent();
            CurrentTenant = tenant;
            TenantViewModel = tenantViewModel;
            DataContext = CurrentTenant;
        }

        /// <summary>
        /// Handles the click event of the Delete button. Marks the tenant as inactive and updates it.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentTenant.Active = false;
            TenantViewModel.UpdateTenant(CurrentTenant);
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
        /// Handles the click event of the Save button. Validates input and updates the current tenant.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentTenant.FirstName = tenantFirstName.Text;
                CurrentTenant.LastName = tenantLastName.Text;
                CurrentTenant.Email = tenantEmail.Text;
                CurrentTenant.Phone = tenantPhone.Text;
                CurrentTenant.Active = tenantActive.IsChecked ?? CurrentTenant.Active;

                CurrentTenant.Address.Number = tenantAddressNumber.Text;
                CurrentTenant.Address.Street = tenantStreet.Text;
                CurrentTenant.Address.PostCode = tenantPostCode.Text;
                CurrentTenant.Address.City = tenantCity.Text;
                CurrentTenant.Address.Country = tenantCountry.Text;

                TenantViewModel.UpdateTenant(CurrentTenant);
                Close();
            } catch (DbUpdateException ex) { MessageBox.Show(ex.Message); }
        }
    }
}
