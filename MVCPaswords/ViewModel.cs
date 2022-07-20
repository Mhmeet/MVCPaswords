using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCPaswords.Models;

namespace MVCPaswords.Models
{
    public class ViewModel
    {
        public List<KayitTalep> kayitTList { get; set; }
        public List<Sifre> sifreList { get; set; }
        public List<Kategori> kategoriList { get; set; }

    }
}