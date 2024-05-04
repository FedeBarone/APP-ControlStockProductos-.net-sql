using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class AccesoDatos
    {
        #region Atributos
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;
        #endregion

        #region Propiedades
        public SqlDataReader Lector { get => lector;}
        #endregion

        #region Constructor
        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true");
            comando = new SqlCommand();
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Setea la consulta de la base de datos
        /// </summary>
        /// <param name="consulta"></param>
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;

        }

        /// <summary>
        /// Lee los datos que se escuentran en la base de datos
        /// </summary>
        public void ejecutarLectura()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Ejecuta una accion en la base de datos
        /// </summary>
        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// Permite cambiar el nombre de los campos de la tabla de manera simple
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="valor"></param>
        public void setearParametros(string nombre, Object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        /// <summary>
        /// Cierra la conexion con la base de datos
        /// </summary>
        public void cerrarConexion()
        {
            if(lector != null)
                lector.Close();
            conexion.Close();
        }

        #endregion
    }
}
