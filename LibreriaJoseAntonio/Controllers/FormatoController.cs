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

namespace LibreriaJoseAntonio.Controllers
{
    public class FormatoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Formato
        public ActionResult Index()
        {
            return View(db.Formatos.ToList());
        }

        // GET: Formato/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formato formato = db.Formatos.Find(id);
            if (formato == null)
            {
                return HttpNotFound();
            }
            return View(formato);
        }

        // GET: Formato/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Formato/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre")] Formato formato)
        {
            if (ModelState.IsValid)
            {
                db.Formatos.Add(formato);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(formato);
        }

        // GET: Formato/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formato formato = db.Formatos.Find(id);
            if (formato == null)
            {
                return HttpNotFound();
            }
            return View(formato);
        }

        // POST: Formato/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre")] Formato formato)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formato).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formato);
        }

        // GET: Formato/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Formato formato = db.Formatos.Find(id);
            if (formato == null)
            {
                return HttpNotFound();
            }
            return View(formato);
        }

        // POST: Formato/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Formato formato = db.Formatos.Find(id);
            db.Formatos.Remove(formato);
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
    }
}
