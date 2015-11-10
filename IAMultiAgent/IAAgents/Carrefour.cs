using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    public delegate void CarrefourUpdated(List<Vehicule> lstVehicule);
    public class Carrefour
    {
       
        public event CarrefourUpdated carrefourUpdatedEvent;


        const uint Height = 480;
        const uint Width = 640;
        const uint DISTANCE_ENTRE_VEHICULES=20;
        const string Couleur = "#ffffff";
        static Random seedAleatoire;
        uint nbVehicule;

        List<Vehicule> lstVehicule;
        List<Feu> lstFeux;
        List<Route> lstRoute;
        static Random randomGenerator;
        TimeSpan simulationSpeed;

        public TimeSpan GetSimulationSpeed()
        {
            return this.simulationSpeed;
        }
        public void SetSimulationSpeed(TimeSpan timeSpeed)
        {
            this.simulationSpeed = timeSpeed;
        }

        public Carrefour()
        {
            nbVehicule = 3;
            lstVehicule = new List<Vehicule>();
            lstFeux = new List<Feu>();
            lstRoute = new List<Route>();
            GenerateRoute();
            seedAleatoire = new Random();
            randomGenerator = new Random(30101993);

        }
        private void GenerateRoute()
        {
            Route route = new Route(300,20,new Position(Width/2-10,Height),Direction.EN_FACE);
            Feu feuPrincipal = new Feu(true, new TimeSpan(0, 0, 15), new TimeSpan(0, 0, 25));
            route.SetFeu(feuPrincipal);
            Route routeSecondaire = new Route(220, 20, new Position(Width / 2 - 10, 0), Direction.EN_FACE);
            Route routePrincipal2 = new Route(300, 20, new Position(0, Height/2), Direction.DROITE);
            Feu feuPrincipal2 = new Feu(false, new TimeSpan(0, 0, 20), new TimeSpan(0, 0, 20));
            routePrincipal2.SetFeu(feuPrincipal2);
            Route routeSecondaire2 = new Route(300, 20, new Position(340, 220), Direction.DROITE);
            KeyValuePair<Direction, Route> routePrincipalLie = new KeyValuePair<Direction, Route>(Direction.EN_FACE, routeSecondaire);
            KeyValuePair<Direction, Route> routePrincipalLie2 = new KeyValuePair<Direction, Route>(Direction.DROITE, routeSecondaire2);
            List<KeyValuePair<Direction,Route>> lstRoutePrincipalLie = new List<KeyValuePair<Direction, Route>>();
            lstRoutePrincipalLie.Add(routePrincipalLie);
            lstRoutePrincipalLie.Add(routePrincipalLie2);
            route.setRoute(lstRoutePrincipalLie);
            List<KeyValuePair<Direction, Route>> lstRoutePrincipal2Lie = new  List<KeyValuePair<Direction, Route>>();

            KeyValuePair<Direction, Route> routePrincipal2Lie = new KeyValuePair<Direction, Route>(Direction.EN_FACE, routeSecondaire2);
            KeyValuePair<Direction, Route> routePrincipal2Lie2 = new KeyValuePair<Direction, Route>(Direction.GAUCHE, routeSecondaire);
            lstRoutePrincipal2Lie.Add(routePrincipal2Lie);
            lstRoutePrincipal2Lie.Add(routePrincipal2Lie2);

            routePrincipal2.setRoute(lstRoutePrincipal2Lie);
            
            this.lstRoute.Add(route);
            this.lstRoute.Add(routePrincipal2);
        }

        public void SetNbVehicule(int nbVehicule)
        {
            this.nbVehicule =(uint) nbVehicule;
        }

        private Direction GetRandomDirection()
        {
            Direction direction;
            int nbAleatoire = seedAleatoire.Next(0, 100);
            nbAleatoire = nbAleatoire > 50 ? 1 : 0;
            switch(nbAleatoire)
            {
                case 0:
                    direction = Direction.EN_FACE;
                    break;
                case 1:
                    direction = Direction.DROITE;
                    break;
                default:
                    direction = Direction.EN_FACE;
                    break;

            }
            direction = Direction.DROITE;
            return direction;
        }
       
        public void UpdateCarrefour()
        {
            UpdateFeux();
            UpdateVoiture();
            UpdateFeux();
            if (carrefourUpdatedEvent != null)
            {
                carrefourUpdatedEvent(lstVehicule );
            }
        }
        private void UpdateVoiture()
        {
            List<int> lstVehiculeASupprimer = new List<int>();
            foreach (Vehicule vehicule in lstVehicule)
            {

                vehicule.Update(lstVehicule);
                //On detruit si hors de la fenêtre
                if(vehicule.getIndexRouteActuel()==1)
                {
                    if (vehicule.getIndexRouteActuel() == 1)
                    {
                        if (vehicule.GetRouteActuel().GetDirection() == Direction.EN_FACE)
                        {
                            if ((vehicule.GetPosition().GetY() + vehicule.GetLongueur()) <= 0)
                            {
                                lstVehiculeASupprimer.Add(lstVehicule.IndexOf(vehicule));
                            }
                        }
                        if (vehicule.GetRouteActuel().GetDirection() == Direction.DROITE)
                        {
                            if ((vehicule.GetPosition().GetX() + vehicule.GetLongueur()) >= Width)
                            {

                                lstVehiculeASupprimer.Add(lstVehicule.IndexOf(vehicule));
                            }
                        }
                    }
                }
                
                
            }
            //On détruit tout les élément de liste à détruire
            foreach(int index in lstVehiculeASupprimer)
            {
                lstVehicule.RemoveAt(index);
            }

            if(nbVehicule>lstVehicule.Count)
            {
                GenererVehicule(((int)nbVehicule) -lstVehicule.Count);
            }
        }

        
        
        private void UpdateFeux()
        {
            foreach(Route route in lstRoute)
            {
                
                Feu feu = route.GetFeu();
                feu.tempsActivite = feu.tempsActivite.Add(this.simulationSpeed);
                if((feu.isVert&&feu.tempsVert==feu.tempsActivite)||(!feu.isVert&&feu.tempsRouge==feu.tempsActivite))
                    {
                    feu.ToggleFeu();
                }
            }
        }
        //Méthode permettant d'ajouter des voitures de manière aléatoire
        private void GenererVehicule(int NbVehiculeToAdd)
        {
            for (int i = 0; i < nbVehicule; i++)
            {
                Vehicule vehicule = VehiculeFactory.GetVehicule(this.GetRandomDirection(), GenererItineraire());
                vehicule.GetPositionInit();
                Console.WriteLine("Direction" + (vehicule.GetRouteActuel().GetDirection() == Direction.EN_FACE ? "face" : "droite") + " " + vehicule.GetRouteActuel().GetPosition().GetX() + " " + vehicule.GetRouteActuel().GetPosition().GetY());

                if (vehicule.GetRouteActuel().GetDirection() == Direction.DROITE)
                {

                    if (!lstVehicule.Exists(v => v.GetRouteActuel() == vehicule.GetRouteActuel()
                                            && ((v.GetPosition().GetX()) < vehicule.GetPosition().GetX() + DISTANCE_ENTRE_VEHICULES + vehicule.GetLongueur() / 1)))
                    {


                        this.lstVehicule.Add(vehicule);
                    List<Vehicule> lstTemp = lstVehicule.FindAll(v => v.GetRouteActuel() == vehicule.GetRouteActuel());
                    Vehicule vehiculeDevant = lstTemp.Count > 0 ? lstTemp.OrderBy(v => v.GetPosition().GetX()).First():null ;
                    if(vehiculeDevant!=null&&vehicule.GetRouteActuel().GetPosition().GetX()>vehiculeDevant.GetPosition().GetX())
                    {
                            vehicule.setPosition(new Position(vehiculeDevant.GetPosition().GetX() - DISTANCE_ENTRE_VEHICULES - vehicule.GetLongueur(), vehicule.GetPosition().GetY()));


                    }
                    lstVehicule.Add(vehicule); 
                }
                else if (vehicule.GetRouteActuel().GetDirection() == Direction.EN_FACE)
                {

                        if (!lstVehicule.Exists(v => v.GetRouteActuel() == vehicule.GetRouteActuel()
                                                && ((v.GetPosition().GetY()) > vehicule.GetPosition().GetY() + DISTANCE_ENTRE_VEHICULES + vehicule.GetLongueur() / 1)))
                        {
                    List<Vehicule> lstTemp = lstVehicule.FindAll(v => v.GetRouteActuel() == vehicule.GetRouteActuel());
                    Vehicule vehiculeDevant = lstTemp.Count > 0 ? lstTemp.OrderBy(v => v.GetPosition().GetY()).Last() : null;
                    if(vehiculeDevant!=null&&vehicule.GetRouteActuel().GetPosition().GetY()<vehiculeDevant.GetPosition().GetY()+vehiculeDevant.GetLongueur())
                    {
                        vehicule.setPosition(new Position(vehiculeDevant.GetPosition().GetX(), vehiculeDevant.GetPosition().GetY() +vehiculeDevant.GetLongueur()+ DISTANCE_ENTRE_VEHICULES));
                    }
                    lstVehicule.Add(vehicule);
                }
            }
        }
            }
        }
        //Méthode permettant de générer un itinéraire aléatoire pour une voiture
        private List<Route> GenererItineraire()
        {
            List<Route> itineraire = new List<Route>();
            Direction randomDirection = GetRandomDirection();
            Route route = this.lstRoute.Find(r => r.GetDirection() == randomDirection);
            seedAleatoire = new Random();
            itineraire.Add(route);
            
            int randomNb = randomGenerator.Next(0, 10);
            int index = route.GetDirection() == Direction.EN_FACE ? randomNb < 4 ? 0 : 1 : randomNb > 4 ? 0 : 1;
            Route route2 = route.getRouteLie().ElementAt(index).Value;/*route.getRouteLie().ElementAt(index).Value;*/
            itineraire.Add(route2);
            return itineraire;
        }
        private void DestructeurVehicule(Vehicule vehicule)
        {

        }
        public List<Vehicule> GetListVehicule()
        {
            return lstVehicule;
        }
        public List<Route> GetListRoute()
        {
            return this.lstRoute;
        }

    }
}
