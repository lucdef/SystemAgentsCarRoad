using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    public class Route
    {
        List<KeyValuePair<Direction,Route>> routeLie;
        uint largeur;
        uint  longueur;
        Position position;
        Direction direction;
        Feu feu;
        public Route(uint longueur,uint largeur,Position position,Direction direction)
        {
            routeLie = new List<KeyValuePair<Direction,Route>>();
            this.longueur = longueur;
            this.largeur = largeur;
            this.position = position;
            this.direction = direction;
        }
        public void SetFeu(Feu feu)
        {
            this.feu = feu;
        }
        public double GetLargeur()
        {
            return this.largeur;
        }
        public double GetLongueur()
        {
            return this.longueur;
        }
        public Position GetPosition()
        {
            return this.position;
        }
        public Direction GetDirection()
        {
            return this.direction;
        }
        
        public void setRoute(List<KeyValuePair<Direction,Route>>routes)
        {
            this.routeLie = routes;
        }

        internal List<KeyValuePair<Direction,Route>> getRouteLie()
        {
            return this.routeLie;
        }

        public Feu GetFeu()
        {
            return this.feu;
        }
    }
}
