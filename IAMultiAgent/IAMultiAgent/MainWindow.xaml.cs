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
using System.Windows.Interop;

namespace IAMultiAgent
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Carrefour carrefour;
        public TimeSpan tempsactivite;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded1;
        }

        private void MainWindow_Loaded1(object sender, RoutedEventArgs e)
        {
            carrefour = new Carrefour();
            tempsactivite= new TimeSpan(0, 0, 0, 0, 0);

            ImageBrush imageBrush = new ImageBrush();
            //Mettre le bon chemin
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(IAMultiAgent.Properties.Resources.carrefour.GetHbitmap(), IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions());
            carrefourCanvas.Background = new ImageBrush(bitmapSource);
            carrefour.carrefourUpdatedEvent += Carrefour_carrefourUpdated;
            TimeSpan simulationSpeed = new TimeSpan(0, 0, 0, 0, 2);
            carrefour.SetSimulationSpeed(simulationSpeed);
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
			dispatcherTimer.Interval = carrefour.GetSimulationSpeed();
            dispatcherTimer.Start(); 
        }
        void dispatcherTimer_Tick(object _sender, EventArgs _e)
        {
            tempsactivite.Add(carrefour.GetSimulationSpeed());
            carrefour.UpdateCarrefour();
        }

        private void Carrefour_carrefourUpdated(List<Vehicule> lstVehicule)
        {
            carrefourCanvas.Children.Clear();
           DrawRoute();
            foreach (Vehicule vehicule in lstVehicule)
            {
                DrawVehicule(vehicule);
            }
            this.SetEtatFeuToUI();
            this.SetInfoVehiculeToUi(lstVehicule);
            
            carrefourCanvas.UpdateLayout();
        }
        private void SetEtatFeuToUI()
        {
            List<Route> lstRoute = carrefour.GetListRoute();
            lbEtatFeuDevannt.Content = lstRoute.ElementAt(0).GetFeu().isVert ? "Vert":"Rouge" ;
            lbEtatFeuGauche.Content = lstRoute.ElementAt(1).GetFeu().isVert ? "Vert" : "Rouge";
        }
        private void SetInfoVehiculeToUi(List<Vehicule>lstVehicule)
        {
            lbNombreDeVoitureInt.Content = lstVehicule.FindAll(v => v is Voiture).Count;

            lbNombreDeCamionInt.Content = lstVehicule.FindAll(v => v is Camion).Count;
            lbNombrTotaleDeVehiculeInt.Content = lstVehicule.Count;
        }
        private void DrawVehicule(Vehicule vehicule)
        {
            Rectangle voiture = new Rectangle();
            voiture.Width = vehicule.GetLargeur();
            voiture.Height = vehicule.GetLongueur();
           
            Color couleur = (Color)ColorConverter.ConvertFromString(vehicule.couleur);
            voiture.Fill =  new SolidColorBrush(couleur);
            double dAngle = vehicule.getAngle();
            voiture.Stroke  = Brushes.Black;
            


            rotateRectangle(voiture, voiture.Width, voiture.Height,vehicule.getAngle());
            
        
        voiture.Margin = new Thickness(vehicule.GetPosition().GetX(),vehicule.GetPosition().GetY(),0,0);
            carrefourCanvas.Children.Add(voiture);

        }
        
        private void DrawRoute()
        {
            foreach (Route road in carrefour.GetListRoute())
            {
                Rectangle route = new Rectangle();
                route.Width = road.GetLargeur();
                route.Height = road.GetLongueur();

                Color couleur = (Color)ColorConverter.ConvertFromString("#ffffff");
                route.Fill = new SolidColorBrush(couleur);

                
                if (road.GetDirection()==Direction.DROITE)
                {
                    rotateRectangle(route, route.Width, route.Height, 90);
                }

                route.RadiusX = 10;
                route.RadiusY = 10;

                route.Stroke = Brushes.Black;

                route.Margin = new Thickness(road.GetPosition().GetX(), Height/2, 0, 0);
                carrefourCanvas.Children.Add(route);
            }
        }
        private void tbNbVehicule_TouchEnter(object sender, TouchEventArgs e)
        {
            carrefour.SetNbVehicule(int.Parse(tbNbVehicule.Text));
        }

        private void tbNbVehicule_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key ==Key.Enter)
            {
                int nbVehicule = carrefour.GetListVehicule().Count;
                if (!int.TryParse(tbNbVehicule.Text, out nbVehicule))
                    tbNbVehicule.Text = nbVehicule.ToString();

                carrefour.SetNbVehicule(nbVehicule);
               
            }
        }
        private void rotateRectangle(Rectangle RectToTransform, double middle_X, double middle_Y, double angle)
        {
            RotateTransform rt = new RotateTransform
            {
                CenterX = middle_X,
                CenterY = middle_Y,
                Angle = angle
            };
            RectToTransform.LayoutTransform = rt;
        }
        private void rectangleMouseOver(object sender,MouseAction e)
        {

        }
        private void tbTempFeuVert_KeyDown_1(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                int tempsFeuVert = 6;
                if(int.TryParse(tbTempsFeuVert.Text,out tempsFeuVert))
                {
                    if(tempsFeuVert>5)
                    {
                        TimeSpan timeSpanFeuVert1 = new TimeSpan(0,0, tempsFeuVert);
                        TimeSpan timeSpanFeuRouge1 = new TimeSpan(0, 0, tempsFeuVert + 5);
                        foreach(Route route in carrefour.GetListRoute())
                        {
                            route.GetFeu().tempsRouge = timeSpanFeuRouge1;
                            route.GetFeu().tempsVert = timeSpanFeuVert1;
                            route.GetFeu().tempsActivite = new TimeSpan(0, 0, 0, 0, 0);
                            
                        }

                    }
                }
            }
        }
       

        private void tbTempFeuRouge_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int tempsFeuRouge = 6;
                if (int.TryParse(tbTempsFeuRouge.Text, out tempsFeuRouge))
                {
                    if (tempsFeuRouge > 10)
                    {
                        TimeSpan timeSpanFeuVert1 = new TimeSpan(0, 0, tempsFeuRouge-5);
                        TimeSpan timeSpanFeuRouge1 = new TimeSpan(0, 0, tempsFeuRouge);
                        foreach (Route route in carrefour.GetListRoute())
                        {
                            route.GetFeu().tempsRouge = timeSpanFeuRouge1;
                            route.GetFeu().tempsVert = timeSpanFeuVert1;
                            route.GetFeu().tempsActivite = new TimeSpan(0, 0, 0, 0, 0);

                        }

                    }
                }
            }
        }
    }
}
