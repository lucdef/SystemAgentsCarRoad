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

        public Carrefour()
        {
            nbVehicule = 1;
            lstVehicule = new List<Vehicule>();
            seedAleatoire = new Random();

            for(int i=0;i<nbVehicule;i++)
            {
                Vehicule vehicule = new Vehicule(this.GetRandomDirection(),this.GetRandomPosition());
                lstVehicule.Add(vehicule);
            }
        }

        public double calcul_distance_entre_les_deux_voitures(Vehicule vAction, Vehicule vToAvoid)
        {
            double dDeltaDistance = 0.0f;
            Position pVoitureAction;
            Position pVoitureToAvoid;
            //Direction dVoitureAction;
            //Direction dVoitureToAvoid;

            pVoitureAction = vAction.GetPosition();
            pVoitureToAvoid = vToAvoid.GetPosition();
            //dVoitureAction = vAction.GetDirection();
            //dVoitureToAvoid = vToAvoid.GetDirection();

            //if (dVoitureAction == Direction.GAUCHE)
            //{
            //  if(dVoitureToAvoid == Direction.GAUCHE)
            //  {
            double dToAvoid =  pVoitureToAvoid.GetX();
            double dAction = pVoitureAction.GetX();
            dDeltaDistance = dToAvoid - dAction;
            //}
            //}
            return dDeltaDistance;
        }

        public float calcul_distance_freinage(Vehicule vToBrake)
        {
            float dDistFreinage = 0.0f;
            float fActualSpeed = vToBrake.GetSpeed();
            fActualSpeed /= 10;
            return dDistFreinage * dDistFreinage;
        }

        public void calcul_courbe_vitesse_distance(Vehicule vCarAction)
        {
            float fDistFreinage = calcul_distance_freinage(vCarAction);
         //   int iCoefDir = VitMax2(km / h) / (iDistFreinage - ((voiture_2.longueur) / 3))
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

    }
}
