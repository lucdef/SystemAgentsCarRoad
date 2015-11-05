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
            randomVehiculeGenerator = new Random();
            double nb = (double)randomVehiculeGenerator.Next(0, 100) / 100;
            Vehicule vehicule;
            if(nb<=0.25)
            {
                return vehicule = new Camion(dir,route,32,12,80);
            }
            if(nb>0.25)
            {
                return vehicule = new Voiture(dir,route,24,12,90);
            }
            return new Voiture(dir, route, 24, 12, 90);

        }
    }
}
