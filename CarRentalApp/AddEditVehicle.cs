using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        private readonly CarRentalEntities _db;

        //For adding vehicle
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            //changes label
            lblTitle.Text = "Add New vehicle";
            //changes title
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = manageVehicleListing;

        }

        //For editing Vehicle
        public AddEditVehicle(TypesOfCar carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            //changes label of form
            lblTitle.Text = "Edit Vehicle";
            //changes title of form
            this.Text = "Edit Vehicle";

            _manageVehicleListing = manageVehicleListing;

            if (carToEdit == null)
            {
                MessageBox.Show("Make sure you have selected valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                PopulateFields(carToEdit);
            }
        }

        //method to populate fields whenever a person wants to edit vehicle 
        private void PopulateFields(TypesOfCar car)
        {
            lblID.Text = car.Id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicensePlateNumber.Text = car.LicensePlateNumber;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tbMake.Text) || string.IsNullOrEmpty(tbModel.Text))
                {

                    MessageBox.Show("Plz make sure that you provide Make and Model of the car");
                }
                else
                {
                    //if EditMode == true then update else add new record
                    if (isEditMode)
                    {
                        var id = int.Parse(lblID.Text);
                        var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);
                        car.Model = tbModel.Text;
                        car.Make = tbMake.Text;
                        car.VIN = tbVIN.Text;
                        car.Year = int.Parse(tbYear.Text);
                        car.LicensePlateNumber = tbLicensePlateNumber.Text;

                    }
                    else
                    {
                        //add new vehicle data
                        var newCar = new TypesOfCar
                        {
                            LicensePlateNumber = tbLicensePlateNumber.Text,
                            Make = tbMake.Text,
                            Model = tbModel.Text,
                            VIN = tbVIN.Text,
                            Year = int.Parse(tbYear.Text)
                        };

                        _db.TypesOfCars.Add(newCar);


                    }
                    _db.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    MessageBox.Show("Operation Completed");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
