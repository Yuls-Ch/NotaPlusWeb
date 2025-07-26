using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class DocenteViewModel
    {
        public Docente Docente { get; set; }
        public List<Curso> CursosDisponibles { get; set; }
        public List<CursoAsignado> CursosAsignados { get; set; }

    }
}