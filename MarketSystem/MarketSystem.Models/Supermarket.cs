namespace MarketSystem.Models
{
    public class Supermarket : Entity
    {
        public int TownId { get; set; }

        public virtual Town Town { get; set; }
    }
}