using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    public delegate void CarrefourUpdated(List<Vehicule> lstVehicule);
    public class Carrefour
    {
       
        public event CarrefourUpdated carrefourUpdatedEvent;


        const uint Height = 480;
        const uint Width = 640;
        const string Couleur = "#ffffff";
        static Random seedAleatoire;
        uint nbVehicule;

        List<Vehicule> lstVehicule;
        List<Feu> lstFeux;

        public Carrefour()
        {
            nbVehicule = 1;
            lstVehicule = new List<Vehicule>();
            seedAleatoire = new Random();
            for(int i=0;i<nbVehicule;i++)
            {
                Vehicule vehicule = VehiculeFactory.GetVehicule(this.GetRandomDirection(), this.GetRandomPosition());
                lstVehicule.Add(vehicule);
            }
            for(int f=0;f<4;f++)
            {
                //Génération des feux attention changer la position 
                lstFeux.Add(new Feu(false,new TimeSpan(0,0,15),new TimeSpan(0,0,15),new Position(50,50)));
            }
        }
        private Direction GetRandomDirection()
        {
            Direction direction;
            uint nbAleatoire = (uint) seedAleatoire.Next(0, 1);
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
            return new Position(50,50);
        }
        public void UpdateCarrefour()
        {
            UpdateVoiture();
            if (carrefourUpdatedEvent != null)
            {
                carrefourUpdatedEvent(lstVehicule );
            }
        }
        private void UpdateVoiture()
        {
            foreach (Vehicule vehicule in lstVehicule)
            {
                vehicule.Update(lstVehicule);
            }
        }
        private void UpdateFeux()
        {
            foreach(Feu feu in lstFeux)
            {
                feu.tempsActivite.Add(new TimeSpan(0, 0, 0, 0, 15));
                if((feu.isVert&&feu.tempsVert==feu.tempsActivite)||(!feu.isVert&&feu.tempsRouge==feu.tempsActivite))
                    {
                    feu.ToggleFeu();
                }
            }
        }

    }
}
