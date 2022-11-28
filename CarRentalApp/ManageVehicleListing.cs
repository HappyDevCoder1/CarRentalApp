using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageVehicleListing : Form
    {
        //object of database model
        private readonly CarRentalEntities _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            //intializing
            _db = new CarRentalEntities();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            //fetching data to show in grid view on form load

            // select * from TypesOfCars
            // var cars = _db.TypesOfCars.ToList();

            //alias
            //select id as CarID, name as Name from TypesOfCars
            // var cars = _db.TypesOfCars.Select(q => new
            //{

            //    CarId = q.Id,
            //   CarName = q.Make

            //}).ToList();

            var cars = _db.TypesOfCars.Select(q => new
            {
                Make = q.Make,
                Model = q.Model,
                VIN = q.VIN,
                Year = q.Year,
                LicensePlateNumber = q.LicensePlateNumber,
                q.Id

            }).ToList();

            gvVehicleListing.DataSource = cars;
            gvVehicleListing.Columns[4].HeaderText = "License Plate Number";
            gvVehicleListing.Columns[5].Visible = false;

            //gvVehicleListing.Columns[0].HeaderText = "ID";
            //gvVehicleListing.Columns[1].HeaderText = "NAME";

        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {

            var addEditVehicle = new AddEditVehicle(this);
            addEditVehicle.MdiParent = this.MdiParent;
            addEditVehicle.Show();


        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                //AddEditVehicle.ActiveForm.Text = "Add New Vehicle";

                if (gvVehicleListing.Rows.Count > 0)
                {
                    //get id of selected row

                    var id = (int)gvVehicleListing.SelectedRows[0].Cells["Id"].Value;

                    //var id = Convert.ToInt32(gvVehicleListing.SelectedRows[0].Cells[0].Value.ToString());

                    //query database for record
                    var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);

                    //launch AddEditVehicle with the data
                    var addEditVehicle = new AddEditVehicle(car, this);
                    addEditVehicle.MdiParent = this.MdiParent;
                    addEditVehicle.Show();

                }
                else
                {
                    MessageBox.Show("There is not any row selected, you select row by clicking on the row header!");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {

                //get id of selected row
                var id = (int)gvVehicleListing.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var car = _db.TypesOfCars.FirstOrDefault(q => q.Id == id);

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this record?",
                    "Delete", MessageBoxButtons.YesNoCancel);

                if (dr == DialogResult.Yes)
                {

                    //Delete Vehicle from table
                    _db.TypesOfCars.Remove(car);
                    _db.SaveChanges();

                }
                PopulateGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        //New Function to PopulateGrid. Can be called anytime we need a grid refresh
        public void PopulateGrid()
        {
            // Select a custom model collection of cars from database
            var cars = _db.TypesOfCars
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    q.Id

                }).ToList();

            gvVehicleListing.DataSource = cars;
            gvVehicleListing.Columns[4].HeaderText = "License Plate Number";
            //Hide the column for ID. Changed from the hard coded column value to the name, 
            // to make it more dynamic. 
            gvVehicleListing.Columns["Id"].Visible = false;
        }

        private void gvVehicleListing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
