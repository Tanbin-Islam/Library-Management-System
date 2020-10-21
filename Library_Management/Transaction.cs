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
    public partial class Transaction : Form
    {
        public Transaction()
        {
            InitializeComponent();
        }

        //connect to the database
        String con = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public int id;

        private void Transaction_Load(object sender, EventArgs e)
        {
            GetTransRecord();
        }

        private void GetTransRecord()
        {
            var cont = new SqlConnection(con);

            cont.Open();
            SqlCommand cmd = new SqlCommand("Select * from transactions", cont);
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            cont.Close();

            dataTransView.DataSource = dt;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                var cont = new SqlConnection(con);
                try
                {
                    cont.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO transactions VALUES (@bookid, @memberid, @issuedate)", cont);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@bookid", BookIdText.Text);
                    cmd.Parameters.AddWithValue("@memberid", MemberIdText.Text);
                    cmd.Parameters.AddWithValue("@issuedate", dateTimePicker.Text);
                    cmd.ExecuteNonQuery();

                    cont.Close();

                    GetTransRecord();
                    clearTextBox();

                    MessageBox.Show("Data Successfully Inserted", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch(Exception)
                {
                    MessageBox.Show("books or member id could not be found!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void clearTextBox()
        {
            id = 0;
            MemberIdText.Clear();
            BookIdText.Clear();

            MemberIdText.Focus();
        }

        private bool IsValid()
        {
            if (MemberIdText.Text == string.Empty || BookIdText.Text == string.Empty || dateTimePicker.Text == string.Empty)
            {
                MessageBox.Show("One or more input feild is empty!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void btnupd_Click(object sender, EventArgs e)
        {
            if (id > 0)
            {
                var cont = new SqlConnection(con);

                cont.Open();

                SqlCommand cmd = new SqlCommand("UPDATE transactions SET member_id = @memberid, book_id = @bookid, issue_date = @issuedate WHERE id = @id", cont);

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", this.id);
                cmd.Parameters.AddWithValue("@memberid", MemberIdText.Text);
                cmd.Parameters.AddWithValue("@bookid", BookIdText.Text);
                cmd.Parameters.AddWithValue("@issuedate", dateTimePicker.Text);

                cmd.ExecuteNonQuery();

                cont.Close();

                GetTransRecord();
                clearTextBox();

                MessageBox.Show("Data Successfully Updated!!!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Please select a row to update!!!", "Select Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDlt_Click(object sender, EventArgs e)
        {
            if (id > 0)
            {
                var cont = new SqlConnection(con);

                cont.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM transactions WHERE id = @id", cont);

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", this.id);

                cmd.ExecuteNonQuery();

                cont.Close();

                GetTransRecord();
                clearTextBox();

                MessageBox.Show("Data successfully deleted!!!", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a row to delete!!!", "Select Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnrst_Click(object sender, EventArgs e)
        {
            clearTextBox();
        }

        private void dataTransView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = this.dataTransView.Rows[e.RowIndex];
            id = Convert.ToInt32(row.Cells["id"].Value.ToString());
            MemberIdText.Text = row.Cells["member_id"].Value.ToString();
            BookIdText.Text = row.Cells["book_id"].Value.ToString();
        }

        private void linkLabelreturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DashBoard dboard = new DashBoard();
            this.Hide();
            dboard.Show();
        }
    }
}
