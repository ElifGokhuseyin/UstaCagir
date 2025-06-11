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
    public partial class UstaDetay : Form
    {
        private MySQLbaglanti db = new MySQLbaglanti();
        private DataRow ustaData;
        private int ustaId;

        public UstaDetay()
        {
            InitializeComponent();
        }

        public UstaDetay(DataRow usta)
        {
            InitializeComponent();
            this.ustaData = usta;
            this.ustaId = Convert.ToInt32(usta["Usta_ID"]);
            UstaBilgileriniYukle();
        }

        private void UstaDetay_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde önce kategorileri doldur
            KategorileriDoldur();

            // Sonra usta bilgilerini yükle (bu kategoriyi seçecek)
            UstaBilgileriniYukle();

            // Hizmetleri doldur
            HizmetleriDoldur();

            // Randevuları yükle
            RandevulariYukle();

            // ComboBox event'lerini bağla
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }

        private void UstaBilgileriniYukle()
        {
            if (ustaData != null)
            {
                adıtextBox1.Text = ustaData["Usta_Adı"].ToString();
                soyadıtextBox2.Text = ustaData["Usta_Soyadı"].ToString();
                maskedTextBox1.Text = ustaData["Usta_Telefon"].ToString();
                emailtextBox4.Text = ustaData["Usta_Email"].ToString();
                adrestextBox5.Text = ustaData["Usta_Adres"].ToString();

                // Kategori ComboBox'ını ayarla
                string kategori = ustaData["Usta_Kategori"].ToString();

                // Kategori listesinde arama yap
                bool kategoriVarMi = false;
                for (int i = 0; i < comboBox1.Items.Count; i++)
                {
                    if (comboBox1.Items[i].ToString() == kategori)
                    {
                        comboBox1.SelectedIndex = i;
                        kategoriVarMi = true;
                        break;
                    }
                }

                // Eğer kategori bulunamazsa ilk elemanı seç
                if (!kategoriVarMi && comboBox1.Items.Count > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
            }
        }

        private void KategorileriDoldur()
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(new string[]
            {
                "Elektrikçi",
                "Tesisatçı",
                "Boyacı",
                "Temizlikçi",
                "Bahçıvan"
            });
        }

        // YENİ - Hizmetleri doldur - ARTIK SADECE BİLGİLENDİRME AMAÇLI
        private void HizmetleriDoldur()
        {
            // Bu metod artık hiçbir şey yapmıyor çünkü comboBox2 kaldırıldı
            // Sadece eski kodların çalışması için boş bırakıldı
        }

        // YENİ - Kategori değiştiğinde hizmetleri filtrele
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex >= 0)
                {
                    string selectedKategori = comboBox1.SelectedItem.ToString();
                    HizmetleriFiltreEt(selectedKategori);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori seçimi hatası: " + ex.Message);
            }
        }

        // YENİ - Kategoriye göre hizmetleri filtrele - ARTIK KULLANILMIYOR
        private void HizmetleriFiltreEt(string kategori)
        {
            // Bu metod artık hiçbir şey yapmıyor çünkü comboBox2 kaldırıldı
            // Sadece eski kodların çalışması için boş bırakıldı
        }

        // DÜZELTİLMİŞ - Bilgileri Güncelle butonu
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string ad = adıtextBox1.Text.Trim();
                string soyad = soyadıtextBox2.Text.Trim();
                string telefon = maskedTextBox1.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                string email = emailtextBox4.Text.Trim();
                string adres = adrestextBox5.Text.Trim();
                string kategori = comboBox1.SelectedItem?.ToString() ?? "";

                // Alanların dolu olup olmadığını kontrol et
                if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad) ||
                    string.IsNullOrEmpty(telefon) || string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(adres) || string.IsNullOrEmpty(kategori))
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

                // Usta bilgilerini güncelle
                bool guncellemeBaşarili = db.UstaBilgiGuncelle(ustaId, ad, soyad, telefon, email, adres, kategori);

                if (guncellemeBaşarili)
                {
                    MessageBox.Show("Bilgiler başarıyla güncellendi!");

                    // Güncel bilgileri yeniden yükle
                    DataRow guncelUsta = db.UstaGetirById(ustaId);
                    if (guncelUsta != null)
                    {
                        this.ustaData = guncelUsta;
                        UstaBilgileriniYukle();
                    }
                }
                else
                {
                    MessageBox.Show("Bilgiler güncellenirken bir hata oluştu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message);
            }
        }

        // DÜZELTİLMİŞ - Hizmet Güncelle butonu - SADECE KATEGORİ GÜNCELLEMESİ
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Kategori kontrolü
                if (comboBox1.SelectedIndex < 0 || comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Lütfen kategori seçiniz!");
                    return;
                }

                // Mevcut usta bilgilerini kontrol et
                if (ustaData == null)
                {
                    MessageBox.Show("Usta bilgileri yüklenemedi!");
                    return;
                }

                string yeniKategori = comboBox1.SelectedItem.ToString();

                // Mevcut bilgileri al
                string mevcutAd = ustaData["Usta_Adı"]?.ToString() ?? "";
                string mevcutSoyad = ustaData["Usta_Soyadı"]?.ToString() ?? "";
                string mevcutTelefon = ustaData["Usta_Telefon"]?.ToString() ?? "";
                string mevcutEmail = ustaData["Usta_Email"]?.ToString() ?? "";
                string mevcutAdres = ustaData["Usta_Adres"]?.ToString() ?? "";

                // Bilgilerin boş olmadığını kontrol et
                if (string.IsNullOrEmpty(mevcutAd) || string.IsNullOrEmpty(mevcutSoyad) ||
                    string.IsNullOrEmpty(mevcutEmail))
                {
                    MessageBox.Show("Usta bilgilerinde eksiklik var! Lütfen önce kişisel bilgileri güncelleyin.");
                    return;
                }

                // Ustanın kategorisini güncelle
                bool guncellemeBaşarili = db.UstaBilgiGuncelle(
                    ustaId,
                    mevcutAd,
                    mevcutSoyad,
                    mevcutTelefon,
                    mevcutEmail,
                    mevcutAdres,
                    yeniKategori
                );

                if (guncellemeBaşarili)
                {
                    MessageBox.Show($"Kategori başarıyla güncellendi!\n\nYeni Kategori: {yeniKategori}");

                    // Güncel bilgileri yeniden yükle
                    DataRow guncelUsta = db.UstaGetirById(ustaId);
                    if (guncelUsta != null)
                    {
                        this.ustaData = guncelUsta;
                        UstaBilgileriniYukle();
                    }
                }
                else
                {
                    MessageBox.Show("Kategori güncellenirken bir hata oluştu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Kategori güncelleme hatası: {ex.Message}\n\nDetay: {ex.StackTrace}");
            }
        }

        // Randevu Sil butonu
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Seçilen randevuyu silmek istediğinizden emin misiniz?",
                                                        "Randevu Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // DataBoundItem kullanarak güvenli bir şekilde ID al
                        DataRowView selectedRow = (DataRowView)dataGridView1.SelectedRows[0].DataBoundItem;
                        int randevuId = Convert.ToInt32(selectedRow["Randevu_ID"]);

                        bool basarili = db.RandevuSil(randevuId);

                        if (basarili)
                        {
                            MessageBox.Show("Randevu başarıyla silindi!");
                            RandevulariYukle(); // Listeyi yenile
                        }
                        else
                        {
                            MessageBox.Show("Randevu silinirken hata oluştu!");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen silmek istediğiniz randevuyu seçiniz!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Randevu silme hatası: " + ex.Message);
            }
        }

        // Hesabı Sil butonu
        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Hesabınızı silmek istediğinizden emin misiniz?",
                                                "Hesap Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                bool basarili = db.UstaSil(ustaId);

                if (basarili)
                {
                    MessageBox.Show("Hesap başarıyla silindi!");
                    this.Hide();

                    // Ana forma dön
                    Form1 anaForm = new Form1();
                    anaForm.Show();
                }
                else
                {
                    MessageBox.Show("Hesap silinirken hata oluştu!");
                }
            }
        }

        // Ustanın randevularını DataGridView'e yükle
        private void RandevulariYukle()
        {
            try
            {
                DataTable randevular = db.UstaRandevulariGetir(ustaId);

                if (randevular != null)
                {
                    dataGridView1.DataSource = randevular;

                    // Kolon başlıklarını ayarla
                    if (dataGridView1.Columns.Count > 0)
                    {
                        // ID kolonlarını gizle
                        if (dataGridView1.Columns.Contains("Randevu_ID"))
                            dataGridView1.Columns["Randevu_ID"].Visible = false;
                        if (dataGridView1.Columns.Contains("Musteri_ID"))
                            dataGridView1.Columns["Musteri_ID"].Visible = false;
                        if (dataGridView1.Columns.Contains("Usta_ID"))
                            dataGridView1.Columns["Usta_ID"].Visible = false;

                        // Görünür kolonları ayarla
                        if (dataGridView1.Columns.Contains("Tarih"))
                        {
                            dataGridView1.Columns["Tarih"].HeaderText = "Tarih";
                            dataGridView1.Columns["Tarih"].Width = 100;
                        }
                        if (dataGridView1.Columns.Contains("Saat"))
                        {
                            dataGridView1.Columns["Saat"].HeaderText = "Saat";
                            dataGridView1.Columns["Saat"].Width = 80;
                        }
                        if (dataGridView1.Columns.Contains("Müşteri"))
                        {
                            dataGridView1.Columns["Müşteri"].HeaderText = "Müşteri";
                            dataGridView1.Columns["Müşteri"].Width = 120;
                        }
                        if (dataGridView1.Columns.Contains("Hizmet"))
                        {
                            dataGridView1.Columns["Hizmet"].HeaderText = "Hizmet";
                            dataGridView1.Columns["Hizmet"].Width = 120;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Randevu listesi yüklenirken hata: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // DataGridView cell click eventi
        }
    }
}