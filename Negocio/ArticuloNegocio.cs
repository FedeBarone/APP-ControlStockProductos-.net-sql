using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ArticuloNegocio
    {
        #region Metodos

        /// <summary>
        /// Muestra los articulos de la base de datos
        /// </summary>
        /// <returns>Devuelve una lista de articulos</returns>
        public List<Articulo> listarArticulos()
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            AccesoDatos accesoDatos = new AccesoDatos();

            try
            {
                accesoDatos.setearConsulta("SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, M.Descripcion Marca, C.Descripcion Categoria, A.ImagenUrl, A.Precio FROM ARTICULOS A INNER JOIN MARCAS M ON M.Id = A.IdMarca INNER JOIN CATEGORIAS C ON C.Id = A.IdCategoria");
                accesoDatos.ejecutarLectura();

                while(accesoDatos.Lector.Read())
                {
                    Articulo articuloAux = new Articulo();
                    articuloAux.Marca = new Marca();
                    articuloAux.Categoria = new Categoria();

                    articuloAux.Id = (int)accesoDatos.Lector["Id"];
                    articuloAux.Codigo = (string)accesoDatos.Lector["Codigo"];
                    articuloAux.Nombre = (string)accesoDatos.Lector["Nombre"];
                    articuloAux.Descripcion = (string)accesoDatos.Lector["Descripcion"];
                    articuloAux.Marca.Id = (int)accesoDatos.Lector["IdMarca"];
                    articuloAux.Marca.Descripcion = (string)accesoDatos.Lector["Marca"];
                    articuloAux.Categoria.Id = (int)accesoDatos.Lector["IdCategoria"];
                    articuloAux.Categoria.Descripcion = (string)accesoDatos.Lector["Categoria"];
                    articuloAux.ImagenUrl = (string)accesoDatos.Lector["ImagenUrl"];
                    articuloAux.Precio = (decimal)accesoDatos.Lector["Precio"];

                    listaArticulos.Add(articuloAux);
                }
                return listaArticulos;

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

        /// <summary>
        /// Agrega un articulo a la base de datos
        /// </summary>
        /// <param name="articuloAgregar"></param>
        public void agregarArticulo(Articulo articuloAAgregar)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.setearConsulta("INSERT INTO ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)Values(@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @imagenUrl, @precio)");
                accesoDatos.setearParametros("@codigo", articuloAAgregar.Codigo);
                accesoDatos.setearParametros("@nombre", articuloAAgregar.Nombre);
                accesoDatos.setearParametros("@descripcion", articuloAAgregar.Descripcion);
                accesoDatos.setearParametros("@idMarca", articuloAAgregar.Marca.Id);
                accesoDatos.setearParametros("@idCategoria", articuloAAgregar.Categoria.Id);
                accesoDatos.setearParametros("@imagenUrl", articuloAAgregar.ImagenUrl);
                accesoDatos.setearParametros("@precio", articuloAAgregar.Precio);
                accesoDatos.ejecutarAccion();

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

        /// <summary>
        /// Modifica un articulo de la base de datos
        /// </summary>
        /// <param name="articuloModificar"></param>
        public void modificarArticulo(Articulo articuloModificar)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.setearConsulta("UPDATE ARTICULOS SET Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @imagenUrl, Precio = @precio WHERE Id = @id");
                accesoDatos.setearParametros("@codigo", articuloModificar.Codigo);
                accesoDatos.setearParametros("@nombre", articuloModificar.Nombre);
                accesoDatos.setearParametros("@descripcion", articuloModificar.Descripcion);
                accesoDatos.setearParametros("@idMarca", articuloModificar.Marca.Id);
                accesoDatos.setearParametros("@idCategoria", articuloModificar.Categoria.Id);
                accesoDatos.setearParametros("@imagenUrl", articuloModificar.ImagenUrl);
                accesoDatos.setearParametros("@precio", articuloModificar.Precio);
                accesoDatos.setearParametros("@id", articuloModificar.Id);
                accesoDatos.ejecutarAccion();

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

        /// <summary>
        /// Elimina un articulo de la base de datos
        /// </summary>
        /// <param name="articuloEliminar"></param>
        public void eliminarArticulo(int id)
        {
            AccesoDatos accesoDatos = new AccesoDatos();
            try
            {
                accesoDatos.setearConsulta("DELETE FROM ARTICULOS WHERE Id = @id");
                accesoDatos.setearParametros("@id", id);
                accesoDatos.ejecutarAccion();
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

        /// <summary>
        /// Filtra articulos de la base de datos por distintos criterios
        /// </summary>
        /// <param name="listaArticulos"></param>
        /// <returns></returns>
        public List<Articulo> filtrarArticulos(string campo, string criterio, string filtro)
        {
            List<Articulo> listaArticulos = new List<Articulo>();
            AccesoDatos accesoDatos = new AccesoDatos();

            try
            {
                string consulta = "SELECT A.Id, A.Codigo, A.Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, M.Descripcion Marca, C.Descripcion Categoria, A.ImagenUrl, A.Precio FROM ARTICULOS A INNER JOIN MARCAS M ON M.Id = A.IdMarca INNER JOIN CATEGORIAS C ON C.Id = A.IdCategoria AND ";

                if (campo == "A.Codigo")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Codigo like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "A.Codigo like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Codigo like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "A.Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Nombre like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "A.Nombre like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                else
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "A.Descripcion like '" + filtro + "%' ";
                            break;
                        case "Termina con":
                            consulta += "A.Descripcion like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "A.Descripcion like '%" + filtro + "%'";
                            break;
                    }
                }

                accesoDatos.setearConsulta(consulta);
                accesoDatos.ejecutarLectura();

                while (accesoDatos.Lector.Read())
                {
                    Articulo articuloAux = new Articulo();
                    articuloAux.Marca = new Marca();
                    articuloAux.Categoria = new Categoria();

                    articuloAux.Id = (int)accesoDatos.Lector["Id"];
                    articuloAux.Codigo = (string)accesoDatos.Lector["Codigo"];
                    articuloAux.Nombre = (string)accesoDatos.Lector["Nombre"];
                    articuloAux.Descripcion = (string)accesoDatos.Lector["Descripcion"];
                    articuloAux.Marca.Id = (int)accesoDatos.Lector["IdMarca"];
                    articuloAux.Marca.Descripcion = (string)accesoDatos.Lector["Marca"];
                    articuloAux.Categoria.Id = (int)accesoDatos.Lector["IdCategoria"];
                    articuloAux.Categoria.Descripcion = (string)accesoDatos.Lector["Categoria"];
                    articuloAux.ImagenUrl = (string)accesoDatos.Lector["ImagenUrl"];
                    articuloAux.Precio = (decimal)accesoDatos.Lector["Precio"];

                    listaArticulos.Add(articuloAux);
                }
                return listaArticulos;

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
