using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    enum Direction
    {
        G,
        D,
        F
    }

    public class Vehicule
    {
        float longueur;
        float largeur;
        float vMax;
        Direction dir;

        Position pos;
        Vehicule vDevant; 
        int a;
        bool arret;

        public Vehicule(float longueur, float largeur, float vMax, Direction dir, Position pos)
        {
            this.longueur = longueur;
            this.largeur = largeur;
            this.vMax = vMax;
            this.dir = dir;
            this.pos = pos;
            this.vDevant = null;
            this.arret = false;
        }
    }
}
