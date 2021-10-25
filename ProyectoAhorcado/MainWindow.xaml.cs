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
        private List<String> listaPalabras = new List<String>() { "JOAKIN", "ESTERNOCLEIDOMASTOIDEO", "ANDROID", "FRANCIA", "ABECEDARIO", "PENINSULA", "PATATAS" };
        private int contadorFallos = 0;
        bool haGanado = false;
        Random seed = new Random();
        private string palabraAAdivinar;


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
                        Tag = c,
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

            // Bucle en el que recorremos los carácteres de la palabra que queremos adivinar
            foreach (TextBlock tb in contenedorPalabraOculta.Children)
            {
                
                if (tb.Tag.ToString() == b.Tag.ToString())
                {
                    encontrado = true;
                    tb.Text = (string)b.Tag;
                }
            }

            if (encontrado) LetraEncontrada(b);
            else LetraNoEncontrada(b);

            // Comprobamos si al jugador le queda alguna letra por descubrir o si ya ha ganado
            StringBuilder palabraActual = new StringBuilder();
            foreach (TextBlock tb in contenedorPalabraOculta.Children)
            {
                palabraActual.Append(tb.Text);
                if (palabraActual.ToString().Contains("_")) haGanado = false;
                else haGanado = true;
            }

            if (haGanado) 
            { 
                MuestraPalabra(); 
                FinDePartidaWin(); 
            }
        }

        private void MuestraPalabra()
        {
            contenedorPalabraOculta.Children.Clear();
            contenedorPalabraOculta.Children.Add(new TextBlock{ Text = palabraAAdivinar,
                                                                Style = (Style)this.Resources["estiloPalabraOculta"]});
        }
        private void LetraEncontrada(Button b)
        {
            b.Style = (Style)this.Resources["estiloLetraEncontrada"];
        }
        private void LetraNoEncontrada(Button b)
        {
            b.Style = (Style)this.Resources["estiloLetraNoEncontrada"];
            contadorFallos++;
            if (contadorFallos == 9) FinDePartidaLose();
            else
            {
                fallosTextBlock.Text = "Numero de Fallos: " + contadorFallos + "/8";
                Uri ruta = new Uri($"assets/{contadorFallos}.jpg", UriKind.Relative);
                ahorcadoImage.Source = new BitmapImage(ruta);
            }
        }
        private void FinDePartidaLose()
        {
            MessageBox.Show("Has perdido!");
            DeshabilitarBotones();
        }
        private void FinDePartidaWin()
        {
            MessageBox.Show("Has ganado !");
            DeshabilitarBotones();
        }
        private void Rendirse_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Te has rendido");
            DeshabilitarBotones();
            MuestraPalabra();
        }
        private void NuevaPartida_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Has elegido empezar una nueva partida");
            contadorFallos = 0;
            // Ponemos a 0 el contador de fallos y ponemos la imagen correspondiente a los 0 fallos
            fallosTextBlock.Text = "Numero de Fallos: " + contadorFallos +"/8";
            Uri ruta = new Uri($"assets/1.jpg", UriKind.Relative);
            ahorcadoImage.Source = new BitmapImage(ruta);
            contenedorPalabraOculta.Children.Clear(); // Elimino el contenedor de la palabra oculta para poder generar una nueva dentro
            HabilitarBotones();
            GenerarPalabra();
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
