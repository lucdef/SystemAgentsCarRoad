namespace IAAgents
{
    internal class Voiture : Vehicule
    {
        public Voiture(Direction dir, Position pos,uint longeur,uint largeur,float vitesseMax) : base(dir, pos)
        {
            this.longueur = longeur;
            this.largeur = largeur;
            this.vitesseMax = vitesseMax;
        }
    }
}