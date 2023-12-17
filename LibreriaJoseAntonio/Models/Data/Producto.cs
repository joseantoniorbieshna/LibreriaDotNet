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
        [RegularExpression("([a-z\\-_0-9\\/\\:\\.]*\\.(jpg|jpeg|png|gif|webp))", ErrorMessage = "Introduce una url que sea imagen")]
        public string Imagen {  get; set; } 

    }
}