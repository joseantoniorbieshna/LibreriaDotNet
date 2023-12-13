using LibreriaJose.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibreriaJose.Models.Repository
{
    public interface LibroDao
    {
        List<Libro> GetAll();
        Libro GetByIsbn(string isbn);
    }
}