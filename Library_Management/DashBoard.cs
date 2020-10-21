using System;
using System.Windows.Forms;

namespace Library_Management
{
    public partial class DashBoard : Form
    {
        public DashBoard()
        {
            InitializeComponent();
        }

        private void btnBooks_Click(object sender, EventArgs e)
        {
            Books boo = new Books();
            this.Hide();
            boo.Show();
        }

        private void btnTrancection_Click(object sender, EventArgs e)
        {
            Transaction trans = new Transaction();
            this.Hide();
            trans.Show();
        }

        private void btnMembers_Click(object sender, EventArgs e)
        {
            Members memb = new Members();
            this.Hide();
            memb.Show();
        }

        private void btnSignout_Click(object sender, EventArgs e)
        {
            Login log_in = new Login();
            this.Close();
            log_in.Show();
        }
    }
}
