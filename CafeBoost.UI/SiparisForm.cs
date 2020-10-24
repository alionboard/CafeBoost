using CafeBoost.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeBoost.UI
{
    public partial class SiparisForm : Form
    {
        public event EventHandler<MasaTasimaEventArgs> MasaTasindi;
        private readonly KafeVeri db;
        private readonly Siparis siparis;
        private readonly BindingList<SiparisDetay> blSiparisDetaylar;
        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            //Constructor parametresi olarak gelen bu nesneleri, daha sonra da erişebileceğimiz fieldlara aktarıyoruz.
            db = kafeVeri;
            this.siparis = siparis;
            InitializeComponent();
            dgvSiparisDetaylar.AutoGenerateColumns = false;
            MasalariListele();//Taşıma alanındaki masaları listeleme
            UrunleriListele();
            MasaNoGuncelle();
            OdemeTutarıGuncelle();

            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            blSiparisDetaylar.ListChanged += BlSiparisDetaylar_ListChanged;
            dgvSiparisDetaylar.DataSource = blSiparisDetaylar;
        }

        private void MasalariListele()
        {
            cboMasalar.Items.Clear();
            for (int i = 1; i <= db.MasaAdet; i++)
            {
                if (!db.AktifSiparisler.Any(x => x.MasaNo == i))
                {
                    cboMasalar.Items.Add(i);
                }
            }
        }


        private void BlSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            OdemeTutarıGuncelle();
        }

        private void OdemeTutarıGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
        }

        private void UrunleriListele()
        {
            cboUrun.DataSource = db.Urunler;
        }

        private void MasaNoGuncelle()
        {
            Text = $"Masa {siparis.MasaNo:00} - Sipariş Detayları (Açılış: {siparis.AcilisZamani.Value.ToShortTimeString()})";
            lblMasaNo.Text = siparis.MasaNo.ToString("00");
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            Urun secilenUrun = (Urun)cboUrun.SelectedItem;
            int adet = (int)nudAdet.Value;

            #region YeniYerineAdeteEklemeYapma
            //SiparisDetay detay = blSiparisDetaylar.FirstOrDefault(x => x.UrunAd == secilenUrun.UrunAd);

            //if (detay!=null)
            //{
            //    detay.Adet += adet;
            //    blSiparisDetaylar.ResetBindings();
            //}
            //else
            //{
            //    detay = new SiparisDetay()
            //    {
            //        UrunAd = secilenUrun.UrunAd,
            //        BirimFiyat = secilenUrun.BirimFiyat,
            //        Adet = adet
            //    };
            //    blSiparisDetaylar.Add(detay);
            //} 
            #endregion

            SiparisDetay detay = new SiparisDetay()
            {
                UrunAd = secilenUrun.UrunAd,
                BirimFiyat = secilenUrun.BirimFiyat,
                Adet = adet
            };
            blSiparisDetaylar.Add(detay);

        }

        private void DgvSiparisDetaylar_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seçili detayları silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void BtnAnasayfa_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void BtnSiparisIptal_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Sipariş iptal edilerek kapatılacaktır. Emin misiniz?", "İptal Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Iptal);
            }
        }

        private void BtnOdemeAl_Click(object sender, EventArgs e)
        {

            DialogResult dr = MessageBox.Show("Ödeme alındıysa sipariş kapatılacaktır. Emin misin?", "Ödeme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr == DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Odendi, siparis.ToplamTutar());
            }
        }

        private void SiparisKapat(SiparisDurum siparisDurum, decimal odenenTutar = 0)
        {
            siparis.OdenenTutar = odenenTutar;
            siparis.KapanisZamani = DateTime.Now;
            siparis.Durum = siparisDurum;
            db.AktifSiparisler.Remove(siparis);
            db.GecmisSiparisler.Add(siparis);
            DialogResult = DialogResult.OK;
            Close();

        }

        private void BtnMasaTasi_Click(object sender, EventArgs e)
        {
            if (cboMasalar.SelectedIndex < 0) return;
            int kaynak = siparis.MasaNo;
            int hedef = (int)cboMasalar.SelectedItem;
            siparis.MasaNo = hedef;
            MasaNoGuncelle();
            MasalariListele();

            MasaTasimaEventArgs args = new MasaTasimaEventArgs()
            {
                EskiMasaNo = kaynak,
                YeniMasaNo = hedef
            };
            MasaTasindiginda(args);
        }

        protected virtual void MasaTasindiginda(MasaTasimaEventArgs args)
        {
            MasaTasindi?.Invoke(this, args);
        }
    }
}
