using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaz
{
    public static class Validador
    {
        /// <summary>
        /// Valida que la cadena no sea nula, vacia o tenga espacios en blanco
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns>string</returns>
        public static bool CadenaEsInvalida(string cadena)
        {
          return string.IsNullOrEmpty(cadena) || string.IsNullOrWhiteSpace(cadena);
        }

        /// <summary>
        /// Valida que la cadena contenga solo numeros
        /// </summary>
        /// <param name="cadena"></param>
        /// <returns>devuelve falso si algun caracter de la cadena no es un numero, sino devuelve true</returns>
        public static bool soloNumeros(string cadena)
        {
            foreach (char caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }
    }
}
