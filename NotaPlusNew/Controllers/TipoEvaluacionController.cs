using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotaPlusNew.DAO;
using NotaPlusNew.Models;

namespace NotaPlusNew.Controllers
{
    public class TipoEvaluacionController : Controller
    {
        TipoEvaluacionDAO dao = new TipoEvaluacionDAO();
        private void CargarCombos()
        {
            ViewBag.Niveles = new List<SelectListItem>
    {
        new SelectListItem { Text = "Primaria", Value = "Primaria" },
        new SelectListItem { Text = "Secundaria", Value = "Secundaria" }
    };

            ViewBag.Grados = ObtenerGradosPersonalizados(); // <- Aquí el cambio

            ViewBag.Secciones = new List<SelectListItem>
    {
        new SelectListItem { Text = "A", Value = "A" },
        new SelectListItem { Text = "B", Value = "B" },
        new SelectListItem { Text = "C", Value = "C" }
    };

            var cursos = new CursoDAO().Listar();
            ViewBag.Cursos = cursos.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            }).ToList();
        }



        public ActionResult TipoEvaluacion(int? nivel, int? grado, int? seccion, int? curso)
        {
            CargarCombos();

            if (nivel.HasValue && grado.HasValue && seccion.HasValue && curso.HasValue)
            {
                var evaluaciones = dao.Listar(nivel.Value, grado.Value, seccion.Value, curso.Value);
                return View("TipoEvaluacion", evaluaciones);
            }

            return View("TipoEvaluacion", new List<TipoEvaluacion>());
        }



        [HttpPost]
        public ActionResult GuardarPesos(List<TipoEvaluacion> tipos)
        {
            foreach (var ev in tipos)
            {
                dao.ActualizarPeso(ev.IdTipoEvaluacion, ev.Peso);
            }

            TempData["mensaje"] = "Pesos actualizados correctamente.";

            // Redireccionar con los filtros actuales
            if (tipos.Count > 0)
            {
                var primero = dao.ObtenerPorId(tipos[0].IdTipoEvaluacion);
                return RedirectToAction("TipoEvaluacion", new
                {
                    nivel = primero.IdNivel,
                    grado = primero.IdGrado,
                    seccion = primero.IdSeccion,
                    curso = primero.IdCurso
                });
            }

            return RedirectToAction("TipoEvaluacion");
        }

        public ActionResult Eliminar(int id)
        {
            var evaluacion = dao.ObtenerPorId(id);
            dao.Eliminar(id);
            TempData["mensaje"] = "Evaluación eliminada correctamente.";

            return RedirectToAction("TipoEvaluacion", new
            {
                nivel = evaluacion.IdNivel,
                grado = evaluacion.IdGrado,
                seccion = evaluacion.IdSeccion,
                curso = evaluacion.IdCurso
            });
        }

        [HttpPost]
        public ActionResult Agregar(TipoEvaluacion nuevo, int? nivel, int? grado, int? seccion, int? curso)
        {
            if (nuevo != null && !string.IsNullOrWhiteSpace(nuevo.NombreEvaluacion))
            {
                nuevo.IdNivel = nivel ?? 0;
                nuevo.IdGrado = grado ?? 0;
                nuevo.IdSeccion = seccion ?? 0;
                nuevo.IdCurso = curso ?? 0;

                dao.Insertar(nuevo);
                TempData["mensaje"] = "Evaluación agregada correctamente.";
            }

            return RedirectToAction("TipoEvaluacion", new { nivel, grado, seccion, curso });
        }

        public ActionResult Editar(int id)
        {
            var evaluacion = dao.ObtenerPorId(id);
            if (evaluacion == null)
            {
                return HttpNotFound();
            }

            return View(evaluacion);
        }

        [HttpPost]
        public ActionResult Editar(TipoEvaluacion evaluacion)
        {
            if (ModelState.IsValid)
            {
                dao.Actualizar(evaluacion);
                TempData["mensaje"] = "Evaluación actualizada correctamente.";
                return RedirectToAction("TipoEvaluacion", new
                {
                    nivel = evaluacion.IdNivel,
                    grado = evaluacion.IdGrado,
                    seccion = evaluacion.IdSeccion,
                    curso = evaluacion.IdCurso
                });
            }

            return View(evaluacion);
        }

        private List<SelectListItem> ObtenerGradosPersonalizados()
        {
            return new List<SelectListItem> {
        new SelectListItem { Text = "1er", Value = "1" },
        new SelectListItem { Text = "2do", Value = "2" },
        new SelectListItem { Text = "3ro", Value = "3" },
        new SelectListItem { Text = "4to", Value = "4" },
        new SelectListItem { Text = "5to", Value = "5" },
        new SelectListItem { Text = "6to", Value = "6" }
    };
        }

    }
}