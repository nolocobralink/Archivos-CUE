using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Archivos_CUE
{
    class TiempoCD
    {
        private int minutos, segundos, cuadros;

        private void CrearInstancia(int minutos, int segundos, int cuadros)
        {
            if (minutos < 0 || segundos < 0 || cuadros < 0)
                throw new Exception("No pueden existir minutos, segundos o cuadros negativos.");
            if (segundos > 59 || cuadros > 74)
                throw new Exception("Los segundos deben ser entre 00 y 59, y los cuadros entre 00 y 74.");
            this.minutos = minutos;
            this.segundos = segundos;
            this.cuadros = cuadros;
        }

        public TiempoCD()
        {
            minutos = 0;
            segundos = 0;
            cuadros = 0;
        }

        public TiempoCD(int minutos, int segundos, int cuadros)
        {
            CrearInstancia(minutos, segundos, cuadros);
        }

        public TiempoCD(string tiempo)
        {
            bool coincide = Regex.IsMatch(tiempo, @"^(\d{2}:\d{2}:\d{2})$");
            if (!coincide)
                throw new Exception("Formato inválido.\nEl formato correcto es MM:SS:FF.\n\nMM: Minutos\nSS: Segundos\nFF: Cuadros");
            string[] strtiempos = tiempo.Split(':');
            int[] tiempos = { int.Parse(strtiempos[0]), int.Parse(strtiempos[1]), int.Parse(strtiempos[2]) };
            CrearInstancia(tiempos[0], tiempos[1], tiempos[2]);
        }

        public static TiempoCD operator +(TiempoCD t1, TiempoCD t2)
        {
            int ttotal = t1.TTotal + t2.TTotal;
            int minutos = ((ttotal / 60) / 75), segundos = (ttotal / 75) - (minutos * 60), cuadros = ttotal - (minutos * 60 * 75) - (segundos * 75);
            return new TiempoCD(minutos, segundos, cuadros);
        }

        public static TiempoCD operator -(TiempoCD t1, TiempoCD t2)
        {
            if (t1 < t2)
                throw new Exception("No se puede restar un tiempo con uno mayor en ese orden.\nLa resta debe ser primero un tiempo mayor y luego uno menor.");
            int ttotal = t1.TTotal - t2.TTotal;
            int minutos = ((ttotal / 60) / 75), segundos = (ttotal / 75) - (minutos * 60), cuadros = ttotal - (minutos * 60 * 75) - (segundos * 75);
            return new TiempoCD(minutos, segundos, cuadros);
        }

        public static bool operator ==(TiempoCD t, string strt)
        {
            return t.ToString() == strt;
        }

        public static bool operator !=(TiempoCD t, string strt)
        {
            return t.ToString() != strt;
        }

        public static bool operator==(TiempoCD t, int cuadrostotales)
        {
            return t.TTotal == cuadrostotales;
        }

        public static bool operator !=(TiempoCD t, int cuadrostotales)
        {
            return t.TTotal != cuadrostotales;
        }

        public static bool operator ==(TiempoCD t1, TiempoCD t2)
        {
            return t1.TTotal == t2.TTotal;
        }

        public static bool operator !=(TiempoCD t1, TiempoCD t2)
        {
            return t1.TTotal != t2.TTotal;
        }

        public static bool operator <(TiempoCD t1, TiempoCD t2)
        {
            return t1.TTotal < t2.TTotal;
        }
        public static bool operator >(TiempoCD t1, TiempoCD t2)
        {
            return t1.TTotal > t2.TTotal;
        }
        public static bool operator <=(TiempoCD t1, TiempoCD t2)
        {
            return t1.TTotal <= t2.TTotal;
        }
        public static bool operator >=(TiempoCD t1, TiempoCD t2)
        {
            return t1.TTotal >= t2.TTotal;
        }

        public int TTotal
        {
            get
            {
                return (minutos * 60 * 75) + (segundos * 75) + cuadros;
            }
        }

        public override string ToString()
        {
            return String.Format("{0:00}:{1:00}:{2:00}", minutos, segundos, cuadros);
        }
    }
}
