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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LaundryNadhifJiddan
{
    public partial class DataDetailorder : Form
    {
        public DataDetailorder()
        {
            InitializeComponent();
            tampildata();

            SqlCommand cmd = new SqlCommand("SELECT kodelayanan , jenislayanan FROM Layanan", conn);
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM Detailorder ", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            DataGridViewLinkColumn link = new DataGridViewLinkColumn();
            link.Text = "Cetak Nota";
            link.Name = "Cetak Nota";
            link.HeaderText = "Cetak Nota";
            link.UseColumnTextForLinkValue = true;
            
            dataGridView1.DataSource = dt;
            dataGridView1.Columns.Add(link);
        }
    

        private void button1_Click(object sender, EventArgs e)
        {
     
        }

        private void clear()
        {
            textBox2.Text = "";
            textBox4.Text = "";
            comboBox1 .SelectedIndex = -1;
            numericUpDown1.Value = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("UPDATE Detailorder SET kodeorder = @kodeorder, kodelayanan = @kodelayanan, jumlahunit = @jumlahunit , biaya = @biaya WHERE kodeorder= @kodeorder", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.Parameters.AddWithValue("@kodeorder", textBox2.Text);
            cmd.Parameters.AddWithValue("@kodelayanan", comboBox1.SelectedValue);
            cmd.Parameters.AddWithValue("@jumlahunit", numericUpDown1.Value);
            cmd.Parameters.AddWithValue("@biaya", textBox4.Text);
            cmd.ExecuteNonQuery();
            conn.Close();
            tampildata();
            MessageBox.Show("Data berhasil diubah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show("Apakah anda yakin ingin menghapus data?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(konfirmasi == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Detailorder WHERE kodeorder = @kodeorder", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@kodeorder", textBox2.Text);
                cmd.ExecuteNonQuery();
                conn.Close();
                tampildata();
                MessageBox.Show("Data berhasil diubah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.SelectedValue = dataGridView1.CurrentRow.Cells["kodelayanan"].Value.ToString();
            numericUpDown1.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["jumlahunit"].Value.ToString());
            textBox4.Text = dataGridView1.CurrentRow.Cells["biaya"].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells["kodeorder"].Value.ToString();


        
        }

        private void DataDetailorder_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string kodeorder = dataGridView1.CurrentRow.Cells["kodeorder"].Value.ToString();
            string kodelayanan = dataGridView1.CurrentRow.Cells["kodelayanan"].Value.ToString();
            string jumlahunit = dataGridView1.CurrentRow.Cells["jumlahunit"].Value.ToString();
            string biaya = dataGridView1.CurrentRow.Cells["biaya"].Value.ToString();

            Print p = new Print(kodeorder, kodelayanan, jumlahunit, biaya);
            p.ShowDialog();
        }
    }
}
