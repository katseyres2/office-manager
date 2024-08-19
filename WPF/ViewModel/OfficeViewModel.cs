using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PGBD_Project.BU;
using PGBD_Project.DB;

namespace WPF.ViewModel
{
    public class OfficeViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly WorkspaceService _workspaceService;

        public ObservableCollection<Office> offices;

        private void OnPropertyRaised(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }


        }
    }
}
