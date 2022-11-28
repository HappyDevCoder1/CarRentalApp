using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities _db;
        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen("AddUser"))
            {
                var adduser = new AddUser(this);
                adduser.MdiParent = this.MdiParent;
                adduser.Show();
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {

                if (gvUserList.Rows.Count > 0)
                {
                    //get id of selected row

                    var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                    //query database for record
                    var users = _db.Users.FirstOrDefault(q => q.id == id);

                    var hashed_password = Utils.DefaultHashedPassword();

                    users.password = hashed_password;

                    _db.SaveChanges();

                    MessageBox.Show($"{users.username}'s Password has been reset!");

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

        private void btnDeactivateUser_Click(object sender, EventArgs e)
        {
            try
            {

                if (gvUserList.Rows.Count > 0)
                {
                    //get id of selected row

                    var id = (int)gvUserList.SelectedRows[0].Cells["id"].Value;

                    //query database for record
                    var users = _db.Users.FirstOrDefault(q => q.id == id);
                    //if (users.isActive == true)
                    //   users.isActive == false;
                    // else
                    //     users.isActive = true;
                    users.isActive = users.isActive == true ? false : true;

                    _db.SaveChanges();

                    MessageBox.Show($"{users.username}'s activity status has been changed");
                    PopulateGrid();

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

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var users = _db.Users.Select(q => new
            {
                q.id,
                q.username,
                q.UserRoles.FirstOrDefault().Role.name,
                q.isActive


            }).ToList();

            gvUserList.DataSource = users;
            gvUserList.Columns["username"].HeaderText = "Username";
            gvUserList.Columns["name"].HeaderText = "Role Name";
            gvUserList.Columns["isActive"].HeaderText = "Active";

            gvUserList.Columns["id"].Visible = false;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }
    }
}
