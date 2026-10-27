using System.Net;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogApp.Client.Models;

namespace ProductCatalogApp.Client.Services
{
    public class ProductCatalogApiClient : IProductCatalogApiClient
    {
        private const string SitesUrl = "api/ProductionSites";
        private const string EquipmentUrl = "api/Equipments";

        private readonly HttpClient _http;

        public ProductCatalogApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProductionSite>> GetProductionSitesAsync(string? search = null)
        {
            var url = AddQuery(SitesUrl, ("search", search));

            return await _http.GetFromJsonAsync<List<ProductionSite>>(url) ?? new();
        }

        public async Task<ProductionSiteDetails?> GetProductionSiteAsync(int id)
        {
            var response = await _http.GetAsync($"{SitesUrl}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<ProductionSiteDetails>();
        }

        public async Task<ApiResult> CreateProductionSiteAsync(ProductionSite site)
        {
            return await ToResultAsync(await _http.PostAsJsonAsync(SitesUrl, site));
        }

        public async Task<ApiResult> UpdateProductionSiteAsync(int id, ProductionSite site)
        {
            return await ToResultAsync(await _http.PutAsJsonAsync($"{SitesUrl}/{id}", site));
        }

        public async Task<ApiResult> DeleteProductionSiteAsync(int id)
        {
            return await ToResultAsync(await _http.DeleteAsync($"{SitesUrl}/{id}"));
        }

        public async Task<List<Equipment>> GetEquipmentAsync(int? productionSiteId = null, string? search = null)
        {
            var url = AddQuery(
                EquipmentUrl,
                ("productionSiteId", productionSiteId?.ToString()),
                ("search", search));

            return await _http.GetFromJsonAsync<List<Equipment>>(url) ?? new();
        }

        public async Task<Equipment?> GetEquipmentByIdAsync(int id)
        {
            var response = await _http.GetAsync($"{EquipmentUrl}/{id}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Equipment>();
        }

        public async Task<ApiResult> CreateEquipmentAsync(Equipment equipment)
        {
            return await ToResultAsync(await _http.PostAsJsonAsync(EquipmentUrl, equipment));
        }

        public async Task<ApiResult> UpdateEquipmentAsync(int id, Equipment equipment)
        {
            return await ToResultAsync(await _http.PutAsJsonAsync($"{EquipmentUrl}/{id}", equipment));
        }

        public async Task<ApiResult> DeleteEquipmentAsync(int id)
        {
            return await ToResultAsync(await _http.DeleteAsync($"{EquipmentUrl}/{id}"));
        }

        private static string AddQuery(string url, params (string Name, string? Value)[] parameters)
        {
            var pairs = parameters
                .Where(p => !string.IsNullOrWhiteSpace(p.Value))
                .Select(p => $"{p.Name}={Uri.EscapeDataString(p.Value!.Trim())}")
                .ToList();

            return pairs.Count == 0 ? url : $"{url}?{string.Join('&', pairs)}";
        }

        private static async Task<ApiResult> ToResultAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return ApiResult.Success();
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return ApiResult.NotFoundResult();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

                return ApiResult.Invalid(problem?.Errors ?? new Dictionary<string, string[]>());
            }

            response.EnsureSuccessStatusCode();

            return ApiResult.Success();
        }
    }
}
