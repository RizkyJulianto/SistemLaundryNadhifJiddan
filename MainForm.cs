using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaundryNadhifJiddan
{
    public partial class MainForm : Form
    {
        public MainForm(string namauser)
        {
            InitializeComponent();

            if(namauser == "KASIR" ) { 
                lOGINToolStripMenuItem.Enabled = false;
                dATAToolStripMenuItem.Enabled = false;
                bIAYATAMBAHANToolStripMenuItem.Enabled = false;
            } else if(namauser == "ADMIN")
            {
                lOGINToolStripMenuItem.Enabled = false;
            }
        }

        private void lOGOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show("Apakah anda yakin ingin logout?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(konfirmasi == DialogResult.Yes)
            {
                Login l = new Login();
                l.Show();
                this.Hide();
            }
        }

        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var konfirmasi = MessageBox.Show("Apakah anda yakin ingin keluar?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (konfirmasi == DialogResult.Yes)
            {
               Application.Exit();
            }
        }

        private void pETUGASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PetugasAntar l = new PetugasAntar();
            l.ShowDialog();
        }

        private void pELANGGANToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Pelanggan l = new Pelanggan();
            l.ShowDialog();
        }

        private void lAYANANToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Layanan l = new Layanan();
            l.ShowDialog();
        }

        private void bIAYATAMBAHANToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BiayaTambahan l = new BiayaTambahan();
            l.ShowDialog();
        }

        private void oRDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Order l = new Order();
            l.ShowDialog();
        }

        private void dETAILORDERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataDetailorder l = new DataDetailorder();
            l.ShowDialog();
        }

        private void uSERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataUser d = new DataUser();
            d.ShowDialog();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void rEPORTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report d = new Report();
            d.ShowDialog();
        }

        private void tAMBAHORDERBAERToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
