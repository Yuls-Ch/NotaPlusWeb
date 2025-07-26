using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NotaPlusNew.Models;

namespace NotaPlusNew.DAO
{
    public class AlumnoDAO
    {
        string cadena = ConfigurationManager.ConnectionStrings["NotaPlusNew"].ConnectionString;

        public List<Alumno> BuscarPorNivelGradoSeccion(string nivel, string grado, string seccion)
        {
            List<Alumno> lista = new List<Alumno>();

            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = @"SELECT * FROM Alumno 
                               WHERE Nivel = @nivel AND Grado = @grado AND Seccion = @seccion";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@nivel", nivel);
                cmd.Parameters.AddWithValue("@grado", grado);
                cmd.Parameters.AddWithValue("@seccion", seccion);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Alumno a = new Alumno
                    {
                        Id = (int)dr["Id"],
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        NumeroIdentificacion = dr["NumeroIdentificacion"].ToString(),
                        Nivel = dr["Nivel"].ToString(),
                        Grado = dr["Grado"].ToString(),
                        Seccion = dr["Seccion"].ToString()
                    };
                    lista.Add(a);
                }
            }

            return lista;
        }

        public Alumno ObtenerPorId(int id)
        {
            Alumno a = null;

            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Alumno WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    a = new Alumno
                    {
                        Id = (int)dr["Id"],
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        NumeroIdentificacion = dr["NumeroIdentificacion"].ToString(),
                        Nivel = dr["Nivel"].ToString(),
                        Grado = dr["Grado"].ToString(),
                        Seccion = dr["Seccion"].ToString()
                    };
                }
            }

            return a;
        }

        public List<Alumno> ListarTodos()
        {
            List<Alumno> lista = new List<Alumno>();

            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Alumno";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Alumno a = new Alumno
                    {
                        Id = (int)dr["Id"],
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        NumeroIdentificacion = dr["NumeroIdentificacion"].ToString(),
                        Nivel = dr["Nivel"].ToString(),
                        Grado = dr["Grado"].ToString(),
                        Seccion = dr["Seccion"].ToString()
                    };
                    lista.Add(a);
                }
            }

            return lista;
        }

        /*******************************************importar nomina*******************************************/
        public void Insertar(Alumno a)
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = @"INSERT INTO Alumno
                       (ApellidoPaterno, ApellidoMaterno, Nombres, NumeroIdentificacion, Nivel, Grado, Seccion,
                        Genero, FechaNacimiento, Direccion, TipoSangre, Apoderado, TelefonoApoderado)
                       VALUES 
                       (@ap, @am, @nom, @dni, @niv, @gra, @sec, @gen, @fec, @dir, @tip, @apo, @tel)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ap", a.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@am", a.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@nom", a.Nombres);
                cmd.Parameters.AddWithValue("@dni", a.NumeroIdentificacion);
                cmd.Parameters.AddWithValue("@niv", a.Nivel);
                cmd.Parameters.AddWithValue("@gra", a.Grado);
                cmd.Parameters.AddWithValue("@sec", a.Seccion);
                cmd.Parameters.AddWithValue("@gen", a.Genero);
                cmd.Parameters.AddWithValue("@fec", a.FechaNacimiento);
                cmd.Parameters.AddWithValue("@dir", a.Direccion);
                cmd.Parameters.AddWithValue("@tip", a.TipoSangre);
                cmd.Parameters.AddWithValue("@apo", a.Apoderado);
                cmd.Parameters.AddWithValue("@tel", a.TelefonoApoderado);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        /*validacion*/
        public bool ExisteAlumno(string dni, string nivel, string grado, string seccion)
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = @"SELECT COUNT(*) FROM Alumno 
                       WHERE NumeroIdentificacion = @dni 
                         AND Nivel = @nivel 
                         AND Grado = @grado 
                         AND Seccion = @seccion";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@dni", dni);
                cmd.Parameters.AddWithValue("@nivel", nivel);
                cmd.Parameters.AddWithValue("@grado", grado);
                cmd.Parameters.AddWithValue("@seccion", seccion);

                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        ////bri
        public List<Alumno> ListarPorProcedimiento(string nivel, string grado, string seccion)
        {
            List<Alumno> lista = new List<Alumno>();

            using (SqlConnection conn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ListarAlumnosPorNivelGradoSeccion", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nivel", nivel);
                cmd.Parameters.AddWithValue("@Grado", grado);
                cmd.Parameters.AddWithValue("@Seccion", seccion);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Alumno a = new Alumno
                    {
                        Id = (int)dr["Id"],
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        NumeroIdentificacion = dr["NumeroIdentificacion"].ToString(),
                        Nivel = dr["Nivel"].ToString(),
                        Grado = dr["Grado"].ToString(),
                        Seccion = dr["Seccion"].ToString()
                    };
                    lista.Add(a);
                }
            }
            return lista;
        }



        public List<Alumno> ListarPorFiltro(string nivel, string grado, string seccion)
        {
            List<Alumno> lista = new List<Alumno>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"SELECT * FROM Alumno 
                       WHERE Nivel = @nivel AND Grado = @grado AND Seccion = @seccion";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nivel", nivel);
                cmd.Parameters.AddWithValue("@grado", grado);
                cmd.Parameters.AddWithValue("@seccion", seccion);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Alumno alumno = new Alumno
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        NumeroIdentificacion = dr["NumeroIdentificacion"].ToString(),
                        Nivel = dr["Nivel"].ToString(),
                        Grado = dr["Grado"].ToString(),
                        Seccion = dr["Seccion"].ToString()
                        // Otros campos si los necesitas
                    };
                    lista.Add(alumno);
                }
            }
            return lista;
        }

        public List<Alumno> ObtenerAlumnos(string nivel, string grado, string seccion)
        {
            List<Alumno> lista = new List<Alumno>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                // 🔧 Aquí es donde debes reemplazar la línea
                string sql = "SELECT * FROM dbo.Alumno WHERE Nivel = @nivel AND Grado = @grado AND Seccion = @seccion";


                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nivel", nivel);
                cmd.Parameters.AddWithValue("@grado", grado);
                cmd.Parameters.AddWithValue("@seccion", seccion);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Alumno
                    {
                        Id = (int)dr["Id"],
                        Nombres = dr["Nombres"].ToString(),
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        NumeroIdentificacion = dr["NumeroIdentificacion"].ToString(),
                        // Y demás campos si los necesitas...
                    });
                }
            }

            return lista;
        }
        public List<Alumno> ObtenerAlumnosPorGrupo(string nivel, string grado, string seccion)
        {
            List<Alumno> alumnos = new List<Alumno>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Alumno WHERE Nivel = @nivel AND Grado = @grado AND Seccion = @seccion";
                SqlCommand cmd = new SqlCommand(sql, con);

                // ✅ Aquí se agregan los parámetros
                cmd.Parameters.AddWithValue("@nivel", nivel);
                cmd.Parameters.AddWithValue("@grado", grado);
                cmd.Parameters.AddWithValue("@seccion", seccion);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Alumno alumno = new Alumno
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString()
                        // Agrega más campos si necesitas
                    };
                    alumnos.Add(alumno);
                }
            }
            return alumnos;
        }


    }
}