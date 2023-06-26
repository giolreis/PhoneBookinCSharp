using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PhoneBookinCSharp
{
    public partial class Form2 : Form
    {


        
       Form1 f1 = new Form1();
       SqlConnection con = new SqlConnection("Data Source=LAPTOP-3QI6H2PA; Initial Catalog=PhoneBookDB; Integrated Security=True;");
       SqlCommand cmd;
       SqlDataReader dr;
        public Form2()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();


            cmd = new SqlCommand("select*from users where username=@username and userpass=@userpass", con);
            cmd.Parameters.AddWithValue("@username", txtlogin.Text);
            cmd.Parameters.AddWithValue("@userpass", txtsenha.Text);
            con.Open();
           
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {

                
                MessageBox.Show("Bem Vindo");
                txtlogin.Clear();
                txtsenha.Clear();   
             
            }
            else
            {
                MessageBox.Show("Erro, tente Novamente");
            }
            con.Close();
            f1.ShowDialog();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
