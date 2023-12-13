using LibreriaJose.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibreriaJoseAntonio.Models.Data
{
    public class ItemCarrito
    {
        [Key]
        public int Id { get; set; }
        public virtual Libro Libro { get; set; }
        public int cantidad { get; set; }
        public string IdUser { get; set; }
    }
}