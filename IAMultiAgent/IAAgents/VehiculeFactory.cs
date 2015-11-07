using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    public static class VehiculeFactory
    {
        static Random randomVehiculeGenerator;
        public static Vehicule GetVehicule(Direction dir,List<Route> route)
        {
            /*
            randomVehiculeGenerator = new Random();
            double nb = (double)randomVehiculeGenerator.Next(0, 100);
            Vehicule vehicule;
            if(nb<=25)
            {
                return vehicule = new Camion(dir,route,32,12,40);
            }
            if(nb>25)
            {
                return vehicule = new Voiture(dir,route,24,12,250);
            }
            */
            return new Voiture(dir, route, 24, 12, 90);

        }
    }
}
