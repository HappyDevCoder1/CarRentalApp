using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{

    public partial class AddUser : Form
    {
        private readonly CarRentalEntities _db;
        private ManageUsers _manageUsers;
        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cbRole.DataSource = roles;
            cbRole.ValueMember = "id";
            cbRole.DisplayMember = "name";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUserName.Text;
                var roleId = (int)cbRole.SelectedValue;
                var password = Utils.DefaultHashedPassword();

                var user = new User
                {
                    username = username,
                    password = password,
                    isActive = true
                };
                _db.Users.Add(user);
                _db.SaveChanges();

                var userid = user.id;

                var userRole = new UserRole
                {
                    roleid = roleId,
                    userid = userid

                };
                _db.UserRoles.Add(userRole);
                _db.SaveChanges();

                MessageBox.Show("New user added successfully");
                _manageUsers.PopulateGrid();
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("An Error has occured");
            }
        }
    }
}
