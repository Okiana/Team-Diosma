namespace MarketSystem.Models
{
    using System;
    using System.Text;

    public class Supermarket : Entity
    {
        public int TownId { get; set; }

        public virtual Town Town { get; set; }

        public override string ToString()
        {
            var supermarket = new StringBuilder(base.ToString());
            supermarket.AppendFormat("Town: {0}{1}", this.TownId, Environment.NewLine);

            return supermarket.ToString();
        }
    }
}