using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotaPlusNew.DAO;
using NotaPlusNew.Models;

namespace NotaPlusNew.Controllers
{
    public class AlumnoController : Controller
    {
        private DetalleAlumnoDAO dao = new DetalleAlumnoDAO();

        // GET: Alumno
        public ActionResult Index(string nivel = "", string grado = "", string seccion = "")
        {
            ViewBag.Nivel = nivel;
            ViewBag.Grado = grado;
            ViewBag.Seccion = seccion;

            if (string.IsNullOrEmpty(nivel) && string.IsNullOrEmpty(grado) && string.IsNullOrEmpty(seccion))
                return View(dao.ListarTodos());

            return View(dao.BuscarPorNivelGradoSeccion(nivel, grado, seccion));
        }

        // GET: Alumno/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Alumno/Create
        [HttpPost]
        public ActionResult Create(Alumno alumno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Insertar(alumno);
                    TempData["mensaje"] = "Alumno registrado correctamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Complete todos los campos correctamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error al registrar alumno: " + ex.Message;
            }
            return View(alumno);
        }


        // GET: Alumno/Edit/5
        public ActionResult Edit(int id)
        {
            Alumno alumno = dao.ObtenerPorId(id);
            if (alumno == null)
                return HttpNotFound();
            return View(alumno);
        }

        // POST: Alumno/Edit/5
        [HttpPost]
        public ActionResult Edit(Alumno alumno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dao.Actualizar(alumno);
                    TempData["mensaje"] = "Alumno actualizado correctamente.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error al actualizar alumno: " + ex.Message;
            }
            return View(alumno);
        }

        // GET: Alumno/Delete/5
        public ActionResult Delete(int id)
        {
            Alumno alumno = dao.ObtenerPorId(id);
            if (alumno == null)
                return HttpNotFound();
            return View(alumno);
        }

        // POST: Alumno/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                dao.Eliminar(id);
                TempData["mensaje"] = "Alumno eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Error al eliminar alumno: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}