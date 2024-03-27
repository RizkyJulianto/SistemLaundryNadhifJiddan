using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryNadhifJiddan
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        SqlConnection conn = Properti.conn;

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                textBox2.PasswordChar = '\0';
            } else
            {
                textBox2.PasswordChar = '*';
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand user = new SqlCommand("SELECT COUNT(*) FROM [User] WHERE email = @email",conn);
                user.CommandType = CommandType.Text;
                conn.Open();
                user.Parameters.AddWithValue("@email", textBox1.Text);


                int userbelumada = (int)user.ExecuteScalar();
                conn.Close();
                if(userbelumada == 0)
                {
                    MessageBox.Show("User tidak dapat ditemukan", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM [User] WHERE email = @email AND password = @password", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@email", textBox1.Text);
                cmd.Parameters.AddWithValue("@password", Properti.enkripsi(textBox2.Text));
                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.Read())
                {
                    string namauser = dr["namauser"].ToString();
                    MainForm mf = new MainForm(namauser);
                    mf.Show();
                    this.Hide();
                } else
                {
                    MessageBox.Show("Login gagal harap periksa kembali email dan password anda", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                conn.Close();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
