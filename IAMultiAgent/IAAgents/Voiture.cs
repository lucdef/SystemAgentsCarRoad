using System.Collections.Generic;

namespace IAAgents
{
    internal class Voiture : Vehicule
    {
        public Voiture(Direction dir, List<Route> lstRoute,uint longeur,uint largeur,float vitesseMax) : base(dir, lstRoute)
        {
            this.longueur = longeur;
            this.largeur = largeur;
            this.vitesseMax = vitesseMax;
        }
    }
}