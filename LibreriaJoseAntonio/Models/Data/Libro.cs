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
        [Index(IsUnique=true)]
        [StringLength(13,MinimumLength =13)]
        [RegularExpression("^([0-9]{13})$", ErrorMessage = "Requiere 13 números en el isbn")]
        public string ISBN { get; set; }

        [ForeignKey("Autor_id")]
        public int AutorId { get; set; }
        public Autor Autor_id { get; set; }


        [ForeignKey("Editorial_id")]
        public int EditorialId { get; set; }
        public Editorial Editorial_id { get; set; }

        [ForeignKey("Formato_id")]
        public int FormatoId { get; set; }
        public Formato Formato_id { get; set; }

        [ForeignKey("Estado_id")]
        public int EstadoId { get; set; }
        public Estado Estado_id { get; set; }
        
    }
}