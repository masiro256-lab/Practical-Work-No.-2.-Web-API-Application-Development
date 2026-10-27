using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Client.Models
{
    public class ProductionSite
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Укажите название участка.")]
        [StringLength(100, ErrorMessage = "Название не должно быть длиннее 100 символов.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        [StringLength(500, ErrorMessage = "Описание не должно быть длиннее 500 символов.")]
        public string? Description { get; set; }

        [Display(Name = "Единиц оборудования")]
        public int EquipmentCount { get; set; }
    }

    public class ProductionSiteDetails
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        public List<Equipment> Equipment { get; set; } = new();
    }
}
