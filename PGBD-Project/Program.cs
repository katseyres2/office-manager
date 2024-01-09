using PGBD_Project.BU;
using PGBD_Project.DB;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        List<Tenant> tenant = UserService.GetTenants();
        foreach (Tenant t in tenant)
        {
            Console.WriteLine(t.FirstName);
        }
    }
}