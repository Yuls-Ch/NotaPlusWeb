using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using NotaPlusNew.Models;
using System.ComponentModel;

namespace NotaPlusNew.DAO
{
    public class UsuarioDAO
    {
        string cadena = ConfigurationManager.ConnectionStrings["NotaPlusNew"].ConnectionString;
        public List<Rol> ListarRoles()
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT IdRol, NombreRol FROM Rol", cn);
                SqlDataReader dr = cmd.ExecuteReader();

                List<Rol> lista = new List<Rol>();
                while (dr.Read())
                {
                    lista.Add(new Rol
                    {
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        NombreRol = dr["NombreRol"].ToString()
                    });
                }
                return lista;
            }
        }
        public Usuario ValidarUsuario(string correo)
        {
            Usuario u = null;

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CorreoE", correo);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    u = new Usuario
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        NombreUsuario = dr["NombreUsuario"].ToString(),
                        ContrasenaHash = dr["ContrasenaHash"].ToString(),
                        NombreRol = dr["NombreRol"].ToString()
                    };
                }
            }

            return u;
        }


        // Mantenimiento Usuario
        public int Registrar(Usuario u)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(cadena))
                {
                    //1 : si funciona
                    System.Console.WriteLine("1");

                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@TipoDocumento", u.TipoDocumento);
                    cmd.Parameters.AddWithValue("@NumeroDocumento", u.NumeroDocumento);
                    cmd.Parameters.AddWithValue("@ApellidoMaterno", u.ApellidoMaterno);
                    cmd.Parameters.AddWithValue("@ApellidoPaterno", u.ApellidoPaterno);
                    cmd.Parameters.AddWithValue("@Nombres", u.Nombres);
                    cmd.Parameters.AddWithValue("@Genero", u.Genero);
                    cmd.Parameters.AddWithValue("@EstadoCivil", u.EstadoCivil ?? "");
                    cmd.Parameters.AddWithValue("@FechaNacimiento", u.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Direccion", u.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@Celular", u.Celular ?? "");
                    cmd.Parameters.AddWithValue("@CorreoElectronico", u.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@NombreUsuario", u.NombreUsuario);
                    cmd.Parameters.AddWithValue("@ContrasenaHash", u.ContrasenaHash);
                    cmd.Parameters.AddWithValue("@IdRol", u.IdRol);

                    con.Open();
                    resultado = (int)cmd.ExecuteScalar();
                   
                }
            }
            catch (Exception ex)
            {
                // Puedes loguear el error o mostrarlo en consola durante pruebas
                Console.WriteLine("Error al registrar: " + ex.Message);
                resultado = -99; // Código de error inesperado
            }
            return resultado;
        }
        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT u.*, r.NombreRol FROM Usuario u 
                                                INNER JOIN Rol r ON u.IdRol = r.IdRol
                                                WHERE u.Activo = 1", con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Usuario
                    {
                        IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                        TipoDocumento = dr["TipoDocumento"].ToString(),
                        NumeroDocumento = dr["NumeroDocumento"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        Genero = dr["Genero"].ToString(),
                        EstadoCivil = dr["EstadoCivil"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                        Direccion = dr["Direccion"].ToString(),
                        Celular = dr["Celular"].ToString(),
                        CorreoElectronico = dr["CorreoElectronico"].ToString(),
                        NombreUsuario = dr["NombreUsuario"].ToString(),
                        NombreRol = dr["NombreRol"].ToString()
                    });
                }
            }
            return lista;
        }
        public Usuario obtenerUsuarioPorId(int id)
        {
            Usuario usuario = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarUsuario", cn);
                cmd.Parameters.AddWithValue("@IdUsuario", id);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usuario = new Usuario
                    {
                        IdUsuario = Convert.ToInt16(dr["IdUsuario"]),
                        TipoDocumento = dr["TipoDocumento"].ToString(),
                        NumeroDocumento = dr["NumeroDocumento"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        Genero = dr["Genero"].ToString(),
                        EstadoCivil = dr["EstadoCivil"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                        Direccion = dr["Direccion"].ToString(),
                        Celular = dr["Celular"].ToString(),
                        CorreoElectronico = dr["CorreoElectronico"].ToString(),
                        NombreUsuario = dr["NombreUsuario"].ToString(),
                        IdRol = Convert.ToInt32(dr["IdRol"]),
                        NombreRol = dr["NombreRol"].ToString(),
                        Activo = Convert.ToBoolean(dr["Activo"])
                    };
                }

            }
            return usuario;
        }
      
        public bool EliminarUsuario(int id)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_EliminarUsuario", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdUsuario", id);

                con.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

    }
}