using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotaPlusNew.DAO;
using NotaPlusNew.Models;

namespace NotaPlusNew.Controllers
{
    public class ImportarAlumnosController : Controller
    {
        public ActionResult Index()
        {
            return View(new List<Alumno>());
        }

        [HttpPost]
        public ActionResult Guardar(List<Alumno> alumnos, string nivel, string grado, string seccion)
        {
            if (alumnos == null || alumnos.Count == 0)
            {
                ViewBag.Mensaje = "⚠ No se recibieron datos de alumnos.";
                return View("Index", new List<Alumno>());
            }

            AlumnoDAO dao = new AlumnoDAO();
            List<Alumno> noInsertados = new List<Alumno>();
            int insertados = 0;

            foreach (var alumno in alumnos)
            {
                // Validamos por DNI + Nivel + Grado + Sección
                if (dao.ExisteAlumno(alumno.NumeroIdentificacion, nivel, grado, seccion))
                {
                    noInsertados.Add(alumno);
                    continue;
                }

                alumno.Nivel = nivel;
                alumno.Grado = grado;
                alumno.Seccion = seccion;

                dao.Insertar(alumno);
                insertados++;
            }

            // Mensaje final
            string mensaje = $"✅ {insertados} alumno(s) fueron importados correctamente.";

            if (noInsertados.Count > 0)
            {
                mensaje += "<br /><strong class='text-danger'>⚠ Los siguientes alumnos ya existen y no se importaron:</strong><ul>";
                foreach (var alumno in noInsertados)
                {
                    mensaje += $"<li>{alumno.Nombres} {alumno.ApellidoPaterno} ({alumno.NumeroIdentificacion})</li>";
                }
                mensaje += "</ul>";
            }

            ViewBag.Mensaje = mensaje;

            return View("Index", new List<Alumno>());
        }
    }
}