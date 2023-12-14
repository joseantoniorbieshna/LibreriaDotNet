﻿using LibreriaJose.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibreriaJoseAntonio.Models.Data
{
    public class ItemCarrito
    {
        public ItemCarrito() { }
        public ItemCarrito(string idUser, Libro libro, int cantida) {
            this.IdUser = idUser;
            this.Libro = libro;
            this.Isbn = libro.ISBN;
            this.Cantidad = cantida;
        }
        [Key]
        public int Id { get; set; }

        [ForeignKey("Libro")]
        public string Isbn { get; set; }
        public Libro Libro { get; set; }
        public int Cantidad { get; set; }
        public string IdUser { get; set; }
    }
}