using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NotaPlusNew.Models;

namespace NotaPlusNew.DAO
{
    public class NotaDAO
    {

        string cadena = ConfigurationManager.ConnectionStrings["NotaPlusNew"].ConnectionString;


        public void RegistrarNota(Nota nota)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string sql = @"INSERT INTO Nota (IdAlumno, IdCurso, IdAsignacion, IdBimestre, 
                            TestSalida, TrabajoClase, Tarea, ExamenMensual, ExamenBimestral, 
                            PromedioFinal, FechaRegistro)
                           VALUES (@IdAlumno, @IdCurso, @IdAsignacion, @IdBimestre,
                                   @TestSalida, @TrabajoClase, @Tarea, @ExamenMensual, 
                                   @ExamenBimestral, @PromedioFinal, GETDATE())";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@IdAlumno", nota.IdAlumno);
                cmd.Parameters.AddWithValue("@IdCurso", nota.IdCurso);
                cmd.Parameters.AddWithValue("@IdAsignacion", nota.IdAsignacion);
                cmd.Parameters.AddWithValue("@IdBimestre", nota.IdBimestre);
                cmd.Parameters.AddWithValue("@TestSalida", nota.TestSalida);
                cmd.Parameters.AddWithValue("@TrabajoClase", nota.TrabajoClase);
                cmd.Parameters.AddWithValue("@Tarea", nota.Tarea);
                cmd.Parameters.AddWithValue("@ExamenMensual", nota.ExamenMensual);
                cmd.Parameters.AddWithValue("@ExamenBimestral", nota.ExamenBimestral);
                cmd.Parameters.AddWithValue("@PromedioFinal", nota.PromedioFinal);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public Nota BuscarNotaPorId(int id)
        {
            Nota nota = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string sql = "SELECT * FROM Nota WHERE IdNota = @Id";
                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    nota = new Nota
                    {
                        IdNota = (int)dr["IdNota"],
                        IdAlumno = (int)dr["IdAlumno"],
                        IdCurso = (int)dr["IdCurso"],
                        IdAsignacion = (int)dr["IdAsignacion"],
                        IdBimestre = (int)dr["IdBimestre"],
                        TestSalida = (decimal)dr["TestSalida"],
                        TrabajoClase = (decimal)dr["TrabajoClase"],
                        Tarea = (decimal)dr["Tarea"],
                        ExamenMensual = (decimal)dr["ExamenMensual"],
                        ExamenBimestral = (decimal)dr["ExamenBimestral"],
                        PromedioFinal = (decimal)dr["PromedioFinal"]
                    };
                }
            }
            return nota;
        }

        public void EditarNota(Nota nota)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string sql = @"UPDATE Nota SET
                            TestSalida = @TestSalida,
                            TrabajoClase = @TrabajoClase,
                            Tarea = @Tarea,
                            ExamenMensual = @ExamenMensual,
                            ExamenBimestral = @ExamenBimestral,
                            PromedioFinal = @PromedioFinal
                           WHERE IdNota = @IdNota";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@IdNota", nota.IdNota);
                cmd.Parameters.AddWithValue("@TestSalida", nota.TestSalida);
                cmd.Parameters.AddWithValue("@TrabajoClase", nota.TrabajoClase);
                cmd.Parameters.AddWithValue("@Tarea", nota.Tarea);
                cmd.Parameters.AddWithValue("@ExamenMensual", nota.ExamenMensual);
                cmd.Parameters.AddWithValue("@ExamenBimestral", nota.ExamenBimestral);
                cmd.Parameters.AddWithValue("@PromedioFinal", nota.PromedioFinal);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void GuardarNotas(List<Nota> notas)
        {
            using (SqlConnection con = new SqlConnection(cadena))
            {
                con.Open();
                foreach (var nota in notas)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Nota(IdAlumno, IdCurso, IdAsignacion, IdBimestre, TestSalida, TrabajoClase, Tarea, ExamenMensual, ExamenBimestral, PromedioFinal) " +
                        "VALUES(@IdAlumno, @IdCurso, @IdAsignacion, @IdBimestre, @TestSalida, @TrabajoClase, @Tarea, @ExamenMensual, @ExamenBimestral, @PromedioFinal)", con);

                    cmd.Parameters.AddWithValue("@IdAlumno", nota.IdAlumno);
                    cmd.Parameters.AddWithValue("@IdCurso", nota.IdCurso);
                    cmd.Parameters.AddWithValue("@IdAsignacion", nota.IdAsignacion);
                    cmd.Parameters.AddWithValue("@IdBimestre", nota.IdBimestre);
                    cmd.Parameters.AddWithValue("@TestSalida", nota.TestSalida);
                    cmd.Parameters.AddWithValue("@TrabajoClase", nota.TrabajoClase);
                    cmd.Parameters.AddWithValue("@Tarea", nota.Tarea);
                    cmd.Parameters.AddWithValue("@ExamenMensual", nota.ExamenMensual);
                    cmd.Parameters.AddWithValue("@ExamenBimestral", nota.ExamenBimestral);
                    cmd.Parameters.AddWithValue("@PromedioFinal", nota.PromedioFinal);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Nota> ObtenerLibretaNotas(int idAlumno)
        {
            List<Nota> lista = new List<Nota>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                string sql = @"SELECT n.IdAlumno, n.IdCurso, n.IdBimestre, n.PromedioFinal,
                              c.Nombre AS NombreCurso
                       FROM Nota n
                       INNER JOIN Curso c ON n.IdCurso = c.Id
                       WHERE n.IdAlumno = @idAlumno";

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.Parameters.AddWithValue("@idAlumno", idAlumno);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Nota nota = new Nota
                    {
                        IdAlumno = (int)dr["IdAlumno"],
                        IdCurso = (int)dr["IdCurso"],
                        IdBimestre = (int)dr["IdBimestre"],
                        PromedioFinal = Convert.ToDecimal(dr["PromedioFinal"]),
                        NombreCurso = dr["NombreCurso"].ToString()
                    };
                    lista.Add(nota);
                }
            }
            return lista;
        }

    }
}