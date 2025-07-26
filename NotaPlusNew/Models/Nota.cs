using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class Nota
    {
        public int IdNota { get; set; }

        public int IdAlumno { get; set; }
        public int IdCurso { get; set; }
        public int IdAsignacion { get; set; }
        public int IdBimestre { get; set; }

        public decimal TestSalida { get; set; }
        public decimal TrabajoClase { get; set; }
        public decimal Tarea { get; set; }
        public decimal ExamenMensual { get; set; }
        public decimal ExamenBimestral { get; set; }
        public decimal PromedioFinal { get; set; }

        public string NombreCurso { get; set; }
        public DateTime FechaRegistro { get; set; }


    }
}