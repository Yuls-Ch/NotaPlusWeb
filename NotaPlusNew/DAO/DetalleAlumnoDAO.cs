using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NotaPlusNew.Models;

namespace NotaPlusNew.DAO
{
    public class DetalleAlumnoDAO
    {
        string cadena = ConfigurationManager.ConnectionStrings["NotaPlusNew"].ConnectionString;

        // Listar todos los alumnos
        public List<Alumno> ListarTodos()
        {
            List<Alumno> lista = new List<Alumno>();

            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Alumno ORDER BY ApellidoPaterno, ApellidoMaterno";
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
                        Seccion = dr["Seccion"].ToString(),
                        Genero = dr["Genero"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                        Direccion = dr["Direccion"].ToString(),
                        TipoSangre = dr["TipoSangre"].ToString(),
                        Apoderado = dr["Apoderado"].ToString(),
                        TelefonoApoderado = dr["TelefonoApoderado"].ToString()
                    };
                    lista.Add(a);
                }
            }

            return lista;
        }

        // Obtener un alumno por ID
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
                        Seccion = dr["Seccion"].ToString(),
                        Genero = dr["Genero"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                        Direccion = dr["Direccion"].ToString(),
                        TipoSangre = dr["TipoSangre"].ToString(),
                        Apoderado = dr["Apoderado"].ToString(),
                        TelefonoApoderado = dr["TelefonoApoderado"].ToString()
                    };
                }
            }

            return a;
        }

        // Insertar un nuevo alumno
        public void Insertar(Alumno alumno)
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = @"INSERT INTO Alumno 
                    (ApellidoPaterno, ApellidoMaterno, Nombres, NumeroIdentificacion, Nivel, Grado, Seccion,
                     Genero, FechaNacimiento, Direccion, TipoSangre, Apoderado, TelefonoApoderado)
                    VALUES 
                    (@ap, @am, @nom, @dni, @niv, @gra, @sec, @gen, @fec, @dir, @tip, @apo, @tel)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ap", alumno.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@am", alumno.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@nom", alumno.Nombres);
                cmd.Parameters.AddWithValue("@dni", alumno.NumeroIdentificacion);
                cmd.Parameters.AddWithValue("@niv", alumno.Nivel);
                cmd.Parameters.AddWithValue("@gra", alumno.Grado);
                cmd.Parameters.AddWithValue("@sec", alumno.Seccion);
                cmd.Parameters.AddWithValue("@gen", alumno.Genero);
                cmd.Parameters.AddWithValue("@fec", alumno.FechaNacimiento);
                cmd.Parameters.AddWithValue("@dir", alumno.Direccion);
                cmd.Parameters.AddWithValue("@tip", alumno.TipoSangre);
                cmd.Parameters.AddWithValue("@apo", alumno.Apoderado);
                cmd.Parameters.AddWithValue("@tel", alumno.TelefonoApoderado);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Actualizar alumno
        public void Actualizar(Alumno alumno)
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = @"UPDATE Alumno SET 
                    ApellidoPaterno = @ap,
                    ApellidoMaterno = @am,
                    Nombres = @nom,
                    NumeroIdentificacion = @dni,
                    Nivel = @niv,
                    Grado = @gra,
                    Seccion = @sec,
                    Genero = @gen,
                    FechaNacimiento = @fec,
                    Direccion = @dir,
                    TipoSangre = @tip,
                    Apoderado = @apo,
                    TelefonoApoderado = @tel
                    WHERE Id = @id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", alumno.Id);
                cmd.Parameters.AddWithValue("@ap", alumno.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@am", alumno.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@nom", alumno.Nombres);
                cmd.Parameters.AddWithValue("@dni", alumno.NumeroIdentificacion);
                cmd.Parameters.AddWithValue("@niv", alumno.Nivel);
                cmd.Parameters.AddWithValue("@gra", alumno.Grado);
                cmd.Parameters.AddWithValue("@sec", alumno.Seccion);
                cmd.Parameters.AddWithValue("@gen", alumno.Genero);
                cmd.Parameters.AddWithValue("@fec", alumno.FechaNacimiento);
                cmd.Parameters.AddWithValue("@dir", alumno.Direccion);
                cmd.Parameters.AddWithValue("@tip", alumno.TipoSangre);
                cmd.Parameters.AddWithValue("@apo", alumno.Apoderado);
                cmd.Parameters.AddWithValue("@tel", alumno.TelefonoApoderado);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar alumno
        public void Eliminar(int id)
        {
            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = "DELETE FROM Alumno WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        /*Filtro*/
        public List<Alumno> BuscarPorNivelGradoSeccion(string nivel, string grado, string seccion)
        {
            List<Alumno> lista = new List<Alumno>();

            using (SqlConnection conn = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Alumno WHERE 1=1";

                if (!string.IsNullOrEmpty(nivel))
                    sql += " AND Nivel = @nivel";

                if (!string.IsNullOrEmpty(grado))
                    sql += " AND Grado = @grado";

                if (!string.IsNullOrEmpty(seccion))
                    sql += " AND Seccion = @seccion";

                SqlCommand cmd = new SqlCommand(sql, conn);

                if (!string.IsNullOrEmpty(nivel))
                    cmd.Parameters.AddWithValue("@nivel", nivel);

                if (!string.IsNullOrEmpty(grado))
                    cmd.Parameters.AddWithValue("@grado", grado);

                if (!string.IsNullOrEmpty(seccion))
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
                        Seccion = dr["Seccion"].ToString(),
                        Genero = dr["Genero"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                        Direccion = dr["Direccion"].ToString(),
                        TipoSangre = dr["TipoSangre"].ToString(),
                        Apoderado = dr["Apoderado"].ToString(),
                        TelefonoApoderado = dr["TelefonoApoderado"].ToString()
                    };
                    lista.Add(a);
                }
            }

            return lista;
        }
    }
}