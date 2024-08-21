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
    public class OwnerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Owner> _owners;

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

        public ObservableCollection<Owner> Owners
        {
            get => _owners ??= new(UserService.GetOwners());
            set
            {
                _owners = value;
                OnPropertyRaised(nameof(Owners));
            }
        }

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

        public void DeleteOwner(Owner owner)
        {
            UserService.DeleteOwner(owner);
        }

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

        private void OnPropertyRaised(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OpenOwnerDetailWindow(Owner owner)
        {
            Window window = new OwnerDetailView(owner, this);
            if (window.ShowDialog() == true)
            {
                
            }
        }
        public void OpenOwnerCreationWindow()
        {
            Window window = new OwnerCreationView(this);
            if (window.ShowDialog() == true)
            {

            }
        }
    }
}
