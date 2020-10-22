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
        private readonly KafeVeri db;
        private readonly Siparis siparis;
        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            //Constructor parametresi olarak gelen bu nesneleri daha sonra da erişebileceğimiz fieldlara aktarıyoruz.
            db = kafeVeri;
            this.siparis = siparis;
            InitializeComponent();
            MasaNoGuncelle();
        }

        private void MasaNoGuncelle()
        {
            Text = $"Masa {siparis.MasaNo:00} - Sipariş Detayları (Açılış: {siparis.AcilisZamani.Value.ToShortTimeString()})";
            lblMasaNo.Text = siparis.MasaNo.ToString("00");

        }
    }
}
