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
    /// <summary>
    /// ViewModel class for managing tenants.
    /// Implements INotifyPropertyChanged to notify the view of changes in properties.
    /// </summary>
    public class TenantViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to notify when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Stores the collection of tenants.
        /// </summary>
        private ObservableCollection<Tenant> _tenants;

        /// <summary>
        /// Initializes a new instance of the TenantViewModel class.
        /// Retrieves the list of tenants from UserService and assigns it to the Tenants property.
        /// </summary>
        public TenantViewModel()
        { 
            _tenants = new(UserService.GetTenants());
        }

        /// <summary>
        /// Raises the PropertyChanged event to notify the UI of a property change.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
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

        /// <summary>
        /// Gets or sets the collection of tenants.
        /// Notifies the UI when the collection is updated.
        /// </summary>
        public ObservableCollection<Tenant> Tenants
        {
            get => _tenants??= new(UserService.GetTenants());
            set
            {
                _tenants = value;
                OnPropertyRaised(nameof(Tenants));
            }
        }

        /// <summary>
        /// Updates the specified tenant in the database and refreshes the Tenants collection.
        /// </summary>
        /// <param name="tenant">The tenant to be updated.</param>
        public void UpdateTenant(Tenant tenant)
        {
            tenant.UpdatedAt = DateTime.Now;
            UserService.UpdateTenant(tenant);
        }

        /// <summary>
        /// Creates a new tenant and adds it to the database.
        /// Refreshes the Tenants collection after creation.
        /// </summary>
        /// <param name="firstname">The first name of the tenant.</param>
        /// <param name="lastname">The last name of the tenant.</param>
        /// <param name="active">Indicates whether the tenant is active.</param>
        /// <param name="phone">The phone number of the tenant.</param>
        /// <param name="email">The email address of the tenant.</param>
        /// <param name="addressNumber">The address number of the tenant's residence.</param>
        /// <param name="street">The street where the tenant resides.</param>
        /// <param name="postCode">The postal code of the tenant's residence.</param>
        /// <param name="city">The city where the tenant resides.</param>
        /// <param name="country">The country where the tenant resides.</param>
        public void CreateTenant(string? firstname, string? lastname, bool active, string? phone, string? email, string addressNumber, string street, string postCode, string city, string country)
        {
            UserService.AddTenant(firstname, lastname, active, phone, email, addressNumber, street, postCode, city, country);
        }

        /// <summary>
        /// Opens the tenant detail window with the specified tenant.
        /// </summary>
        /// <param name="tenant">The tenant to be viewed.</param>
        public void OpenTenantDetailWindow(Tenant tenant)
        {
            Window window = new TenantDetailView(tenant, this);
            if (window.ShowDialog() == true)
            {
                // Optional actions if dialog result is true
            }
        }

        /// <summary>
        /// Opens the tenant creation window.
        /// </summary>
        public void OpenTenantCreationWindow()
        {
            Window window = new TenantCreationView(this);
            if (window.ShowDialog() == true)
            {
                // Optional actions if dialog result is true
            }
        }
    }
}
