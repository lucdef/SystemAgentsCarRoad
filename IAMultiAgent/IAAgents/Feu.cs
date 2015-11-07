using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IAAgents
{
    public class Feu
    {
        
        public bool isVert { get; set; }
       public  TimeSpan tempsRouge { get; set; }
        public TimeSpan tempsVert { get; set; }
        public TimeSpan tempsActivite { get; set; }
        

        public Feu(bool isVert, TimeSpan tRouge, TimeSpan tVert)
        {
            this.isVert = isVert;
            this.tempsRouge = tRouge;
            this.tempsVert = tVert;
            
        }
        public void ToggleFeu()
        {
            this.isVert = !this.isVert;
            this.tempsActivite = new TimeSpan(0, 0, 0, 0, 0);
        }
    }
}
