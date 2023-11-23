using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibreriaJose.Models.Data
{
    public class Formato
    {
        [Key]
        public int Id { get; set; }
        public string Nombre {  get; set; }


    }
}