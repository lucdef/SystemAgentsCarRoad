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
    }
}
