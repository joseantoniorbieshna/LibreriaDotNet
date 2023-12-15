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
using LibreriaJoseAntonio.Models.Repository;

namespace LibreriaJoseAntonio.Controllers
{
    public class LibroController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private LibroRepository libroRepository=new LibroRepository();

        // GET: Libro
        public ActionResult Index()
        {
            return View(libroRepository.GetAll());
        }

        // GET: Libro/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = libroRepository.GetByIsbn(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // GET: Libro/Create
        public ActionResult Create()
        {
            ViewBag.AutorId = new SelectList(db.Autores, "Id", "Nombre");
            ViewBag.EditorialId = new SelectList(db.Editoriales, "Id", "Nombre");
            ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre");
            ViewBag.FormatoId = new SelectList(db.Formatos, "Id", "Nombre");
            return View();
        }

        // POST: Libro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ISBN,AutorId,EditorialId,FormatoId,EstadoId,Titulo,Precio,Cantidad,Imagen")] Libro libro)
        {
            //Si el isbn ya existe, manda el error
            if (db.Libros.Find(libro.ISBN)!=null) {
                ModelState.AddModelError("ISBN", "El ISBN es ya existente");
                ViewBag.AutorId = new SelectList(db.Autores, "Id", "Nombre", libro.AutorId);
                ViewBag.EditorialId = new SelectList(db.Editoriales, "Id", "Nombre", libro.EditorialId);
                ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", libro.EstadoId);
                ViewBag.FormatoId = new SelectList(db.Formatos, "Id", "Nombre", libro.FormatoId);
                return View(libro);
            }
            

            if (ModelState.IsValid)
            {
                db.Libros.Add(libro);   
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AutorId = new SelectList(db.Autores, "Id", "Nombre", libro.AutorId);
            ViewBag.EditorialId = new SelectList(db.Editoriales, "Id", "Nombre", libro.EditorialId);
            ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", libro.EstadoId);
            ViewBag.FormatoId = new SelectList(db.Formatos, "Id", "Nombre", libro.FormatoId);
            return View(libro);
        }

        // GET: Libro/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = libroRepository.GetByIsbn(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            ViewBag.AutorId = new SelectList(db.Autores, "Id", "Nombre", libro.AutorId);
            ViewBag.EditorialId = new SelectList(db.Editoriales, "Id", "Nombre", libro.EditorialId);
            ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", libro.EstadoId);
            ViewBag.FormatoId = new SelectList(db.Formatos, "Id", "Nombre", libro.FormatoId);
            return View(libro);
        }

        // POST: Libro/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ISBN,AutorId,EditorialId,FormatoId,EstadoId,Titulo,Precio,Cantidad,Imagen")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(libro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AutorId = new SelectList(db.Autores, "Id", "Nombre", libro.AutorId);
            ViewBag.EditorialId = new SelectList(db.Editoriales, "Id", "Nombre", libro.EditorialId);
            ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", libro.EstadoId);
            ViewBag.FormatoId = new SelectList(db.Formatos, "Id", "Nombre", libro.FormatoId);
            return View(libro);
        }

        // GET: Libro/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = libroRepository.GetByIsbn(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Libro libro = libroRepository.GetByIsbn(id);
            libroRepository.RemoveBook(libro);
            libroRepository.guardarCambios();
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
    }
}
