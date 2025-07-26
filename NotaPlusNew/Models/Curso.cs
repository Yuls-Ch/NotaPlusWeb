using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class Curso
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese el nombre del curso")]
        public string Nombre { get; set; }
        public string Area { get; set; }
        public string Descripcion { get; set; }

        public List<HorarioCurso> Horarios { get; set; } = new List<HorarioCurso>();
    }
}