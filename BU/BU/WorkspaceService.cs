using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.DB;

namespace PGBD_Project.BU
{
    public class WorkspaceService
    {
        public static Predicate<Office> FindOfficeById(int id)
        {
            return delegate (Office office)
            {
                return office.OfficeId == id;
            };
        }

        public static Predicate<Contract> FindContractById(int id)
        {
            return delegate (Contract contract)
            {
                return contract.ContractId == id;
            };
        }

        public static List<Office> GetOffices()
        {
            using FlexiWorkspaceContext db = new();
            
            List<Office> offices = db.Offices.ToList();
            List<Address> addresses = UserService.GetAddresses();
            List<Owner> owners = UserService.GetOwners();
            List<Contract> contracts = RentService.GetContracts(true);

            foreach (Office o in offices)
            {
                o.Address = addresses.Find(UserService.FindAddressById(o.AddressId));
                o.Owner = owners.Find(UserService.FindOwnerById(o.OwnerId));

                foreach (Contract contract in contracts)
                {
                    if (contract.OfficeId == o.OfficeId)
                    {
                        o.Contracts.Add(contract);
                    }
                }
            }

            return offices;
        }

        private static Office CreateOffice(double? surface, string? description, double? rent, int? type, int idOwner, string addressNumber, string street, string postCode, string city, string country)
        {
            Office office = new()
            {
                Surface = surface,
                Description = description,
                Rent = rent,
                Active = true,
                Type = type,
                OwnerId = idOwner,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Address = UserService.CreateAddress(addressNumber, street, postCode, city, country)
            };

            return office;
        }

        public static void AddOffice(double? surface, string? description, double? rent, int? type, int ownerId, string addressNumber, string street, string postCode, string city, string country)
        {
            using FlexiWorkspaceContext db = new();
            db.Offices.Add(CreateOffice(surface, description, rent, type, ownerId, addressNumber, street, postCode, city, country));
            db.SaveChanges();
        }

        public static void DeleteOffice(Office office)
        {
            using FlexiWorkspaceContext db = new();
            db.Offices.Remove(office);
            db.SaveChanges();
        }

        public static void UpdateOffice(Office office)
        {
            using FlexiWorkspaceContext db = new();
            db.Offices.Update(office);
            db.SaveChanges();
        }
    }
}
