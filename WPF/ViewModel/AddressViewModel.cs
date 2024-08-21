using PGBD_Project.BU;
using PGBD_Project.DB;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.ViewModel
{
    public class AddressViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Address> _addresses;

        public AddressViewModel()
        {
            _addresses = new(UserService.GetAddresses());
        }

        /// <summary>
        /// Raises the PropertyChanged event to notify the UI of a property change.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyRaised(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Refresh()
        {
            _addresses.Clear();
            List<Address> DBAddresses = UserService.GetAddresses();

            foreach (Address DBAddress in DBAddresses)
            {
                _addresses.Add(DBAddress);
            }
        }

        public ObservableCollection<Address> Addresses
        {
            get => _addresses;
            set
            {
                _addresses = value;
                OnPropertyRaised(nameof(Addresses));
            }
        }

        public void UpdateAddress(Address address)
        {
            address.UpdatedAt = DateTime.Now;
            UserService.UpdateAddress(address);
        }
    }
}
