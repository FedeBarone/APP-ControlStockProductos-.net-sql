using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class MarcaNegocio
    {
        #region Metodos
        public List<Marca> listarMarcas()
        {
            List<Marca> listaMarcas = new List<Marca>();
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.setearConsulta("SELECT Id, Descripcion FROM MARCAS");
                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    Marca marcaAux = new Marca();
                    
                    marcaAux.Id = (int)accesoDatos.Lector["Id"];
                    marcaAux.Descripcion = (string)accesoDatos.Lector["Descripcion"];

                    listaMarcas.Add(marcaAux);
                }
               return listaMarcas;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                accesoDatos.cerrarConexion();
            }

        }
        #endregion

    }
}
