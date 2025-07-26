using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotaPlusNew.DAO;

namespace NotaPlusNew.Controllers
{
    public class ConsultarNotasController : Controller
    {
        private AlumnoDAO alumnoDAO = new AlumnoDAO();
        private NotaDAO notaDAO = new NotaDAO();

        [HttpGet]
        public ActionResult ConsultarNotas()
        {
            ViewBag.Niveles = ObtenerNiveles();
            ViewBag.Grados = ObtenerGradosPersonalizados();
            ViewBag.Secciones = ObtenerSecciones();
            return View();
        }

        [HttpPost]
        public ActionResult ConsultarNotas(string Nivel, string Grado, string Seccion, string busqueda)
        {
            ViewBag.Niveles = ObtenerNiveles();
            ViewBag.Grados = ObtenerGradosPersonalizados();
            ViewBag.Secciones = ObtenerSecciones();

            var alumnos = alumnoDAO.ObtenerAlumnosPorGrupo(Nivel, Grado, Seccion);

            var alumno = alumnos.FirstOrDefault(a =>
                a.Nombres.ToLower().Contains(busqueda.ToLower()) ||
                a.ApellidoPaterno.ToLower().Contains(busqueda.ToLower()) ||
                a.ApellidoMaterno.ToLower().Contains(busqueda.ToLower()) ||
                a.NumeroIdentificacion == busqueda
            );

            if (alumno == null)
            {
                ViewBag.Mensaje = "Alumno no encontrado con los datos ingresados.";
                return View();
            }

            var libreta = notaDAO.ObtenerLibretaNotas(alumno.Id);
            ViewBag.Alumno = alumno;
            ViewBag.Libreta = libreta;

            return View();
        }

        // 🔽 Métodos utilitarios igual que en RegistroNotasController
        private List<SelectListItem> ObtenerNiveles()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "Inicial", Value = "Inicial" },
                new SelectListItem { Text = "Primaria", Value = "Primaria" },
                new SelectListItem { Text = "Secundaria", Value = "Secundaria" }
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

        private List<SelectListItem> ObtenerSecciones()
        {
            return new List<SelectListItem> {
                new SelectListItem { Text = "A", Value = "A" },
                new SelectListItem { Text = "B", Value = "B" },
                new SelectListItem { Text = "C", Value = "C" }
            };
        }
    }
}