using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using PGBD_Project.BU;
using PGBD_Project.DB;

namespace WPF.Command
{
    class CreateOfficeCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //Debug.WriteLine("Create");
            //Office office = new Office();
            //office.Active = true;
            //office.Description = "Description";
            //WorkspaceService.AddOffice(office);
        }
    }
}
