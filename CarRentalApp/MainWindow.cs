using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class MainWindow : Form
    {
        private Login _login;
        public string _roleName;
        public User _user;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Login login, User user)
        {
            InitializeComponent();
            _login = login;
            _user = user;
            _roleName = user.UserRoles.FirstOrDefault().Role.shortname;
        }

        //public MainWindow(Login login, User user)
        // {
        //     this.login = login;
        //     this._user = user;
        //  }

        private void addToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //var OpenForms = Application.OpenForms.Cast<Form>();
            // var isOpen = OpenForms.Any(q => q.Name == "AddEditRentalRecord");

            if (!Utils.FormIsOpen("AddEditRentalRecords"))
            {
                //making object of the add rental record form class
                var addRentalRecord = new AddEditRentalRecord();
                //this keyword refers to the MainWindow form class
                addRentalRecord.MdiParent = this.MdiParent;
                //show add rental record form
                addRentalRecord.Show();
            }

        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //var OpenForms = Application.OpenForms.Cast<Form>();
            // var isOpen = OpenForms.Any(q => q.Name == "ManageVehicleListing");

            if (!Utils.FormIsOpen("ManageVehicleListing"))
            {

                var vehicleListing = new ManageVehicleListing();
                vehicleListing.MdiParent = this.MdiParent;
                vehicleListing.Show();
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //var OpenForms = Application.OpenForms.Cast<Form>();
            //var isOpen = OpenForms.Any(q => q.Name == "ManageRentalRecords");

            if (!Utils.FormIsOpen("ManageRentalRecords"))
            {
                var manageRentalRecords = new ManageRentalRecords();
                manageRentalRecords.MdiParent = this.MdiParent;
                manageRentalRecords.Show();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _login.Close();
        }

        private void manageUsers_Click(object sender, System.EventArgs e)
        {

            //var OpenForms = Application.OpenForms.Cast<Form>();
            //var isOpen = OpenForms.Any(q => q.Name == "ManageUsers");

            if (!Utils.FormIsOpen("ManageUsers"))
            {

                var manageUsers = new ManageUsers();
                manageUsers.MdiParent = this.MdiParent;
                manageUsers.Show();

            }
        }

        private void MainWindow_Load(object sender, System.EventArgs e)
        {
            if (_user.password == Utils.DefaultHashedPassword())
            {
                var resetPassword = new ResetPassword(_user);
                resetPassword.ShowDialog();

            }

            var username = _user.username;
            tssLoginStrip.Text = $"Logged in as: {username}";

            if (_roleName != "admin")
            {
                manageUsers.Visible = false;

            }
        }
    }
}
