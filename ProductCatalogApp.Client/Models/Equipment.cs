using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Client.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Укажите название оборудования.")]
        [StringLength(100, ErrorMessage = "Название не должно быть длиннее 100 символов.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Модель")]
        [Required(ErrorMessage = "Укажите модель.")]
        [StringLength(100, ErrorMessage = "Модель не должна быть длиннее 100 символов.")]
        public string Model { get; set; } = string.Empty;

        [Display(Name = "Инвентарный номер")]
        [Required(ErrorMessage = "Укажите инвентарный номер.")]
        [StringLength(50, ErrorMessage = "Инвентарный номер не должен быть длиннее 50 символов.")]
        public string InventoryNumber { get; set; } = string.Empty;

        [Display(Name = "Производственный участок")]
        [Required(ErrorMessage = "Выберите производственный участок.")]
        [Range(1, int.MaxValue, ErrorMessage = "Выберите производственный участок.")]
        public int? ProductionSiteId { get; set; }

        [Display(Name = "Производственный участок")]
        public string ProductionSiteName { get; set; } = string.Empty;
    }
}
