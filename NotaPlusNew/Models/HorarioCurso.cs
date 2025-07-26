using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class HorarioCurso
    {
        public int Id { get; set; }
        public int CursoId { get; set; }

        public string Grupo { get; set; }
        public string Dia { get; set; }
        public string HoraInicio { get; set; } // Usa formato string "11:00 AM"
        public string HoraFin { get; set; }
    }
}