using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibreriaJose.Models.Data
{
    public abstract class Producto
    {
        [Required]
        public string Titulo { get; set; }
        public float Precio { get; set; }
        public int Cantidad { get; set; }

        [Display(Name ="Introduce un URL:")]
        public string Imagen {  get; set; } 

    }
}