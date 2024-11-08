﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LaundryNadhifJiddan
{
    public partial class Pelanggan : Form
    {
        public Pelanggan()
        {
            InitializeComponent();
            tampildata();
        }

        SqlConnection conn = Properti.conn;

        private void tampildata()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pelanggan", conn);
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
            textBox2.Text = "";
            richTextBox1.Text = "";
        }
    

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Pelanggan_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Properti.validasi(this.Controls))
                {
                    MessageBox.Show("Inputan tidak boleh kosong", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!Properti.Number(textBox2.Text))
                {
                    MessageBox.Show("Inputan Nomortelepon hanya boleh diisi angka", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    var konfirmasi = MessageBox.Show("Apakah anda yakin data yang ingin ditambahkan sudah benar", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (konfirmasi == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO Pelanggan VALUES(@nomortelepon,@nama, @alamat)", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@nomortelepon", textBox2.Text);
                        cmd.Parameters.AddWithValue("@nama", textBox1.Text);
                        cmd.Parameters.AddWithValue("@alamat", richTextBox1.Text);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        tampildata();
                        MessageBox.Show("Data berhasil ditambahkan", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                    }
                }
            }
            catch (Exception ex)
            {
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
                else if (!Properti.Number(textBox2.Text))
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
                        SqlCommand cmd = new SqlCommand("UPDATE Pelanggan SET nama = @nama, alamat = @alamat WHERE nomortelepon = @nomortelepon", conn);
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        cmd.Parameters.AddWithValue("@nomortelepon", textBox2.Text);
                        cmd.Parameters.AddWithValue("@nama", textBox1.Text);
                        cmd.Parameters.AddWithValue("@alamat", richTextBox1.Text);
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
                
                SqlCommand cmd = new SqlCommand("DELETE FROM Pelanggan WHERE nomortelepon = @nomortelepon", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@nomortelepon", textBox2.Text);
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
            textBox2.Text = dataGridView1.CurrentRow.Cells["nomortelepon"].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells["nama"].Value.ToString();
            richTextBox1.Text = dataGridView1.CurrentRow.Cells["alamat"].Value.ToString();
        }
    }
}
