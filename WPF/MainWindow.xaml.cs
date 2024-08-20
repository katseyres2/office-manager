﻿using System;
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
        public readonly OwnerViewModel OwnerViewModel;
        public readonly TenantViewModel TenantViewModel;
        public readonly OfficeViewModel OfficeViewModel;
        private readonly ContractViewModel contractViewModel;
        private int currentTabIndex;

        public MainWindow()
        {
            InitializeComponent();

            OwnerViewModel = new();
            TenantViewModel = new();
            OfficeViewModel = new();
            contractViewModel = new();

            DataContext = OwnerViewModel;
        }

        private void DataGridCell_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = (DataGridCell)sender;
            int row = DataGridRow.GetRowContainingElement(cell).GetIndex();

            if (tabControl.SelectedIndex == OwnerTab.TabIndex)
            {
                Owner owner = (Owner)dataGridOwners.Items.GetItemAt(row);
                OwnerViewModel.OpenOwnerDetailWindow(owner);
            }
            else if (tabControl.SelectedIndex == TenantTab.TabIndex)
            {
                Tenant tenant = (Tenant)dataGridTenants.Items.GetItemAt(row);
                TenantViewModel.OpenTenantDetailWindow(tenant);
            }
            else if (tabControl.SelectedIndex == OfficeTab.TabIndex)
            {
                Office office = (Office)dataGridOffices.Items.GetItemAt(row);
                OfficeViewModel.OpenOfficeDetailWindow(office, OwnerViewModel);
            }
            else if (tabControl.SelectedIndex == ContractTab.TabIndex)
            {
                Contract contract = (Contract)dataGridContracts.Items.GetItemAt(row);
                contractViewModel.OpenContractDetailWindow(contract, OfficeViewModel, TenantViewModel);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl && tabControl.SelectedIndex != currentTabIndex)
            {
                int selectedIndex = tabControl.SelectedIndex;
                List<TabItem> tabs = new() { OwnerTab, TenantTab };

                if (OwnerTab.IsSelected) DataContext = OwnerViewModel;
                else if (TenantTab.IsSelected) DataContext = TenantViewModel;
                else if (OfficeTab.IsSelected) DataContext = OfficeViewModel;
                else if (ContractTab.IsSelected) DataContext = contractViewModel;
                else throw new TabNotFoundException();

                currentTabIndex = selectedIndex;
            }
        }

        private void CreateOwner_Click(object sender, RoutedEventArgs e)
        {
            OwnerViewModel.OpenOwnerCreationWindow();
        }

        private void CreateTenant_Click(object sender, RoutedEventArgs e)
        {
            TenantViewModel.OpenTenantCreationWindow();
        }

       

        private void CreateOffice_Click(object sender, RoutedEventArgs e)
        {
            OfficeViewModel.OpenOfficeCreationWindow(OwnerViewModel);
        }

        private void SwitchDeletedItemDisplay_Click(object sender, RoutedEventArgs e)
        {
            //_hideDeletedItems = !_hideDeletedItems;
        }

        private void CreateContract_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
