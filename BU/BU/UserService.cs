using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.DB;

namespace PGBD_Project.BU
{
    /// <summary>
    /// Provides services related to managing users, including owners and tenants, in the application.
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// Retrieves a list of owners from the database, with their related offices.
        /// </summary>
        /// <returns>A list of owners with their related offices.</returns>
        public static List<Owner> GetOwners()
        {
            using FlexiWorkspaceContext db = new();
            List<Office> offices = db.Offices.ToList();
            List<Owner> owners = db.Owners.ToList();

            foreach (Office office in offices)
            {
                office.Owner = owners.Find(FindOwnerById(office.OwnerId));
            }

            return owners;
        }

        /// <summary>
        /// Finds an owner by their ID.
        /// </summary>
        /// <param name="id">The ID of the owner to find.</param>
        /// <returns>A predicate that matches the owner with the specified ID.</returns>
        public static Predicate<Owner> FindOwnerById(int id)
        {
            return delegate (Owner owner)
            {
                return owner.OwnerId == id;
            };
        }

        /// <summary>
        /// Finds a tenant by their ID.
        /// </summary>
        /// <param name="id">The ID of the tenant to find.</param>
        /// <returns>A predicate that matches the tenant with the specified ID.</returns>
        public static Predicate<Tenant> FindTenantById(int id)
        {
            return delegate (Tenant tenant)
            {
                return tenant.TenantId == id;
            };
        }

        /// <summary>
        /// Finds an address by its ID.
        /// </summary>
        /// <param name="id">The ID of the address to find.</param>
        /// <returns>A predicate that matches the address with the specified ID.</returns>
        public static Predicate<Address> FindAddressById(int id)
        {
            return delegate (Address address)
            {
                return address.AddressId == id;
            };
        }

        /// <summary>
        /// Retrieves a list of tenants from the database, with their related addresses.
        /// </summary>
        /// <returns>A list of tenants with their related addresses.</returns>
        public static List<Tenant> GetTenants()
        {
            using FlexiWorkspaceContext db = new();

            List<Address> addresses = GetAddresses();
            List<Tenant> tenants = db.Tenants.ToList();

            foreach (Tenant t in tenants)
            {
                t.Address = addresses.Find(FindAddressById(t.AddressId));
            }

            return tenants;
        }

        /// <summary>
        /// Retrieves a list of addresses from the database.
        /// </summary>
        /// <returns>A list of addresses.</returns>
        public static List<Address> GetAddresses()
        {
            using FlexiWorkspaceContext db = new();
            return db.Addresses.ToList();
        }

        /// <summary>
        /// Retrieves a list of all users (owners and tenants) from the database.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public static List<object> GetUsers()
        {
            using FlexiWorkspaceContext db = new();
            List<object> users = new();
            users.AddRange(GetOwners());
            users.AddRange(GetTenants());
            return users;
        }

        /// <summary>
        /// Creates a new owner with the specified details.
        /// </summary>
        /// <param name="label">The label of the owner.</param>
        /// <param name="tva">The TVA (tax identifier) of the owner.</param>
        /// <param name="active">Indicates whether the owner is active.</param>
        /// <returns>A new instance of the Owner class.</returns>
        private static Owner CreateOwner(string? label, string? tva, bool active)
        {
            Owner owner = new()
            {
                Label = label,
                Tva = tva,
                Active = active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return owner;
        }

        /// <summary>
        /// Creates a new tenant with the specified details.
        /// </summary>
        /// <param name="firstname">The first name of the tenant.</param>
        /// <param name="lastname">The last name of the tenant.</param>
        /// <param name="active">Indicates whether the tenant is active.</param>
        /// <param name="phone">The phone number of the tenant.</param>
        /// <param name="email">The email address of the tenant.</param>
        /// <param name="addressNumber">The address number of the tenant's residence.</param>
        /// <param name="street">The street where the tenant resides.</param>
        /// <param name="postCode">The postal code of the tenant's residence.</param>
        /// <param name="city">The city where the tenant resides.</param>
        /// <param name="country">The country where the tenant resides.</param>
        /// <returns>A new instance of the Tenant class.</returns>
        private static Tenant CreateTenant(string? firstname, string? lastname, bool active, string? phone, string? email, string addressNumber, string street, string postCode, string city, string country)
        {
            Tenant tenant = new()
            {
                FirstName = firstname,
                LastName = lastname,
                Phone = phone,
                Email = email,
                Active = active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Address = CreateAddress(addressNumber, street, postCode, city, country)
            };

            return tenant;
        }

        /// <summary>
        /// Creates a new address with the specified details.
        /// </summary>
        /// <param name="number">The address number.</param>
        /// <param name="street">The street name.</param>
        /// <param name="postCode">The postal code.</param>
        /// <param name="city">The city name.</param>
        /// <param name="country">The country name.</param>
        /// <returns>A new instance of the Address class.</returns>
        public static Address CreateAddress(string number, string street, string postCode, string city, string country)
        {
            Address address = new()
            {
                Number = number,
                Street = street,
                PostCode = postCode,
                City = city,
                Country = country,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            return address;
        }

        /// <summary>
        /// Adds a new owner to the database.
        /// </summary>
        /// <param name="label">The label of the owner.</param>
        /// <param name="tva">The TVA (tax identifier) of the owner.</param>
        /// <param name="active">Indicates whether the owner is active.</param>
        public static void AddOwner(string? label, string? tva, bool active)
        {
            using FlexiWorkspaceContext db = new();
            db.Owners.Add(CreateOwner(label, tva, active));
            db.SaveChanges();
        }

        /// <summary>
        /// Adds a new tenant to the database.
        /// </summary>
        /// <param name="firstname">The first name of the tenant.</param>
        /// <param name="lastname">The last name of the tenant.</param>
        /// <param name="active">Indicates whether the tenant is active.</param>
        /// <param name="phone">The phone number of the tenant.</param>
        /// <param name="email">The email address of the tenant.</param>
        /// <param name="addressNumber">The address number of the tenant's residence.</param>
        /// <param name="street">The street where the tenant resides.</param>
        /// <param name="postCode">The postal code of the tenant's residence.</param>
        /// <param name="city">The city where the tenant resides.</param>
        /// <param name="country">The country where the tenant resides.</param>
        public static void AddTenant(string? firstname, string? lastname, bool active, string? phone, string? email, string addressNumber, string street, string postCode, string city, string country)
        {
            using FlexiWorkspaceContext db = new();
            db.Tenants.Add(CreateTenant(firstname, lastname, active, phone, email, addressNumber, street, postCode, city, country));
            db.SaveChanges();
        }

        /// <summary>
        /// Adds a new address to the database.
        /// </summary>
        /// <param name="address">The address to add.</param>
        public static void AddAddress(Address address)
        {
            using FlexiWorkspaceContext db = new();
            db.Addresses.Add(address);
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes an existing owner from the database.
        /// </summary>
        /// <param name="owner">The owner to delete.</param>
        public static void DeleteOwner(Owner owner)
        {
            using FlexiWorkspaceContext db = new();
            db.Owners.Remove(owner);
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes an existing tenant from the database.
        /// </summary>
        /// <param name="tenant">The tenant to delete.</param>
        public static void DeleteTenant(Tenant tenant)
        {
            using FlexiWorkspaceContext db = new();
            db.Tenants.Remove(tenant);
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes an existing address from the database.
        /// </summary>
        /// <param name="address">The address to delete.</param>
        public static void DeleteAddress(Address address)
        {
            using FlexiWorkspaceContext db = new();
            db.Addresses.Remove(address);
            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing owner in the database.
        /// </summary>
        /// <param name="owner">The owner to update.</param>
        public static void UpdateOwner(Owner owner)
        {
            using FlexiWorkspaceContext db = new();
            owner.UpdatedAt = DateTime.Now;
            db.Owners.Update(owner);
            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing tenant in the database.
        /// </summary>
        /// <param name="tenant">The tenant to update.</param>
        public static void UpdateTenant(Tenant tenant)
        {
            using FlexiWorkspaceContext db = new();
            tenant.UpdatedAt = DateTime.Now;
            db.Tenants.Update(tenant);
            db.SaveChanges();
        }

        /// <summary>
        /// Updates an existing address in the database.
        /// </summary>
        /// <param name="address">The address to update.</param>
        public static void UpdateAddress(Address address)
        {
            using FlexiWorkspaceContext db = new();
            address.UpdatedAt = DateTime.Now;
            db.Addresses.Update(address);
            db.SaveChanges();
        }
    }
}
