namespace Systems.Entities
{
    public interface IEntity
    {
        public void Birth();
        public void Tick();
        public void Die();
    }
}