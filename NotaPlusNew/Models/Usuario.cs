using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotaPlusNew.Models
{
    public class Usuario
    {
        [Display(Name = "Id Usuario")]
        public int IdUsuario { get; set; }

        [Display(Name = "Tipo de Documento"), Required]
        public string TipoDocumento { get; set; }

        [Display(Name = "Número de Documento"), Required]
        public string NumeroDocumento { get; set; }

        [Display(Name = "Apellido Materno"), Required]
        public string ApellidoMaterno { get; set; }

        [Display(Name = "Apellido Paterno"),Required]
        public string ApellidoPaterno { get; set; }

        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Genero { get; set; }

        [Display(Name = "Estado Civil")]
        public string EstadoCivil { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        public string Direccion { get; set; }

        public string Celular { get; set; }

        [Display(Name = "Correo Electrónico"), Required]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Nombre de Usuario"),Required]
        public string NombreUsuario { get; set; }

        public string ContrasenaHash { get; set; }

        [Display(Name = "Fecha de Registro")]
        public DateTime? FechaRegistro { get; set; }

        [Display(Name = "Último Acceso")]
        public DateTime? UltimoAcceso { get; set; }
        public bool Activo { get; set; }

        public int IdRol { get; set; }
        public string NombreRol { get; set; }

        // Campos auxiliares
        [NotMapped]
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }

        [NotMapped]
        [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        [DataType(DataType.Password)]
        public string ConfirmarClave { get; set; }
    }
}