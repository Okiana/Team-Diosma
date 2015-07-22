namespace MarketSystem.Models
{
    using System.Collections.Generic;

    public class Town : Entity
    {
        public Town()
        {
            this.Supermarkets = new HashSet<Supermarket>();
        }

        public virtual ICollection<Supermarket> Supermarkets { get; set; }
    }
}