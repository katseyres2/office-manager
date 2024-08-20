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
    public class ContractViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Contract> _contracts;

        public ContractViewModel()
        {
            _contracts = new(RentService.GetContracts());
        }

        private void OnPropertyRaised(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Contract> Contracts
        {
            get => _contracts ??= new(RentService.GetContracts());
            set
            {
                _contracts= value;
                OnPropertyRaised(nameof(Contracts));
            }
        }

        public void UpdateContract(Contract contract)
        {
            contract.UpdatedAt = DateTime.Now;
            RentService.UpdateContract(contract);
            Contracts.Clear();

            foreach (Contract DBContract in RentService.GetContracts())
            {
                Contracts.Add(DBContract);
            }
        }

        public void DeleteContract(Contract contract)
        {
            RentService.DeleteContract(contract);
        }

        public void CreateContract(DateTime? startDate, DateTime? endDate, Office office, Tenant tenant)
        {
            RentService.AddContract(startDate, endDate, office, tenant);
            Contracts.Clear();
            List<Contract> DBContracts = RentService.GetContracts();

            foreach (Contract DBContract in DBContracts)
            {
                Contracts.Add(DBContract);
            }
        }

        public void OpenContractDetailWindow(Contract contract, OfficeViewModel officeViewModel, TenantViewModel tenantViewModel)
        {
            Window window = new ContractDetailView(contract, officeViewModel, tenantViewModel, this);
            if (window.ShowDialog() == true)
            {

            }
        }

        public void OpenContractCreationWindow(OfficeViewModel officeViewModel, TenantViewModel tenantViewModel)
        {
            Window window = new ContractCreationView(officeViewModel, tenantViewModel, this);
            if (window.ShowDialog() == true)
            {

            }
        }
    }
}
