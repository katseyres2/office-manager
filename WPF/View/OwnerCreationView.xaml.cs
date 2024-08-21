using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.EntityFrameworkCore;

using PGBD_Project.DB;

using WPF.Helper;
using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for OwnerCreationView.xaml
    /// </summary>
    public partial class OwnerCreationView : Window
    {
        private OwnerViewModel OwnerViewModel;

        /// <summary>
        /// Initializes a new instance of the OwnerCreationView class.
        /// </summary>
        /// <param name="ownerViewModel">The OwnerViewModel used to handle owner creation.</param>
        public OwnerCreationView(OwnerViewModel ownerViewModel)
        {
            InitializeComponent();
            OwnerViewModel = ownerViewModel;
        }

        /// <summary>
        /// Handles the click event of the Create button. Validates input and creates a new owner.
        /// </summary>
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OwnerViewModel.CreateOwner(ownerLabel.Text, ownerVAT.Text);
                Close();
            } catch (DbUpdateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the click event of the Cancel button. Closes the window without creating an owner.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
