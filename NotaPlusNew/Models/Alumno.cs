using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class Alumno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Apellido Paterno es obligatorio.")]
        public string ApellidoPaterno { get; set; }

        [Required(ErrorMessage = "El Apellido Materno es obligatorio.")]
        public string ApellidoMaterno { get; set; }

        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [StringLength(8, ErrorMessage = "El DNI debe tener 8 dígitos.")]
        public string NumeroIdentificacion { get; set; }

        [Required(ErrorMessage = "Seleccione el Nivel.")]
        public string Nivel { get; set; }

        [Required(ErrorMessage = "Seleccione el Grado.")]
        public string Grado { get; set; }

        [Required(ErrorMessage = "Seleccione la Sección.")]
        public string Seccion { get; set; }

        [Required(ErrorMessage = "Seleccione el género.")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "Ingrese la fecha de nacimiento.")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Ingrese el tipo de sangre.")]
        public string TipoSangre { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre del apoderado.")]
        public string Apoderado { get; set; }

        [Required(ErrorMessage = "Ingrese el teléfono del apoderado.")]
        public string TelefonoApoderado { get; set; }
    }
}