namespace MarketSystem.Models
{
    using System;
    using System.Text;

    public abstract class Entity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            var entity = new StringBuilder();
            entity.AppendFormat("Id: {0}{1}", this.Id, Environment.NewLine);
            entity.AppendFormat("Name: {0}{1}", this.Name, Environment.NewLine);

            return entity.ToString();
        }
    }
}