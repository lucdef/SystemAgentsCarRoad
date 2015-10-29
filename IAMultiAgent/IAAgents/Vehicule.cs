using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    

    public class Vehicule
    {
        uint longueur;
        uint largeur;
        float vitesseMax;
        Direction direction;
        float vitesse;
        static Random seedCouleurRandom;

        Position position;
        Vehicule vehiculeDevant; 
        bool estArreter;
        public string couleur { private set; get; }


        public Vehicule( Direction dir, Position pos)
        {
           
            this.longueur = 8;
            this.largeur = 4;
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
    }
}
