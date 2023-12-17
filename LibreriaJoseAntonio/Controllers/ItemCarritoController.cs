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
             

            ViewBag.Total = 0f;
            if (User.Identity.IsAuthenticated) {
                string idUsuarioActual = User.Identity.GetUserId();
                var itemsCarrito = db.ItemsCarrito.Include(i => i.Libro).Include(e=>e.Libro.Autor_id)
                    .Include(e => e.Libro.Estado_id)
                    .Include(e => e.Libro.Editorial_id)
                    .Include(e => e.Libro.Autor_id)
                    .Include(e => e.Libro.Formato_id)
                    .Where( libro=>libro.IdUser.Equals(idUsuarioActual) );

                float total=itemsCarrito.ToList().Sum(item => item.calcularTotal());
                ViewBag.Total = total;
                return View(itemsCarrito.ToList());
            }
            return View(new List<ItemCarrito>());
            
        }


        public ActionResult Comprar()
        {
                if (User.Identity.IsAuthenticated) {
                    string idUsuarioActual = User.Identity.GetUserId();

                    var itemsCarrito = db.ItemsCarrito.Include(i => i.Libro)
                              .Where(libro => libro.IdUser.Equals(idUsuarioActual)).ToList();
               
                foreach (var item in itemsCarrito){
                        Libro libro = libroRepository.GetById(item.Id_libro);

                        db.Entry(libro).State = EntityState.Modified;
                        if (item.Cantidad > libro.Cantidad) {
                        TempData["Error"] = "EL libro " + libro.Titulo + " no cumple los requisitos";
                            return RedirectToAction("Index");
                        }
                        libro.Cantidad -= item.Cantidad;
                        db.ItemsCarrito.Remove(item);
                }


                    db.SaveChanges();

                }
            return RedirectToAction("Index");
        }
        // GET: ItemCarritoes/Details/5
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

        // GET: ItemCarritoes/Create
        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Id_libro = new SelectList(db.Libros, "Id", "ISBN");
            return View();
        }

        // POST: ItemCarritoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Create([Bind(Include = "Id,Id_libro,Cantidad,IdUser")] ItemCarrito itemCarrito)
        {
            if (ModelState.IsValid)
            {
                db.ItemsCarrito.Add(itemCarrito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_libro = new SelectList(db.Libros, "Id", "ISBN", itemCarrito.Id_libro);
            return View(itemCarrito);
        }

        // GET: ItemCarritoes/Edit/5
        [Authorize(Roles = "Administrador")]
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
            ViewBag.Id_libro = new SelectList(db.Libros, "Id", "ISBN", itemCarrito.Id_libro);
            return View(itemCarrito);
        }

        // POST: ItemCarritoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit([Bind(Include = "Id,Id_libro,Cantidad,IdUser")] ItemCarrito itemCarrito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemCarrito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_libro = new SelectList(db.Libros, "Id", "ISBN", itemCarrito.Id_libro);
            return View(itemCarrito);
        }

        // GET: ItemCarritoes/Delete/5
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

        // POST: ItemCarritoes/Delete/5
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
        public JsonResult GetTotalCarrito()
        {
            if (User.Identity.IsAuthenticated)
            {
                string idUsuarioActual = User.Identity.GetUserId();
                var itemsCarrito = db.ItemsCarrito.Include(i => i.Libro)
                    .Where(libro => libro.IdUser.Equals(idUsuarioActual));

                float total = itemsCarrito.ToList().Sum(item => item.calcularTotal());
                return Json(total);
            }
            return Json(0);
        }

        [HttpPost]
        public JsonResult AgregarItem(string isbn,int cantidad, bool remplazar) {
            // Respuestas "true","false","outStock","full"
            if (User.Identity.IsAuthenticated) { 
                //Libro libro = db.Libros;
                string userId = User.Identity.GetUserId();
                Libro libro = libroRepository.GetByIsbn(isbn);

                if (cantidad == 0 ) { return Json("false"); }
                if (libro.Cantidad == 0) { return Json("outStock"); }

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
                        if (remplazar && cantidad<= libro.Cantidad)
                        {
                            itemsCarrito.Cantidad = cantidad;
                        }
                        // comprobar que si añado,no se pasa de la cantidad
                        else if (!remplazar && itemsCarrito.Cantidad+cantidad <= libro.Cantidad)
                        {
                            itemsCarrito.Cantidad += cantidad;
                        }
                        else {
                            return Json("full");
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
