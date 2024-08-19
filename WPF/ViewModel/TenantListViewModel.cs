using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.BU;
using PGBD_Project.DB;

namespace WPF.ViewModel
{
    public class TenantListViewModel
    {
        public TenantListViewModel() { }

        public List<Tenant> Tenants {
            get { return UserService.GetTenants(); }
        }
    }
}
