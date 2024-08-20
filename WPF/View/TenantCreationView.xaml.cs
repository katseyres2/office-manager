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

        public TenantCreationView(TenantViewModel tenantViewModel)
        {
            InitializeComponent();
            _tenantViewModel = tenantViewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
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
        }
    }
}
