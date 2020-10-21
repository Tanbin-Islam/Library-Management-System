using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library_Management
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        //database connection
        string con = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration reg = new Registration();
            this.Hide();
            reg.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            index home = new index();
            this.Hide();
            home.Show();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            var cont = new SqlConnection(con);

            if (userBox.Text == "" || passwordBox.Text == "")
            {
                MessageBox.Show("Please enter username or password!!!", "Empty Feild", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cont.Open();
                    string query = "SELECT * FROM admin WHERE Name = '" + userBox.Text + "' and Password = '" + passwordBox.Text + "'";

                    SqlCommand cmd = new SqlCommand(query, cont);
                    SqlDataReader sdr = cmd.ExecuteReader();

                    if (sdr.HasRows == true)
                    {
                        DashBoard objdashboard = new DashBoard();
                        this.Hide();
                        objdashboard.Show();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect User name or Password!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Incorrect Username or Password!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
