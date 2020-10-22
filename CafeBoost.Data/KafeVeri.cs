using System.Collections.Generic;

namespace CafeBoost.Data
{
    public class KafeVeri
    {
        public List<Urun> Urunler { get; set; }
        public List<Siparis> AktifSiparisler { get; set; }
        public List<Siparis> GecmisSiparisler { get; set; }
    }
}
