using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFilmSon.Models
{
    public class Kategori
    {
        [Key]
        public int KategoriID { get; set; }
        public string KategoriAdi { get; set; }


        public virtual List<Film> FilmListesi { get; set; }
    }
}
