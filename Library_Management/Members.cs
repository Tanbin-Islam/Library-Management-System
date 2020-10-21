using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Library_Management
{
    public partial class Members : Form
    {
        public Members()
        {
            InitializeComponent();
        }

        //Database Connection
        String con = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public int id;

        private void Members_Load(object sender, EventArgs e)
        {
            GetUsersRecord();
        }

        private void GetUsersRecord()
        {
            var cont = new SqlConnection(con);

            cont.Open();

            SqlCommand cmd = new SqlCommand("Select * From members", cont);
            DataTable dtbl = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dtbl.Load(sdr);

            cont.Close();

            dataMView.DataSource = dtbl;
        }

        //insert
        private void btnIns_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                var cont = new SqlConnection(con);
                try
                {
                    cont.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO members VALUES (@name, @email, @address, @phone)", cont);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@name", UnameBox.Text);
                    cmd.Parameters.AddWithValue("@email", mailBox.Text);
                    cmd.Parameters.AddWithValue("@address", addressBox.Text);
                    cmd.Parameters.AddWithValue("@phone", PhoneBox.Text);
                    cmd.ExecuteNonQuery();

                    cont.Close();

                    GetUsersRecord();
                    clearTextBoxes();

                    MessageBox.Show("Data Successfully Inserted", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid input", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void clearTextBoxes()
        {
            id = 0;
            UnameBox.Clear();
            mailBox.Clear();
            addressBox.Clear();
            PhoneBox.Clear();
        }

        private bool IsValid()
        {
            if (UnameBox.Text == string.Empty || mailBox.Text == string.Empty || addressBox.Text == string.Empty || PhoneBox.Text == string.Empty)
            {
                MessageBox.Show("One or more input feild is empty!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        //update
        private void button2_Click(object sender, EventArgs e)
        {
            if (id > 0)
            {
                var cont = new SqlConnection(con);

                cont.Open();

                SqlCommand cmd = new SqlCommand(@"Update members set name = @name, email = @email, address = @address, phone = @phone Where id = @id", cont);

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", this.id);
                cmd.Parameters.AddWithValue("@name", UnameBox.Text);
                cmd.Parameters.AddWithValue("@email", mailBox.Text);
                cmd.Parameters.AddWithValue("@address", addressBox.Text);
                cmd.Parameters.AddWithValue("@phone", PhoneBox.Text);

                cmd.ExecuteNonQuery();

                cont.Close();

                GetUsersRecord();
                clearTextBoxes();

                MessageBox.Show("Data Updated Successfully!!!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a row to update!!!", "Select Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataMView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataMView.Rows[e.RowIndex];
            id = Convert.ToInt32(row.Cells["id"].Value.ToString());
            UnameBox.Text = row.Cells["name"].Value.ToString();
            mailBox.Text = row.Cells["email"].Value.ToString();
            addressBox.Text = row.Cells["address"].Value.ToString();
            PhoneBox.Text = row.Cells["phone"].Value.ToString();
        }


        //Delete
        private void button3_Click(object sender, EventArgs e)
        {
            if (id > 0)
            {
                var cont = new SqlConnection(con);

                cont.Open();

                SqlCommand cmd = new SqlCommand("Delete From members Where id = @id", cont);

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", this.id);

                cmd.ExecuteNonQuery();

                cont.Close();

                GetUsersRecord();
                clearTextBoxes();

                MessageBox.Show("Data successfully deleted!!!", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a row to delete!!!", "Select Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clearTextBoxes();
        }

        private void linkLabelreturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DashBoard dboard = new DashBoard();
            this.Hide();
            dboard.Show();
        }
    }
}
