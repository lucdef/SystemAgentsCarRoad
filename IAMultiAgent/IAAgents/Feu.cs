using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IAAgents
{
    class Feu
    {
        
        public bool isVert { get; set; }
       public  TimeSpan tempsRouge { get; set; }
        public TimeSpan tempsVert { get; set; }
        public TimeSpan tempsActivite { get; set; }
        public Position position { get; set; }

        public Feu(bool isVert, TimeSpan tRouge, TimeSpan tVert,Position position)
        {
            this.isVert = isVert;
            this.tempsRouge = tRouge;
            this.tempsVert = tVert;
            this.position = position;
        }
        public void ToggleFeu()
        {
            this.isVert = !this.isVert;
            this.tempsActivite = new TimeSpan(0, 0, 0, 0, 0);
        }
    }
}
