using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using PGBD_Project.BU;
using PGBD_Project.DB;

using WPF.View;

namespace WPF.ViewModel
{
    public class TenantViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Tenant> _tenants;

        public TenantViewModel()
        { 
            _tenants = new(UserService.GetTenants());
        }

        private void OnPropertyRaised(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Refresh(bool hideDeletedItems)
        {
            _tenants.Clear();
            List<Tenant> DBTenants = UserService.GetTenants();

            if (hideDeletedItems)
            {
                DBTenants = DBTenants.Where(dbt => dbt.Active).ToList();
            }

            foreach (Tenant DBTenant in DBTenants)
            {
                _tenants.Add(DBTenant);
            }
        }

        public ObservableCollection<Tenant> Tenants
        {
            get => _tenants??= new(UserService.GetTenants());
            set
            {
                _tenants = value;
                OnPropertyRaised(nameof(Tenants));
            }
        }

        public void UpdateTenant(Tenant tenant)
        {
            tenant.UpdatedAt = DateTime.Now;
            UserService.UpdateTenant(tenant);
        }

        public void DeleteTenant(Tenant tenant)
        {
            UserService.DeleteTenant(tenant);
        }

        public void CreateTenant(string? firstname, string? lastname, bool active, string? phone, string? email, string addressNumber, string street, string postCode, string city, string country)
        {
            UserService.AddTenant(firstname, lastname, active, phone, email, addressNumber, street, postCode, city, country);
        }

        public void OpenTenantDetailWindow(Tenant tenant)
        {
            Window window = new TenantDetailView(tenant, this);
            if (window.ShowDialog() == true)
            {

            }
        }

        public void OpenTenantCreationWindow()
        {
            Window window = new TenantCreationView(this);
            if (window.ShowDialog() == true)
            {

            }
        }
    }
}
