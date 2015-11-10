namespace IAAgents
{
    public interface ICollisionable //Interface pour être plus générique
    {
         Position GetPosition();
        double GetX();
        double GetY();
        Direction GetDirection();

    }
}