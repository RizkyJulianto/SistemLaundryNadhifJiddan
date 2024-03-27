using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LaundryNadhifJiddan
{
    public partial class Detailorder : Form
    {
        private string kodeorder;
        public Detailorder(string kodeorder)
        {
            InitializeComponent();
            this.kodeorder = kodeorder;
            tampildata();

            SqlCommand cmd = new SqlCommand("SELECT kodelayanan , jenislayanan FROM Layanan",conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "jenislayanan";
            comboBox1.ValueMember = "kodelayanan";
            comboBox1.SelectedIndex = -1;

            conn.Close();
        }

        SqlConnection conn = Properti.conn;

        private void tampildata()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Detailorder WHERE kodeorder = @kodeorder ", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.Parameters.AddWithValue("@kodeorder", kodeorder);
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();

            dataGridView1.DataSource = dt;
        }

        private void Detailorder_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Detailorder VALUES(@kodeorder,@kodelayanan,@jumlahunit,@biaya)", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.Parameters.AddWithValue("@kodeorder", kodeorder);
            cmd.Parameters.AddWithValue("@kodelayanan", comboBox1.SelectedValue);
            cmd.Parameters.AddWithValue("@jumlahunit", numericUpDown1.Value);
            cmd.Parameters.AddWithValue("@biaya", textBox1.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            tampildata();
            MessageBox.Show("Data berhasil ditambahkan", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            clear();
        }

        private void clear()
        {
            comboBox1.SelectedIndex = -1;
            numericUpDown1.Value = 0;
            textBox1.Text = "";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex != -1)
            {
                SqlCommand cmd = new SqlCommand("SELECT biaya FROM Layanan WHERE kodelayanan = @kodelayanan", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@kodelayanan", comboBox1.SelectedValue);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox1.Text = dr["biaya"].ToString();
                }
                conn.Close();
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.SelectedValue = dataGridView1.CurrentRow.Cells["kodelayanan"].Value.ToString();
            numericUpDown1.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["jumlahunit"].Value.ToString());
            textBox1.Text = dataGridView1.CurrentRow.Cells["biaya"].Value.ToString();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Order o = new Order();
            o.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var row = dataGridView1.CurrentRow;
            int kodeorder = Convert.ToInt32(row.Cells["kodeorder"].Value.ToString());
            SqlCommand cmd = new SqlCommand("UPDATE Detailorder SET kodeorder = @kodeorder, kodelayanan = @kodelayanan, jumlahunit = @jumlahunit , biaya = @biaya WHERE kodeorder= @kodeorder", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.Parameters.AddWithValue("@kodeorder", kodeorder);
            cmd.Parameters.AddWithValue("@kodelayanan", comboBox1.SelectedValue);
            cmd.Parameters.AddWithValue("@jumlahunit", numericUpDown1.Value);
            cmd.Parameters.AddWithValue("@biaya", textBox1.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            tampildata();
            MessageBox.Show("Data berhasil diubah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            clear();
        }
    }
}
