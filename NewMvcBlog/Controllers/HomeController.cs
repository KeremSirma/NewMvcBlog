using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewMvcBlog.Models;
using PagedList.Mvc;
using PagedList;

namespace NewMvcBlog.Controllers
{
    public class HomeController : Controller
    {
        MVCBlogDb db = new MVCBlogDb();
        // GET: Home
        public ActionResult Index(int Page=1)
        {
            var makale = db.Makales.OrderByDescending(m => m.MakaleId).ToPagedList(Page,5);
            return View(makale);
        }
        public ActionResult SonYorumlar()
        {
            return View(db.Yorums.OrderByDescending(y=>y.YorumId).Take(5));
        }

        public ActionResult PopulerMakaleler()
        {
            return View(db.Makales.OrderByDescending(m => m.Okunma).Take(5));
        }

        public ActionResult BlogAra(String Ara = null)
        {
            var aranan = db.Makales.Where(m => m.Baslik.Contains(Ara)).ToList();
            return View(aranan.OrderByDescending(m=>m.Tarih));
        }

        public ActionResult KategoriMakale(int id)
        {
            var makaleler = db.Makales.Where(m => m.KategoriId == id).ToList();
            return View(makaleler);
        }
        public ActionResult MakaleDetay(int id)
        {
            var makale = db.Makales.Where(m => m.MakaleId == id).SingleOrDefault();
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult Iletisim()
        {
            return View();
        }

        public ActionResult KategoriPartial()
        {
            return View(db.Kategoris.ToList());
        }

        public JsonResult YorumYap(string Yorum,int Makaleid)
        {
            var uyeId = Session["uyeId"];
            if(Yorum!=null)
                {
                db.Yorums.Add(new Yorum { UyeId = Convert.ToInt32(uyeId),MakaleId=Makaleid,Icerik=Yorum,Tarih=DateTime.Now });
                db.SaveChanges();
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult YorumSil(int id)
        {
            var uyeid = Session["uyeid"];
            var yorum = db.Yorums.Where(y => y.YorumId == id).SingleOrDefault();
            var makale = db.Makales.Where(m => m.MakaleId == yorum.MakaleId).SingleOrDefault();
            if (yorum.UyeId == Convert.ToInt32(uyeid))
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.MakaleId });

            }
            else
                return HttpNotFound();
        }
        public ActionResult OkunmaArttir(int Makaleid)
        {
            var makale = db.Makales.Where(m => m.MakaleId == Makaleid).SingleOrDefault();
            makale.Okunma += 1;
            db.SaveChanges();

            return View();
        }

    }
}