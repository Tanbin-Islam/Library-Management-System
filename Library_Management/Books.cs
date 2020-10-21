using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Library_Management
{
    public partial class Books : Form
    {

        //connect to the database
        String con = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        public int id;

        public Books()
        {
            InitializeComponent();
        }

        private void Books_Load(object sender, EventArgs e)
        {
            GetBooksRecord();
            GetComboData();
        }

        private void GetComboData()
        {
            var cont = new SqlConnection(con);
            cont.Open();

            String query = "Select * from category";

            SqlCommand cmd = new SqlCommand(query, cont);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            da.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                comboBox1.Items.Add(dr["name"].ToString());
            }
        }

        private void GetBooksRecord()
        {
            var cont = new SqlConnection(con);
            cont.Open();

            string query = "Select * from books";
            SqlCommand cmd = new SqlCommand(query, cont);
            DataTable dt = new DataTable();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);

            cont.Close();

            dataBooksView.DataSource = dt;
        }


        //insert data
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                var cont = new SqlConnection(con);


                    cont.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO books VALUES (@name, @author, @categoy, @edition, @price)", cont);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@name", BnameText.Text);
                    cmd.Parameters.AddWithValue("@author", AnameText.Text);
                    cmd.Parameters.AddWithValue("@categoy", comboBox1.Text);
                    cmd.Parameters.AddWithValue("@edition", BeditionText.Text);
                    cmd.Parameters.AddWithValue("@price", BpriceText.Text);

                    cmd.ExecuteNonQuery();

                    cont.Close();
                    saveImage();
                    GetBooksRecord();
                    clearTextBox();

                    MessageBox.Show("Data Successfully Inserted", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


            }
        }

        //clear text boxes
        private void clearTextBox()
        {
            id = 0;
            BnameText.Clear();
            AnameText.Clear();
            comboBox1.ResetText();
            BeditionText.Clear();
            BpriceText.Clear();

            BnameText.Focus();
        }

        //input field validation
        private bool IsValid()
        {
            if (BnameText.Text == string.Empty || AnameText.Text == string.Empty || BeditionText.Text == string.Empty || BpriceText.Text == string.Empty)
            {
                MessageBox.Show("One or more input feild is empty!!!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }


        //get datas into text feild
        private void dataBooksView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                DataGridViewRow row = this.dataBooksView.Rows[e.RowIndex];
                id = Convert.ToInt32(row.Cells["id"].Value.ToString());
                BnameText.Text = row.Cells["name"].Value.ToString();
                AnameText.Text = row.Cells["author"].Value.ToString();
                comboBox1.Text = row.Cells["category"].Value.ToString();
                BeditionText.Text = row.Cells["edition"].Value.ToString();
                BpriceText.Text = row.Cells["price"].Value.ToString();
            }
        }


        //data update
        private void button2_Click(object sender, EventArgs e)
        {
            if (id > 0)
            {
                var cont = new SqlConnection(con);

                cont.Open();
                SqlCommand cmd = new SqlCommand("UPDATE books SET name = @name, author = @author, category = @category, edition = @edition, price = @price WHERE id = @id", cont);

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", this.id);
                cmd.Parameters.AddWithValue("@name", BnameText.Text);
                cmd.Parameters.AddWithValue("@author", AnameText.Text);
                cmd.Parameters.AddWithValue("@category", comboBox1.Text);
                cmd.Parameters.AddWithValue("@edition", BeditionText.Text);
                cmd.Parameters.AddWithValue("@price", BpriceText.Text);

                cmd.ExecuteNonQuery();

                cont.Close();

                GetBooksRecord();
                clearTextBox();

                MessageBox.Show("Data Successfully Updated!!!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Please select a row to update!!!", "Select Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        //data delete
        private void button3_Click(object sender, EventArgs e)
        {
            if (id > 0)
            {
                var cont = new SqlConnection(con);

                cont.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM books " +
                    "WHERE id = @id", cont);

                cmd.CommandType = CommandType.Text;

                cmd.Parameters.AddWithValue("@id", this.id);

                cmd.ExecuteNonQuery();

                cont.Close();

                GetBooksRecord();
                clearTextBox();

                MessageBox.Show("Data successfully deleted!!!", "Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Please select a row to delete!!!", "Select Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        //input feild reset
        private void button4_Click(object sender, EventArgs e)
        {
            clearTextBox();
        }


        //Image upload
        private void btnImgUp_Click(object sender, EventArgs e)
        {
            OpenFileDialog image = new OpenFileDialog();
            image.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp;) | *.jpg; *.jpeg; *.png; *.bmp;";

            if (image.ShowDialog() == DialogResult.OK)
            {
                pictureBox.Text = image.FileName;
            }
        }


        private void saveImage()
        {
            File.Copy(pictureBox.Text, Path.Combine(@"F:\Windows Form\Project\Library_Management\Library_Management\images", Path.GetFileName(pictureBox.Text)), true);
            pictureBox.Clear();
        }

        private void linkLabelreturn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DashBoard dashBoard = new DashBoard();
            this.Hide();
            dashBoard.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using(var cont = new SqlConnection(con))
            {
                var cmd = cont.CreateCommand();
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "Select * from books";
                cont.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                List<Book> booksrpt = new List<Book>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Book obj = new Book();
                    obj.id = Convert.ToInt32(dt.Rows[i]["id"].ToString());
                    obj.name = dt.Rows[i]["name"].ToString();
                    obj.author = dt.Rows[i]["author"].ToString();
                    obj.category = dt.Rows[i]["category"].ToString();
                    obj.edition = dt.Rows[i]["edition"].ToString();
                    obj.price = dt.Rows[i]["price"].ToString();
                    booksrpt.Add(obj);
                }

                using (BooksRptViewer prForm = new BooksRptViewer(booksrpt))
                {
                    prForm.ShowDialog();
                }
            }
        }
    }
}
