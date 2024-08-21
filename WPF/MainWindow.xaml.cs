using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PGBD_Project.BU;
using PGBD_Project.DB;

using WPF.Exception;
using WPF.ViewModel;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly OwnerViewModel _ownerViewModel;
        private readonly TenantViewModel _tenantViewModel;
        private readonly OfficeViewModel _officeViewModel;
        private readonly ContractViewModel _contractViewModel;
        private readonly AddressViewModel _addressViewModel;
        
        private int _currentTabIndex;
        private bool _hideDeletedItems = true;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// Sets up ViewModels and binds the DataContext to the OwnerViewModel.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _ownerViewModel = new();
            _tenantViewModel = new();
            _officeViewModel = new();
            _contractViewModel = new();
            _addressViewModel = new();
            
            RefreshFilterDeletedItemButtonContent();
            RefreshModelViews();

            DataContext = _ownerViewModel;
        }

        /// <summary>
        /// Refreshes the views of all ViewModel instances.
        /// </summary>
        private void RefreshModelViews()
        {
            _ownerViewModel.Refresh(_hideDeletedItems);
            _tenantViewModel.Refresh(_hideDeletedItems);
            _officeViewModel.Refresh(_hideDeletedItems);
            _contractViewModel.Refresh();
        }

        /// <summary>
        /// Handles double-click events on data grid cells to open detail views for selected items.
        /// </summary>
        private void DataGridCell_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = (DataGridCell)sender;
            int row = DataGridRow.GetRowContainingElement(cell).GetIndex();

            if (tabControl.SelectedIndex == OwnerTab.TabIndex)
            {
                Owner owner = (Owner)dataGridOwners.Items.GetItemAt(row);
                _ownerViewModel.OpenOwnerDetailWindow(owner);
            }
            else if (tabControl.SelectedIndex == TenantTab.TabIndex)
            {
                Tenant tenant = (Tenant)dataGridTenants.Items.GetItemAt(row);
                _tenantViewModel.OpenTenantDetailWindow(tenant);
            }
            else if (tabControl.SelectedIndex == OfficeTab.TabIndex)
            {
                Office office = (Office)dataGridOffices.Items.GetItemAt(row);
                _officeViewModel.OpenOfficeDetailWindow(office, _ownerViewModel, _contractViewModel, _addressViewModel);
            }
            else if (tabControl.SelectedIndex == ContractTab.TabIndex)
            {
                Contract contract = (Contract)dataGridContracts.Items.GetItemAt(row);
                _contractViewModel.OpenContractDetailWindow(contract, _officeViewModel, _tenantViewModel);
            }

            RefreshModelViews();
        }

        /// <summary>
        /// Handles tab selection changes to update the DataContext based on the selected tab.
        /// </summary>
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedIndex != _currentTabIndex)
            {
                int selectedIndex = tabControl.SelectedIndex;
                List<TabItem> tabs = new() { OwnerTab, TenantTab };

                if (OwnerTab.IsSelected) DataContext = _ownerViewModel;
                else if (TenantTab.IsSelected) DataContext = _tenantViewModel;
                else if (OfficeTab.IsSelected) DataContext = _officeViewModel;
                else if (ContractTab.IsSelected) DataContext = _contractViewModel;
                else throw new TabNotFoundException();

                _currentTabIndex = selectedIndex;
            }
        }

        /// <summary>
        /// Opens the owner creation window when the corresponding button is clicked.
        /// </summary>
        private void CreateOwner_Click(object sender, RoutedEventArgs e)
        {
            _ownerViewModel.OpenOwnerCreationWindow();
        }

        /// <summary>
        /// Opens the tenant creation window when the corresponding button is clicked.
        /// </summary>
        private void CreateTenant_Click(object sender, RoutedEventArgs e)
        {
            _tenantViewModel.OpenTenantCreationWindow();
        }

        /// <summary>
        /// Opens the office creation window when the corresponding button is clicked.
        /// </summary>
        private void CreateOffice_Click(object sender, RoutedEventArgs e)
        {
            _officeViewModel.OpenOfficeCreationWindow(_ownerViewModel);
        }

        /// <summary>
        /// Opens the contract creation window when the corresponding button is clicked.
        /// </summary>
        private void CreateContract_Click(object sender, RoutedEventArgs e)
        {
            _contractViewModel.OpenContractCreationWindow(_officeViewModel, _tenantViewModel);
        }

        /// <summary>
        /// Updates the content of the button that switches the display of deleted items.
        /// </summary>
        private void RefreshFilterDeletedItemButtonContent()
        {
            SwitchDeletedItemDisplay.Content = (_hideDeletedItems ? "Show" : "Hide") + " Deleted Items";
        }

        /// <summary>
        /// Toggles the display of deleted items and refreshes the views when the corresponding button is clicked.
        /// </summary>
        private void SwitchDeletedItemDisplay_Click(object sender, RoutedEventArgs e)
        {
            _hideDeletedItems = !_hideDeletedItems;
            RefreshFilterDeletedItemButtonContent();
            RefreshModelViews();
        }
    }
}
