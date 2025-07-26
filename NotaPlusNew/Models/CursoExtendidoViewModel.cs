using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class CursoExtendidoViewModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Area { get; set; }
        public string Descripcion { get; set; }
        public List<HorarioCurso> Horarios { get; set; } = new List<HorarioCurso>();
    }
}