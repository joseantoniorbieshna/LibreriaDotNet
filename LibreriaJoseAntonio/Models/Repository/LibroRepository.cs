using LibreriaJose.Models.Data;
using LibreriaJoseAntonio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibreriaJose.Models.Repository
{
    public class LibroRepository : LibroDao
    {
        private ApplicationDbContext db;
        public LibroRepository() {
            db =new ApplicationDbContext();
        }
        public List<Libro> GetAll()
        {
            return null;//db.Libr.ToList();
        }

        public Producto GetByIsbn(int isbn)
        {
            return null;//db.Libros.FirstOrDefault(e => e.ISBN==isbn);
        }

        

    }
}