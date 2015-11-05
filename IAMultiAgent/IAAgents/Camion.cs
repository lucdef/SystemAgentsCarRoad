using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    public class Camion : Vehicule
    {
        public Camion(Direction dir, List<Route> lstRoute, uint longeur, uint largeur, float vitesseMax) : base(dir, lstRoute)
        {
            this.longueur = longeur;
            this.largeur = largeur;
            this.vitesseMax =  vitesseMax;
        }
    }
}
