namespace MarketSystem.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public abstract class Entity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}