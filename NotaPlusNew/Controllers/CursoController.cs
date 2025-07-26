using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using NotaPlusNew.DAO;
using NotaPlusNew.Models;

namespace NotaPlusNew.Controllers
{
    public class CursoController : Controller
    {
        private CursoDAO dao = new CursoDAO();

        // Vista Index
        public ActionResult Index()
        {
            var cursos = dao.Listar();
            return View(cursos);
        }

        // GET: Crear
        public ActionResult Create()
        {
            var model = new Curso
            {
                Horarios = new List<HorarioCurso>() // Vacío al crear
            };

            return View("Edit", model); // Usa vista Edit
        }

        // POST: Crear
        [HttpPost]
        public ActionResult Create(Curso curso, string horariosJson)
        {
            if (!ModelState.IsValid)
            {
                curso.Horarios = new List<HorarioCurso>();
                return View("Edit", curso);
            }

            int nuevoId = dao.Insertar(curso);

            if (!string.IsNullOrEmpty(horariosJson))
            {
                var horarios = JsonConvert.DeserializeObject<List<HorarioCurso>>(horariosJson);
                foreach (var h in horarios)
                {
                    h.CursoId = nuevoId;
                    dao.InsertarHorario(h);
                }
            }

            TempData["mensaje"] = "Curso registrado correctamente.";
            return RedirectToAction("Index");
        }

        // GET: Edit
        public ActionResult Edit(int? id)
        {
            Curso curso = id.HasValue ? dao.ObtenerPorId(id.Value) : new Curso();
            return View(curso);
        }

        // POST: Save (Create o Edit)
        [HttpPost]
        public ActionResult Edit(Curso curso, string horariosJson)
        {
            if (!ModelState.IsValid)
                return View(curso);

            if (!string.IsNullOrEmpty(horariosJson))
                curso.Horarios = JsonConvert.DeserializeObject<List<HorarioCurso>>(horariosJson);

            if (curso.Id == 0)
                dao.Insertar(curso);
            else
                dao.Actualizar(curso);

            TempData["mensaje"] = "Curso actualizado correctamente.";
            return RedirectToAction("Index");
        }

        // Eliminar
        public ActionResult Delete(int id)
        {
            dao.EliminarCursosAsignadosPorCurso(id);
            dao.Eliminar(id);

            TempData["mensaje"] = "Curso eliminado correctamente.";
            return RedirectToAction("Index");
        }
    }
}