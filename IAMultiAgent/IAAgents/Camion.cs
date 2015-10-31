using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    public class Camion : Vehicule
    {
        public Camion(Direction dir, Position pos, uint longeur, uint largeur, float vitesseMax) : base(dir, pos)
        {
            this.longueur = longeur;
            this.largeur = largeur;
            this.vitesseMax =  vitesseMax;
        }
    }
}
