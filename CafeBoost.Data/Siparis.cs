using System;
using System.Collections.Generic;

namespace CafeBoost.Data
{
    public class Siparis
    {
        public int MasaNo { get; set; }
        public List<SiparisDetay> SiparisDetaylar { get; set; }
        public DateTime? AcilisZamani { get; set; }
        public DateTime? KapanisZamani { get; set; }
        public SiparisDurum Durum { get; set; }
        public string ToplamTutarTl => string.Format("{0:00}TL",ToplamTutar());
        private decimal ToplamTutar()
        {
            decimal toplam = 0;
            foreach (var item in SiparisDetaylar)
            {
                toplam += decimal.Parse(item.TutarTL);
            }
            return toplam;
        }
    }
}
