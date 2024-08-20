using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.DB;

namespace PGBD_Project.BU
{
    public class RentService
    {
        public static List<Contract> GetContracts()
        {
            using FlexiWorkspaceContext db = new();

            List<Contract> contracts = db.Contracts.ToList();
            List<Tenant> tenants = UserService.GetTenants();
            List<Office> offices = WorkspaceService.GetOffices();

            foreach (Contract contract in contracts)
            {
                contract.Tenant = tenants.Find(UserService.FindTenantById(contract.TenantId));
                contract.Office = offices.Find(WorkspaceService.FindOfficeById(contract.OfficeId));
            }

            return contracts;
        }

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

        public static void AddContract(DateTime? startDate, DateTime? endDate, Office office, Tenant tenant)
        {
            using FlexiWorkspaceContext db = new();
            db.Contracts.Add(CreateContract(startDate, endDate, office.OfficeId, tenant.TenantId));
            db.SaveChanges();
        }

        public static void UpdateContract(Contract contract)
        {
            using FlexiWorkspaceContext db = new();
            db.Contracts.Update(contract);
            db.SaveChanges();
        }

        public static void DeleteContract(Contract contract)
        {
            using FlexiWorkspaceContext db = new();
            db.Contracts.Remove(contract);
            db.SaveChanges();
        }

        public static bool StopContract(Contract contract)
        {
            using FlexiWorkspaceContext db = new();
            if (contract.EndDate < DateTime.Now || contract.EndDate.HasValue) { return false; }
            contract.EndDate = DateTime.Now;
            db.Contracts.Update(contract);
            db.SaveChanges();
            return true;
        }

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
