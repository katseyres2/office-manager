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
    public class OfficeViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Office> _offices;

        public OfficeViewModel()
        {
            _offices = new(WorkspaceService.GetOffices());
        }

        private void OnPropertyRaised(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Office> Offices
        {
            get => _offices ??= new(WorkspaceService.GetOffices());
            set
            {
                _offices= value;
                OnPropertyRaised(nameof(Offices));
            }
        }

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

        public void DeleteOffice(Office office)
        {
            WorkspaceService.DeleteOffice(office);
        }

        public void CreateOffice(double? surface, string? description, double? rent, int? type, Owner owner, string addressNumber, string street, string postCode, string city, string country)
        {
            WorkspaceService.AddOffice(surface, description, rent, type, owner.OwnerId, addressNumber, street, postCode, city, country);
            Offices.Clear();
            
            List<Office> DBOffices= WorkspaceService.GetOffices();

            foreach (Office o in DBOffices)
            {
                Offices.Add(o);
            }
        }

        public void OpenOfficeDetailWindow(Office office, OwnerViewModel ownerViewModel)
        {
            Window window = new OfficeDetailView(office, this, ownerViewModel);
            if (window.ShowDialog() == true)
            {

            }
        }

        public void OpenOfficeCreationWindow(OwnerViewModel ownerViewModel)
        {
            Window window = new OfficeCreationView(this, ownerViewModel);
            if (window.ShowDialog() == true)
            {

            }
        }

    }
}
