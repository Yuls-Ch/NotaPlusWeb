using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NotaPlusNew.Models;

namespace NotaPlusNew.DAO
{
    public class TipoEvaluacionDAO
    {
        string cadena = ConfigurationManager.ConnectionStrings["NotaPlusNew"].ConnectionString;


        public List<TipoEvaluacion> Listar(int idNivel, int idGrado, int idSeccion, int idCurso)
        {
            List<TipoEvaluacion> lista = new List<TipoEvaluacion>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"SELECT te.IdTipoEvaluacion, te.NombreEvaluacion, te.Peso, te.IdCurso,
                               te.IdGrado, te.IdSeccion, te.IdNivel, c.Nombre AS NombreCurso
                        FROM TipoEvaluacion te
                        JOIN Curso c ON te.IdCurso = c.Id
                        WHERE te.IdNivel = @nivel AND te.IdGrado = @grado
                        AND te.IdSeccion = @seccion AND te.IdCurso = @curso";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nivel", idNivel);
                cmd.Parameters.AddWithValue("@grado", idGrado);
                cmd.Parameters.AddWithValue("@seccion", idSeccion);
                cmd.Parameters.AddWithValue("@curso", idCurso);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new TipoEvaluacion
                    {
                        IdTipoEvaluacion = (int)dr["IdTipoEvaluacion"],
                        NombreEvaluacion = dr["NombreEvaluacion"].ToString(),
                        Peso = Convert.ToDecimal(dr["Peso"]),
                        IdCurso = (int)dr["IdCurso"],
                        IdGrado = (int)dr["IdGrado"],
                        IdSeccion = (int)dr["IdSeccion"],
                        IdNivel = (int)dr["IdNivel"],
                        NombreCurso = dr["NombreCurso"].ToString()
                    });
                }
            }
            return lista;
        }


        public void ActualizarPeso(int id, decimal nuevoPeso)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "UPDATE TipoEvaluacion SET Peso = @peso WHERE IdTipoEvaluacion = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@peso", nuevoPeso);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "DELETE FROM TipoEvaluacion WHERE IdTipoEvaluacion = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Insertar(TipoEvaluacion nuevo)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"INSERT INTO TipoEvaluacion 
                       (NombreEvaluacion, Peso, IdCurso, IdGrado, IdSeccion, IdNivel)
                       VALUES (@nombre, @peso, @curso, @grado, @seccion, @nivel)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nombre", nuevo.NombreEvaluacion);
                cmd.Parameters.AddWithValue("@peso", nuevo.Peso);
                cmd.Parameters.AddWithValue("@curso", nuevo.IdCurso);
                cmd.Parameters.AddWithValue("@grado", nuevo.IdGrado);
                cmd.Parameters.AddWithValue("@seccion", nuevo.IdSeccion);
                cmd.Parameters.AddWithValue("@nivel", nuevo.IdNivel);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public TipoEvaluacion ObtenerPorId(int id)
        {
            TipoEvaluacion tipo = null;
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM TipoEvaluacion WHERE IdTipoEvaluacion = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    tipo = new TipoEvaluacion
                    {
                        IdTipoEvaluacion = (int)dr["IdTipoEvaluacion"],
                        NombreEvaluacion = dr["NombreEvaluacion"].ToString(),
                        Peso = Convert.ToDecimal(dr["Peso"]),
                        IdCurso = (int)dr["IdCurso"],
                        IdGrado = (int)dr["IdGrado"],
                        IdSeccion = (int)dr["IdSeccion"],
                        IdNivel = (int)dr["IdNivel"]
                    };
                }
            }
            return tipo;
        }
        public void Actualizar(TipoEvaluacion tipo)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"UPDATE TipoEvaluacion 
                       SET NombreEvaluacion = @nombre,
                           Peso = @peso
                       WHERE IdTipoEvaluacion = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nombre", tipo.NombreEvaluacion);
                cmd.Parameters.AddWithValue("@peso", tipo.Peso);
                cmd.Parameters.AddWithValue("@id", tipo.IdTipoEvaluacion);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}