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
using IAAgents;
using System.Windows.Threading;

namespace IAMultiAgent
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Carrefour carrefour;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded1;
        }

        private void MainWindow_Loaded1(object sender, RoutedEventArgs e)
        {
            carrefour = new Carrefour();
            carrefour.carrefourUpdatedEvent += Carrefour_carrefourUpdated;

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            dispatcherTimer.Start(); 
        }
        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            carrefour.UpdateCarrefour();
        }

       
        private void Carrefour_carrefourUpdated(List<Vehicule> lstVehicule)
        {
            carrefourCanvas.Children.Clear();
            foreach (Vehicule vehicule in lstVehicule)
            {
                DrawVehicule(vehicule);
            }
            carrefourCanvas.UpdateLayout();
        }
        private void DrawVehicule(Vehicule vehicule)
        {
            Rectangle voiture = new Rectangle();
            voiture.Width = vehicule.GetLargeur();
            voiture.Height = vehicule.GetLongueur();
           
            Color couleur = (Color)ColorConverter.ConvertFromString(vehicule.couleur);
            voiture.Fill =  new SolidColorBrush(couleur);
            voiture.RadiusX = 90;
            voiture.Stroke  = Brushes.Black;

            voiture.Margin = new Thickness(vehicule.GetPosition().GetX(),vehicule.GetPosition().GetY(),0,0);
            carrefourCanvas.Children.Add(voiture);

        }
    }
}
