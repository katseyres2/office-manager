using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.DB;

namespace PGBD_Project.BU
{
    /// <summary>
    /// Provides services related to managing workspaces, including offices and contracts.
    /// </summary>
    public class WorkspaceService
    {
        /// <summary>
        /// Finds an office by its ID.
        /// </summary>
        /// <param name="id">The ID of the office to find.</param>
        /// <returns>A predicate that matches the office with the specified ID.</returns>
        public static Predicate<Office> FindOfficeById(int id)
        {
            return delegate (Office office)
            {
                return office.OfficeId == id;
            };
        }

        /// <summary>
        /// Finds a contract by its ID.
        /// </summary>
        /// <param name="id">The ID of the contract to find.</param>
        /// <returns>A predicate that matches the contract with the specified ID.</returns>
        public static Predicate<Contract> FindContractById(int id)
        {
            return delegate (Contract contract)
            {
                return contract.ContractId == id;
            };
        }

        /// <summary>
        /// Retrieves a list of offices from the database, with their related addresses, owners, and contracts.
        /// </summary>
        /// <returns>A list of offices with their related data.</returns>
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

        /// <summary>
        /// Creates a new office with the specified details.
        /// </summary>
        /// <param name="surface">The surface area of the office.</param>
        /// <param name="description">The description of the office.</param>
        /// <param name="rent">The rent amount for the office.</param>
        /// <param name="type">The type of the office.</param>
        /// <param name="idOwner">The ID of the owner associated with the office.</param>
        /// <param name="addressNumber">The address number of the office.</param>
        /// <param name="street">The street where the office is located.</param>
        /// <param name="postCode">The postal code of the office's location.</param>
        /// <param name="city">The city where the office is located.</param>
        /// <param name="country">The country where the office is located.</param>
        /// <returns>A new instance of the Office class.</returns>
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

        /// <summary>
        /// Adds a new office to the database.
        /// </summary>
        /// <param name="surface">The surface area of the office.</param>
        /// <param name="description">The description of the office.</param>
        /// <param name="rent">The rent amount for the office.</param>
        /// <param name="type">The type of the office.</param>
        /// <param name="ownerId">The ID of the owner associated with the office.</param>
        /// <param name="addressNumber">The address number of the office.</param>
        /// <param name="street">The street where the office is located.</param>
        /// <param name="postCode">The postal code of the office's location.</param>
        /// <param name="city">The city where the office is located.</param>
        /// <param name="country">The country where the office is located.</param>
        public static void AddOffice(double? surface, string? description, double? rent, int? type, int ownerId, string addressNumber, string street, string postCode, string city, string country)
        {
            using FlexiWorkspaceContext db = new();
            db.Offices.Add(CreateOffice(surface, description, rent, type, ownerId, addressNumber, street, postCode, city, country));
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes an existing office from the database.
        /// </summary>
        /// <param name="office">The office to delete.</param>
        public static void DeleteOffice(Office office)
        {
            using FlexiWorkspaceContext db = new();
            db.Offices.Remove(office);
            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing office in the database.
        /// </summary>
        /// <param name="office">The office to update.</param>
        public static void UpdateOffice(Office office)
        {
            using FlexiWorkspaceContext db = new();
            db.Offices.Update(office);
            db.SaveChanges();
        }
    }
}
