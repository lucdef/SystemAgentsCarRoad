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
        protected const double STEP = 0.030;
        protected int iDansZone = 0;
        protected const double DISTANCE_MARGE_VEHICULES = 20;
        protected const double DISTANCE_MARGE_PASSAGE_PIETON = 10;
        protected double coefDir_XFreinage_YVitesse;
        protected double vitesse;
        protected double angle;
        static uint toto;
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
        public void setPosition(Position position)
        {
            this.position = position;
        }


        public Vehicule(Direction dir, List<Route> itineraire)
        {
            this.itineraire = itineraire;
            this.longueur = 24;
            this.largeur = 12;
            this.vitesse = 0;
            this.vitesseMax = 20;
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
                    setAngle(0);
                    X = (routeInitial.GetPosition().GetX() + routeInitial.GetLargeur() / 2) - this.GetLargeur() / 2;
                    Y = routeInitial.GetPosition().GetY() - this.longueur;
                    break;
                case Direction.DROITE:
                    setAngle(90);
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
        public double getAngle()
        {
            return this.angle;
        }
        public void setAngle(double Angle)
        {
            this.angle = Angle;
        }

        public void Update(List<Vehicule> lstVehicule)
        {

            UpdatePosition(lstVehicule);

        }
        //Méthode permettant de déterminer le vehicule devant 
        private Vehicule GetVehiculeDevant(List<Vehicule> lstVehicule)
        {
            Vehicule vDev = null;
            List<Vehicule> lstVehiculeOnTheRoad = null;

            if (this.GetRouteActuel().GetDirection() == Direction.EN_FACE)
            {
                lstVehiculeOnTheRoad = lstVehicule.FindAll(v => v.GetRouteActuel() == this.GetRouteActuel() && v != this && v.GetPosition().GetY() > this.position.GetY()).OrderBy(v => this.GetPosition().GetY() - v.GetPosition().GetY()).ToList();
                if (lstVehiculeOnTheRoad.Count > 0)
                    vDev = lstVehiculeOnTheRoad.First();
            }
            else if (this.GetRouteActuel().GetDirection() == Direction.DROITE)
            {
                lstVehiculeOnTheRoad = lstVehicule.FindAll(v => v.GetRouteActuel() == this.GetRouteActuel() && v != this && v.GetPosition().GetX() > this.position.GetX()).OrderBy(v => this.GetPosition().GetX() - v.GetPosition().GetX()).ToList();
                if (lstVehiculeOnTheRoad.Count > 0)
                    vDev = lstVehiculeOnTheRoad.First();
            }
            return vDev;
        }


        //Changer nom update positionetvitesse
        private void UpdatePosition(List<Vehicule> listVehicule)
        {
            slow_down_or_accelerate(listVehicule);
            //If englobant tout pour tourner c le cas ou on est sur la premiere route
                
            if (this.GetRouteActuel().GetDirection()==Direction.EN_FACE)
            {
                if(this.indexRouteActuel==0)
                {
                    if (this.vitesse > 0 && (this.GetRouteActuel().GetPosition().GetY() - this.GetRouteActuel().GetLongueur() + 10) >= this.GetPosition().GetY())
                    {
                        if (this.GetRouteActuel().GetDirection() == Direction.DROITE)
                        {
                            this.angle = this.angle + 45;
                            double posX = this.GetPosition().GetX() + 3;
                            double posY = this.position.GetY() - STEP * vitesse;
                            this.position = new Position(posX, posY);
                            if (this.angle == 90)
                                this.indexRouteActuel = 1;

                        }
                        else
                        {
                            double posX = this.GetPosition().GetX();
                            double posY = this.position.GetY() - STEP * vitesse;
                            this.position = new Position(posX, posY);
                            indexRouteActuel = 1;
                        }
                    }
                    else
                    {
                        double posX = this.GetPosition().GetX();
                        double posY = this.position.GetY() - STEP * vitesse;
                        this.position = new Position(posX, posY);
                    }
                   
                }
                else
                {
                    double posX = this.GetPosition().GetX();
                    double posY = this.position.GetY() - STEP * vitesse;
                    this.position = new Position(posX, posY);
                }
    
            }
            else if(this.GetRouteActuel().GetDirection()==Direction.DROITE)
            {
                if (this.indexRouteActuel == 0)
                {
                    if (this.vitesse > 0 && (this.GetRouteActuel().GetPosition().GetX() + this.GetRouteActuel().GetLongueur() + 10) <= this.GetPosition().GetX())
                    {
                        //Changement de route
                        if (this.itineraire.ElementAt(1).GetDirection() == Direction.EN_FACE)
                        {
                            this.angle = this.angle - 45;
                            double posX = this.GetPosition().GetX() + STEP * vitesse;
                            double posY = this.position.GetY() + 3;
                            this.position = new Position(posX, posY);
                            if (this.angle == 0)
                                this.indexRouteActuel = 1;
                        }
                        else
                        {
                            double posX = this.GetPosition().GetX() + STEP * vitesse;
                            double posY = this.position.GetY();
                            this.position = new Position(posX, posY);
                            this.indexRouteActuel = 1;
                        }
                    }
                    else
                    {
                        double posX = this.GetPosition().GetX() + STEP * vitesse;
                        double posY = this.position.GetY();
                        this.position = new Position(posX, posY);
                    }
                }
                else
                {
                    double posX = this.GetPosition().GetX() + STEP * vitesse;
                    double posY = this.position.GetY();
                    this.position = new Position(posX, posY);
                }
            }

            //if (this.GetRouteActuel().GetDirection() == Direction.EN_FACE)
            //{
            //    double posX = this.GetPosition().GetX();
            //    double posY = this.position.GetY() - STEP * vitesse;
            //    this.position = new Position(posX, posY);
            //}
            //else if (this.GetRouteActuel().GetDirection() == Direction.DROITE)
            //{
            //    double posX = this.GetPosition().GetX() + STEP * vitesse;
            //    double posY = this.position.GetY();
            //    this.position = new Position(posX, posY);
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

            //  Récupération des positions, soit en X ou soit en Y suivant dans quel direction les deux véhicules circulent
            if (this.GetRouteActuel().GetDirection() == Direction.EN_FACE)
            {
                xyToAvoid = pVoitureToAvoid.GetY();
                xyAction = pVoitureAction.GetY();
            }
            else
            {
                xyToAvoid = pVoitureToAvoid.GetX();
                xyAction = pVoitureAction.GetX();
            }
            dDeltaDistance = Math.Abs(xyToAvoid - xyAction);

            return dDeltaDistance;
        }

        public double calcul_distance_freinage()
        {
            double dDistFreinage = 0.0f;
            double dActualSpeedVehiculeToBreak = this.GetVitesse();
            dDistFreinage = dActualSpeedVehiculeToBreak / 10;
            dDistFreinage = dDistFreinage * dDistFreinage;
            return dDistFreinage;
        }

        private void slow_down_or_accelerate(List<Vehicule> listVehicule)
        {
            Vehicule vDevant = GetVehiculeDevant(listVehicule);
            double dDistanceFreinage = calcul_distance_freinage();
            double routeLong = GetRouteActuel().GetLongueur();
            double vehiLeftX = this.GetPosition().GetX();
            double vehiRightX = vehiLeftX + this.GetLongueur();
            string result = null;
            double dDistanceBetweenWithSecurity = 0;
            double dPositionMaxNoCollision;

            if (vDevant == null)
            {
                if (this.indexRouteActuel == 0 && GetRouteActuel().GetFeu().isVert)
                {
                    result = string.Format("Le feu est vert : Boucle {0} Vitesse = {1}", toto, this.vitesse);
                    if (this.vitesse < this.vitesseMax)
                        this.vitesse += 1;
                }
                else
                {
                    //  Calcul de la position du point de début de freinage
                    double dPositionDébutFreinage = (routeLong - DISTANCE_MARGE_PASSAGE_PIETON - dDistanceFreinage * 3);

                    //  Si le véhicule n'est pas rentré dans la zone de freinage
                    if ((vehiRightX < dPositionDébutFreinage) && (iDansZone != 1))
                    {
                        result = string.Format("Boucle {0} : Je suis avant ma zone de freinage : Vitesse = {1}", toto, this.vitesse);
                        if (this.vitesse < this.vitesseMax)
                            this.vitesse += 1;
                    }

                    //  Le véhicule a depassé le feu rouge
                    else if (vehiRightX > routeLong)
                    {
                        result = string.Format("Boucle {0} : J'ai depassé le feu rouge : Vitesse = {1}", toto, this.vitesse);
                        if (this.vitesse < this.vitesseMax)
                            this.vitesse += 1;
                        iDansZone = 0;
                    }
                    else
                    {
                        result = string.Format("Boucle {0} : Je suis dedans la zone de freinage : Vitesse = {1}", toto, this.vitesse);
                        //  La on voit que l'on aura pas le temps de freiner.
                        //  Donc on freine en urgence
                        iDansZone = 1;
                        if ((vehiRightX + dDistanceFreinage * 3) > (routeLong - DISTANCE_MARGE_PASSAGE_PIETON))
                        {
                            if (this.vitesse > 4)
                                this.vitesse -= 4;
                        }
                        else
                        {
                            //  Il faut freiner au bon moment, c'est a dire, suivant la vitesse actuelle de la voiture, 
                            //  il faut commencer a décrémenter la vitesse pour s'arreter pile au passage pour piéton
                            if (routeLong - DISTANCE_MARGE_PASSAGE_PIETON < vehiRightX + dDistanceFreinage * 3)
                            {
                                if (this.vitesse > 0)
                                    this.vitesse -= 1;
                            }
                        }

                        if (routeLong - DISTANCE_MARGE_PASSAGE_PIETON < vehiRightX + dDistanceFreinage * 3)
                            if (this.vitesse > 0)
                                this.vitesse -= 1;
                    }
                }
               // Console.WriteLine(result);
            }

            else //(voiture_devant_exist)
            {
                //  Calcul distance entre la voiture de devant et la voiture de derrière
                //dDistanceBetween = calcul_distance_entre_les_deux_voitures(this, vDevant);
                dDistanceBetweenWithSecurity = calcul_distance_entre_les_deux_voitures(this, vDevant) + DISTANCE_MARGE_VEHICULES;

                if (this.indexRouteActuel == 0 && GetRouteActuel().GetFeu().isVert)
                {
                    //  On a laissé suffisamment de distance avant de redémarrer et on peut encore accélérer
                    if (dDistanceBetweenWithSecurity >= dDistanceFreinage + DISTANCE_MARGE_VEHICULES)
                    {
                        if (this.vitesse < this.vitesseMax)
                            this.vitesse += 1;
                    }
                    else  //  On doit freiner car la distance de freinage est insuffisante
                    {
                        if (this.vitesse >= 0)
                            this.vitesse -= 1;
                        dDistanceFreinage = calcul_distance_freinage();
                    }
                }
                else    //  feu rouge
                {                    
                    //  Si le véhicule de devant a depassé le feu 
                    if ((vDevant.GetPosition().GetX() + vDevant.GetLongueur()) > (routeLong))
                        //  On se fixe comme limite pour freiner le X du feu
                        dPositionMaxNoCollision = routeLong - DISTANCE_MARGE_PASSAGE_PIETON;
                    else
                        //  On se fixe l'arrière de la voiture de devant car elle a pas encore depassé le feu
                        //  et donc c'est elle qui doit nous servir de limite pour le freinage
                        dPositionMaxNoCollision = vehiRightX + DISTANCE_MARGE_VEHICULES + dDistanceFreinage;


                    if (dPositionMaxNoCollision > (vDevant.GetPosition().GetX()))
                    {
                        if (listVehicule.ElementAt(2) != null)
                        {
                            if ((this == listVehicule.ElementAt(2)))
                            {
                                result = string.Format("Boucle {0} : Je dois freiner !!!", toto);
                                Console.WriteLine(result);
                            }
                        }
                        if (this.vitesse > 0)
                            this.vitesse -= 1;
                    }
                    else
                    {
                        if ((dPositionMaxNoCollision) <= (vehiRightX + dDistanceFreinage + DISTANCE_MARGE_VEHICULES))
                        {
                            if (listVehicule.ElementAt(2) != null)
                            {
                                if ((this == listVehicule.ElementAt(2)))
                                {
                                    result = string.Format("Boucle {0} : Je dois accelerer !!!", toto);
                                    Console.WriteLine(result);
                                }
                            }

                            if (this.vitesse < this.vitesseMax)
                                    this.vitesse += 1;
                        }
                    }
                }
            }
            toto++;
        }
    }
}
