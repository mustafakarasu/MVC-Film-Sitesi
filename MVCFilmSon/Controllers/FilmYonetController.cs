using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCFilmSon.Models;

namespace MVCFilmSon.Controllers
{
    [Authorize(Roles = "Admin")] //Buraya yazarak classın içindekiler dahil
    public class FilmYonetController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FilmYonet
        //[Authorize(Roles ="Admin")] --> Buraya yazarsak her meotodun üzerine yazmamız gerekir.
        //[AllowAnonymous] --> Classı sadece admine açtık. Ama bazı metodları herkese açık hale getirebiliriz. 
        public ActionResult Index()
        {
            var filmler = db.Filmler.Include(f => f.FilminKategorisi);
            return View(filmler.ToList());
        }

        // GET: FilmYonet/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Filmler.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // GET: FilmYonet/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi");
            return View();
        }

        // POST: FilmYonet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FilmID,FilmAdi,Yonetmen,Konu,KategoriID,ResimURL,Fragman,Yabanci,Yil")] Film film, HttpPostedFileBase ResimURL)
        {
            if (ModelState.IsValid) /*Kullanıcı modeler uygun veri girişi yaptıysa*/
            {
                if (ResimURL.ContentLength > 0)
                    ResimURL.SaveAs(Server.MapPath("/Content/filmresim/")+ResimURL.FileName);

                film.ResimURL = ResimURL.FileName;

                var yid = film.Fragman.Split('=').Last(); //Last() dizideki sondaki elemanı alır.

                string sonuc = string.Format("<iframe width='560' height='315' src='https://www.youtube.com/embed/{0}' frameborder='0' allowfullscreen></iframe>",yid);

                film.Fragman = sonuc;

                db.Filmler.Add(film);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi", film.KategoriID);
            return View(film);
        }

        // GET: FilmYonet/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Filmler.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi", film.KategoriID);
            return View(film);
        }

        // POST: FilmYonet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] //HTML kodları iletebilmek için. <iframe> gibi
        public ActionResult Edit([Bind(Include = "FilmID,FilmAdi,Yonetmen,Konu,KategoriID,ResimURL,Fragman,Yabanci,Yil")] Film film)
        {
            if (ModelState.IsValid)
            {
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategoriID = new SelectList(db.Kategoriler, "KategoriID", "KategoriAdi", film.KategoriID);
            return View(film);
        }

        // GET: FilmYonet/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Filmler.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: FilmYonet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Filmler.Find(id);
            db.Filmler.Remove(film);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
