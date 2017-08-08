using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewMvcBlog.Models;
using System.Web.Helpers;
using System.IO;

namespace NewMvcBlog.Controllers
{
    public class UyeController : Controller
    {
        MVCBlogDb db = new MVCBlogDb();
        // GET: Uye
        public ActionResult Index(int id)
        {
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();

            if (Convert.ToInt32(Session["uyeId"]) != uye.UyeId)
            {
                return HttpNotFound();
            }
            return View(uye);
        }
        [HttpPost]
        public ActionResult Login(Uye uye)
        {
            var login = db.Uyes.Where(u => u.KullaniciAdi == uye.KullaniciAdi).SingleOrDefault();
            if (login.KullaniciAdi == uye.KullaniciAdi && login.Email == uye.Email && login.Sifre == uye.Sifre)
            {
                Session["uyeId"] = login.UyeId;
                Session["kullaniciadi"] = login.KullaniciAdi;
                Session["yetkiId"] = login.YetkiId;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Uyari = ("Kullanıcı adı,mail ya da şifrenizi kontrol ediniz...");
                return View();
            }

        }
        public ActionResult Logout()
        {
            Session["uyeId"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Uye uye, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newfoto);
                    uye.Foto = "/Uploads/UyeFoto/" + newfoto;
                    uye.YetkiId = 2;
                    db.Uyes.Add(uye);
                    db.SaveChanges();
                    Session["uyeId"] = uye.UyeId;
                    Session["kullaniciAdi"] = uye.KullaniciAdi;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz");
                }
            }
            return View(uye);
        }
       
       
        public ActionResult Edit(int id)
        {
            var uye = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();

            if (Convert.ToInt32(Session["uyeId"]) != uye.UyeId)
            {
                return HttpNotFound();
            }

            return View(uye);
        }
        [HttpPost]
        public ActionResult Edit(Uye uye,int id,HttpPostedFileBase Foto)
        {
            if(ModelState.IsValid)
            {
                var uyes = db.Uyes.Where(u => u.UyeId == id).SingleOrDefault();
                if(Foto!=null)
                {
                    if (System.IO.File.Exists(Server.MapPath(uye.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(uyes.Foto));

                    }

                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoInfo = new FileInfo(Foto.FileName);
                    string newfoto = Guid.NewGuid().ToString() + fotoInfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Uploads/UyeFoto/" + newfoto);
                    uyes.Foto = "/Uploads/UyeFoto/" + newfoto;
                }
                uyes.AdSoyad = uye.AdSoyad;
                    uyes.KullaniciAdi = uye.KullaniciAdi;
                    uyes.Sifre = uye.Sifre;
                    uyes.Email = uye.Email;
                    db.SaveChanges();
                    Session["kullaniciAdi"] = uye.KullaniciAdi;
                    return RedirectToAction("Index", "Home",new { id = uyes.UyeId });
                

            }
            return View();
        }
    }
}