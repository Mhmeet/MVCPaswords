using MVCPaswords.Models;
using MVCPaswords.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCPaswords.Controllers
{
    [LoginControl]
    public class UserController : Controller
    {
        // GET: User
        Entities db = new Entities();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult KayitliSifre()
        {
            int sesid = (int)Session["UserID"];
            MultiView mv = new MultiView();

            mv.sifreList = db.Sifres.Where(x => x.Kullanici_ID == sesid).ToList();
            mv.Kategori = db.Kategoris.ToList();
            return View(mv);
        }
        [HttpPost]
        public JsonResult KategoriEkle(string a, string v)
        {
            var durum = 0;
            int id = (int)Session["UserID"];
            Kategori_Talep newkat = new Kategori_Talep();
            newkat.kullanici_ID = id;
            newkat.Isim = a;
            newkat.Aciklama = v;
            newkat.durum = true;
            db.Kategori_Talep.Add(newkat);
            durum = db.SaveChanges();
            if (durum > 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult SifreEkle()
        {
            var kategorilist = db.Kategoris.ToList();
            return View(kategorilist);
        }
        [HttpPost]
        public JsonResult SifreEkle(string sifre, string ack, int katid)
        {
            int sess = (int)Session["UserID"];
            var temp = db.Sifres.Where(x => x.Kullanici_ID.Equals(sess)).ToList();
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
                s.Kullanici_ID = sess;
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
            if (temp.Kullanici_ID == sesid)
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
    }
}