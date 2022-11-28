using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            var addRentalRecords = new AddEditRentalRecord(this)
            {
                MdiParent = this.MdiParent
            };
            addRentalRecords.Show();
        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                //AddEditVehicle.ActiveForm.Text = "Add New Vehicle";

                if (gvRecordListing.Rows.Count > 0)
                {
                    //get id of selected row
                    var id = (int)gvRecordListing.SelectedRows[0].Cells["Id"].Value;

                    //query database for record
                    var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                    //launch AddEditRentalRecord with the data

                    var addEditRecords = new AddEditRentalRecord(record, this);
                    addEditRecords.MdiParent = this.MdiParent;
                    addEditRecords.Show();

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
                var id = (int)gvRecordListing.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                DialogResult dr = MessageBox.Show("Are you sure you want to delete this record?",
                    "Delete", MessageBoxButtons.YesNoCancel);

                if (dr == DialogResult.Yes)
                {

                    //Delete Record from table
                    _db.CarRentalRecords.Remove(record);
                    _db.SaveChanges();
                }
                //refreshes grid when record gets deleted
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

        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void PopulateGrid()
        {
            var records = _db.CarRentalRecords.Select(q => new
            {

                Customer = q.CustomerName,
                Date_out = q.DateRented,
                Date_in = q.DateReturned,
                Id = q.id,
                Cost = q.Cost,
                //sql joins
                Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model

            }).ToList();

            gvRecordListing.DataSource = records;
            gvRecordListing.Columns["Date_in"].HeaderText = "Date in";
            gvRecordListing.Columns["Date_out"].HeaderText = "Date out";
            gvRecordListing.Columns["Id"].Visible = false;
        }
    }
}
