using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Api.Dtos
{
    public record EquipmentDto(
        int Id,
        string Name,
        string Model,
        string InventoryNumber,
        int ProductionSiteId,
        string ProductionSiteName);

    public class EquipmentInput
    {
        [Required(ErrorMessage = "Укажите название оборудования.")]
        [StringLength(100, ErrorMessage = "Название не должно быть длиннее 100 символов.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите модель.")]
        [StringLength(100, ErrorMessage = "Модель не должна быть длиннее 100 символов.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите инвентарный номер.")]
        [StringLength(50, ErrorMessage = "Инвентарный номер не должен быть длиннее 50 символов.")]
        public string InventoryNumber { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Выберите производственный участок.")]
        public int ProductionSiteId { get; set; }
    }
}
