using ProductCatalogApp.Client.Models;

namespace ProductCatalogApp.Client.Services
{
    public interface IProductCatalogApiClient
    {
        Task<List<ProductionSite>> GetProductionSitesAsync(string? search = null);

        Task<ProductionSiteDetails?> GetProductionSiteAsync(int id);

        Task<ApiResult> CreateProductionSiteAsync(ProductionSite site);

        Task<ApiResult> UpdateProductionSiteAsync(int id, ProductionSite site);

        Task<ApiResult> DeleteProductionSiteAsync(int id);

        Task<List<Equipment>> GetEquipmentAsync(int? productionSiteId = null, string? search = null);

        Task<Equipment?> GetEquipmentByIdAsync(int id);

        Task<ApiResult> CreateEquipmentAsync(Equipment equipment);

        Task<ApiResult> UpdateEquipmentAsync(int id, Equipment equipment);

        Task<ApiResult> DeleteEquipmentAsync(int id);
    }
}
