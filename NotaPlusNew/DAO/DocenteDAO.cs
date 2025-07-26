using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NotaPlusNew.Models;

namespace NotaPlusNew.DAO
{
    public class DocenteDAO
    {
        private string cadena = ConfigurationManager.ConnectionStrings["NotaPlusNew"].ConnectionString;

        // Obtener docente por ID
        public Docente ObtenerPorId(int id)
        {
            Docente d = null;
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Docente WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    d = new Docente
                    {
                        Id = (int)dr["Id"],
                        ApellidoPaterno = dr["ApellidoPaterno"].ToString(),
                        ApellidoMaterno = dr["ApellidoMaterno"].ToString(),
                        Nombres = dr["Nombres"].ToString(),
                        Genero = dr["Genero"].ToString(),
                        EstadoCivil = dr["EstadoCivil"].ToString(),
                        FechaNacimiento = DateTime.Parse(dr["FechaNacimiento"].ToString()),
                        Direccion = dr["Direccion"].ToString(),
                        Celular = dr["Celular"].ToString(),
                        Correo = dr["Correo"].ToString(),
                        GradoAcademico = dr["GradoAcademico"].ToString(),
                        AreaEnsenanza = dr["AreaEnsenanza"].ToString(),
                        TipoDocente = dr["TipoDocente"].ToString(),
                        TipoContrato = dr["TipoContrato"].ToString()
                    };
                }
            }
            return d;
        }

        // Actualizar datos del docente
        public void Actualizar(Docente d)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"UPDATE Docente SET
                                ApellidoPaterno = @ap,
                                ApellidoMaterno = @am,
                                Nombres = @nombres,
                                Genero = @genero,
                                EstadoCivil = @ec,
                                FechaNacimiento = @fn,
                                Direccion = @dir,
                                Celular = @cel,
                                Correo = @correo,
                                GradoAcademico = @ga,
                                AreaEnsenanza = @ae,
                                TipoDocente = @td,
                                TipoContrato = @tc
                               WHERE Id = @id";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ap", d.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@am", d.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@nombres", d.Nombres);
                cmd.Parameters.AddWithValue("@genero", d.Genero);
                cmd.Parameters.AddWithValue("@ec", d.EstadoCivil);
                cmd.Parameters.AddWithValue("@fn", d.FechaNacimiento);
                cmd.Parameters.AddWithValue("@dir", d.Direccion);
                cmd.Parameters.AddWithValue("@cel", d.Celular);
                cmd.Parameters.AddWithValue("@correo", d.Correo);
                cmd.Parameters.AddWithValue("@ga", d.GradoAcademico);
                cmd.Parameters.AddWithValue("@ae", d.AreaEnsenanza);
                cmd.Parameters.AddWithValue("@td", d.TipoDocente);
                cmd.Parameters.AddWithValue("@tc", d.TipoContrato);
                cmd.Parameters.AddWithValue("@id", d.Id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // (Opcional) Listar docentes para el Index
        public List<Docente> Listar()
        {
            List<Docente> lista = new List<Docente>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Docente";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Docente
                    {
                        Id = (int)dr["Id"],
                        Nombres = dr["Nombres"].ToString(),
                        Correo = dr["Correo"].ToString()
                    });
                }
            }
            return lista;
        }

        /**********************************************************************************************************/

        // Registrar nuevo docente (retorna ID generado)
        public int Registrar(Docente d)
        {
            int nuevoId = 0;
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"INSERT INTO Docente 
                       (ApellidoPaterno, ApellidoMaterno, Nombres, Genero, EstadoCivil, FechaNacimiento, Direccion,
                        Celular, Correo, GradoAcademico, AreaEnsenanza, TipoDocente, TipoContrato)
                       VALUES
                       (@ap, @am, @nombres, @genero, @ec, @fn, @dir, @cel, @correo, @ga, @ae, @td, @tc);
                       SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@ap", d.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@am", d.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@nombres", d.Nombres);
                cmd.Parameters.AddWithValue("@genero", d.Genero);
                cmd.Parameters.AddWithValue("@ec", d.EstadoCivil);
                cmd.Parameters.AddWithValue("@fn", d.FechaNacimiento);
                cmd.Parameters.AddWithValue("@dir", d.Direccion);
                cmd.Parameters.AddWithValue("@cel", d.Celular);
                cmd.Parameters.AddWithValue("@correo", d.Correo);
                cmd.Parameters.AddWithValue("@ga", d.GradoAcademico);
                cmd.Parameters.AddWithValue("@ae", d.AreaEnsenanza);
                cmd.Parameters.AddWithValue("@td", d.TipoDocente);
                cmd.Parameters.AddWithValue("@tc", d.TipoContrato);

                con.Open();
                nuevoId = Convert.ToInt32(cmd.ExecuteScalar()); // ← Aquí capturas el ID insertado
            }
            return nuevoId;
        }

        // Eliminar docente
        public void Eliminar(int id)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "DELETE FROM Docente WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}