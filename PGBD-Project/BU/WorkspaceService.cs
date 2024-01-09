using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.DB;

namespace PGBD_Project.BU
{
    internal class WorkspaceService
    {
        public static List<Office> GetOffices()
        {
            using FlexiWorkspaceContext db = new();
            return db.Offices.ToList();
        }

        public static Office CreateOffice(double? surface, string? description, double? rent, bool active, int? type, Owner owner, Address address)
        {
            Office office = new ()
            {
                Surface = surface,
                Description = description,
                Rent = rent,
                Active = active,
                Type = type,
                Owner = owner,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Address = address
            };

            return office;
        }

        public static void AddOffice(Office office)
        {
            using FlexiWorkspaceContext db = new();
            db.Offices.Add(office);
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
