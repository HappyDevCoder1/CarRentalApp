using System;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class Login : Form
    {
        private readonly CarRentalEntities _db;
        public Login()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SHA256 sha = SHA256.Create();

                var username = tbUserName.Text.Trim();
                var password = tbPassword.Text;

                var hashed_password = Utils.HashPassword(password);

                var user = _db.Users.FirstOrDefault(q => q.username == username && q.password == hashed_password
                && q.isActive == true);
                if (user == null)
                {
                    MessageBox.Show("plz provide correct credentials!");
                }
                else
                {
                    //var role = user.UserRoles.FirstOrDefault();
                    //var roleShortName = role.Role.shortname;
                    var mainWindow = new MainWindow(this, user);
                    mainWindow.Show();
                    Hide();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Something Went Wrong,plz try again");
            }
        }
    }
}
