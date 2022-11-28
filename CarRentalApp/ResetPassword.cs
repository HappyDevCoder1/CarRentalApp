using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ResetPassword : Form
    {
        private readonly CarRentalEntities _db;
        private User _user;

        public ResetPassword(User user)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _user = user;
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                var passowrd = tbNewPassword.Text;
                var confirm_password = tbConfirmPassword.Text;
                var user = _db.Users.FirstOrDefault(q => q.id == _user.id);

                if (passowrd != confirm_password)
                {
                    MessageBox.Show("Password do not match,plz try again");
                }

                user.password = Utils.HashPassword(passowrd);
                _db.SaveChanges();

                MessageBox.Show("Password has been reset");

                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("an error has occured plz try again");
            }
        }
    }
}
