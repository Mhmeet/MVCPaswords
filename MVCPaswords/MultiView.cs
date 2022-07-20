using MVCPaswords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCPaswords
{
    public class MultiView
    {
        public IEnumerable<KayitTalep> kayitTList { get; set; }
        public IEnumerable<Sifre> sifreList { get; set; }
        public IEnumerable<Kategori_Talep> KategoriTList { get; set; }
        public IEnumerable<Kategori> Kategori { get; set; }
        public IEnumerable<Kullanici> kullanicilar { get; set; }
        public IEnumerable<Yetki> Yetki { get; set; }
    }
}