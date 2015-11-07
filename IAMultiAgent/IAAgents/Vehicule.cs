using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IAAgents
{


    public abstract class Vehicule
    {
        protected uint longueur;
        protected uint largeur;
        protected double vitesseMax;
        Direction direction;
        protected const uint STEP = 3;
        protected double vitesse;
        protected uint angle;
        protected List<Route> itineraire { get; set; }
        protected int indexRouteActuel { get; set; }

        public List<Route> GetItineraire()
        {
            return this.itineraire;
        }
        public Route GetRouteActuel()
        {
            return this.itineraire.ElementAt(this.indexRouteActuel);
        }




        static Random seedCouleurRandom;

        Position position;
        Vehicule vehiculeDevant;
        bool estArreter;
        public string couleur { private set; get; }


        public Vehicule(Direction dir, List<Route> itineraire)
        {
            this.itineraire = itineraire;
            this.longueur = 24;
            this.largeur = 12;
            this.vitesse = 25;
            this.vitesseMax = 50;
            this.direction = dir;
            this.vehiculeDevant = null;
            this.estArreter = false;
            seedCouleurRandom = new Random();
            this.couleur = this.SetCouleur();
            this.angle = 90;

        }
        public void GetPositionInit()
        {
            Route routeInitial = this.itineraire.ElementAt(0);
            Direction direction = routeInitial.GetDirection();
            double X = 0;
            double Y = 0;
            switch (direction)
            {
                case Direction.EN_FACE:
                    X = (routeInitial.GetPosition().GetX() + routeInitial.GetLargeur() / 2) - this.GetLargeur() / 2;
                    Y = routeInitial.GetPosition().GetY() - this.longueur;
                    break;
                case Direction.DROITE:
                    X = routeInitial.GetPosition().GetX() - this.largeur;
                    Y = (routeInitial.GetPosition().GetY() + routeInitial.GetLargeur() / 2) - this.GetLargeur() / 2;
                    break;
                case Direction.GAUCHE:
                    X = routeInitial.GetPosition().GetX() + this.largeur;
                    Y = (routeInitial.GetPosition().GetY() + routeInitial.GetLargeur() / 2) - this.largeur / 2;
                    break;
            }
            this.position = new Position(X, Y);
        }
        private string SetCouleur()
        {
            string couleur;
            int nbAleatoire = seedCouleurRandom.Next(0, 3);
            switch (nbAleatoire)
            {
                case 0:
                    couleur = "#FF0000";
                    break;
                case 1:
                    couleur = "#0000CC";
                    break;
                case 2:
                    couleur = "#000000";
                    break;
                case 3:
                    couleur = "#C0C0C0";
                    break;
                default:
                    couleur = "#C0C0C0";
                    break;
            }
            return couleur;
        }

        internal int getIndexRouteActuel()
        {
            return this.indexRouteActuel; 
        }

        public uint GetLongueur()
        {
            return this.longueur;
        }
        public uint GetLargeur()
        {
            return this.largeur;
        }
        public double GetVitesse()
        {
            return this.vitesse;
        }
        public Position GetPosition()
        {
            return this.position;
        }
        public Vehicule vDevant()
        {
            return this.vehiculeDevant;
        }
        public uint getAngle()
        {
            return this.angle;
        }

        public void Update(List<Vehicule> lstVehicule)
        {

            UpdatePosition(lstVehicule);

        }
        //Méthode permettant de déterminer le vehicule devant 
        private Vehicule GetVehiculeDevant(Vehicule vehicule, List<Vehicule> lstVehicule)
        {
            Vehicule vehiculeDevant = null;
            if (this.direction == Direction.EN_FACE)
            {
                List<Vehicule>lstVehiculeOnTheRoad = lstVehicule.FindAll(v =>v.GetRouteActuel()==vehicule.GetRouteActuel()&& vehicule.direction == Direction.EN_FACE && v != vehicule).OrderBy(v => vehicule.GetPosition().GetY() - v.GetPosition().GetY()).ToList();
                if(lstVehiculeOnTheRoad.Count>0)
                {
                    vehiculeDevant = lstVehiculeOnTheRoad.First();
                }
            }
            return vehiculeDevant;
        }


        //Changer nom update positionetvitesse
        private void UpdatePosition(List<Vehicule> listVehicule)
        {
            //if(this.GetRouteActuel().GetDirection()==Direction.EN_FACE)
            // {
            ////foreach (Vehicule vehicule in listVehicule)
           // {
                slow_down_or_accelerate(listVehicule);
                //If englobant tout pour tourner c le cas ou on est sur la premiere route
                
                if(this.GetRouteActuel().GetDirection()==Direction.EN_FACE)
                {
                  double posX = this.GetPosition().GetX();
                    double posY = this.position.GetY() + STEP * vitesse;
                    this.position = new Position(posX, posY);
                }
                if(this.GetRouteActuel().GetDirection()==Direction.DROITE)
                {
                    double posX = this.GetPosition().GetX() + STEP * vitesse;
                    double posY = this.position.GetY() ;
                    this.position = new Position(posX, posY);
                }
                if(this.GetRouteActuel().GetDirection()==Direction.GAUCHE)
                {

                }
                // }

                //Condition selon la direction actuel
                //double posX = this.GetPosition().GetX();
                //double posY = this.position.GetY() + STEP * vitesse;
                //this.position = new Position(posX, posY);
                //}
            }

        public double calcul_distance_entre_les_deux_voitures(Vehicule vAction, Vehicule vToAvoid)
        {
            double dDeltaDistance = 0.0f;
            double xyToAvoid = 0.0f;
            double xyAction = 0.0f;
            Position pVoitureAction;
            Position pVoitureToAvoid;

            //  Récupération de la position actuelle de la voiture de devant et de derrière
            pVoitureAction = GetPosition();
            pVoitureToAvoid = vToAvoid.GetPosition();

            //  Récupération des positions, soit en X ou soit en Y suivant dans quel direction le véhicule de derrière circule
            if (this.GetRouteActuel().GetDirection() == Direction.EN_FACE)
            {
                xyToAvoid = pVoitureToAvoid.GetX();
                xyAction = pVoitureAction.GetX();
            }
            else
            {
                xyToAvoid = pVoitureToAvoid.GetY();
                xyAction = pVoitureAction.GetY();
            }
            dDeltaDistance = xyToAvoid - xyAction;

            return dDeltaDistance;
        }

        public float calcul_distance_freinage(Vehicule vToBrake)
        {
            float dDistFreinage = 0.0f;
            float fActualSpeedVehiculeToBreak = (float)vToBrake.GetVitesse();
            //  Pour calculer la distance de freinage, il faut mettre au carré le chiffre des dizaines
            //  donc il faut diviser par 10 pour avoir seulement le chiffre des dizaines.
            fActualSpeedVehiculeToBreak /= 10;
            return dDistFreinage * dDistFreinage;
        }

        public void calcul_courbe_vitesse_distance(Vehicule vCarAction)
        {
            float fDistFreinage = calcul_distance_freinage(vCarAction);

            //  Calcul du coefficient directeur pour la courbe (abscisse : distance; ordonnée : vitesse)
            float iCoefDir = (float)vitesse / (fDistFreinage - ((vDevant().longueur) / 3));
        }
#if true
        private void slow_down_or_accelerate(List<Vehicule> listVehicule)
        {
            Vehicule vDevant = GetVehiculeDevant(this, listVehicule);
            double dDistanceBetween;
            float fDistanceFreinage;

            if (vDevant == null)
            {
                if (this.vitesse < this.vitesseMax)
                    this.vitesse += 1;
            }
            else //(voiture_devant_exist)
            {
                //if (feu_vert)
                {
                    dDistanceBetween = calcul_distance_entre_les_deux_voitures(this, vDevant);
                    fDistanceFreinage = calcul_distance_freinage(this);
                    if (dDistanceBetween >= this.longueur / 2)
                    {

                        //  On a laissé suffisamment de distance avant de redémarrer et on peut encore accélérer
                        if (dDistanceBetween + ((vDevant.longueur) / 3) >= fDistanceFreinage)
                        {
                            if (this.vitesse < this.vitesseMax)
                                this.vitesse += 1;
                        }
                        else  //  On doit freiner car la distance de freinage est insuffisante
                        {
                            if (this.vitesse >= 0)
                                this.vitesse -= 1;
                        }
                    }
                    else
                    {
                        //Freinage d'urgence 
                        this.vitesse = 0;
                    }
                    //}
                    //else    //  feu rouge
                    //{
                    //  if (this.vitesse >= 0)
                    //      this.vitesse -= 1;
                    //}
                }
            }
        }
    }
}
#endif