using MVCPaswords.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCPaswords.Security;

namespace MVCPaswords.Controllers
{
    public class LoginController : Controller
    {

        // GET: AdminPassword
        Entities db = new Entities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Kullanici kullanici)
        {

            var dbkullanici = db.Kullanicis.FirstOrDefault(x => x.Mail == kullanici.Mail && x.Sifre == kullanici.Sifre);
            if (dbkullanici != null)
            {
                if (kullanici.Mail.Contains("@oksitem.com"))
                {
                    Session["UserID"] = dbkullanici.ID;
                    dbkullanici.SonGiris = DateTime.Now;
                    db.SaveChanges();
                    if (dbkullanici.RolID == 1)
                    {
                        return RedirectToAction("Index", "admin");
                    }
                    else if (dbkullanici.RolID == 2)
                    {
                        return RedirectToAction("Index", "user");
                    }
                }
            }
            else
            {
                ViewBag.Mesaj = "deneme@oksitem.com";
                return View();

            }
            ViewBag.Mesaj = "Kullanıcı Adı veya Şifre Hatalı";
            return View();

        }
        public ActionResult YeniKayit()
        {
            return View();
        }
        [HttpPost]
        public JsonResult YeniKayit(string i, string s, string m, string ack)
        {

            var durum = 0;
            KayitTalep kt = new KayitTalep();
            var mailvarMi = db.Kullanicis.Where(x => x.Mail.Equals(m)).ToList();
            if (m.Contains("@oksitem.com"))
            {
                if (mailvarMi.Count == 0)
                {
                    kt.isim_soyisim = i + " " + s;
                    kt.mail = m;

                    kt.aciklama = ack;
                    kt.durum = true;
                    db.KayitTaleps.Add(kt);
                    durum = db.SaveChanges();

                }
                else
                {
                    return Json(3, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(4, JsonRequestBehavior.AllowGet);
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
        [HttpGet]
        public ActionResult YeniSifre()
        {
            string token = null;
            string[] splitli = null;
            string currenturl = Request.Url.ToString();
            splitli = currenturl.Split('/');
            token = splitli[5];
            var temp = db.Kullanicis.FirstOrDefault(x => x.Token == token);

            return View();

        }
        [HttpPost]
        public JsonResult YeniSifre(string sifre)
        {
            var durum = 0;
            string token = null;
            string[] splitli = null;
            string currenturl = Request.Url.ToString();
            splitli = currenturl.Split('/');
            token = splitli[5];
            var temp = db.Kullanicis.FirstOrDefault(x => x.Token == token);
            temp.Token = Guid.NewGuid().ToString();
            temp.Sifre = sifre;
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

    }
}
