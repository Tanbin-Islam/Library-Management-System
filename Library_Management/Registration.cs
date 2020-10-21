using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Library_Management
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        //connect to the database
        String con = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        private void linkhome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            index home = new index();
            this.Hide();
            home.Show();
        }

        private void linkLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login log_in = new Login();
            this.Hide();
            log_in.Show();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                var cont = new SqlConnection(con);

                cont.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO members VALUES (@name, @email, @address, @phone)", cont);
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@name", userNameBox.Text);
                cmd.Parameters.AddWithValue("@email", userEmailBox.Text);
                cmd.Parameters.AddWithValue("@address", userAdrsBox.Text);
                cmd.Parameters.AddWithValue("@phone", userPhoneBox.Text);

                cmd.ExecuteNonQuery();

                cont.Close();

                clearTextBox();

                MessageBox.Show("Registration Successfull!!!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Login objlogin = new Login();
                this.Hide();
                objlogin.Show();

            }
        }

        private void clearTextBox()
        {
            userNameBox.Clear();
            userEmailBox.Clear();
            userAdrsBox.Clear();
            userPhoneBox.Clear();

            userNameBox.Focus();
        }

        private bool IsValid()
        {
            if (userNameBox.Text == string.Empty || userEmailBox.Text == string.Empty || userAdrsBox.Text == string.Empty || userPhoneBox.Text == string.Empty)
            {
                MessageBox.Show("One or more input feild is empty!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
    }
}