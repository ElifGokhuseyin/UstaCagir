using System;
using System.Windows.Forms;

namespace UstaCagir
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Usta Giri� butonuna t�kland���nda
            UstaGiris ustaGirisForm = new UstaGiris();
            ustaGirisForm.Show();
            this.Hide();
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            // M��teri Giri� butonuna t�kland���nda
            try
            {
                MusteriGiris musteriGirisForm = new MusteriGiris();
                musteriGirisForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}


