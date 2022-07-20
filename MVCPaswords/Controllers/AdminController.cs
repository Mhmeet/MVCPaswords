using MVCPaswords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPaswords.Controllers
{
    [LoginControl]
    public class AdminController : Controller
    {
        // GET: Admin

        Entities db = new Entities();
        public ActionResult Index()
        {
            MultiView vm = new MultiView();
            //vm.sifreList = db.Sifres.ToList();
            vm.kayitTList = db.KayitTaleps.Where(x => x.durum == true).ToList();
            vm.KategoriTList = db.Kategori_Talep.Where(x => x.durum == true).ToList();


            return View(vm);
        }
        public ActionResult KullaniciList()
        {
            MultiView vm = new MultiView();
            vm.Yetki = db.Yetkis.ToList();
            vm.kullanicilar = db.Kullanicis.ToList();
            vm.kayitTList = db.KayitTaleps.Where(x => x.durum == true).ToList();
            vm.KategoriTList = db.Kategori_Talep.Where(x => x.durum == true).ToList();
            return View(vm);

        }
       

        [HttpPost]
        public JsonResult KayitTalep(int id)
        {
            MailGonderme mg = new MailGonderme();
            var durum = 0;
            Guid g = Guid.NewGuid();
            var guidvar = g.ToString();
            Kullanici kullanici = new Kullanici();
            var talepkisi = db.KayitTaleps.FirstOrDefault(x => x.ID.Equals(id));
            if (talepkisi != null)
            {
                kullanici.Mail = talepkisi.mail;
                kullanici.IsimSoyisim = talepkisi.isim_soyisim;
                kullanici.Token = guidvar;
                talepkisi.durum = false;
                kullanici.RolID = 2;
                kullanici.KayıtTarih = DateTime.Now;
                db.Kullanicis.Add(kullanici);
                durum = db.SaveChanges();
                string s = "Kaydınızı tamamlamak için linke tıklayarak yeni şifrenizi oluşturabilirsiniz. ";
                if (durum > 0)
                {
                    mg.Gonder("Oksitem Onay", s + "Link: https://localhost:44382/Login/YeniSifre/" + guidvar);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult Secmece()
        {
            MultiView vm = new MultiView();
            vm.kullanicilar = db.Kullanicis.ToList();
            vm.Kategori = db.Kategoris.ToList();
            vm.kayitTList = db.KayitTaleps.Where(x => x.durum == true).ToList();
            vm.KategoriTList = db.Kategori_Talep.Where(x => x.durum == true).ToList();
            return View(vm);
        }

        [HttpPost]
        public JsonResult Secmece(int kulid, int kattid)
        {
            var sifres = db.Sifres.Where(k => k.Kullanici_ID.Equals(kulid) && k.Kategori_ID.Equals(kattid)).FirstOrDefault();
            if (sifres != null)
            {
                return Json(new
                {
                    Result = new
                    {
                        isimsoyisimler = sifres.Kullanici.IsimSoyisim,
                        sifres.KayıtliSifre,
                        sifres.Aciklama,
                        kategoriisim = sifres.Kategori.Kategori_Isim
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DetayKullanici(int idkull)
        {

            var temp = db.Kullanicis.Where(x => x.ID.Equals(idkull)).FirstOrDefault();

            if (temp != null)
            {
                return Json(new
                {
                    Values = new
                    {
                        temp.IsimSoyisim,
                        temp.Mail,
                        temp.Sifre,
                        temp.RolID
                    }
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult kulguncelle(Kullanici kul)
        {
            var durum = 0;
            var temp = db.Kullanicis.Where(x => x.ID.Equals(kul.ID)).FirstOrDefault();
            if (temp != null && kul.RolID != 0 && !kul.Mail.Contains("@oksitem.com") && !string.IsNullOrEmpty(kul.IsimSoyisim))
            {
                temp.IsimSoyisim = kul.IsimSoyisim;
                temp.Mail = kul.Mail;
                if (!string.IsNullOrEmpty(kul.Sifre) && kul.Sifre.Length > 5)
                {
                    temp.Sifre = kul.Sifre;
                   
                }
                temp.RolID = kul.RolID;
                durum = db.SaveChanges();
            }
            if (durum > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult KullaniciSifres(int id)
        {
            MultiView mv = new MultiView();
            mv.kayitTList = db.KayitTaleps.Where(x => x.durum == true).ToList();
            mv.KategoriTList = db.Kategori_Talep.Where(x => x.durum == true).ToList();
            mv.sifreList = db.Sifres.Where(x => x.Kullanici_ID.Equals(id)).ToList();
            mv.Kategori = db.Kategoris.ToList();
            return View(mv);
        }
        [HttpPost]
        public JsonResult SifDetail(int id)
        {
            var temp = db.Sifres.Where(s => s.ID == id).FirstOrDefault();
            return Json(new
            {
                Result = new
                {
                    temp.KayıtliSifre,
                    temp.Aciklama,
                    temp.Kategori_ID
                }
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult sifGuncelle(Sifre sif)
        {
            bool varmi = true;

            int sess = (int)Session["UserID"];
            var kul = db.Sifres.Where(x => x.Kullanici_ID.Equals(sess)).ToList();

            var temp = db.Sifres.FirstOrDefault(x => x.ID == sif.ID);
            for (int i = 0; i < kul.Count; i++)
            {
                if (kul[i].Kategori_ID == sif.Kategori_ID)
                {
                    varmi = false;
                }
            }
            if (sif.Kategori_ID > 0)
            {
                temp.KayıtliSifre = sif.KayıtliSifre;
                temp.Aciklama = sif.Aciklama;
                if (varmi == true)
                {
                    temp.Kategori_ID = sif.Kategori_ID;
                }
                db.SaveChanges();
                return (Json(true, JsonRequestBehavior.AllowGet));

            }
            else
            {
                return (Json(false, JsonRequestBehavior.AllowGet));
            }


        }
        [HttpPost]
        public JsonResult SifreSil(int silid)
        {
            var durum = 0;
            int sesid = (int)Session["UserID"];
            var temp = db.Sifres.Where(s => s.ID == silid).FirstOrDefault();
            if (durum == 0)
            {
                db.Sifres.Remove(temp);
                durum = db.SaveChanges();
                if (durum > 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return (Json(false, JsonRequestBehavior.AllowGet));
                }
            }
            else
            {
                return (Json(false, JsonRequestBehavior.AllowGet));
            }
        }
        public ActionResult KullaniciSifreEkle()
        {
            MultiView mv = new MultiView();
            mv.Kategori = db.Kategoris.ToList();
            mv.kullanicilar = db.Kullanicis.Where(x => x.RolID==2).ToList();
            return View(mv);
        }
        [HttpPost]
        public JsonResult KullaniciSifreEkle(int id, string sifre, string ack, int katid)
        {
            
            var temp = db.Sifres.Where(x => x.Kullanici_ID.Equals(id)).ToList();
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i].Kategori_ID == katid)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            Sifre s = new Sifre();
            if (!string.IsNullOrEmpty(sifre) && katid > 0)
            {
                s.Kullanici_ID = id;
                s.Aciklama = ack;
                s.Durum = true;
                s.Kategori_ID = katid;
                s.KayıtliSifre = sifre;                  /*security.SHA512Crypto(sifre)*/
                db.Sifres.Add(s);
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
    }
}