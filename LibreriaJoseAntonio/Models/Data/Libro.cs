using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

namespace LibreriaJose.Models.Data
{
    public class Libro:Producto{
        [Key]
        [Required]
        [Index(IsUnique =true)]
        [StringLength(13,MinimumLength =13)]
        [RegularExpression("^([0-9]{13})$", ErrorMessage = "Requiere 13 números en el isbn")]
        public string ISBN { get; set; }

        public int AutorId { get; set; }
        [Required]
        [ForeignKey("AutorId")]
        public Autor Autor_id { get; set; }


        public int EditorialId { get; set; }
        [Required]
        [ForeignKey("EditorialId")]
        public Editorial Editorial_id { get; set; }

        public int FormatoId { get; set; }
        [Required]
        [ForeignKey("FormatoId")]
        public Formato Formato_id { get; set; }

        public int EstadoId { get; set; }
        [Required]
        [ForeignKey("EstadoId")]
        public Estado Estado_id { get; set; }


        
    }
}