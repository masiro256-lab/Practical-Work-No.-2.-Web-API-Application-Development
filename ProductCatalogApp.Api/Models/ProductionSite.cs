namespace ProductCatalogApp.Api.Models
{
    public class ProductionSite
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public List<Equipment> EquipmentItems { get; set; } = new();
    }
}
