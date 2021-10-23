using System;
using System.Collections.Generic;
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

namespace ProyectoAhorcado
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<String> abecedario = new List<String>() {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","ñ","o","p","q","r","s","t","u","v","w","x","y","z"};
        private List<String> listaPalabras = new List<String>() { "JOAKIN", "ESTERNOCLEIDOMASTOIDEO", "ANDROID", "ESPAÑA", "FRANCIA", "ABECEDARIO", "PENINSULA", "PATATAS" };
        private List<String> listaLetrasEncontradas = new List<String>();
        private List<String> listaLetrasNoEncontradas = new List<String>();
        private int contadorFallos = 0;
        Random seed = new Random();
        private String palabraAAdivinar;


        public MainWindow()
        {
            InitializeComponent();
            GenerarPalabra();
            CrearBotones();
        }
        public void GenerarPalabra()
        {
            palabraAAdivinar = listaPalabras[seed.Next(0, listaPalabras.Count - 1)];

            foreach (char c in palabraAAdivinar)
            {
                contenedorPalabraOculta.Children.Add(
                    new TextBlock
                    {
                        Text = "_",
                        Style = (Style)this.Resources["estiloPalabraOculta"]
                    });
            }
        }

        public void CrearBotones()
        {
            int contadorLetras = 0;
            int contadorFilas = grid.Rows;
            int contadorColumnas = grid.Columns;

            for (int i = 0; i < contadorFilas; i++)
            {
                for (int j = 0; j < contadorColumnas; j++, contadorLetras++)
                {

                    Button b = new Button
                    {
                        Style = (Style)this.Resources["botonLetra"],
                        Tag = abecedario[contadorLetras].ToUpper(),
                        Content = abecedario[contadorLetras].ToUpper(),
                    };
                    b.Click += OnClick;
                    grid.Children.Add(b);
                }
            }
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            BuscarLetra(b);
        }
        private void LetraPulsada(object sender, KeyEventArgs e)
        {
            // Bucle para encontrar el botón coincidente con la tecla pulsada
            foreach (var item in grid.Children)
            {
                if (e.Key.ToString() == (item as Button).Tag.ToString()) BuscarLetra(item as Button);
            }
        }
        private void BuscarLetra(Button b)
        {
            bool encontrado = false;
            int contadorPosicion = 0;

            char[] caracteresPalabra = palabraAAdivinar.ToCharArray();
             
            // Bucle en el que recorremos los carácteres de la palabra que queremos adivinar
            foreach (char c in caracteresPalabra)
            {
                // Comprobamos si el carácter actual es el mismo que el que corresponde al botón pulsado
                if (c == char.Parse((string)b.Tag))
                {
                    encontrado = true;
                    (contenedorPalabraOculta.Children[contadorPosicion] as TextBlock).Text = c.ToString();
                }
                contadorPosicion++;
            }

            if (encontrado) LetraEncontrada(b);
            else LetraNoEncontrada(b);
        }
        private void LetraEncontrada(Button b)
        {
            b.Style = (Style)this.Resources["estiloLetraEncontrada"];
        }
        private void LetraNoEncontrada(Button b)
        {
            b.Style = (Style)this.Resources["estiloLetraNoEncontrada"];
            contadorFallos++;
            if (contadorFallos == 9) FinDePartida();
            else
            {
                fallosTextBlock.Text = "Numero de Fallos: " + contadorFallos;
                // ahorcadoImage.Source = $"assets/{contadorFallos}.jpg";
            }

        }
        private void FinDePartida()
        {
            MessageBox.Show("Has perdido!");
            DeshabilitarBotones();
        }
        private void Rendirse_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cobarde");
            DeshabilitarBotones();
        }
        private void NuevaPartida_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Has elegido empezar una nueva partida");
            contadorFallos = 0;
            fallosTextBlock.Text = "Numero de Fallos: " + contadorFallos;
            contenedorPalabraOculta.Children.Clear(); // Elimino el contenedor de la palabra oculta para poder generar una nueva dentro
            GenerarPalabra();
            HabilitarBotones();
        }

        private void DeshabilitarBotones()
        {
            foreach (var item in grid.Children)
            {
                (item as Button).IsEnabled = false;
            }
        }
        private void HabilitarBotones()
        {
            foreach (var item in grid.Children)
            {
                (item as Button).Style = (Style)this.Resources["botonLetra"];
            }
        }
    }
}
