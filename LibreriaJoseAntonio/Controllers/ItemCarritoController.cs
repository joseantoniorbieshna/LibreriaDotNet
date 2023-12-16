using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibreriaJose.Models.Data;
using LibreriaJoseAntonio.Models;
using LibreriaJoseAntonio.Models.Data;
using LibreriaJoseAntonio.Models.Repository;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace LibreriaJoseAntonio.Controllers
{
    public class ItemCarritoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private LibroRepository libroRepository;
        public ItemCarritoController() {
            libroRepository=new LibroRepository(db);
        }

        // GET: ItemCarrito
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated) {
                var itemsCarrito = db.ItemsCarrito.Include(i => i.Libro);
                return View(itemsCarrito.ToList());
            }
            return View(new List<ItemCarrito>());
            
            
        }

        // GET: ItemCarrito/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCarrito itemCarrito = db.ItemsCarrito.Find(id);
            if (itemCarrito == null)
            {
                return HttpNotFound();
            }
            return View(itemCarrito);
        }

        // GET: ItemCarrito/Create
        public ActionResult Create()
        {
            ViewBag.Isbn = new SelectList(db.Libros, "ISBN", "Titulo");
            return View();
        }

        // POST: ItemCarrito/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Isbn,Cantidad,IdUser")] ItemCarrito itemCarrito)
        {
            if (ModelState.IsValid)
            {
                db.ItemsCarrito.Add(itemCarrito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Isbn = new SelectList(db.Libros, "ISBN", "Titulo", itemCarrito.Id);
            return View(itemCarrito);
        }

        // GET: ItemCarrito/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCarrito itemCarrito = db.ItemsCarrito.Find(id);
            if (itemCarrito == null)
            {
                return HttpNotFound();
            }
            ViewBag.Isbn = new SelectList(db.Libros, "ISBN", "Titulo", itemCarrito.Id);
            return View(itemCarrito);
        }

        // POST: ItemCarrito/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Isbn,Cantidad,IdUser")] ItemCarrito itemCarrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemCarrito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Isbn = new SelectList(db.Libros, "ISBN", "Titulo", itemCarrito.Id);
            return View(itemCarrito);
        }

        // GET: ItemCarrito/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemCarrito itemCarrito = db.ItemsCarrito.Find(id);
            if (itemCarrito == null)
            {
                return HttpNotFound();
            }
            return View(itemCarrito);
        }

        // POST: ItemCarrito/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemCarrito itemCarrito = db.ItemsCarrito.Find(id);
            db.ItemsCarrito.Remove(itemCarrito);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult AgregarItem(string isbn,int cantidad, bool remplazar) {
            if (User.Identity.IsAuthenticated) { 
                //Libro libro = db.Libros;
                string userId = User.Identity.GetUserId();
                Libro libro = libroRepository.GetByIsbn(isbn);

                if (cantidad == 0) { return Json(false); }

                if (libro != null) {
                    ItemCarrito itemsCarrito = db.ItemsCarrito.Include(e => e.Libro).FirstOrDefault(item => item.IdUser.Equals(userId) && item.Libro.ISBN.Equals(libro.ISBN));
                    //Si no hay item del libro para ese usuario se crea
                    if (itemsCarrito == null)
                    {
                        db.ItemsCarrito.Add(new ItemCarrito(userId, libro, cantidad));
                        db.Entry(libro).State = EntityState.Modified;
                        db.SaveChanges();

                        return Json("true");
                    }
                    //Si existe un libro para el usuario
                    else
                    {
                        db.Entry(itemsCarrito).State = EntityState.Modified;
                        db.Entry(libro).State = EntityState.Modified;

                        //Remplazar o Añadir
                        if (remplazar)
                        {
                            itemsCarrito.Cantidad = cantidad;
                        }
                        else {
                            itemsCarrito.Cantidad += cantidad;
                        }

                       

                        db.SaveChanges();
                        return Json("true");
                    }
                }
 
            }
            return Json("false");
        }
    }
}
