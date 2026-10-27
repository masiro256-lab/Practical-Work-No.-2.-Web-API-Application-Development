using System.ComponentModel.DataAnnotations;

namespace ProductCatalogApp.Api.Dtos
{
    public record ProductionSiteDto(int Id, string Name, string Description, int EquipmentCount);

    public record ProductionSiteDetailsDto(
        int Id,
        string Name,
        string Description,
        IReadOnlyList<EquipmentDto> Equipment);

    public class ProductionSiteInput
    {
        [Required(ErrorMessage = "Укажите название участка.")]
        [StringLength(100, ErrorMessage = "Название не должно быть длиннее 100 символов.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Описание не должно быть длиннее 500 символов.")]
        public string? Description { get; set; }
    }
}
