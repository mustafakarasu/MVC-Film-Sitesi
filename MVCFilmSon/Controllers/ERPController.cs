using MVCFilmSon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace MVCFilmSon.Controllers
{
    public class ERPController : Controller
    {
        ApplicationDbContext ctx = new ApplicationDbContext();

        // GET: ERP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialSlider()
        {
            return View(ctx.Filmler.Take(3).ToList());
        }

        public ActionResult PartialOzetler()
        {
            return View(ctx.Filmler.OrderByDescending(x => x.FilmID).Take(4).ToList());
        }

        public ActionResult Arama(string aranan)
        {
            //.Startswith("A") ile başlayan
            //.EndsWith("B") ile biten
            var liste = ctx.Filmler
                .Where(x => x.FilmAdi.Contains(aranan))
                .ToList();

            ViewBag.aranankelime = aranan;                   
            return View(liste);
        }
        public ActionResult Yerli(string sirala, int? y, int? gelenSayfa)
        {
            int simdikiSayfa = gelenSayfa ?? 1; //gelenSayfa null ise 1, değilse gelenSayfa
            //NuGetlarda pagedlist ekle.
            int sayfaBasiGosterim = 1;

            ViewBag.s = sirala;
            ViewBag.yil = y;

            var liste = ctx.Filmler.Where(x => !x.Yabanci).ToList();

            if (sirala != null)
                liste = FilmleriSirala(sirala, liste);

            if (y != null)
            {
                liste = liste.Where(x => x.Yil == y).ToList();
            }

            return View(liste.ToPagedList(simdikiSayfa, sayfaBasiGosterim));
        }

        public ActionResult Yabanci(string sirala, int? y, int? gelenSayfa)
        {
            int simdikiSayfa = gelenSayfa ?? 1; //gelenSayfa null ise 1, değilse gelenSayfa
            //NuGetlarda pagedlist ekle.
            int sayfaBasiGosterim = 1;

            ViewBag.s = sirala;
            ViewBag.yil = y;

            var liste = ctx.Filmler.Where(x => x.Yabanci).ToList();

            if (sirala != null)
                liste = FilmleriSirala(sirala, liste);

            if(y!= null)
            {
                liste = liste.Where(x => x.Yil == y).ToList();
            }

            return View(liste.ToPagedList(simdikiSayfa, sayfaBasiGosterim));
        }

        public ActionResult Yil(short y, string sirala, int? gelenSayfa)
        {
            int simdikiSayfa = gelenSayfa ?? 1;
            int sayfaBasiGosterim = 1;

            ViewBag.yil = y;

            var liste = ctx.Filmler.Where(x => x.Yil == y).ToList();

            if (sirala != null)
                liste = FilmleriSirala(sirala, liste);

            return View(liste.ToPagedList(simdikiSayfa, sayfaBasiGosterim));
        }
        
        List<Film> FilmleriSirala(string sirala, List<Film> liste)
        {
            switch (sirala)
            {
                case "Alfebetik A-Z":
                    liste = liste.OrderBy(x => x.FilmAdi).ToList();
                    break;
                case "Alfebetik Z-A":
                    liste = liste.OrderByDescending(x => x.FilmAdi).ToList();
                    break;
                case "Yeniden Eskiye":
                    liste = liste.OrderByDescending(x => x.FilmID).ToList();
                    break;
                case "Eskiden Yeniye":
                    liste = liste.OrderBy(x => x.FilmID).ToList();
                    break;
                default:
                    break;
            }
            return liste;
        }
    }
}