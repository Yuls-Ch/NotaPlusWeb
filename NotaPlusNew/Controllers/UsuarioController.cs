using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using NotaPlusNew.Models;
using NotaPlusNew.DAO;

namespace NotaPlusNew.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioDAO daoUsu = new UsuarioDAO();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string correo, string clave)
        {
            Usuario u = daoUsu.ValidarUsuario(correo);

            if (u != null && BCrypt.Net.BCrypt.Verify(clave, u.ContrasenaHash))
            {
                Session["usuario"] = u.NombreUsuario;
                Session["rol"] = u.NombreRol;

                if (u.NombreRol == "Administrador")
                    return RedirectToAction("VistaAdmin");
                else if (u.NombreRol == "Educador")
                    return RedirectToAction("VistaEducador");
            }

            ViewBag.Error = "Correo o contraseña incorrectos";
            return View();
        }


        public ActionResult VistaAdmin()
        {
            if (Session["usuario"] == null || Session["rol"] == null || Session["rol"].ToString() != "Administrador")
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult VistaEducador()
        {
            if (Session["usuario"] == null || Session["rol"] == null || Session["rol"].ToString() != "Educador")
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // MANTENIMIENTO USUARIO

        [HttpGet]
        public ActionResult Registrar()
        {
            ViewBag.ListaRoles = new SelectList(daoUsu.ListarRoles(), "IdRol", "NombreRol");
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario u)
        {
            if (u.Contrasena != u.ConfirmarClave)
            {
                System.Console.WriteLine("2");
                ViewBag.Mensaje = "Las contraseñas no coinciden.";
                ViewBag.ListaRoles = new SelectList(daoUsu.ListarRoles(), "IdRol", "NombreRol");
                return View(u);
            }

            if (ModelState.IsValid)
            {
                System.Console.WriteLine("3");
                u.ContrasenaHash = BCrypt.Net.BCrypt.HashPassword(u.Contrasena);
                int resultado = daoUsu.Registrar(u);

                if (resultado == 1)
                    ViewBag.Mensaje = "Usuario registrado correctamente";
                else if (resultado == -1)
                    ViewBag.Mensaje = "Ya existe un usuario con ese nombre o documento";
                else
                    ViewBag.Mensaje = "Ocurrió un error inesperado. Intenta nuevamente.";


                return RedirectToAction("Login");
            }

            System.Console.WriteLine("4");
            ViewBag.ListaRoles = new SelectList(daoUsu.ListarRoles(), "IdRol", "NombreRol");
            return View(u);
            
        }

        public ActionResult ListarUsuarios()
        {
            var lista = daoUsu.ListarUsuarios();
            return View(lista);
        }

        public ActionResult DetalleUsuario(int id)
        {
            Usuario usuario = daoUsu.obtenerUsuarioPorId(id);
            if (usuario == null)
            {
                TempData["Error"] = "El usuario no fue encontrado";
                return RedirectToAction("ListarUsuarios");
            }

            ViewBag.Roles = daoUsu.ListarRoles();
            return View(usuario);
        }


        [HttpPost]
        public ActionResult Eliminar(int id) {
            bool eliminado = daoUsu.EliminarUsuario(id);
            if (eliminado)
                TempData["Exito"] = "Usuario eliminado correctamente.";
            else
                TempData["Error"] = "No se pudo eliminar el usuario.";

            return RedirectToAction("ListarUsuarios");
        }
    }
}