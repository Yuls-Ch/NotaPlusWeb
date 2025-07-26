using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotaPlusNew.DAO;
using NotaPlusNew.Models;

namespace NotaPlusNew.Controllers
{
    public class DocenteController : Controller
    {
        private DocenteDAO docenteDAO = new DocenteDAO();
        private CursoDAO cursoDAO = new CursoDAO();

        // Mostrar formulario de edición (mantenimiento)
        public ActionResult Edit(int id)
        {
            var docente = docenteDAO.ObtenerPorId(id);
            if (docente == null)
            {
                return HttpNotFound();
            }

            var viewModel = new DocenteViewModel
            {
                Docente = docente,
                CursosDisponibles = cursoDAO.ListarCursos(),
                CursosAsignados = cursoDAO.ObtenerPorDocente(id)
            };

            return View(viewModel);
        }

        // Guardar cambios en el docente y cursos asignados
        [HttpPost]
        public ActionResult Edit(DocenteViewModel model, string cursosJson)
        {
            if (!ModelState.IsValid)
            {
                model.CursosDisponibles = cursoDAO.ListarCursos();
                model.CursosAsignados = cursoDAO.ObtenerPorDocente(model.Docente.Id);
                return View(model);
            }

            // Actualizar datos del docente
            docenteDAO.Actualizar(model.Docente);

            // Eliminar cursos anteriores
            cursoDAO.EliminarCursosPorDocente(model.Docente.Id);

            // Parsear cursosJson (de JavaScript)
            if (!string.IsNullOrEmpty(cursosJson))
            {
                var cursos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CursoAsignado>>(cursosJson);
                foreach (var curso in cursos)
                {
                    curso.DocenteId = model.Docente.Id;
                    cursoDAO.InsertarCursoAsignado(curso);
                }
            }

            // Redireccionar (o mostrar mensaje de éxito)
            TempData["mensaje"] = "Docente actualizado correctamente.";
            //return RedirectToAction("Edit", new { id = model.Docente.Id });
            return RedirectToAction("Index");
        }

        // (Opcional) Lista de docentes (para vista Index)
        public ActionResult Index()
        {
            var lista = docenteDAO.Listar();
            return View(lista);
        }

        /**********************************************************************************************************/

        // Mostrar formulario para nuevo docente
        public ActionResult Create()
        {
            var viewModel = new DocenteViewModel
            {
                Docente = new Docente(),
                CursosDisponibles = cursoDAO.ListarCursos(),
                CursosAsignados = new List<CursoAsignado>()
            };

            return View("Edit", viewModel); // Usa la misma vista que Edit
        }

        [HttpPost]
        public ActionResult Create(DocenteViewModel model, string cursosJson)
        {
            if (!ModelState.IsValid)
            {
                model.CursosDisponibles = cursoDAO.ListarCursos();
                return View("Edit", model);
            }

            int nuevoId = docenteDAO.Registrar(model.Docente);

            Console.WriteLine("Nuevo docente ID: " + nuevoId); // <-- Asegúrate que no sea 0

            if (!string.IsNullOrEmpty(cursosJson))
            {
                var cursos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CursoAsignado>>(cursosJson);
                foreach (var curso in cursos)
                {
                    curso.DocenteId = nuevoId;
                    cursoDAO.InsertarCursoAsignado(curso); // Aquí puede fallar si nuevoId = 0
                }
            }

            TempData["mensaje"] = "Docente registrado correctamente.";
            return RedirectToAction("Index");
        }

        // Eliminar docente
        public ActionResult Delete(int id)
        {
            cursoDAO.EliminarCursosPorDocente(id); // Primero elimina cursos asignados
            docenteDAO.Eliminar(id);

            TempData["mensaje"] = "Docente eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}