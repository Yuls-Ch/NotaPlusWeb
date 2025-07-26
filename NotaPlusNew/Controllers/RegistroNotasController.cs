using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotaPlusNew.DAO;
using NotaPlusNew.Models;

namespace NotaPlusNew.Controllers
{
    public class RegistroNotasController : Controller
    {
        private CursoDAO cursoDAO = new CursoDAO();
        private NotaDAO notaDAO = new NotaDAO();
        private AlumnoDAO alumnoDAO = new AlumnoDAO();

        // GET: Vista inicial
        [HttpGet]
        public ActionResult RegistrarNotas()
        {
            ViewBag.Niveles = ObtenerNiveles();
            ViewBag.Grados = ObtenerGradosPersonalizados();  // <-- Aquí
            ViewBag.Secciones = ObtenerSecciones();
            ViewBag.Cursos = cursoDAO.Listar().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            }).ToList();
            ViewBag.Bimestres = ObtenerBimestres();

            ViewBag.Alumnos = new List<Alumno>();
            ViewBag.IdCurso = 0;
            ViewBag.IdBimestre = 0;
            ViewBag.IdAsignacion = 0;

            return View();
        }


        // POST: Cargar alumnos por filtro
        [HttpPost]
        public ActionResult RegistrarNotasConDatos(string nivel, string grado, string seccion, int IdCurso, int IdBimestre)
        {
            ViewBag.Niveles = ObtenerNiveles();
            ViewBag.Grados = ObtenerGradosPersonalizados();  // <-- Aquí
            ViewBag.Secciones = ObtenerSecciones();
            ViewBag.Cursos = cursoDAO.Listar().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Nombre
            }).ToList();
            ViewBag.Bimestres = ObtenerBimestres();

            ViewBag.IdCurso = IdCurso;
            ViewBag.IdBimestre = IdBimestre;
            ViewBag.IdAsignacion = 1;

            var alumnos = alumnoDAO.ListarPorProcedimiento(nivel, grado, seccion);
            ViewBag.Alumnos = alumnos;

            return View("RegistrarNotas", new List<Nota>());
        }




        // POST: Guardar notas
        [HttpPost]
        public ActionResult GuardarNotas(List<Nota> notas, int IdCurso, int IdBimestre, int IdAsignacion)
        {
            foreach (var nota in notas)
            {
                nota.IdCurso = IdCurso;
                nota.IdBimestre = IdBimestre;
                nota.IdAsignacion = IdAsignacion;
            }

            notaDAO.GuardarNotas(notas);
            TempData["mensaje"] = "Notas registradas correctamente.";

            return RedirectToAction("RegistrarNotas");
        }

        // Utilidades
        private List<SelectListItem> ObtenerNiveles()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "Inicial", Value = "Inicial" },
                new SelectListItem { Text = "Primaria", Value = "Primaria" },
                new SelectListItem { Text = "Secundaria", Value = "Secundaria" }
            };
        }

        private List<SelectListItem> ObtenerGrados()
        {
            return Enumerable.Range(1, 6).Select(i => new SelectListItem
            {
                Text = i.ToString(),
                Value = i.ToString()
            }).ToList();
        }

        private List<SelectListItem> ObtenerSecciones()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "A", Value = "A" },
                new SelectListItem { Text = "B", Value = "B" },
                new SelectListItem { Text = "C", Value = "C" }
            };
        }

        private List<SelectListItem> ObtenerBimestres()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "I", Value = "1" },
                new SelectListItem { Text = "II", Value = "2" },
                new SelectListItem { Text = "III", Value = "3" },
                new SelectListItem { Text = "IV", Value = "4" }
            };
        }

        private List<SelectListItem> ObtenerGradosPersonalizados()
        {
            return new List<SelectListItem> {
        new SelectListItem { Text = "1er", Value = "1er" },
        new SelectListItem { Text = "2do", Value = "2do" },
        new SelectListItem { Text = "3ro", Value = "3ro" },
        new SelectListItem { Text = "4to", Value = "4to" },
        new SelectListItem { Text = "5to", Value = "5to" },
        new SelectListItem { Text = "6to", Value = "6to" }
    };
        }

    }
}