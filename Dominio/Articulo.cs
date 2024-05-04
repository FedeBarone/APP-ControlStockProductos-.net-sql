using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {
        #region Atributos
        private int _id;
        private string _codigo;
        private string _nombre;
        private string _descripcion;
        private Marca _marca;
        private Categoria _categoria;
        private string _imagenUrl;
        private decimal _precio;
        #endregion

        #region Propiedades
        public int Id { get => _id; set => _id = value; }
        public string Codigo { get => _codigo; set => _codigo = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public string Descripcion { get => _descripcion; set => _descripcion = value; }
        public Marca Marca { get => _marca; set => _marca = value; }
        public Categoria Categoria { get => _categoria; set => _categoria = value; }
        public string ImagenUrl { get => _imagenUrl; set => _imagenUrl = value; }
        public decimal Precio { get => _precio; set => _precio = value; }
        #endregion
    }
}
