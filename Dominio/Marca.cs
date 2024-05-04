using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Marca
    {
        #region Atributos
        private int _id;
        private string _descripcion;
        #endregion

        #region Propiedades
        public int Id { get => _id; set => _id = value; }
        public string Descripcion { get => _descripcion; set => _descripcion = value; }
        #endregion

        #region Metodos
        public override string ToString()
        {
            return _descripcion.ToString();
        }
        #endregion
    }
}
