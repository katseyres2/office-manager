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
    /// ViewModel class for managing offices.
    /// Implements INotifyPropertyChanged to notify the view of changes in properties.
    /// </summary>
    public class OfficeViewModel: INotifyPropertyChanged
    {
        /// <summary>
        /// Event to notify when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Stores the collection of offices.
        /// </summary>
        private ObservableCollection<Office> _offices;

        /// <summary>
        /// Initializes a new instance of the OfficeViewModel class.
        /// Retrieves the list of offices from WorkspaceService and assigns it to the Offices property.
        /// </summary>
        public OfficeViewModel()
        {
            _offices = new(WorkspaceService.GetOffices());
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
            _offices.Clear();
            List<Office> DBOffices = WorkspaceService.GetOffices();
            
            if (hideDeletedItems)
            {
                DBOffices = DBOffices.Where(dbo => dbo.Active).ToList();
            }

            List<Owner> DBOwners = UserService.GetOwners();
            List<Address> DBAddresses = UserService.GetAddresses();

            foreach (Office DBOffice in DBOffices)
            {
                DBOffice.Owner = DBOwners.FirstOrDefault(o => o.OwnerId == DBOffice.OwnerId)!;
                DBOffice.Address = DBAddresses.FirstOrDefault(a => a.AddressId == DBOffice.AddressId)!;
                _offices.Add(DBOffice);
            }
        }

        /// <summary>
        /// Gets or sets the collection of offices.
        /// Notifies the UI when the collection is updated.
        /// </summary>
        public ObservableCollection<Office> Offices
        {
            get => _offices;
            set
            {
                _offices= value;
                OnPropertyRaised(nameof(Offices));
            }
        }

        /// <summary>
        /// Updates the specified office in the database and refreshes the Offices collection.
        /// </summary>
        /// <param name="office">The office to be updated.</param>
        public void UpdateOffice(Office office)
        {
            office.UpdatedAt = DateTime.Now;
            WorkspaceService.UpdateOffice(office);
            Offices.Clear();

            foreach (Office DBOffice in WorkspaceService.GetOffices())
            {
                Offices.Add(DBOffice);
            }
        }

        /// <summary>
        /// Creates a new office and adds it to the database.
        /// Refreshes the Offices collection after creation.
        /// </summary>
        /// <param name="surface">The surface area of the office.</param>
        /// <param name="description">The description of the office.</param>
        /// <param name="rent">The rent amount for the office.</param>
        /// <param name="type">The type of the office.</param>
        /// <param name="owner">The owner of the office.</param>
        /// <param name="addressNumber">The address number of the office.</param>
        /// <param name="street">The street where the office is located.</param>
        /// <param name="postCode">The postal code of the office's location.</param>
        /// <param name="city">The city where the office is located.</param>
        /// <param name="country">The country where the office is located.</param>
        public void CreateOffice(double? surface, string? description, double? rent, int? type, Owner owner, string addressNumber, string street, string postCode, string city, string country)
        {
            WorkspaceService.AddOffice(surface, description, rent, type, owner.OwnerId, addressNumber, street, postCode, city, country);

            // Refresh the office list
            Offices.Clear();
            List<Office> DBOffices= WorkspaceService.GetOffices();

            foreach (Office o in DBOffices)
            {
                Offices.Add(o);
            }
        }

        /// <summary>
        /// Opens the office detail window with the specified office and ownerViewModel.
        /// </summary>
        /// <param name="office">The office to be viewed.</param>
        /// <param name="ownerViewModel">The owner view model.</param>
        public void OpenOfficeDetailWindow(Office office, OwnerViewModel ownerViewModel, ContractViewModel contractViewModel, AddressViewModel addressViewModel)
        {
            Window window = new OfficeDetailView(office, this, ownerViewModel, contractViewModel, addressViewModel);
            if (window.ShowDialog() == true)
            {
                // Optional actions if dialog result is true
            }
        }

        /// <summary>
        /// Opens the office creation window with the specified ownerViewModel.
        /// </summary>
        /// <param name="ownerViewModel">The owner view model.</param>
        public void OpenOfficeCreationWindow(OwnerViewModel ownerViewModel)
        {
            Window window = new OfficeCreationView(this, ownerViewModel);
            if (window.ShowDialog() == true)
            {
                // Optional actions if dialog result is true
            }
        }

    }
}
