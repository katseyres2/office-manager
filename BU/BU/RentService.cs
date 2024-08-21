using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.DB;

namespace PGBD_Project.BU
{
    /// <summary>
    /// Provides services related to managing contracts in the application.
    /// </summary>
    public class RentService
    {
        /// <summary>
        /// Retrieves a list of contracts from the database, optionally preventing cycling through related entities.
        /// </summary>
        /// <param name="preventCyclingOffice">Indicates whether to prevent cycling through offices.</param>
        /// <param name="preventCyclingTenant">Indicates whether to prevent cycling through tenants.</param>
        /// <returns>A list of contracts with related office and tenant information.</returns>
        public static List<Contract> GetContracts(bool preventCyclingOffice=false, bool preventCyclingTenant=false)
        {
            using FlexiWorkspaceContext db = new();

            List<Contract> contracts = db.Contracts.ToList();
            List<Tenant> tenants = new();
            List<Office> offices= new();

            if (!preventCyclingOffice)
            {
                offices = WorkspaceService.GetOffices();
            }

            if (!preventCyclingTenant)
            {
                tenants = UserService.GetTenants();
            }

            foreach (Contract contract in contracts)
            {
                contract.Tenant = tenants.Find(UserService.FindTenantById(contract.TenantId));
                contract.Office = offices.Find(WorkspaceService.FindOfficeById(contract.OfficeId));
            }

            return contracts;
        }

        /// <summary>
        /// Creates a new contract with the specified details.
        /// </summary>
        /// <param name="startDate">The start date of the contract.</param>
        /// <param name="endDate">The end date of the contract.</param>
        /// <param name="officeId">The ID of the office associated with the contract.</param>
        /// <param name="tenantId">The ID of the tenant associated with the contract.</param>
        /// <returns>A new instance of the Contract class.</returns>
        private static Contract CreateContract(DateTime? startDate, DateTime? endDate, int officeId, int tenantId)
        {
            Contract contract = new()
            {
                StartDate = startDate,
                EndDate = endDate,
                OfficeId = officeId,
                TenantId = tenantId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            return contract;
        }

        /// <summary>
        /// Adds a new contract to the database.
        /// </summary>
        /// <param name="startDate">The start date of the contract.</param>
        /// <param name="endDate">The end date of the contract.</param>
        /// <param name="office">The office associated with the contract.</param>
        /// <param name="tenant">The tenant associated with the contract.</param>
        public static void AddContract(DateTime? startDate, DateTime? endDate, Office office, Tenant tenant)
        {
            using FlexiWorkspaceContext db = new();
            db.Contracts.Add(CreateContract(startDate, endDate, office.OfficeId, tenant.TenantId));
            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing contract in the database.
        /// </summary>
        /// <param name="contract">The contract to be updated.</param>
        public static void UpdateContract(Contract contract)
        {
            using FlexiWorkspaceContext db = new();
            db.Contracts.Update(contract);
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes an existing contract from the database.
        /// </summary>
        /// <param name="contract">The contract to be deleted.</param>
        public static void DeleteContract(Contract contract)
        {
            using FlexiWorkspaceContext db = new();
            db.Contracts.Remove(contract);
            db.SaveChanges();
        }

        /// <summary>
        /// Stops an active contract by setting its end date to the current date.
        /// </summary>
        /// <param name="contract">The contract to be stopped.</param>
        /// <returns>True if the contract was successfully stopped, otherwise false.</returns>
        public static bool StopContract(Contract contract)
        {
            using FlexiWorkspaceContext db = new();
            if (contract.EndDate < DateTime.Now || contract.EndDate.HasValue) { return false; }
            contract.EndDate = DateTime.Now;
            db.Contracts.Update(contract);
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// Starts a contract by setting its start date to the current date.
        /// </summary>
        /// <param name="contract">The contract to be started.</param>
        /// <returns>True if the contract was successfully started, otherwise false.</returns>
        public static bool StartContract(Contract contract)
        {
            using FlexiWorkspaceContext db = new();
            if (contract.StartDate < DateTime.Now || contract.StartDate.HasValue) { return false; }
            contract.StartDate = DateTime.Now;
            db.Contracts.Update(contract);
            db.SaveChanges();
            return true;
        }
    }
}
