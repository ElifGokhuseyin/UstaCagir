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
    public partial class MusteriKayit : Form
    {
        private MySQLbaglanti db = new MySQLbaglanti();

        public MusteriKayit()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Label3 click event
        }

        private void kayıtmusteributton1_Click(object sender, EventArgs e)
        {
            // Kayıt Ol butonuna tıklandığında
            string ad = adıtextBox1.Text.Trim();
            string soyad = soyadıtextBox2.Text.Trim();
            string telefon = maskedTextBox1.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
            string email = emailtextBox4.Text.Trim();
            string adres = adrestextBox5.Text.Trim();
            string sifre = textBox1.Text.Trim(); // Şifre textBox1'den alınıyor

            // Alanların dolu olup olmadığını kontrol et
            if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad) ||
                string.IsNullOrEmpty(telefon) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(adres) || string.IsNullOrEmpty(sifre))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz!");
                return;
            }

            // Email formatını kontrol et
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Geçerli bir email adresi giriniz!");
                return;
            }

            // Telefon uzunluğunu kontrol et
            if (telefon.Length != 10)
            {
                MessageBox.Show("Telefon numarası 10 haneli olmalıdır!");
                return;
            }
            if (sifre.Length < 6)
            {
                MessageBox.Show("Şifre en az 6 karakter olmalıdır!");
                return;
            }

            // Müşteriyi veritabanına kaydet - şifre parametresi eklendi
            bool kayitBasarili = db.MusteriEkle(ad, soyad, telefon, email, adres, sifre);

            if (kayitBasarili)
            {
                MessageBox.Show("Kayıt başarıyla tamamlandı! Giriş yapabilirsiniz.");

                // Kayıt başarılı, giriş formuna dön
                MusteriGiris musteriGirisForm = new MusteriGiris();
                musteriGirisForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kayıt sırasında bir hata oluştu!");
            }
        }

        private void MusteriKayit_Load(object sender, EventArgs e)
        {

        }
    }
}