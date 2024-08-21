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

using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for OwnerDetailView.xaml
    /// </summary>
    public partial class OwnerDetailView : Window
    {
        public Owner CurrentOwner;
        private OwnerViewModel OwnerViewModel;

        /// <summary>
        /// Initializes a new instance of the OwnerDetailView class.
        /// </summary>
        /// <param name="owner">The current owner to be displayed and edited.</param>
        /// <param name="ownerViewModel">The OwnerViewModel used to handle owner updates.</param>
        public OwnerDetailView(Owner owner, OwnerViewModel ownerViewModel)
        {
            InitializeComponent();
            CurrentOwner = owner;
            OwnerViewModel = ownerViewModel;
            DataContext = CurrentOwner;
        }

        /// <summary>
        /// Handles the click event of the Save button. Validates input and updates the current owner.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentOwner.Active = ownerActive.IsChecked ?? CurrentOwner.Active;
            CurrentOwner.Tva = ownerVAT.Text;
            CurrentOwner.Label = ownerLabel.Text;
            CurrentOwner.UpdatedAt = DateTime.Now;

            try
            {
                OwnerViewModel.UpdateOwner(CurrentOwner);
                Close();
            } catch (DbUpdateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the click event of the Cancel button. Closes the window without saving changes.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the click event of the Delete button. Marks the owner as inactive and updates it.
        /// </summary>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentOwner.Active = false;
            OwnerViewModel.UpdateOwner(CurrentOwner);
            Close();
        }
    }
}
