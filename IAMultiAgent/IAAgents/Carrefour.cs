using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    public class Carrefour
    {
        public delegate void CarrefourUpdated(List<Vehicule> lstVehicule);
        public event CarrefourUpdated oceanUpdatedEvent;


        const uint Height = 480;
        const uint Width = 640;
        const string Couleur = "#ffffff";
        static Random seedAleatoire;
        uint nbVehicule;

        List<Vehicule> lstVehicule;

        public event CarrefourUpdated oceanUpdatedEvent;

        public Carrefour()
        {
            lstVehicule = new List<Vehicule>();
            seedAleatoire = new Random();
            for(int i=0;i<nbVehicule;i++)
            {
                Vehicule vehicule = new Vehicule(this.GetRandomDirection,this.GetRandomPosition());
                lstVehicule.Add(vehicule);
            }
        }
        private Direction GetRandomDirection()
        {
            Direction direction;
            uint nbAleatoire = seedAleatoire.Next(0, 1);
            switch(nbAleatoire)
            {
                case 0:
                    direction = Direction.EN_FACE;
                    break;
                case 1:
                    direction = Direction.DROITE;
                    break;
                default:
                    direction = Direction.EN_FACE;
                    break;

            }
            return direction;
        }
        private Position GetRandomPosition()
        {

        }
        public void UpdateCarrefour()
        {

        }

    }
}
