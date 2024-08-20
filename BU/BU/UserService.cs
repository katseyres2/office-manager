using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.DB;

namespace PGBD_Project.BU
{
    public class UserService
    {
        public static List<Owner> GetOwners()
        {
            using FlexiWorkspaceContext db = new();
            return db.Owners.ToList();
        }

        public static Predicate<Owner> FindOwnerById(int id)
        {
            return delegate (Owner owner)
            {
                return owner.OwnerId == id;
            };
        }

        public static Predicate<Tenant> FindTenantById(int id)
        {
            return delegate (Tenant tenant)
            {
                return tenant.TenantId == id;
            };
        }

        public static Predicate<Address> FindAddressById(int id)
        {
            return delegate (Address address)
            {
                return address.AddressId == id;
            };
        }

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

        public static List<Address> GetAddresses()
        {
            using FlexiWorkspaceContext db = new();
            return db.Addresses.ToList();
        }

        public static List<object> GetUsers()
        {
            using FlexiWorkspaceContext db = new();
            List<object> users = new();
            users.AddRange(GetOwners());
            users.AddRange(GetTenants());
            return users;
        }

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

        public static void AddOwner(string? label, string? tva, bool active)
        {
            using FlexiWorkspaceContext db = new();
            db.Owners.Add(CreateOwner(label, tva, active));
            db.SaveChanges();
        }

        public static void AddTenant(string? firstname, string? lastname, bool active, string? phone, string? email, string addressNumber, string street, string postCode, string city, string country)
        {
            using FlexiWorkspaceContext db = new();
            db.Tenants.Add(CreateTenant(firstname, lastname, active, phone, email, addressNumber, street, postCode, city, country));
            db.SaveChanges();
        }

        public static void AddAddress(Address address)
        {
            using FlexiWorkspaceContext db = new();
            db.Addresses.Add(address);
            db.SaveChanges();
        }

        public static void DeleteOwner(Owner owner)
        {
            using FlexiWorkspaceContext db = new();
            db.Owners.Remove(owner);
            db.SaveChanges();
        }

        public static void DeleteTenant(Tenant tenant)
        {
            using FlexiWorkspaceContext db = new();
            db.Tenants.Remove(tenant);
            db.SaveChanges();
        }

        public static void DeleteAddress(Address address)
        {
            using FlexiWorkspaceContext db = new();
            db.Addresses.Remove(address);
            db.SaveChanges();
        }

        public static void UpdateOwner(Owner owner)
        {
            using FlexiWorkspaceContext db = new();
            owner.UpdatedAt = DateTime.Now;
            db.Owners.Update(owner);
            db.SaveChanges();
        }

        public static void UpdateTenant(Tenant tenant)
        {
            using FlexiWorkspaceContext db = new();
            tenant.UpdatedAt = DateTime.Now;
            db.Tenants.Update(tenant);
            db.SaveChanges();
        }

        public static void UpdateAddress(Address address)
        {
            using FlexiWorkspaceContext db = new();
            address.UpdatedAt = DateTime.Now;
            db.Addresses.Update(address);
            db.SaveChanges();
        }
    }
}
