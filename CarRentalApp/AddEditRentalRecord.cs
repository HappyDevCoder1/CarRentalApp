using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditRentalRecord : Form
    {
        //this object will provide access to all data stored in our model
        private readonly CarRentalEntities _db;
        private bool isEditMode;
        private ManageRentalRecords _manageRentalRecords;

        //add records
        public AddEditRentalRecord(ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();
            //initialize
            _db = new CarRentalEntities();
            //changes label
            lblTitle.Text = "Add New Rental Record";
            //changes title
            this.Text = "Add New Rental Record";
            isEditMode = false;
            _manageRentalRecords = manageRentalRecords;
        }

        //edit records
        public AddEditRentalRecord(CarRentalRecord recordToEdit, ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();

            //changes label of form
            lblTitle.Text = "Edit Record Record";
            //changes title of form
            this.Text = "Edit Record Record";

            _manageRentalRecords = manageRentalRecords;

            if (recordToEdit == null)
            {
                MessageBox.Show("Make sure you have selected valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(recordToEdit);
            }

        }

        private void PopulateFields(CarRentalRecord record)
        {
            tbCustomerName.Text = record.CustomerName;
            DateRented.Value = (DateTime)record.DateRented;
            DateReturned.Value = (DateTime)record.DateReturned;
            tbCost.Text = record.Cost.ToString();
            lblRecordId.Text = record.id.ToString();

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            // MessageBox.Show($"Thank tou for renting: {tbCustomerName.Text} \n\r " +
            //   $"{DateRented.Value.ToString()} \n\r " +
            //  $"{ToCComboBox.SelectedItem.ToString()}");

            try
            {

                string customerName = tbCustomerName.Text;
                var Date_out = DateRented.Value;
                var Date_in = DateReturned.Value;
                var Car_Type = CarTypecb.Text;

                double cost = Convert.ToDouble(tbCost.Text);
                var isValid = true;
                var errorMessage = "";

                //Validation Check
                //checking if customer name or car type is null
                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(Car_Type))
                {
                    isValid = false;
                    errorMessage += "Error: plz provide missing data!\n\r";
                }

                //condtion if date rented is greater than date returned
                if (Date_out > Date_in)
                {
                    isValid = false;
                    errorMessage += "Error: Invalid date selection\n\r";
                }

                //Showing Data 
                //isValid == true
                if (isValid)
                {
                    var rentalRecord = new CarRentalRecord();
                    //if isEditMode == true
                    if (isEditMode)
                    {
                        //if in edit mode,then get the id and retrieve the record from the database
                        //and place the result in the rental record object
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                    }

                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = Date_out;
                    rentalRecord.DateReturned = Date_in;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarId = (int)CarTypecb.SelectedValue;

                    // if not in edit mode then add the record object
                    if (!isEditMode)
                        //Saving data into the database
                        _db.CarRentalRecords.Add(rentalRecord);

                    //saving changes in the entity
                    _db.SaveChanges();

                    //refreshes the grid
                    _manageRentalRecords.PopulateGrid();

                    //Showing the data in message box
                    MessageBox.Show($"Customer Name: {customerName} \n\r " +
                        $"Date Rented: {Date_out} \n\r" +
                        $"Date Returned: {Date_in} \n\r" +
                        $"Cost: {cost} \n\r" +
                        $"Type of Car: {Car_Type}  \n\r" +
                        $"Thank You For Your Business!");

                    Close();

                }
                else
                {
                    MessageBox.Show(errorMessage);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw; 

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //select * from TypesOfCars table

            //var cars = carRentalEntities.TypesOfCars.ToList();

            var cars = _db.TypesOfCars.Select(q => new
            {

                Id = q.Id,
                Name = q.Make + " " + q.Model

            }).ToList();

            CarTypecb.DisplayMember = "Name";
            CarTypecb.ValueMember = "Id";
            CarTypecb.DataSource = cars;
        }

    }
}
