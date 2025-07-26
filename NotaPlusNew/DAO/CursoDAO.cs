using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NotaPlusNew.Models;

namespace NotaPlusNew.DAO
{
    public class CursoDAO
    {
        string cadena = ConfigurationManager.ConnectionStrings["NotaPlusNew"].ConnectionString;

        // Obtener todos los cursos disponibles
        public List<Curso> ListarCursos()
        {
            List<Curso> lista = new List<Curso>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "SELECT Id, Nombre FROM Curso";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Curso
                    {
                        Id = (int)dr["Id"],
                        Nombre = dr["Nombre"].ToString()
                    });
                }
            }
            return lista;
        }

        // Obtener cursos asignados a un docente
        public List<CursoAsignado> ObtenerPorDocente(int docenteId)
        {
            List<CursoAsignado> lista = new List<CursoAsignado>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"SELECT ca.Id, ca.CursoId, c.Nombre AS NombreCurso,
                                      ca.Nivel, ca.Grado, ca.Grupo, ca.Horario
                               FROM CursoAsignado ca
                               INNER JOIN Curso c ON ca.CursoId = c.Id
                               WHERE ca.DocenteId = @id";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", docenteId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new CursoAsignado
                    {
                        Id = (int)dr["Id"],
                        CursoId = (int)dr["CursoId"],
                        NombreCurso = dr["NombreCurso"].ToString(),
                        Nivel = dr["Nivel"].ToString(),
                        Grado = dr["Grado"].ToString(),
                        Grupo = dr["Grupo"].ToString(),
                        Horario = dr["Horario"].ToString()
                    });
                }
            }
            return lista;
        }

        // Insertar un curso asignado
        public void InsertarCursoAsignado(CursoAsignado curso)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"INSERT INTO CursoAsignado 
                               (DocenteId, CursoId, Nivel, Grado, Grupo, Horario)
                               VALUES (@DocenteId, @CursoId, @Nivel, @Grado, @Grupo, @Horario)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@DocenteId", curso.DocenteId);
                cmd.Parameters.AddWithValue("@CursoId", curso.CursoId);
                cmd.Parameters.AddWithValue("@Nivel", curso.Nivel);
                cmd.Parameters.AddWithValue("@Grado", curso.Grado);
                cmd.Parameters.AddWithValue("@Grupo", curso.Grupo);
                cmd.Parameters.AddWithValue("@Horario", curso.Horario);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Eliminar cursos asignados por docente
        public void EliminarCursosPorDocente(int docenteId)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "DELETE FROM CursoAsignado WHERE DocenteId = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", docenteId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /**************************************************************************************************************/



        public List<Curso> Listar()
        {
            List<Curso> lista = new List<Curso>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Curso";
                SqlCommand cmd = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Curso
                    {
                        Id = (int)dr["Id"],
                        Nombre = dr["Nombre"].ToString(),
                        Area = dr["Area"].ToString(),
                        Descripcion = dr["Descripcion"].ToString()
                    });
                }
            }
            return lista;
        }

        public Curso ObtenerPorId(int id)
        {
            Curso curso = null;
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Curso WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    curso = new Curso
                    {
                        Id = (int)dr["Id"],
                        Nombre = dr["Nombre"].ToString(),
                        Area = dr["Area"].ToString(),
                        Descripcion = dr["Descripcion"].ToString()
                    };
                }
            }

            if (curso != null)
                curso.Horarios = ListarHorarios(curso.Id);

            return curso;
        }

        public int Insertar(Curso curso)
        {
            int nuevoId = 0;

            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"INSERT INTO Curso (Nombre, Area, Descripcion)
                       VALUES (@Nombre, @Area, @Descripcion);
                       SELECT SCOPE_IDENTITY();";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Nombre", curso.Nombre);
                cmd.Parameters.AddWithValue("@Area", curso.Area);
                cmd.Parameters.AddWithValue("@Descripcion", curso.Descripcion);

                con.Open();
                nuevoId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return nuevoId;
        }

        public void Actualizar(Curso curso)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"UPDATE Curso SET Nombre=@nombre, Area=@area, Descripcion=@desc WHERE Id=@id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@nombre", curso.Nombre);
                cmd.Parameters.AddWithValue("@area", curso.Area);
                cmd.Parameters.AddWithValue("@desc", curso.Descripcion);
                cmd.Parameters.AddWithValue("@id", curso.Id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            EliminarHorarios(curso.Id);
            InsertarHorarios(curso);
        }

        public void Eliminar(int id)
        {
            EliminarHorarios(id);
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "DELETE FROM Curso WHERE Id = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private List<HorarioCurso> ListarHorarios(int cursoId)
        {
            List<HorarioCurso> lista = new List<HorarioCurso>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM HorarioCurso WHERE CursoId = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", cursoId);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new HorarioCurso
                    {
                        Id = (int)dr["Id"],
                        CursoId = (int)dr["CursoId"],
                        Grupo = dr["Grupo"].ToString(),
                        Dia = dr["Dia"].ToString(),
                        HoraInicio = dr["HoraInicio"].ToString(),
                        HoraFin = dr["HoraFin"].ToString()
                    });
                }
            }
            return lista;
        }

        private void InsertarHorarios(Curso curso)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                con.Open();
                foreach (var h in curso.Horarios)
                {
                    string sql = @"INSERT INTO HorarioCurso (CursoId, Grupo, Dia, HoraInicio, HoraFin)
                               VALUES (@cursoId, @grupo, @dia, @hini, @hfin)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@cursoId", curso.Id);
                    cmd.Parameters.AddWithValue("@grupo", h.Grupo);
                    cmd.Parameters.AddWithValue("@dia", h.Dia);
                    cmd.Parameters.AddWithValue("@hini", h.HoraInicio);
                    cmd.Parameters.AddWithValue("@hfin", h.HoraFin);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void InsertarHorario(HorarioCurso horario)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = @"INSERT INTO HorarioCurso (CursoId, Grupo, Dia, HoraInicio, HoraFin)
                       VALUES (@CursoId, @Grupo, @Dia, @HoraInicio, @HoraFin)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@CursoId", horario.CursoId);
                cmd.Parameters.AddWithValue("@Grupo", horario.Grupo);
                cmd.Parameters.AddWithValue("@Dia", horario.Dia);
                cmd.Parameters.AddWithValue("@HoraInicio", horario.HoraInicio);
                cmd.Parameters.AddWithValue("@HoraFin", horario.HoraFin);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }


        private void EliminarHorarios(int cursoId)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "DELETE FROM HorarioCurso WHERE CursoId = @id";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", cursoId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void EliminarCursosAsignadosPorCurso(int cursoId)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                string sql = "DELETE FROM CursoAsignado WHERE CursoId = @cursoId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@cursoId", cursoId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}