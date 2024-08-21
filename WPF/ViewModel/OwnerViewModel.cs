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

using WPF.Exception;
using WPF.View;

namespace WPF.ViewModel
{
    /// <summary>
    /// ViewModel class for managing owners.
    /// Implements INotifyPropertyChanged to notify the view of changes in properties.
    /// </summary>
    public class OwnerViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to notify when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Stores the collection of owners.
        /// </summary>
        private ObservableCollection<Owner> _owners;

        /// <summary>
        /// Initializes a new instance of the OwnerViewModel class.
        /// Retrieves the list of owners from UserService and assigns it to the Owners property.
        /// </summary>
        public OwnerViewModel()
        {
            _owners = new(UserService.GetOwners());
        }

        public void Refresh(bool hideDeletedItems)
        {
            _owners.Clear();
            List<Owner> DBOwners = UserService.GetOwners();

            if (hideDeletedItems)
            {
                DBOwners = DBOwners.Where(dbo => dbo.Active).ToList();
            }

            foreach (Owner owner in DBOwners)
            {
                _owners.Add(owner);
            }
        }

        /// <summary>
        /// Gets or sets the collection of owners.
        /// Notifies the UI when the collection is updated.
        /// </summary>
        public ObservableCollection<Owner> Owners
        {
            get => _owners ??= new(UserService.GetOwners());
            set
            {
                _owners = value;
                OnPropertyRaised(nameof(Owners));
            }
        }

        /// <summary>
        /// Updates the specified owner in the collection and the database.
        /// </summary>
        /// <param name="owner">The owner to be updated.</param>
        public void UpdateOwner(Owner owner)
        {
            Owners.Remove(owner);

            for (int i = 0; i < Owners.Count; i++)
            {
                Owner o = Owners[i];

                if (o.OwnerId < owner.OwnerId) continue;
                Owners.Insert(i, owner);
                break;
            }

            UserService.UpdateOwner(owner);
        }

        /// <summary>
        /// Creates a new owner and adds it to the database.
        /// Refreshes the Owners collection after creation.
        /// </summary>
        /// <param name="label">The label for the owner.</param>
        /// <param name="tva">The TVA (tax identifier) for the owner.</param>
        public void CreateOwner(string? label, string? tva)
        {
            UserService.AddOwner(label, tva, true);
            Owners.Clear();
            List<Owner> DBOwners = UserService.GetOwners();

            foreach (Owner o in DBOwners)
            {
                Owners.Add(o);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event to notify the UI of a property change.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyRaised(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Opens the owner detail window with the specified owner.
        /// </summary>
        /// <param name="owner">The owner to be viewed.</param>
        public void OpenOwnerDetailWindow(Owner owner)
        {
            Window window = new OwnerDetailView(owner, this);
            if (window.ShowDialog() == true)
            {
                // Optional actions if dialog result is true
            }
        }

        /// <summary>
        /// Opens the owner creation window.
        /// </summary>
        public void OpenOwnerCreationWindow()
        {
            Window window = new OwnerCreationView(this);
            if (window.ShowDialog() == true)
            {
                // Optional actions if dialog result is true
            }
        }
    }
}
