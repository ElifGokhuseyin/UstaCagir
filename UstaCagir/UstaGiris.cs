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
    public partial class UstaGiris : Form
    {
        private MySQLbaglanti db = new MySQLbaglanti();
        public UstaGiris()
        {
            InitializeComponent();
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

            // Usta giriş kontrolü
            DataRow usta = db.UstaGirisKontrol(email, sifre);

            if (usta != null)
            {
                // Giriş başarılı, UstaDetay formuna git
                UstaDetay ustaDetayForm = new UstaDetay(usta);
                ustaDetayForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Email veya şifre hatalı!");
            }
        }

     

        private void UstaGiris_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
