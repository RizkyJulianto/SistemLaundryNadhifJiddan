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

namespace LaundryNadhifJiddan
{
    public partial class Order : Form
    {
        public Order()
        {
            InitializeComponent();
            tampildata();
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "0";

            SqlCommand cmd = new SqlCommand("SELECT kodepetugas ,namapetugas FROM PetugasAntar", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "namapetugas";
            comboBox1.ValueMember = "kodepetugas";
            comboBox1.SelectedIndex = -1;

            conn.Close();




        }

        private void tampildata()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM [Order]", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            conn.Close();
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            if (dt.Rows.Count > 0)
            {
                DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
                combo.Name = "Status";
                combo.DataSource = new string[] { "PENDING", "DICUCI", "DIANTAR/DIJEMPUT" };
                combo.HeaderText = "Status";
                combo.DataPropertyName = "Status";

                DataGridViewLinkColumn link = new DataGridViewLinkColumn();
                link.Name = "Pilih Layanan";
                link.Text = "Pilih Layanan";
                link.HeaderText = "Pilih Layanan";
                link.UseColumnTextForLinkValue = true;

                dataGridView1.Columns.Add("kodeorder", "kodeorder");
                dataGridView1.Columns.Add("nomortelepon", "nomortelepon");
                dataGridView1.Columns.Add("tanggalorder", "tanggalorder");
                dataGridView1.Columns.Add("tanggalselesai", "tanggalselesai");
                dataGridView1.Columns.Add("biayaantar", "biayaantar");
                dataGridView1.Columns.Add("biayajemput", "biayajemput");
                dataGridView1.Columns.Add("biayahari", "biayahari");
                dataGridView1.Columns.Add("petugasantar", "petugasantar");
                dataGridView1.Columns.Add(combo);
                dataGridView1.Columns.Add(link);

                foreach (DataRow row in dt.Rows)
                {
                    int rowIndex = dataGridView1.Rows.Add();
                    dataGridView1.Rows[rowIndex].Cells["kodeorder"].Value = row["kodeorder"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["nomortelepon"].Value = row["nomortelepon"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["tanggalorder"].Value = row["tanggalorder"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["tanggalselesai"].Value = row["tanggalselesai"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["biayaantar"].Value = row["biayaantar"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["biayajemput"].Value = row["biayajemput"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["biayahari"].Value = row["biayahari"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["petugasantar"].Value = row["petugasantar"].ToString();
                    dataGridView1.Rows[rowIndex].Cells["Status"].Value = row["statusorder"].ToString();


                    dataGridView1.EditingControlShowing += DataGridView1_EditingControlShowing;
                }




            }
        }

        private void DataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox comboBox)
            {
                comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            var row = dataGridView1.CurrentRow;
            int kodeorder = Convert.ToInt32(row.Cells["kodeorder"].Value.ToString());
            string status = comboBox.SelectedItem.ToString();

            SqlCommand cmd = new SqlCommand("UPDATE [Order] SET statusorder = @statusorder WHERE kodeorder = @kodeorder ", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.Parameters.AddWithValue("@kodeorder", kodeorder);
            cmd.Parameters.AddWithValue("@statusorder", status);
            cmd.ExecuteNonQuery();
            conn.Close();
            tampildata();
            MessageBox.Show("Data berhasil diubah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CariNomortelepon(string nomortelepon)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Pelanggan WHERE nomortelepon = @nomortelepon", conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                string nama = dr["nama"].ToString();
                string alamat = dr["alamat"].ToString();
                TampilkanPelanggan(nama, alamat);
            } else
            {
                textBox2.Focus();
            }
            conn.Close();

        }

        private void TampilkanPelanggan(string nama, string alamat)
        {
            textBox2.Text = nama;
            richTextBox1.Text = alamat;
            dateTimePicker2.Focus(); 
        }

        SqlConnection conn = Properti.conn;

        private void Order_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string nomortelepo = textBox1.Text;
                CariNomortelepon(nomortelepo);
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            DateTime order = dateTimePicker1.Value;
            DateTime selesai = dateTimePicker2.Value;

            TimeSpan selisih = selesai - order;
            int lamahari = selisih.Days;
            textBox3.Text = lamahari.ToString();

            if (lamahari > 3)
            {
                SqlCommand cmd = new SqlCommand("SELECT biaya FROM BiayaTambahan WHERE kodebiaya = 1", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox6.Text = dr["biaya"].ToString();
                }
            }
            else
            {
                textBox6.Text = "0";
            }
            conn.Close();
            hitungtotal();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                SqlCommand cmd = new SqlCommand("SELECT biaya FROM BiayaTambahan WHERE kodebiaya = 3", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox4.Text = dr["biaya"].ToString();
                }
            }
            else
            {
                textBox4.Text = "0";
            }

            conn.Close();
            hitungtotal();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                SqlCommand cmd = new SqlCommand("SELECT biaya FROM BiayaTambahan WHERE kodebiaya = 2", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox5.Text = dr["biaya"].ToString();
                }

                label13.Enabled = true;
                comboBox1.Enabled = true;
            }
            else
            {
                textBox5.Text = "0";
                label13.Enabled = false;
                comboBox1.Enabled = false;
            }
            conn.Close();
            hitungtotal();

        }

        private void hitungtotal()
        {
            int jemput = Convert.ToInt32(textBox4.Text);
            int antar = Convert.ToInt32(textBox5.Text);
            int hari = Convert.ToInt32(textBox6.Text);

            int total = jemput + antar + hari;
            textBox7.Text = total.ToString("C", CultureInfo.GetCultureInfo("id-ID"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (Properti.validasi(this.Controls, textBox8))
                {
                    MessageBox.Show("Inputan tidak boleh kosong", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;

                }
                else if (checkBox1.Checked == false && checkBox2.Checked == false)
                {
                    MessageBox.Show("Pilih ingin diantar atau dijemput", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    SqlCommand pelanggan = new SqlCommand("SELECT COUNT (*) FROM Pelanggan WHERE nomortelepon = @nomortelepon", conn);
                    pelanggan.CommandType = CommandType.Text;
                    conn.Open();
                    pelanggan.Parameters.AddWithValue("@nomortelepon", textBox1.Text);

                    int pelanggantidakada = (int)pelanggan.ExecuteScalar();
                    conn.Close();
                    if (pelanggantidakada == 0)
                    {
                        SqlCommand tambahpelanggan = new SqlCommand("INSERT INTO Pelanggan VALUES(@nomortelepon,@nama, @alamat)", conn);
                        tambahpelanggan.CommandType = CommandType.Text;
                        conn.Open();
                        tambahpelanggan.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                        tambahpelanggan.Parameters.AddWithValue("@nama", textBox2.Text);
                        tambahpelanggan.Parameters.AddWithValue("@alamat", richTextBox1.Text);
                        tambahpelanggan.ExecuteNonQuery();
                        conn.Close();
                    }

                    SqlCommand cmd = new SqlCommand("INSERT INTO [Order] VALUES(@nomortelepon,@tanggalorder,@tanggalselesai,@biayaantar,@biayajemput,@biayahari,@petugasantar,@statusorder)", conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                    cmd.Parameters.AddWithValue("@tanggalorder", dateTimePicker1.Value);
                    cmd.Parameters.AddWithValue("@tanggalselesai", dateTimePicker2.Value);
                    cmd.Parameters.AddWithValue("@biayaantar", textBox5.Text);
                    cmd.Parameters.AddWithValue("@biayajemput", textBox4.Text);
                    cmd.Parameters.AddWithValue("@biayahari", textBox6.Text);
                    if(checkBox1.Checked)
                    {
                        cmd.Parameters.AddWithValue("@petugasantar", "-");
                    } else if(checkBox2.Checked)
                    {
                        cmd.Parameters.AddWithValue("@petugasantar", comboBox1.SelectedValue);
                    }
                    cmd.Parameters.AddWithValue("@statusorder", "PENDING");
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    hitungtotal();
                    tampildata();
                    MessageBox.Show("Data berhasil ditambahkan", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "0";
            textBox5.Text = "0";
            textBox6.Text = "0";
            textBox7.Text = "0";
            richTextBox1.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells["nomortelepon"].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells["biayajemput"].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells["biayaantar"].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells["biayahari"].Value.ToString();
            comboBox1.SelectedValue = dataGridView1.CurrentRow.Cells["petugasantar"].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["tanggalorder"].Value.ToString());
            dateTimePicker2.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["tanggalselesai"].Value.ToString());
            DateTime order = dateTimePicker1.Value;
            DateTime selesai = dateTimePicker2.Value;

            TimeSpan selisih = selesai - order;
            int lamahari = selisih.Days;
            textBox3.Text = lamahari.ToString();

            if (lamahari > 3)
            {
                SqlCommand cmd = new SqlCommand("SELECT biaya FROM BiayaTambahan WHERE kodebiaya = 1", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    textBox6.Text = dr["biaya"].ToString();
                }
                conn.Close();
            }


            if (textBox4.Text != "0")
            {
                checkBox1.Checked = true;
            }
            else if (textBox5.Text != "0")
            {
                checkBox2.Checked = true;
            }
            hitungtotal();

            SqlCommand command = new SqlCommand("SELECT * FROM Pelanggan WHERE nomortelepon = @nomortelepon", conn);
            command.CommandType = CommandType.Text;
            conn.Open();
            command.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                string nama = reader["nama"].ToString();
                string alamat = reader["alamat"].ToString();
                TampilkanPelanggan(nama, alamat);
            }
            conn.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommand pelanggan = new SqlCommand("SELECT COUNT (*) FROM Pelanggan WHERE nomortelepon = @nomortelepon", conn);
                pelanggan.CommandType = CommandType.Text;
                conn.Open();
                pelanggan.Parameters.AddWithValue("@nomortelepon", textBox1.Text);

                int pelanggantidakada = (int)pelanggan.ExecuteScalar();
                conn.Close();
                if (pelanggantidakada == 0)
                {
                    SqlCommand tambahpelanggan = new SqlCommand("INSERT INTO Pelanggan VALUES(@nomortelepon,@nama, @alamat)", conn);
                    tambahpelanggan.CommandType = CommandType.Text;
                    conn.Open();
                    tambahpelanggan.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                    tambahpelanggan.Parameters.AddWithValue("@nama", textBox2.Text);
                    tambahpelanggan.Parameters.AddWithValue("@alamat", richTextBox1.Text);
                    tambahpelanggan.ExecuteNonQuery();
                    conn.Close();
                }

                var row = dataGridView1.CurrentRow;
                int kodeorder = Convert.ToInt32(row.Cells["kodeorder"].Value.ToString());
                SqlCommand cmd = new SqlCommand("UPDATE [Order] SET nomortelepon = @nomortelepon, tanggalorder = @tanggalorder, tanggalselesai = @tanggalselesai, biayaantar = @biayaantar, biayajemput = @biayajemput, biayahari = @biayahari, petugasantar = @petugasantar  WHERE kodeorder = @kodeorder ", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@kodeorder", kodeorder);
                cmd.Parameters.AddWithValue("@nomortelepon", textBox1.Text);
                cmd.Parameters.AddWithValue("@tanggalorder", dateTimePicker1.Value);
                cmd.Parameters.AddWithValue("@tanggalselesai", dateTimePicker2.Value);
                cmd.Parameters.AddWithValue("@biayaantar", textBox5.Text);
                cmd.Parameters.AddWithValue("@biayajemput", textBox4.Text);
                cmd.Parameters.AddWithValue("@biayahari", textBox6.Text);
                cmd.Parameters.AddWithValue("@petugasantar", comboBox1.SelectedValue);
                cmd.ExecuteNonQuery();
                conn.Close();
                tampildata();
                MessageBox.Show("Data berhasil diubah", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
                hitungtotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show("Apakah anda yakin ingin membatalkan pesanan?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (konfirmasi == DialogResult.Yes)
            {
                clear();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dataGridView1.CurrentRow;
            string kodeorder = row.Cells["kodeorder"].Value.ToString();

            Detailorder d = new Detailorder(kodeorder);
            d.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show("Apakah anda yakin ingin menghapus data?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (konfirmasi == DialogResult.Yes)
            {
                var row = dataGridView1.CurrentRow;
                int kodeorder = Convert.ToInt32(row.Cells["kodeorder"].Value.ToString());
                SqlCommand cmd = new SqlCommand("DELETE FROM [Order] WHERE kodeorder = @kodeorder ", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@kodeorder", kodeorder);
                cmd.ExecuteNonQuery();
                conn.Close();
                tampildata();
                MessageBox.Show("Data berhasil dihapus", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox8.Text;
            if (!string.IsNullOrEmpty(keyword))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM [Order] WHERE kodeorder LIKE @keyword", conn);
                cmd.CommandType = CommandType.Text;
                conn.Open();
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                DataTable dt = new DataTable();
                SqlDataReader dr = cmd.ExecuteReader();
                dt.Load(dr);
                conn.Close();

                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                if (dt.Rows.Count > 0)
                {
                    DataGridViewComboBoxColumn combo = new DataGridViewComboBoxColumn();
                    combo.Name = "Status";
                    combo.DataSource = new string[] { "PENDING", "DICUCI", "DIANTAR/DIJEMPUT" };
                    combo.HeaderText = "Status";
                    combo.DataPropertyName = "Status";

                    DataGridViewLinkColumn link = new DataGridViewLinkColumn();
                    link.Name = "Pilih Layanan";
                    link.Text = "Pilih Layanan";
                    link.HeaderText = "Pilih Layanan";
                    link.UseColumnTextForLinkValue = true;

                    dataGridView1.Columns.Add("kodeorder", "kodeorder");
                    dataGridView1.Columns.Add("nomortelepon", "nomortelepon");
                    dataGridView1.Columns.Add("tanggalorder", "tanggalorder");
                    dataGridView1.Columns.Add("tanggalselesai", "tanggalselesai");
                    dataGridView1.Columns.Add("biayaantar", "biayaantar");
                    dataGridView1.Columns.Add("biayajemput", "biayajemput");
                    dataGridView1.Columns.Add("biayahari", "biayahari");
                    dataGridView1.Columns.Add("petugasantar", "petugasantar");
                    dataGridView1.Columns.Add(combo);
                    dataGridView1.Columns.Add(link);

                    foreach (DataRow row in dt.Rows)
                    {
                        int rowIndex = dataGridView1.Rows.Add();
                        dataGridView1.Rows[rowIndex].Cells["kodeorder"].Value = row["kodeorder"].ToString();
                        dataGridView1.Rows[rowIndex].Cells["nomortelepon"].Value = row["nomortelepon"].ToString();
                        dataGridView1.Rows[rowIndex].Cells["tanggalorder"].Value = row["tanggalorder"].ToString();
                        dataGridView1.Rows[rowIndex].Cells["tanggalselesai"].Value = row["tanggalselesai"].ToString();
                        dataGridView1.Rows[rowIndex].Cells["biayaantar"].Value = row["biayaantar"].ToString();
                        dataGridView1.Rows[rowIndex].Cells["biayajemput"].Value = row["biayajemput"].ToString();
                        dataGridView1.Rows[rowIndex].Cells["biayahari"].Value = row["biayahari"].ToString();
                        dataGridView1.Rows[rowIndex].Cells["petugasantar"].Value = row["petugasantar"].ToString();
                        dataGridView1.Rows[rowIndex].Cells["Status"].Value = row["statusorder"].ToString();


                        dataGridView1.EditingControlShowing += DataGridView1_EditingControlShowing;
                    }



                } 


            }
            else 
            {
                tampildata();
            }
        }
    }
}

