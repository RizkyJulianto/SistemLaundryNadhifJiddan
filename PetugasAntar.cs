using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryNadhifJiddan
{
    public partial class PetugasAntar : Form
    {
        public PetugasAntar()
        {
            InitializeComponent();
            tampildata();
        }

        SqlConnection conn = Properti.conn;

        private void tampildata()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PetugasAntar", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            dataGridView1.DataSource = dt;
        }

        int cell = -1;


        private void clear()
        {
            textBox1.Text = "";
            textBox4.Text = "";
        }

        private void PetugasAntar_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(Properti.validasi(this.Controls))
                {
                    MessageBox.Show("Inputan tidak boleh kosong", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }  else if(!Properti.Number(textBox1.Text))
                {
                    MessageBox.Show("Inputan Nomortelepon hanya boleh diisi angka", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                } else
                {
                    var konfirmasi = MessageBox.Show("Apakah anda yakin data yang ingin ditambahkan sudah benar", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if(konfirmasi == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO PetugasAntar VALUES(@namapetugas,@nomortelepon)", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@namapetugas", textBox4.Text);
                        cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        tampildata();
                        MessageBox.Show("Data berhasil ditambahkan", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                } 
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Properti.validasi(this.Controls))
                {
                    MessageBox.Show("Inputan tidak boleh kosong", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!Properti.Number(textBox1.Text))
                {
                    MessageBox.Show("Inputan Nomortelepon hanya boleh diisi angka", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    var konfirmasi = MessageBox.Show("Apakah anda yakin data yang ingin diubah sudah benar", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (konfirmasi == DialogResult.Yes)
                    {
                        var row = dataGridView1.CurrentRow;
                        int kodepetugas = Convert.ToInt32(row.Cells["kodepetugas"].Value.ToString());
                        SqlCommand cmd = new SqlCommand("UPDATE PetugasAntar SET namapetugas = @namapetugas , nomortelepon = @nomortelepon WHERE kodepetugas = @kodepetugas", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@kodepetugas", kodepetugas);
                        cmd.Parameters.AddWithValue("@namapetugas", textBox4.Text);
                        cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        tampildata();
                        MessageBox.Show("Data berhasil diubah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
                var konfirmasi = MessageBox.Show("Apakah anda yakin data akan dihapus?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (konfirmasi == DialogResult.Yes)
                {
                    var row = dataGridView1.CurrentRow;
                    int kodepetugas = Convert.ToInt32(row.Cells["kodepetugas"].Value.ToString());
                    SqlCommand cmd = new SqlCommand("DELETE FROM PetugasAntar WHERE kodepetugas = @kodepetugas", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.Parameters.AddWithValue("@kodepetugas", kodepetugas);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    tampildata();
                    MessageBox.Show("Data berhasil dihapus", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox4.Text = dataGridView1.CurrentRow.Cells["namapetugas"].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells["nomortelepon"].Value.ToString();
        }
    }
}
