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
        protected float vitesse;
        protected uint angle;
        
        static Random seedCouleurRandom;

        Position position;
        Vehicule vehiculeDevant; 
        bool estArreter;
        public string couleur { private set; get; }


        public Vehicule( Direction dir, Position pos)
        {
           
            this.longueur = 24;
            this.largeur = 12;
            this.vitesse = 0;
            this.vitesseMax = 50;
            this.direction = dir;
            this.position = pos;
            this.vehiculeDevant = null;
            this.estArreter = false;
            seedCouleurRandom = new Random();
            this.couleur = this.SetCouleur();
        }
        private string  SetCouleur()
        {
            string couleur;
            int nbAleatoire = seedCouleurRandom.Next(0, 3);
            switch(nbAleatoire)
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
        public uint GetLongueur()
        {
            return this.longueur;
        }
        public uint GetLargeur()
        {
            return this.largeur;
        }
        public float GetVitesse()
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
        public void Update(List<Vehicule>lstVehicule)
        {
            foreach (Vehicule vehicule in lstVehicule)
            {

            }
            UpdatePosition();

        }
        private void UpdatePosition()
        {
            double posX = this.position.GetX()+ STEP * vitesse;
            double posY =this.position.GetY() + STEP * vitesse;
            this.position = new Position(posX, posY);
        }

        public double calcul_distance_entre_les_deux_voitures(Vehicule vAction, Vehicule vToAvoid)
        {
            double dDeltaDistance = 0.0f;
            double xToAvoid = 0.0f;
            double xAction = 0.0f;
            Position pVoitureAction;
            Position pVoitureToAvoid;

            pVoitureAction = GetPosition();
            pVoitureToAvoid = vToAvoid.GetPosition();
            xToAvoid = pVoitureToAvoid.GetX();
            xAction = pVoitureAction.GetX();
            dDeltaDistance = xToAvoid - xAction;

            return dDeltaDistance;
        }

        public float calcul_distance_freinage(Vehicule vToBrake)
        {
            float dDistFreinage = 0.0f;
            float fActualSpeedVehiculeToBreak = vToBrake.GetVitesse();
            //  Pour calculer la distance de freinage, il faut mettre au carré le chiffre des dizaines
            //  donc il faut diviser par 10 pour avoir seulement le chiffre des dizaines.
            fActualSpeedVehiculeToBreak /= 10;
            return dDistFreinage * dDistFreinage;
        }

        public void calcul_courbe_vitesse_distance(Vehicule vCarAction)
        {
            float fDistFreinage = calcul_distance_freinage(vCarAction);

            float iCoefDir = vitesse / (fDistFreinage - ((vDevant().longueur) / 3));
        }
#if false
        public void slow_down_or_accelerate()
        {
            if (!voiture_devant_exist)
                vitesse++->vitesse_max
            else //(voiture_devant_exist)
            {
                if (feu_vert)
                {
                    calcul_distance_entre_les_deux_voitures
                    calcul_distance_freinage_voiture_derriere

                    //  On a laissé suffisamment de distance avant de redémarrer et on peut encore accélérer
                    if (distance_entre_les_deux_voitures + ((voiture_2.longueur) / 3) >= distance_freinage_voiture_derriere)
                    {
                        if (vitesse_voiture_derriere <= vitesse_max)
                            vitesse_voiture_derriere++
                    }
                    else  //  On doit freiner car la distance de freinage est insuffisante
                    {
                        if (vitesse_voiture_derriere >= vitesse_min)
                        vitesse_voiture_derriere--
                    }
                }
                else    //  feu rouge
                    vitesse_voiture_derriere--
            } 
#endif

    }

}
