using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoneBookinCSharp
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source=LAPTOP-3QI6H2PA; Initial Catalog=PhoneBookDB; Integrated Security=True;";
        int PhoneBookID = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFirstName.Text.Trim() != "" && txtLastName.Text.Trim() != "" && txtContact.Text.Trim() != "")
            {
                Regex reg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = reg.Match(txtEmail.Text.Trim());
                if (match.Success)
                {
                    using (SqlConnection sqlCon = new SqlConnection(connectionString))
                    {
                        sqlCon.Open();
                        SqlCommand sqlCmd = new SqlCommand("ContactAddOrEdit", sqlCon);
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@PhoneBookID", PhoneBookID);
                        sqlCmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Contact", txtContact.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                        sqlCmd.ExecuteNonQuery();
                        MessageBox.Show("Contato Cadastrado");
                        Clear();
                        GridFill();
                    }
                }
                else
                    MessageBox.Show("Endereço de Email invalido");
            }
            else
                MessageBox.Show("Por favor, preencha os campos obrigatórios");
        }

        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtContact.Text = txtEmail.Text = txtAddress.Text = txtSearch.Text = "";
            PhoneBookID = 0;
            btnSave.Text = "Save";
            btnDelete.Enabled = false;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        void GridFill()
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactViewAll",sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgvPhoneBook.DataSource = dtbl;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GridFill();
            btnDelete.Enabled = false;
        }

        private void dgvPhoneBook_DoubleClick(object sender, EventArgs e)
        {
            if (dgvPhoneBook.CurrentRow.Index != -1)
            {
                txtFirstName.Text = dgvPhoneBook.CurrentRow.Cells[1].Value.ToString();
                txtLastName.Text = dgvPhoneBook.CurrentRow.Cells[2].Value.ToString();
                txtContact.Text = dgvPhoneBook.CurrentRow.Cells[3].Value.ToString();
                txtEmail.Text = dgvPhoneBook.CurrentRow.Cells[4].Value.ToString();
                txtAddress.Text = dgvPhoneBook.CurrentRow.Cells[5].Value.ToString();
                PhoneBookID = Convert.ToInt32(dgvPhoneBook.CurrentRow.Cells[0].Value.ToString());

                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("ContactDeleteByID", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@PhoneBookID", PhoneBookID);
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Contado Deletado");
                Clear();
                GridFill();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("ContactSearchByValue", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.SelectCommand.Parameters.AddWithValue("@SearchValue",txtSearch.Text.Trim());
                DataTable dtbl = new DataTable();
                sqlDa.Fill(dtbl);
                dgvPhoneBook.DataSource = dtbl;
            }
        }

        private void dgvPhoneBook_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();    
        }
    }
}
