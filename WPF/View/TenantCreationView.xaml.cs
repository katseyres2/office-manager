using Microsoft.EntityFrameworkCore;

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

using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for TenantCreationView.xaml
    /// </summary>
    public partial class TenantCreationView : Window
    {
        private TenantViewModel _tenantViewModel;

        /// <summary>
        /// Initializes a new instance of the TenantCreationView class.
        /// </summary>
        /// <param name="tenantViewModel">The TenantViewModel used to handle tenant creation.</param>
        public TenantCreationView(TenantViewModel tenantViewModel)
        {
            InitializeComponent();
            _tenantViewModel = tenantViewModel;
        }

        /// <summary>
        /// Handles the click event of the Cancel button. Closes the window without saving changes.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the click event of the Save button. Validates input and creates a new tenant.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _tenantViewModel.CreateTenant(
                    tenantFirstName.Text,
                    tenantLastName.Text,
                    true,
                    tenantPhone.Text,
                    tenantEmail.Text,
                    tenantAddressNumber.Text,
                    tenantStreet.Text,
                    tenantPostCode.Text,
                    tenantCity.Text,
                    tenantCountry.Text
                );

                Close();
            } catch (DbUpdateException ex) { MessageBox.Show( ex.Message ); }
        }
    }
}
