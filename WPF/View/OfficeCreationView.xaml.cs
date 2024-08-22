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

using PGBD_Project.BU;
using PGBD_Project.DB;

using WPF.ViewModel;

namespace WPF.View
{
    /// <summary>
    /// Interaction logic for OfficeCreationView.xaml
    /// </summary>
    public partial class OfficeCreationView : Window
    {
        private readonly OfficeViewModel _officeViewModel;
        private readonly OwnerViewModel _ownerViewModel;
        private Owner? _currentOwner;

        /// <summary>
        /// Initializes a new instance of the OfficeCreationView class.
        /// </summary>
        /// <param name="officeViewModel">The OfficeViewModel used to handle office creation.</param>
        /// <param name="ownerViewModel">The OwnerViewModel used to populate the owner ComboBox.</param>
        public OfficeCreationView(OfficeViewModel officeViewModel, OwnerViewModel ownerViewModel)
        {
            InitializeComponent();
            _officeViewModel = officeViewModel;
            _ownerViewModel = ownerViewModel;

            PopulateComboBoxOwner();
        }

        private void PopulateComboBoxOwner()
        {
            List<Owner> owners = _ownerViewModel.Owners.OrderBy(o => o.Label).ToList();

            // Populate owner ComboBox with owners from OwnerViewModel
            foreach (Owner o in owners)
            {
                ComboBoxOwner.Items.Add(o);
            }
        }

        /// <summary>
        /// Handles the click event of the Cancel button. Closes the window without saving.
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles the click event of the Save button. Validates input and creates a new office.
        /// </summary>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOwner == null) 
            {
                MessageBox.Show("Please select an owner.");
                return;
            }

            if (officeAddressNumber.Text.Length == 0 || officeStreet.Text.Length == 0 || officePostCode.Text.Length == 0 || officeCity.Text.Length == 0 || officeCountry.Text.Length == 0)
            {
                MessageBox.Show("Please fill all address inputs.");
                return;
            }

            try
            {
                _officeViewModel.CreateOffice(
                    Double.Parse(officeSurface.Text),
                    officeDescription.Text,
                    Double.Parse(officeRent.Text),
                    Int32.Parse(officeType.Text),
                    _currentOwner,
                    officeAddressNumber.Text,
                    officeStreet.Text,
                    officePostCode.Text,
                    officeCity.Text,
                    officeCountry.Text
                 );
                Close();
            } 
            catch (FormatException ex) { MessageBox.Show(ex.Message); }
            catch (ArgumentNullException ex) { MessageBox.Show(ex.Message); }
            catch (OverflowException ex) { MessageBox.Show(ex.Message); }
            catch (DbUpdateException ex) { MessageBox.Show(ex.Message); }
            
        }

        /// <summary>
        /// Handles the selection change event for the owner ComboBox. Updates the selected owner.
        /// </summary>
        private void ComboBoxOwner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = (ComboBox)sender;
            Owner selectedOwner = (Owner)combo.SelectedItem;
            _currentOwner = selectedOwner;
        }
    }
}
