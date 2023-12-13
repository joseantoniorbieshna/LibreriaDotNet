using LibreriaJose.Models.Data;
using LibreriaJose.Models.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LibreriaJoseAntonio.Models.Repository
{
    public class LibroRepository : LibroDao
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<Libro> GetAll()
        {
            
            return db.Libros.Include(l => l.Autor_id).Include(l => l.Editorial_id).Include(l => l.Estado_id).Include(l => l.Formato_id).ToList();
        }

        public Libro GetByIsbn(string isbn)
        {
            return db.Libros.Include(l => l.Autor_id).Include(l => l.Editorial_id).Include(l => l.Estado_id).Include(l => l.Formato_id).FirstOrDefault(libro=>libro.ISBN==isbn);
        }
    }
}