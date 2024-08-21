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
    /// ViewModel class for managing contracts.
    /// Implements INotifyPropertyChanged to notify view of changes in properties.
    /// </summary>
    public class ContractViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Event to notify when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        
        /// <summary>
        /// Stores the collection of contracts.
        /// </summary>
        private ObservableCollection<Contract> _contracts;

        /// <summary>
        /// Initializes a new instance of the ContractViewModel class.
        /// Retrieves the list of contracts from RentService and assigns it to the Contracts property.
        /// </summary>
        public ContractViewModel()
        {
            _contracts = new(RentService.GetContracts());
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
        /// Gets or sets the collection of contracts.
        /// Notifies the UI when the collection is updated.
        /// </summary>
        public ObservableCollection<Contract> Contracts
        {
            get => _contracts ??= new(RentService.GetContracts());
            set
            {
                _contracts= value;
                OnPropertyRaised(nameof(Contracts));
            }
        }

        public void Refresh()
        {
            _contracts.Clear();
            List<Contract> DBContracts = RentService.GetContracts();

            foreach (Contract DBContract in DBContracts)
            {
                DBContract.Tenant = UserService.GetTenants().FirstOrDefault(t => t.TenantId == DBContract.TenantId)!;
                DBContract.Office = WorkspaceService.GetOffices().FirstOrDefault(o => o.OfficeId == DBContract.OfficeId)!;
                _contracts.Add(DBContract);
            }
        }

        /// <summary>
        /// Updates the specified contract in the database and refreshes the Contracts collection.
        /// </summary>
        /// <param name="contract">The contract to be updated.</param>
        public void UpdateContract(Contract contract)
        {
            contract.UpdatedAt = DateTime.Now;
            RentService.UpdateContract(contract);

            // Refresh the contract list
            _contracts.Clear();

            foreach (Contract DBContract in RentService.GetContracts())
            {
                Contracts.Add(DBContract);
            }
        }

        /// <summary>
        /// Deletes the specified contract from the database.
        /// </summary>
        /// <param name="contract">The contract to be deleted.</param>
        public void DeleteContract(Contract contract)
        {
            RentService.DeleteContract(contract);
        }

        /// <summary>
        /// Creates a new contract and adds it to the database.
        /// Refreshes the Contracts collection after creation.
        /// </summary>
        /// <param name="startDate">The start date of the contract.</param>
        /// <param name="endDate">The end date of the contract.</param>
        /// <param name="office">The office associated with the contract.</param>
        /// <param name="tenant">The tenant associated with the contract.</param>
        public void CreateContract(DateTime? startDate, DateTime? endDate, Office office, Tenant tenant)
        {
            RentService.AddContract(startDate, endDate, office, tenant);
            
            // Refresh the contract list
            _contracts.Clear();
            List<Contract> DBContracts = RentService.GetContracts();

            foreach (Contract DBContract in DBContracts)
            {
                Contracts.Add(DBContract);
            }
        }

        /// <summary>
        /// Opens the contract detail window with the specified contract, officeViewModel, and tenantViewModel.
        /// </summary>
        /// <param name="contract">The contract to be viewed.</param>
        /// <param name="officeViewModel">The office view model.</param>
        /// <param name="tenantViewModel">The tenant view model.</param>
        public void OpenContractDetailWindow(Contract contract, OfficeViewModel officeViewModel, TenantViewModel tenantViewModel)
        {
            Window window = new ContractDetailView(contract, officeViewModel, tenantViewModel, this);
            if (window.ShowDialog() == true)
            {
                // Optional actions if dialog result is true
            }
        }

        /// <summary>
        /// Opens the contract creation window with the specified officeViewModel and tenantViewModel.
        /// </summary>
        /// <param name="officeViewModel">The office view model.</param>
        /// <param name="tenantViewModel">The tenant view model.</param>
        public void OpenContractCreationWindow(OfficeViewModel officeViewModel, TenantViewModel tenantViewModel)
        {
            Window window = new ContractCreationView(officeViewModel, tenantViewModel, this);
            if (window.ShowDialog() == true)
            {
                // Optional actions if dialog result is true
            }
        }
    }
}
