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
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = libroRepository.GetById(id);
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
        public ActionResult Create([Bind(Include = "Id,ISBN,AutorId,EditorialId,FormatoId,EstadoId,Titulo,Precio,Cantidad,Imagen")] Libro libro)
        {
            //Si el isbn ya existe, manda el error
            if (libroRepository.GetByIsbn(libro.ISBN)!=null) {
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
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = libroRepository.GetById(id);
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
        public ActionResult Edit([Bind(Include = "Id,ISBN,AutorId,EditorialId,FormatoId,EstadoId,Titulo,Precio,Cantidad,Imagen")] Libro libro)
        {
            //diferente del isbn entre la base de datos y actual
            //y ya existe en la bbdd
            if (libroRepository.GetById(libro.Id).ISBN!=libro.ISBN 
                && libroRepository.GetByIsbn(libro.ISBN) != null)
            {
                ModelState.AddModelError("ISBN", "El ISBN es ya existente");
                ViewBag.AutorId = new SelectList(db.Autores, "Id", "Nombre", libro.AutorId);
                ViewBag.EditorialId = new SelectList(db.Editoriales, "Id", "Nombre", libro.EditorialId);
                ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", libro.EstadoId);
                ViewBag.FormatoId = new SelectList(db.Formatos, "Id", "Nombre", libro.FormatoId);
                return View(libro);
            }

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
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Libro libro = libroRepository.GetById(id);
            if (libro == null)
            {
                return HttpNotFound();
            }
            return View(libro);
        }

        // POST: Libro/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Libro libro = libroRepository.GetById(id);
            libroRepository.RemoveBook(libro);
            libroRepository.guardarCambios();
            return RedirectToAction("Index");
        }


        private bool RemoteFileExists(string url)
        {
            try
            {
                //Creamos el HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //ajustamos a Head, aunque también se puede GET
                request.Method = "HEAD";
                //Obtenemos la respuesta we.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Retorna verdadero si el status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Si algo sale mal retornamos false.
                return false;
            }
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
