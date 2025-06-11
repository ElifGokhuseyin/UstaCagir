using MySql.Data.MySqlClient;
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
    public partial class MusteriDetay : Form
    {
        private MySQLbaglanti db = new MySQLbaglanti();
        private DataRow musteriData;
        private int musteriId;

        public MusteriDetay()
        {
            InitializeComponent();
        }

        public MusteriDetay(DataRow musteri)
        {
            InitializeComponent();
            this.musteriData = musteri;
            this.musteriId = Convert.ToInt32(musteri["Musteri_ID"]);

            // Event'leri manuel olarak bağla
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            hizmetCombo.SelectedIndexChanged += hizmetCombo_SelectedIndexChanged;

            MusteriBilgileriniYukle();
        }

        private void MusteriBilgileriniYukle()
        {
            if (musteriData != null)
            {
                adıtextBox1.Text = musteriData["Adı"].ToString();
                soyadıtextBox2.Text = musteriData["Soyadı"].ToString();
                maskedTextBox1.Text = musteriData["Telefon"].ToString();
                emailtextBox4.Text = musteriData["Email"].ToString();
                adrestextBox5.Text = musteriData["Adres"].ToString();
                sifretextBox3.Text = musteriData["Musteri_Sifre"].ToString();
            }
        }

        private void MusteriDetay_Load(object sender, EventArgs e)
        {
            // Form yüklendiğinde kategorileri, ustaları ve randevuları doldur
            KategorileriDoldur();
            UstalariDoldur();
            RandevulariYukle();
            DegerlendirmeleriYukle(); // Değerlendirmeleri de yükle
            DegerlendirmePuanlariniDoldur(); // Puan seçeneklerini doldur
        }

        private void KategorileriDoldur()
        {
            hizmetCombo.Items.Clear();
            hizmetCombo.Items.Add("Kategori Seçiniz...");
            hizmetCombo.Items.AddRange(new string[] { "Elektrikçi", "Tesisatçı", "Boyacı", "Temizlikçi", "Bahçıvan" });
            hizmetCombo.SelectedIndex = 0;
        }

        // Değerlendirme puanlarını doldur
        private void DegerlendirmePuanlariniDoldur()
        {
            comboBox3.Items.Clear();
            comboBox3.Items.Add("Puan Seçiniz...");
            for (int i = 1; i <= 5; i++)
            {
                comboBox3.Items.Add(i.ToString());
            }
            comboBox3.SelectedIndex = 0;
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        // Değerlendirme Yap butonu
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                // DataGridView'de seçilen randevu var mı kontrol et
                if (dataGridView1.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Lütfen değerlendirme yapmak istediğiniz randevuyu seçiniz!");
                    return;
                }

               
                if (comboBox3.SelectedIndex <= 0 || string.IsNullOrEmpty(textBox1.Text.Trim()))
                {
                    MessageBox.Show("Lütfen puan seçin ve yorum yazın!");
                    return;
                }

               
                DataRowView selectedRow = (DataRowView)dataGridView1.SelectedRows[0].DataBoundItem;
                int randevuId = Convert.ToInt32(selectedRow["Randevu_ID"]);

                int puan = Convert.ToInt32(comboBox3.SelectedItem.ToString());
                string yorum = textBox1.Text.Trim();

                bool basarili = db.DegerlendirmeEkle(puan, yorum, DateTime.Now, randevuId);

                if (basarili)
                {
                    MessageBox.Show("Değerlendirme başarıyla eklendi!");
                    comboBox3.SelectedIndex = 0;
                    textBox1.Clear();
                    DegerlendirmeleriYukle(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Değerlendirme hatası: " + ex.Message);
            }
        }

        private void kayıtmusteributton1_Click(object sender, EventArgs e)
        {
            // Bilgileri Güncelle butonuna tıklandığında
            string ad = adıtextBox1.Text.Trim();
            string soyad = soyadıtextBox2.Text.Trim();
            string telefon = maskedTextBox1.Text.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
            string email = emailtextBox4.Text.Trim();
            string adres = adrestextBox5.Text.Trim();
            string sifre = sifretextBox3.Text.Trim();

            
            if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad) ||
                string.IsNullOrEmpty(telefon) || string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(adres))
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz!");
                return;
            }

            
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

            
            bool guncellemeBaşarili = db.MusteriBilgiGuncelle(musteriId, ad, soyad, telefon, email, adres, sifre);

            if (guncellemeBaşarili)
            {
                MessageBox.Show("Bilgiler başarıyla güncellendi!");

                // Güncel bilgileri yeniden yükle
                DataRow guncelMusteri = db.MusteriGetirById(musteriId);
                if (guncelMusteri != null)
                {
                    this.musteriData = guncelMusteri;
                    MusteriBilgileriniYukle();
                }
            }
            else
            {
                MessageBox.Show("Bilgiler güncellenirken bir hata oluştu!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Randevu Oluştur butonuna tıklandığında
            if (ValidateRandevuFields())
            {
                CreateRandevu();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            DialogResult result = MessageBox.Show("Hesabınızı silmek istediğinizden emin misiniz?",
                                                "Hesap Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                bool basarili = db.MusteriSil(musteriId);

                if (basarili)
                {
                    MessageBox.Show("Hesap başarıyla silindi!");
                    this.Hide();

                    
                    Form1 anaForm = new Form1();
                    anaForm.Show();
                }
                else
                {
                    MessageBox.Show("Hesap silinirken hata oluştu!");
                }
            }
        }
        //  Değerlendirme Sil butonu
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Lütfen silmek istediğiniz değerlendirmeyi seçiniz!");
                    return;
                }

                DialogResult result = MessageBox.Show("Seçilen değerlendirmeyi silmek istediğinizden emin misiniz?",
                                                    "Değerlendirme Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    
                    DataRowView selectedRow = (DataRowView)dataGridView2.SelectedRows[0].DataBoundItem;
                    int degerlendirmeId = Convert.ToInt32(selectedRow["Degerlendirme_ID"]);

                    bool basarili = db.DegerlendirmeSil(degerlendirmeId);

                    if (basarili)
                    {
                        MessageBox.Show("Değerlendirme başarıyla silindi!");
                        DegerlendirmeleriYukle(); 
                    }
                    else
                    {
                        MessageBox.Show("Değerlendirme silinirken hata oluştu!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Değerlendirme silme hatası: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Usta Seç ComboBox değiştiğinde
        }

        // Kategori seçildiğinde (hizmetCombo)
        private void hizmetCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (hizmetCombo.SelectedIndex > 0) 
                {
                    string selectedKategori = hizmetCombo.SelectedItem.ToString();

                    // Seçilen kategoriye göre hizmetleri doldur
                    HizmetleriDoldur(selectedKategori);

                    // Kategoriye göre ustaları doldur
                    UstalariKategoriyeGoreDoldur(selectedKategori);
                }
                else
                {
                    // Hizmet ve usta ComboBox'larını temizle
                    comboBox2.Items.Clear();
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("Usta Seçiniz...");
                    comboBox1.SelectedIndex = 0;
                    SetTutarField(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori seçimi hatası: " + ex.Message);
            }
        }

        // Hizmet seçildiğinde (comboBox2)
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedIndex > 0 && comboBox2.SelectedItem is HizmetComboBoxItem selectedHizmet)
                {
                    SetTutarField(selectedHizmet.Fiyat);
                }
                else
                {
                    SetTutarField(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hizmet seçimi hatası: " + ex.Message);
            }
        }

        // Kategoriye göre hizmetleri doldur
        private void HizmetleriDoldur(string kategori)
        {
            try
            {
                DataTable hizmetler = db.HizmetleriKategoriyeGoreGetir(kategori);

                comboBox2.Items.Clear();
                comboBox2.Items.Add("Hizmet Seçiniz...");

                if (hizmetler != null && hizmetler.Rows.Count > 0)
                {
                    foreach (DataRow row in hizmetler.Rows)
                    {
                        string hizmetText = $"{row["Hizmet_Adi"]} - {row["Fiyat"]} TL ({row["Süre_Birimi"]})";

                        HizmetComboBoxItem item = new HizmetComboBoxItem
                        {
                            Text = hizmetText,
                            HizmetId = Convert.ToInt32(row["Hizmet_ID"]),
                            Fiyat = Convert.ToInt32(row["Fiyat"])
                        };

                        comboBox2.Items.Add(item);
                    }
                }
                else
                {
                    comboBox2.Items.Add("Bu kategoride hizmet bulunamadı");
                }

                comboBox2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hizmet listesi doldurma hatası: " + ex.Message);
            }
        }

        // Kategoriye göre ustaları doldur
        private void UstalariKategoriyeGoreDoldur(string kategori)
        {
            try
            {
                DataTable ustalar = db.UstalariKategoriyeGoreGetir(kategori);

                comboBox1.Items.Clear();
                comboBox1.Items.Add("Usta Seçiniz...");

                if (ustalar != null && ustalar.Rows.Count > 0)
                {
                    foreach (DataRow row in ustalar.Rows)
                    {
                        ComboBoxItem item = new ComboBoxItem
                        {
                            Text = $"{row["Usta_Adı"]} {row["Usta_Soyadı"]}",
                            Value = row["Usta_ID"].ToString()
                        };
                        comboBox1.Items.Add(item);
                    }
                }
                else
                {
                    comboBox1.Items.Add("Bu kategoride usta bulunamadı");
                }

                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Usta listesi doldurma hatası: " + ex.Message);
            }
        }

        // Usta listesini ComboBox'a doldur
        private void UstalariDoldur()
        {
            try
            {
                DataTable ustalar = db.UstalariGetir();

                if (ustalar != null && ustalar.Rows.Count > 0)
                {
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("Usta Seçiniz...");

                    foreach (DataRow row in ustalar.Rows)
                    {
                        string ustaText = $"{row["Usta_Adı"]} {row["Usta_Soyadı"]}";

                        ComboBoxItem item = new ComboBoxItem
                        {
                            Text = ustaText,
                            Value = row["Usta_ID"].ToString()
                        };

                        comboBox1.Items.Add(item);
                    }

                    comboBox1.SelectedIndex = 0;
                    comboBox1.DisplayMember = "Text";
                    comboBox1.ValueMember = "Value";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Usta listesi doldurma hatası: " + ex.Message);
            }
        }

        // Hizmete göre ustaları doldur
        private void UstalariHizmetineGoreDoldur(string hizmet)
        {
            try
            {
                DataTable ustalar = db.UstalariKategoriyeGoreGetir(hizmet);

                if (ustalar != null && ustalar.Rows.Count > 0)
                {
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("Usta Seçiniz...");

                    foreach (DataRow row in ustalar.Rows)
                    {
                        string ustaText = $"{row["Usta_Adı"]} {row["Usta_Soyadı"]}";

                        ComboBoxItem item = new ComboBoxItem
                        {
                            Text = ustaText,
                            Value = row["Usta_ID"].ToString()
                        };

                        comboBox1.Items.Add(item);
                    }

                    comboBox1.SelectedIndex = 0;
                    comboBox1.DisplayMember = "Text";
                    comboBox1.ValueMember = "Value";
                }
                else
                {
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("Bu kategoride usta bulunamadı");
                    comboBox1.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hizmet bazlı usta listesi doldurma hatası: " + ex.Message);
            }
        }

        // Hizmet için rastgele tutar belirle
        private int GetHizmetTutari(string hizmet)
        {
            Random rand = new Random();

            switch (hizmet.ToLower())
            {
                case "elektrikçi":
                    return rand.Next(500, 3500);
                case "tesisatçı":
                    return rand.Next(800, 4500);
                case "boyacı":
                    return rand.Next(1000, 6000);
                case "temizlikçi":
                    return rand.Next(300, 2000);
                case "bahçıvan":
                    return rand.Next(600, 3000);
                default:
                    return rand.Next(500, 2500);
            }
        }

        // Tutar alanını doldur
        private void SetTutarField(int tutar)
        {
            try
            {
                string tutarText = tutar > 0 ? tutar.ToString() + " TL" : "";

                // textBox2 tutar alanı olarak kullanılıyor
                if (textBox2 != null)
                {
                    textBox2.Text = tutarText;
                    return;
                }

                
                foreach (Control control in this.Controls)
                {
                    if (control is GroupBox groupBox)
                    {
                        foreach (Control subControl in groupBox.Controls)
                        {
                            if (subControl is TextBox textBox &&
                                (subControl.Name.ToLower().Contains("tutar") ||
                                 subControl.TabIndex > 10))
                            {
                                textBox.Text = tutarText;
                                return;
                            }
                        }
                    }
                }

                //  MessageBox ile göster
                if (tutar > 0)
                {
                    MessageBox.Show($"Tahmini Tutar: {tutarText}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Tutar alanı güncellenirken hata: " + ex.Message);
            }
        }

        // Randevu alanlarını doğrula
        private bool ValidateRandevuFields()
        {
            if (hizmetCombo.SelectedIndex <= 0)
            {
                MessageBox.Show("Lütfen bir kategori seçiniz!");
                return false;
            }

            if (comboBox2.SelectedIndex <= 0 || !(comboBox2.SelectedItem is HizmetComboBoxItem))
            {
                MessageBox.Show("Lütfen bir hizmet seçiniz!");
                return false;
            }

            if (comboBox1.SelectedIndex <= 0)
            {
                MessageBox.Show("Lütfen bir usta seçiniz!");
                return false;
            }

            return true;
        }

        // Randevu oluştur
        private void CreateRandevu()
        {
            try
            {
                // Validasyon
                if (!ValidateRandevuFields())
                    return;

                // Seçilen usta ID'sini al
                ComboBoxItem selectedUsta = (ComboBoxItem)comboBox1.SelectedItem;
                int ustaId = Convert.ToInt32(selectedUsta.Value);

                // Seçilen hizmet ID'sini al
                HizmetComboBoxItem selectedHizmet = (HizmetComboBoxItem)comboBox2.SelectedItem;
                int hizmetId = selectedHizmet.HizmetId;
                int tutar = selectedHizmet.Fiyat;

                // Tarih ve saat al
                DateTime tarih = dateTimePicker1.Value.Date;
                TimeSpan saat = TimeSpan.Parse(dateTimePicker2.Value.ToString("HH:mm"));

                // Ödeme türü al
                string odemeTuru = comboBox4.SelectedItem?.ToString() ?? "Nakit";
                string aciklama = textBox3.Text.Trim();

                // Randevu oluştur
                bool randevuBasarili = db.RandevuEkle(tarih, saat, musteriId, ustaId, hizmetId);

                if (randevuBasarili)
                {
                    // Randevu ID'sini al (son eklenen randevu)
                    int randevuId = GetLastInsertedRandevuId();

                    if (randevuId > 0)
                    {
                        // Ödeme bilgisini ekle
                        bool odemeBasarili = db.OdemeEkle(randevuId, DateTime.Now, odemeTuru, aciklama, tutar);

                        if (odemeBasarili)
                        {
                            MessageBox.Show("Randevu ve ödeme bilgisi başarıyla oluşturuldu!");
                        }
                        else
                        {
                            MessageBox.Show("Randevu oluşturuldu ancak ödeme bilgisi eklenirken hata oluştu!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Randevu oluşturuldu ancak ödeme bilgisi eklenemedi!");
                    }

                    // Alanları temizle
                    hizmetCombo.SelectedIndex = 0;
                    comboBox1.Items.Clear();
                    comboBox1.Items.Add("Usta Seçiniz...");
                    comboBox1.SelectedIndex = 0;
                    comboBox2.Items.Clear();
                    comboBox4.SelectedIndex = 0;
                    textBox3.Clear();
                    SetTutarField(0);

                    // Randevu listesini yenile
                    RandevulariYukle();
                }
                else
                {
                    MessageBox.Show("Randevu oluşturulurken bir hata oluştu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Randevu oluşturma hatası: " + ex.Message);
            }
        }

        // Son eklenen randevu ID'sini al
        private int GetLastInsertedRandevuId()
        {
            try
            {
                using (MySqlConnection conn = db.baglanti())
                {
                    if (conn == null) return 0;

                    string query = @"SELECT Randevu_ID FROM randevular 
                           WHERE Musteri_ID = @musteriId 
                           ORDER BY Randevu_ID DESC LIMIT 1";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@musteriId", musteriId);
                        var result = cmd.ExecuteScalar();
                        return result != null ? Convert.ToInt32(result) : 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Randevu ID alma hatası: " + ex.Message);
                return 0;
            }
        }

        // Müşterinin randevularını DataGridView'e yükle
        private void RandevulariYukle()
        {
            try
            {
                DataTable randevular = db.MusteriRandevulariGetir(musteriId);

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
                        if (dataGridView1.Columns.Contains("Usta"))
                        {
                            dataGridView1.Columns["Usta"].HeaderText = "Usta";
                            dataGridView1.Columns["Usta"].Width = 120;
                        }
                        if (dataGridView1.Columns.Contains("Açıklama"))
                        {
                            dataGridView1.Columns["Açıklama"].HeaderText = "Açıklama";
                            dataGridView1.Columns["Açıklama"].Width = 150;
                        }
                    }
                }
                else
                {
                    dataGridView1.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Randevu listesi yüklenirken hata: " + ex.Message);
            }
        }

        //  Değerlendirmeleri yükle
        private void DegerlendirmeleriYukle()
        {
            try
            {
                DataTable degerlendirmeler = db.MusteriDegerlendirmeleriniGetir(musteriId);

                if (degerlendirmeler != null)
                {
                    dataGridView2.DataSource = degerlendirmeler;

                    // Kolon başlıklarını ayarla
                    if (dataGridView2.Columns.Count > 0)
                    {
                        // ID kolonlarını gizle
                        if (dataGridView2.Columns.Contains("Degerlendirme_ID"))
                            dataGridView2.Columns["Degerlendirme_ID"].Visible = false;
                        if (dataGridView2.Columns.Contains("Randevu_ID"))
                            dataGridView2.Columns["Randevu_ID"].Visible = false;

                        // Görünür kolonları ayarla
                        if (dataGridView2.Columns.Contains("Puan"))
                        {
                            dataGridView2.Columns["Puan"].HeaderText = "Puan";
                            dataGridView2.Columns["Puan"].Width = 60;
                        }
                        if (dataGridView2.Columns.Contains("Yorum"))
                        {
                            dataGridView2.Columns["Yorum"].HeaderText = "Yorum";
                            dataGridView2.Columns["Yorum"].Width = 200;
                        }
                        if (dataGridView2.Columns.Contains("Tarih"))
                        {
                            dataGridView2.Columns["Tarih"].HeaderText = "Değerlendirme Tarihi";
                            dataGridView2.Columns["Tarih"].Width = 120;
                        }
                        if (dataGridView2.Columns.Contains("Usta"))
                        {
                            dataGridView2.Columns["Usta"].HeaderText = "Usta";
                            dataGridView2.Columns["Usta"].Width = 120;
                        }
                        if (dataGridView2.Columns.Contains("Randevu_Tarihi"))
                        {
                            dataGridView2.Columns["Randevu_Tarihi"].HeaderText = "Randevu Tarihi";
                            dataGridView2.Columns["Randevu_Tarihi"].Width = 100;
                        }
                    }
                }
                else
                {
                    dataGridView2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Değerlendirme listesi yüklenirken hata: " + ex.Message);
            }
        }

        // Seçilen ustanın ID'sini almak için
        private string GetSelectedUstaId()
        {
            if (comboBox1.SelectedIndex > 0)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox1.SelectedItem;
                return selectedItem.Value;
            }
            return null;
        }

        //  RANDEVU SİL METODU
        private void button3_Click(object sender, EventArgs e)
        {
            // Randevu Sil butonuna tıklandığında
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Seçilen randevuyu silmek istediğinizden emin misiniz?",
                                                    "Randevu Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // DataBoundItem'dan randevu ID'sini al
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Randevu silme hatası: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz randevuyu seçiniz!");
            }
        }

        private void güncellegroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Seçilen randevuyu silmek istediğinizden emin misiniz?",
                                                    "Randevu Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // DataBoundItem'dan randevu ID'sini al
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
                    catch (Exception ex)
                    {
                        MessageBox.Show("Randevu silme hatası: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek istediğiniz randevuyu seçiniz!");
            }
        }
    }


    // ComboBoxItem helper class'ı
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    // Hizmet ComboBox için helper class
    public class HizmetComboBoxItem
    {
        public string Text { get; set; }
        public int HizmetId { get; set; }
        public int Fiyat { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}