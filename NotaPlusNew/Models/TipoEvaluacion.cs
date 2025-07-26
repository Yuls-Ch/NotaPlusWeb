using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class TipoEvaluacion
    {
        public int IdTipoEvaluacion { get; set; }
        public string NombreEvaluacion { get; set; }
        public decimal Peso { get; set; }

        public int IdCurso { get; set; }
        public int IdGrado { get; set; }
        public int IdSeccion { get; set; }
        public int IdNivel { get; set; }

        public string NombreCurso { get; set; } 
    }
}