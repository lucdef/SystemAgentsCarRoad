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
        const string Couleur = "#ffffff";
        static Random seedAleatoire;
        uint nbVehicule;

        List<Vehicule> lstVehicule;
        List<Feu> lstFeux;
        List<Route> lstRoute;

        public Carrefour()
        {
            nbVehicule = 3;
            lstVehicule = new List<Vehicule>();
            lstFeux = new List<Feu>();
            lstRoute = new List<Route>();
            GenerateRoute();
            seedAleatoire = new Random();
            for(int i=0;i<nbVehicule;i++)
            {
                Vehicule vehicule = VehiculeFactory.GetVehicule(this.GetRandomDirection(), GenererItineraire());
                vehicule.GetPositionInit();
                lstVehicule.Add(vehicule);
            }
            for(int f=0;f<4;f++)
            {
                //Génération des feux attention changer la position 
                lstFeux.Add(new Feu(false,new TimeSpan(0,0,15),new TimeSpan(0,0,15),new Position(50,50)));
            }
        }
        private void GenerateRoute()
        {
            Route route = new Route(300,20,new Position(290,0),Direction.EN_FACE);
            this.lstRoute.Add(route);
        }
        private Direction GetRandomDirection()
        {
            Direction direction;
            uint nbAleatoire = (uint) seedAleatoire.Next(0, 1);
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
            return direction;
        }
        private Position GetRandomPosition()
        {
            return new Position(50,50);
        }
        public void UpdateCarrefour()
        {
            UpdateVoiture();
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
                    if(vehicule.GetRouteActuel().GetDirection()==Direction.EN_FACE)
                    {
                        if(vehicule.GetPosition().GetY()>=vehicule.GetRouteActuel().GetPosition().GetY()+vehicule.GetRouteActuel().GetLongueur())
                        {
                            lstVehiculeASupprimer.Add(lstVehicule.IndexOf(vehicule));
                        }
                    }
                    if(vehicule.GetRouteActuel().GetDirection()==Direction.DROITE)
                    {
                        if(vehicule.GetPosition().GetX()>=vehicule.GetRouteActuel().GetPosition().GetY()+vehicule.GetLongueur())
                        {
                           
                            lstVehiculeASupprimer.Add(lstVehicule.IndexOf(vehicule));
                        }
                    }
                }
                
                
            }
            foreach(int index in lstVehiculeASupprimer)
            {
                lstVehicule.RemoveAt(index);
            }
            if(nbVehicule>lstVehicule.Count)
            {
                GenererVehicule(lstVehicule.Count);
            }
        }

        
        
        private void UpdateFeux()
        {
            foreach(Feu feu in lstFeux)
            {
                feu.tempsActivite.Add(new TimeSpan(0, 0, 0, 0, 15));
                if((feu.isVert&&feu.tempsVert==feu.tempsActivite)||(!feu.isVert&&feu.tempsRouge==feu.tempsActivite))
                    {
                    feu.ToggleFeu();
                }
            }
        }
        //Méthode permettant d'ajouter des voitures de manière aléatoire
        private void GenererVehicule(int NbVehiculeToAdd)
        {
            for(int i=0;i<nbVehicule;i++)
            {
                Vehicule vehicule = VehiculeFactory.GetVehicule(this.GetRandomDirection(),GenererItineraire());
                this.lstVehicule.Add(vehicule);
            }
        }
        //Méthode permettant de générer un itinéraire aléatoire pour une voiture
        private List<Route> GenererItineraire()
        {
            List<Route> itineraire = new List<Route>();
            Route route = this.lstRoute.Find(r => r.GetDirection() == GetRandomDirection());
            itineraire.Add(route);
            Route route2 = route.getRouteLie().ElementAt(seedAleatoire.Next(0, route.getRouteLie().Count()-1)).Value;
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

    }
}
