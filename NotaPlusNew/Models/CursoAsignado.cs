using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class CursoAsignado
    {
        public int Id { get; set; }
        public int DocenteId { get; set; }
        public int CursoId { get; set; }

        public string NombreCurso { get; set; }
        public string Nivel { get; set; }
        public string Grado { get; set; }
        public string Grupo { get; set; }
        public string Horario { get; set; }

    }
}