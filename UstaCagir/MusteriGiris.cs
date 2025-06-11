using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UstaCagir
{
    public partial class MusteriGiris : Form
    {
        private MySQLbaglanti db = new MySQLbaglanti();

        public MusteriGiris()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Şifre textbox değişikliği
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Giriş Yap butonuna tıklandığında
            string email = giristxtMail.Text.Trim();
            string sifre = textBox1.Text.Trim(); // Şifre olarak telefon kullanıyoruz

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Email ve şifre alanlarını doldurunuz!");
                return;
            }

            // Müşteri giriş kontrolü
            DataRow musteri = db.MusteriGirisKontrol(email, sifre);

            if (musteri != null)
            {
                // Giriş başarılı, MusteriDetay formuna git
                MusteriDetay musteriDetayForm = new MusteriDetay(musteri);
                musteriDetayForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email veya şifre hatalı!");
            }
        }

        
        private void MusteriGiris_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                MusteriKayit musteriKayitForm = new MusteriKayit();
                musteriKayitForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
    }
}
