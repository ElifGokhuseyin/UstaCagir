using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace UstaCagir
{
    internal class MySQLbaglanti
    {
        private string connectionString = "Server=localhost;Database=ustacagir;Uid=root;Pwd=1234";

        public MySqlConnection baglanti()
        {
            try
            {
                MySqlConnection baglan = new MySqlConnection(connectionString);
                baglan.Open();
                return baglan;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message);
                return null;
            }
        }

        // Müşteri kaydetme metodu
        public bool MusteriEkle(string ad, string soyad, string telefon, string email, string adres, string sifre)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    using (MySqlCommand cmd = new MySqlCommand("CALL MusteriEkle(@ad, @soyad, @telefon, @email, @adres, @sifre)", conn))
                    {
                        cmd.Parameters.AddWithValue("@ad", ad);
                        cmd.Parameters.AddWithValue("@soyad", soyad);
                        cmd.Parameters.AddWithValue("@telefon", telefon);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@adres", adres);
                        cmd.Parameters.AddWithValue("@sifre", sifre);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri kaydetme hatası: " + ex.Message);
                return false;
            }
        }

        // Müşteri giriş kontrolü
        public DataRow MusteriGirisKontrol(string email, string sifre)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query = "SELECT * FROM Müşteriler WHERE Email = @email AND Musteri_Sifre = @sifre";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@sifre", sifre);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Giriş kontrolü hatası: " + ex.Message);
                return null;
            }
        }

        
        public DataRow UstaGirisKontrol(string email, string sifre)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query = "SELECT * FROM Ustalar WHERE Usta_Email = @email AND Usta_Sifre = @sifre";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@sifre", sifre);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Usta giriş kontrolü hatası: " + ex.Message);
                return null;
            }
        }

        
        public bool MusteriBilgiGuncelle(int musteriId, string ad, string soyad, string telefon, string email, string adres, string sifre)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    using (MySqlCommand cmd = new MySqlCommand("CALL MusteriGuncelle(@id, @ad, @soyad, @telefon, @email, @adres,@sifre)", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", musteriId);
                        cmd.Parameters.AddWithValue("@ad", ad);
                        cmd.Parameters.AddWithValue("@soyad", soyad);
                        cmd.Parameters.AddWithValue("@telefon", telefon);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@adres", adres);
                        cmd.Parameters.AddWithValue("@sifre", sifre);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri güncelleme hatası: " + ex.Message);
                return false;
            }
        }

       
        public DataTable HizmetleriGetir()
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    
                    string query = "SELECT * FROM Hizmetler ORDER BY Kategori, Hizmet_Adi";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hizmetler getirme hatası: " + ex.Message);
                return null;
            }
        }

       
        public DataTable HizmetleriKategoriyeGoreGetir(string kategori)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                     
                    string query = "SELECT * FROM Hizmetler WHERE Kategori = @kategori ORDER BY Hizmet_Adi";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kategori", kategori);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori bazlı hizmet listesi getirme hatası: " + ex.Message);
                return null;
            }
        }

       
        public DataTable UstalariGetir()
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query = "SELECT Usta_ID, Usta_Adı, Usta_Soyadı, Usta_Kategori FROM Ustalar ORDER BY Usta_Adı";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Usta listesi getirme hatası: " + ex.Message);
                return null;
            }
        }

        public DataTable UstalariKategoriyeGoreGetir(string kategori)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query = "SELECT Usta_ID, Usta_Adı, Usta_Soyadı, Usta_Kategori FROM Ustalar WHERE Usta_Kategori = @kategori ORDER BY Usta_Adı";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@kategori", kategori);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kategori bazlı usta listesi getirme hatası: " + ex.Message);
                return null;
            }
        }

        
        public bool RandevuEkle(DateTime tarih, TimeSpan saat, int musteriId, int ustaId, int hizmetId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    string query = @"INSERT INTO randevular (Tarih, Saat, Musteri_ID, Usta_ID, Hizmet_ID) 
                                   VALUES (@tarih, @saat, @musteriId, @ustaId, @hizmetId)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tarih", tarih.Date);
                        cmd.Parameters.AddWithValue("@saat", saat);
                        cmd.Parameters.AddWithValue("@musteriId", musteriId);
                        cmd.Parameters.AddWithValue("@ustaId", ustaId);
                        cmd.Parameters.AddWithValue("@hizmetId", hizmetId);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Randevu ekleme hatası: " + ex.Message);
                return false;
            }
        }

         
        public DataTable MusteriRandevulariGetir(int musteriId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query = @"SELECT 
                                r.Randevu_ID,
                                DATE_FORMAT(r.Tarih, '%d.%m.%Y') AS Tarih,
                                TIME_FORMAT(r.Saat, '%H:%i') AS Saat,
                                CONCAT(IFNULL(u.Usta_Adı, ''), ' ', IFNULL(u.Usta_Soyadı, '')) AS Usta,
                                IFNULL(h.Hizmet_Adi, IFNULL(u.Usta_Kategori, 'Genel Hizmet')) AS Açıklama,
                                r.Musteri_ID,
                                r.Usta_ID
                            FROM randevular r
                            LEFT JOIN ustalar u ON r.Usta_ID = u.Usta_ID
                            LEFT JOIN hizmetler h ON r.Hizmet_ID = h.Hizmet_ID
                            WHERE r.Musteri_ID = @musteriId
                            ORDER BY r.Tarih DESC, r.Saat DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@musteriId", musteriId);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri randevuları getirme hatası: " + ex.Message);
                return null;
            }
        }

        
        public DataTable UstaRandevulariGetir(int ustaId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query= @"SELECT 
                                r.Randevu_ID,
                                DATE_FORMAT(r.Tarih, '%d.%m.%Y') AS Tarih,
                                TIME_FORMAT(r.Saat, '%H:%i') AS Saat,
                                CONCAT(m.Adı, ' ', m.Soyadı) AS Müşteri,
                                u.Usta_Kategori AS Hizmet,
                                r.Musteri_ID,
                                r.Usta_ID
                            FROM randevular r
                            INNER JOIN müşteriler m ON r.Musteri_ID = m.Musteri_ID
                            INNER JOIN ustalar u ON r.Usta_ID = u.Usta_ID
                            WHERE r.Usta_ID = @ustaId
                            ORDER BY r.Tarih DESC, r.Saat DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ustaId", ustaId);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Usta randevuları getirme hatası: {ex.Message}");
                return null;
            }
        }

        
        public bool OdemeEkle(int randevuId, DateTime odemeTarihi, string odemeTuru, string aciklama, int tutar)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    using (MySqlCommand cmd = new MySqlCommand("CALL OdemeEkle(@randevuId, @odemeTarihi, @odemeTuru, @aciklama, @tutar)", conn))
                    {
                        cmd.Parameters.AddWithValue("@randevuId", randevuId);
                        cmd.Parameters.AddWithValue("@odemeTarihi", odemeTarihi.Date);
                        cmd.Parameters.AddWithValue("@odemeTuru", odemeTuru);
                        cmd.Parameters.AddWithValue("@aciklama", aciklama);
                        cmd.Parameters.AddWithValue("@tutar", tutar);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ödeme ekleme hatası: " + ex.Message);
                return false;
            }
        }

        
        public bool DegerlendirmeEkle(int puan, string yorum, DateTime tarih, int randevuId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    string randevuKontrolQuery = "SELECT COUNT(*) FROM Randevular WHERE Randevu_ID = @randevuId";
                    using (MySqlCommand kontrolCmd = new MySqlCommand(randevuKontrolQuery, conn))
                    {
                        kontrolCmd.Parameters.AddWithValue("@randevuId", randevuId);
                        int randevuVar = Convert.ToInt32(kontrolCmd.ExecuteScalar());

                        if (randevuVar == 0)
                        {
                            MessageBox.Show("Geçersiz randevu ID'si! Lütfen geçerli bir randevu seçiniz.");
                            return false;
                        }
                    }

                    
                    string degerlendirmeKontrolQuery = "SELECT COUNT(*) FROM Değerlendirmeler WHERE Randevu_ID = @randevuId";
                    using (MySqlCommand kontrolCmd = new MySqlCommand(degerlendirmeKontrolQuery, conn))
                    {
                        kontrolCmd.Parameters.AddWithValue("@randevuId", randevuId);
                        int degerlendirmeVar = Convert.ToInt32(kontrolCmd.ExecuteScalar());

                        if (degerlendirmeVar > 0)
                        {
                            MessageBox.Show("Bu randevu için zaten değerlendirme yapılmış!");
                            return false;
                        }
                    }

                     
                    string query = @"INSERT INTO Değerlendirmeler (Puan, Yorum, Degerlendirme_Tarih, Randevu_ID) 
                           VALUES (@puan, @yorum, @tarih, @randevuId)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@puan", puan);
                        cmd.Parameters.AddWithValue("@yorum", yorum);
                        cmd.Parameters.AddWithValue("@tarih", tarih.Date);
                        cmd.Parameters.AddWithValue("@randevuId", randevuId);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Değerlendirme ekleme hatası: " + ex.Message);
                return false;
            }
        }

        
        public DataTable MusteriDegerlendirmeleriniGetir(int musteriId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query = @"SELECT 
                                d.Degerlendirme_ID,
                                d.Puan,
                                d.Yorum,
                                DATE_FORMAT(d.Degerlendirme_Tarih, '%d.%m.%Y') AS Tarih,
                                CONCAT(u.Usta_Adı, ' ', u.Usta_Soyadı) AS Usta,
                                DATE_FORMAT(r.Tarih, '%d.%m.%Y') AS Randevu_Tarihi,
                                d.Randevu_ID
                            FROM Değerlendirmeler d
                            INNER JOIN Randevular r ON d.Randevu_ID = r.Randevu_ID
                            INNER JOIN Ustalar u ON r.Usta_ID = u.Usta_ID
                            WHERE r.Musteri_ID = @musteriId
                            ORDER BY d.Degerlendirme_Tarih DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@musteriId", musteriId);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Değerlendirme listesi getirme hatası: " + ex.Message);
                return null;
            }
        }

        
        public bool DegerlendirmeSil(int degerlendirmeId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    string query = "DELETE FROM Değerlendirmeler WHERE Degerlendirme_ID = @degerlendirmeId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@degerlendirmeId", degerlendirmeId);
                        int etkilenenSatir = cmd.ExecuteNonQuery();
                        return etkilenenSatir > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Değerlendirme silme hatası: " + ex.Message);
                return false;
            }
        }

        
        public DataRow MusteriGetirById(int musteriId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query = "SELECT * FROM Müşteriler WHERE Musteri_ID = @musteriId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@musteriId", musteriId);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri bilgisi getirme hatası: " + ex.Message);
                return null;
            }
        }

        

        public DataRow UstaGetirById(int ustaId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return null;

                    string query = "SELECT * FROM Ustalar WHERE Usta_ID = @ustaId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ustaId", ustaId);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                            return dt.Rows[0];
                        else
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Usta bilgisi getirme hatası: " + ex.Message);
                return null;
            }
        }

        
        public bool RandevuSil(int randevuId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    string query = "DELETE FROM Randevular WHERE Randevu_ID = @randevuId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@randevuId", randevuId);

                        int etkilenenSatir = cmd.ExecuteNonQuery();
                        return etkilenenSatir > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Randevu silme hatası: {ex.Message}");
                return false;
            }
        }

        
        public bool MusteriSil(int musteriId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    using (MySqlCommand cmd = new MySqlCommand("CALL MusteriSil(@id)", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", musteriId);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Müşteri silme hatası: " + ex.Message);
                return false;
            }
        }

        
        public bool UstaSil(int ustaId)
        {
            try
            {
                using (MySqlConnection conn = baglanti())
                {
                    if (conn == null) return false;

                    string query = "DELETE FROM ustalar WHERE Usta_ID = @ustaId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ustaId", ustaId);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Usta silme hatası: " + ex.Message);
                return false;
            }
        }

        
       

         
        public bool UstaBilgiGuncelle(int ustaId, string ad, string soyad, string telefon, string email, string adres, string Usta_Kategori)
        {
            MySqlConnection conn = null;
            try
            {
                conn = baglanti();
                if (conn == null)
                {
                    MessageBox.Show("Veritabanı bağlantısı kurulamadı!");
                    return false;
                }

               
                if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad) ||
                    string.IsNullOrEmpty(telefon) || string.IsNullOrEmpty(email) ||
                    string.IsNullOrEmpty(adres) || string.IsNullOrEmpty(Usta_Kategori))
                {
                    MessageBox.Show("Tüm alanlar dolu olmalıdır!");
                    return false;
                }

                
                string kontrolQuery = "SELECT COUNT(*) FROM Ustalar WHERE Usta_ID = @ustaId";
                using (MySqlCommand kontrolCmd = new MySqlCommand(kontrolQuery, conn))
                {
                    kontrolCmd.Parameters.AddWithValue("@ustaId", ustaId);
                    int ustaVar = Convert.ToInt32(kontrolCmd.ExecuteScalar());

                    if (ustaVar == 0)
                    {
                        MessageBox.Show("Usta bulunamadı!");
                        return false;
                    }
                }

               
                string emailKontrolQuery = "SELECT COUNT(*) FROM Ustalar WHERE Usta_Email = @email AND Usta_ID != @ustaId";
                using (MySqlCommand emailCmd = new MySqlCommand(emailKontrolQuery, conn))
                {
                    emailCmd.Parameters.AddWithValue("@email", email);
                    emailCmd.Parameters.AddWithValue("@ustaId", ustaId);

                    int emailSayisi = Convert.ToInt32(emailCmd.ExecuteScalar());

                    if (emailSayisi > 0)
                    {
                        MessageBox.Show("Bu email adresi başka bir usta tarafından kullanılmaktadır!");
                        return false;
                    }
                }

                
                string updateQuery = @"UPDATE Ustalar SET 
                              Usta_Adı = @ad, 
                              Usta_Soyadı = @soyad, 
                              Usta_Telefon = @telefon, 
                              Usta_Email = @email, 
                              Usta_Adres = @adres, 
                              Usta_Kategori = @kategori 
                              WHERE Usta_ID = @id";

                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@id", ustaId);
                    updateCmd.Parameters.AddWithValue("@ad", ad);
                    updateCmd.Parameters.AddWithValue("@soyad", soyad);
                    updateCmd.Parameters.AddWithValue("@telefon", telefon);
                    updateCmd.Parameters.AddWithValue("@email", email);
                    updateCmd.Parameters.AddWithValue("@adres", adres);
                    updateCmd.Parameters.AddWithValue("@kategori", Usta_Kategori);

                    int etkilenenSatir = updateCmd.ExecuteNonQuery();

                    if (etkilenenSatir > 0)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme işlemi başarısız!");
                        return false;
                    }
                }
            }
            catch (MySqlException mysqlEx)
            {
                MessageBox.Show($"MySQL Hatası: {mysqlEx.Message}\nHata Kodu: {mysqlEx.Number}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Genel Hata: {ex.Message}\n\nDetay: {ex.StackTrace}");
                return false;
            }
            finally
            {
               


                try
                {
                    if (conn != null && conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                catch (Exception closeEx)
                {
                    MessageBox.Show($"Bağlantı kapatma hatası: {closeEx.Message}");
                }
            }
        }
    }
}