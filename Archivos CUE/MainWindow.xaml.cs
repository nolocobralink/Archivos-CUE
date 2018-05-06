using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;

namespace Archivos_CUE
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Pista> origen;
        int nump = 0;
        public MainWindow()
        {
            InitializeComponent();
            origen = new ObservableCollection<Pista>();
        }

        private void txtAño_KeyDown(object sender, KeyEventArgs e)
        {
            bool numeros = e.Key >= Key.D0 && e.Key <= Key.D9, numpad = e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9, tabu = e.Key == Key.Tab;
            if (numeros || numpad || tabu)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            origen.Add(new Pista() { NumPista = ++nump, Titulo = "", Artista = "", Indice00 = "", Indice01 = "", NomArch = "" });
            dtgDatos.ItemsSource = origen;
        }

        private void btnMostrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDatos.SelectedIndex < 0)
                MessageBox.Show("Seleccione una pista, por favor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                Pista sel = origen[dtgDatos.SelectedIndex];
                MessageBox.Show("Pista seleccionada: " + sel.NumPista.ToString() + "\nTítulo: " + sel.Titulo + "\nArtista: " + sel.Artista + "\nÍndice 00: " + sel.Indice00 + "\nÍndice 01: " + sel.Indice01 + "\nNombre de archivo: " + sel.NomArch + ".");
            }
        }

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if(dtgDatos.SelectedIndex < 0)
                MessageBox.Show("Seleccione una pista, por favor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                origen.RemoveAt(dtgDatos.SelectedIndex);
                for(int i=0; i < origen.Count; i++)
                {
                    origen[i].NumPista = i + 1;
                }
                dtgDatos.Items.Refresh();
                nump = origen.Count;
            }
        }

        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("¿Seguro que desea limpiar los datos?","Confirmación",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                LimpiarDatos();
            }
        }

        private void LimpiarDatos()
        {
            txtTítulo.Text = string.Empty;
            txtArtista.Text = string.Empty;
            txtGenero.Text = string.Empty;
            txtAño.Text = string.Empty;

            origen.Clear();
            dtgDatos.Items.Refresh();
            MessageBox.Show("Se han eliminado los datos.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            txtTítulo.Focus();
        }

        private void SoloNums(object sender, KeyEventArgs e)
        {
            bool num = e.Key >= Key.D0 && e.Key <= Key.D9, numpad = e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9, dosp = Keyboard.Modifiers == ModifierKeys.Shift && e.Key == Key.OemPeriod, tabu = e.Key == Key.Tab;
            if (num || numpad || dosp || tabu)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComprobarDatosBásicos(txtTítulo.Text, txtArtista.Text);
                ComprobarTiempos(origen);
                ComprobarIndices(origen);
                ComprobarTiempoEntrePistas(origen);
                ComprobarNombres(origen);
                PrimeraPistaNomArch();
                NombresArchivosVacios();
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Archivo CUE (*.cue)|*.cue";
                dlg.Title = "Busque la carpeta donde se encuentre el/los archivos de audio, y asígnele un nombre al archivo CUE.";
                Nullable<bool> guardar = dlg.ShowDialog();
                if (guardar == true)
                {
                    FileStream fs = new FileStream(dlg.FileName, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs,Encoding.GetEncoding(1252));
                    if (txtGenero.Text != string.Empty)
                        sw.WriteLine("REM GENRE " + txtGenero.Text);
                    if (txtAño.Text != string.Empty)
                        sw.WriteLine("REM DATE " + txtAño.Text);
                    sw.WriteLine("TITLE \"" + txtTítulo.Text + "\"");
                    sw.WriteLine("PERFORMER \"" + txtArtista.Text + "\"");
                    string archf = origen[0].NomArch;
                    sw.WriteLine("FILE \"" + archf + "\" WAVE");
                    sw.WriteLine("  TRACK 01 AUDIO");
                    sw.WriteLine("    TITLE \"" + origen[0].Titulo + "\"");
                    sw.WriteLine("    PERFORMER \"" + origen[0].Artista + "\"");
                    if (origen[0].Indice00 != string.Empty)
                        sw.WriteLine("    INDEX 00 " + origen[0].Indice00);
                    sw.WriteLine("    INDEX 01 " + origen[0].Indice01);
                    for (int i = 1; i < origen.Count; i++)
                    {
                        if (origen[i].NomArch != origen[i - 1].NomArch)
                            sw.WriteLine("FILE \"" + origen[i].NomArch + "\" WAVE");
                        sw.WriteLine(string.Format("  TRACK {0} AUDIO",(i+1).ToString("0#")));
                        sw.WriteLine("    TITLE \"" + origen[i].Titulo + "\"");
                        sw.WriteLine("    PERFORMER \"" + origen[i].Artista + "\"");
                        if (origen[i].Indice00 != string.Empty)
                            sw.WriteLine("    INDEX 00 " + origen[i].Indice00);
                        sw.WriteLine("    INDEX 01 " + origen[i].Indice01);
                    }
                    sw.Close();
                    if(MessageBox.Show("Se ha creado correctamente el archivo. ¿Desea limpiar los datos para crear otro?","Éxito en guardado",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        LimpiarDatos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ha ocurrido el siguiente error:\n\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ComprobarNombres(ObservableCollection<Pista> datos)
        {
            for (int i = 0; i < datos.Count; i++)
            {
                if (datos[i].Titulo == string.Empty)
                    throw new PistaException(i + 1, PistaExceptions.Exceptions.Titulo);
                if (datos[i].Artista == string.Empty)
                    throw new PistaException(i + 1, PistaExceptions.Exceptions.Artista);
            }
        }

        private void ComprobarDatosBásicos(string título, string artista)
        {
            if (título == string.Empty)
                throw new Exception("Ingrese un título para el álbum.");
            if (artista == string.Empty)
                throw new Exception("Ingrese el nombre del artista del álbum.");
        }

        private void ComprobarTiempoEntrePistas(ObservableCollection<Pista> datos)
        {
            for (int i = 0; i < datos.Count - 1; i++)
            {
                string[] strn1 = datos[i].Indice01.Split(':');
                string[] strn2 = datos[i + 1].Indice01.Split(':');
                uint[] n1 = { uint.Parse(strn1[0]), uint.Parse(strn1[1]), uint.Parse(strn1[2]) }, n2 = { uint.Parse(strn2[0]), uint.Parse(strn2[1]), uint.Parse(strn2[2]) };
                uint tn1 = n1[0] * 60 * 75 + n1[1] * 75 + n1[2], tn2 = n2[0] * 60 * 75 + n2[1] * 75 + n2[2];
                if(tn1 > tn2)
                {
                    throw new PistaException(i + 1, PistaExceptions.Exceptions.I01PMayor);
                }
                else if (tn1 == tn2)
                {
                    throw new PistaException(i + 1, PistaExceptions.Exceptions.I01PIgual);
                }
            }
        }

        private void ComprobarIndices(ObservableCollection<Pista> datos)
        {
            for (int i = 0; i < datos.Count; i++)
            {
                if (datos[i].Indice00 != string.Empty)
                {
                    string[] stri0 = datos[i].Indice00.Split(':'), stri1 = datos[i].Indice01.Split(':');
                    uint[] i0 = { uint.Parse(stri0[0]), uint.Parse(stri0[1]), uint.Parse(stri0[2]) }, i1 = { uint.Parse(stri1[0]), uint.Parse(stri1[1]), uint.Parse(stri1[2]) };
                    uint ti0 = i0[0] * 60 * 75 + i0[1] * 75 + i0[2], ti1 = i1[0] * 60 * 75 + i1[1] * 75 + i1[2];
                    if (ti0 >= ti1)
                        throw new PistaException(i + 1, PistaExceptions.Exceptions.I00PMayor);
                }
            }
        }

        private void ComprobarTiempos(ObservableCollection<Pista> datos)
        {
            for (int i = 0; i < datos.Count; i++)
            {
                if (datos[i].Indice00 != string.Empty)
                {
                    string[] strtimes = datos[i].Indice00.Split(':');
                    uint[] times = { uint.Parse(strtimes[0]), uint.Parse(strtimes[1]), uint.Parse(strtimes[2]) };
                    if (times[0] >= 80 || times[1] >= 60 || times[2] >= 75)
                    {
                        throw new PistaException(i + 1, PistaExceptions.Exceptions.I00Error);
                    }
                }
            }
            for (int i = 0; i < datos.Count; i++)
            {
                string[] strtimes = datos[i].Indice01.Split(':');
                uint[] times = { uint.Parse(strtimes[0]), uint.Parse(strtimes[1]), uint.Parse(strtimes[2]) };
                if (times[0] >= 80 || times[1] >= 60 || times[2] >= 75)
                {
                    throw new PistaException(i + 1, PistaExceptions.Exceptions.I01Error);
                }

            }
        }

        private void btnArchivo_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDatos.SelectedIndex < 0)
                MessageBox.Show("Seleccione una pista.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Archivos de onda (*.wav)|*.wav|Archivos de onda comprimido (*.flac)|*.flac";
                dlg.Title = "Seleccione el archivo de audio fuente";
                Nullable<bool> archivo = dlg.ShowDialog();
                if(archivo == true)
                {
                    string[] ruta = dlg.FileName.Split('\\');
                    string nombrearch = ruta[ruta.Length - 1];
                    origen[dtgDatos.SelectedIndex].NomArch = nombrearch;
                    dtgDatos.Items.Refresh();
                }

            }
        }

        private void txtI01_LostFocus(object sender, RoutedEventArgs e)
        {
            var texto = sender as TextBox;
            string[] partes = texto.Text.Split(':');
            try
            {
                int segundos = int.Parse(partes[1]), frames = int.Parse(partes[2]);
                bool coincide = Regex.IsMatch(texto.Text, @"^(\d{2}:\d{2}:\d{2})$"), segs = segundos > 59, frm = frames > 75;
                if (!coincide || (coincide && (segs || frm)))
                {
                    throw new PistaException(dtgDatos.SelectedIndex + 1, PistaExceptions.Exceptions.I01Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtI00_LostFocus(object sender, RoutedEventArgs e)
        {
            var texto = sender as TextBox;
            if (texto.Text != string.Empty)
            {
                string[] partes = texto.Text.Split(':');
                try
                {
                    int segundos = int.Parse(partes[1]), frames = int.Parse(partes[2]);
                    bool coincide = Regex.IsMatch(texto.Text, @"^(\d{2}:\d{2}:\d{2})$"), segs = segundos > 59, frm = frames > 75;
                    if (!coincide || (coincide && (segs || frm)))
                    {
                        throw new PistaException(dtgDatos.SelectedIndex + 1, PistaExceptions.Exceptions.I00Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("¿Está seguro que desea salir? Cualquier cambio no guardado se perderá.", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrimeraPistaNomArch();
                string nomarch = origen[0].NomArch;
                for (int i = 1; i < origen.Count; i++)
                {
                    if (nomarch != origen[i].NomArch)
                        if (origen[i].NomArch == string.Empty)
                            origen[i].NomArch = nomarch;
                        else
                            nomarch = origen[i].NomArch;
                }
                dtgDatos.Items.Refresh();
                MessageBox.Show("Nombres de archivos actualizados en todas las pistas.", "Pistas cambiadas.", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PrimeraPistaNomArch()
        {
            if (origen[0].NomArch == string.Empty)
                throw new Exception("Asignele un archivo fuente a la primera pista.");
        }

        private void NombresArchivosVacios()
        {
            for (int i = 1; i < origen.Count; i++)
            {
                if (origen[i].NomArch == string.Empty)
                    throw new Exception("Existen pistas sin archivos fuentes asociados.\nQuizás olvidó asignar esos archivos o quizás olvidó actualizar las pistas.\nPara actualizar las pistas, haga clic en el botón \"Actualizar\" y pruebe a guardar de nuevo.");
            }
        }
    }
}
