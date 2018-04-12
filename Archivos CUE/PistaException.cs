using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivos_CUE
{
    class PistaException : Exception
    {
        public PistaException(int numpista, int coderr)
            : base(string.Format("Error en la pista {0}.\nError número {1}: {2}", numpista, coderr, PistaExceptions.Errores(coderr,numpista)))
        {
            
        }
    }
    class PistaExceptions
    {
        private static string[] errores = { "Nada", 
                                            "Tiempos erróneos en índice 00.\nRecuerde, en un CD hay un máximo de 80 minutos, en un minuto hay 60 segundos, y en un segundo hay 75 fotogramas.",
                                            "Tiempos erróneos en índice 01.\nRecuerde, en un CD hay un máximo de 80 minutos, en un minuto hay 60 segundos, y en un segundo hay 75 fotogramas." ,
                                            "El índice 00 en la pista {0} es mayor o igual al índice 01, si es igual, no necesita rellenar el campo \"Índice 00\".",
                                            "El índice 01 de la pista {0} es mayor al de la pista {1}. Arregle los tiempos.",
                                            "El índice 01 de la pista {0} es igual al de la pista {1}. Recuerde que una pista debe durar al menos 5 segundos.",
                                            "Introduzca un título para la pista {0}.",
                                            "Introduzca el artista para la pista {0}."};

        public static string Errores(int coderr, int numpista)
        {
            if (coderr == 4 || coderr == 5)
                return string.Format(errores[coderr], numpista, numpista + 1);
            return string.Format(errores[coderr], numpista);
        }
    }
}
