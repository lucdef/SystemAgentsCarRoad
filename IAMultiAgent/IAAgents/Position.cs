using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    public class Position
    {
        double x {get; set;}
        double y {get; set;}

        public Position(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double GetX()
        {
            return this.x;
        }
        public double GetY()
        {
            return this.y;
        }
    }
}
