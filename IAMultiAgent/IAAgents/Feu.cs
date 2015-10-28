using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAAgents
{
    class Feu
    {
        bool isVert { get; set; }
        TimeSpan tempsRouge { get; set; }
        TimeSpan tempsVert { get; set; }

        public Feu(bool isVert, TimeSpan tRouge, TimeSpan tVert)
        {
            this.isVert = isVert;
            this.tempsRouge = tRouge;
            this.tempsVert = tVert;
        }
    }
}
