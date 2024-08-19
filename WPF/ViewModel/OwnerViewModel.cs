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
    public class OwnerViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Owner> _owners;

        public OwnerViewModel()
        {
            _owners = new ObservableCollection<Owner>(UserService.GetOwners());
        }

        public ObservableCollection<Owner> Owners
        {
            get => _owners ??= new ObservableCollection<Owner>();
            set
            {
                _owners = value;
                OnPropertyRaised(nameof(Owners));
            }
        }

        public void UpdateOwner(Owner owner)
        {
            UserService.UpdateOwner(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            UserService.DeleteOwner(owner);
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
    }
}
