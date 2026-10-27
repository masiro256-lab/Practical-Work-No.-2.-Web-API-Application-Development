using Microsoft.AspNetCore.Mvc;
using ProductCatalogApp.Client.Models;
using ProductCatalogApp.Client.Services;

namespace ProductCatalogApp.Client.Controllers
{
    public class ProductionSitesController : Controller
    {
        private readonly IProductCatalogApiClient _api;

        public ProductionSitesController(IProductCatalogApiClient api)
        {
            _api = api;
        }

        // GET: PRODUCTIONSITES
        public async Task<IActionResult> Index(string? search)
        {
            ViewData["Search"] = search;

            return View(await _api.GetProductionSitesAsync(search));
        }

        // GET: PRODUCTIONSITES/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productionSite = await _api.GetProductionSiteAsync(id.Value);

            if (productionSite == null)
            {
                return NotFound();
            }

            return View(productionSite);
        }

        // GET: PRODUCTIONSITES/Create
        public IActionResult Create()
        {
            return View(new ProductionSite());
        }

        // POST: PRODUCTIONSITES/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductionSite productionSite)
        {
            if (ModelState.IsValid)
            {
                var result = await _api.CreateProductionSiteAsync(productionSite);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                result.CopyErrorsTo(ModelState);
            }

            return View(productionSite);
        }

        // GET: PRODUCTIONSITES/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var details = await _api.GetProductionSiteAsync(id.Value);

            if (details == null)
            {
                return NotFound();
            }

            return View(ToProductionSite(details));
        }

        // POST: PRODUCTIONSITES/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductionSite productionSite)
        {
            if (id != productionSite.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _api.UpdateProductionSiteAsync(productionSite.Id, productionSite);

                if (result.NotFound)
                {
                    return NotFound();
                }

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                result.CopyErrorsTo(ModelState);
            }

            return View(productionSite);
        }

        // GET: PRODUCTIONSITES/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var details = await _api.GetProductionSiteAsync(id.Value);

            if (details == null)
            {
                return NotFound();
            }

            return View(ToProductionSite(details));
        }

        // POST: PRODUCTIONSITES/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeleteProductionSiteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private static ProductionSite ToProductionSite(ProductionSiteDetails details)
        {
            return new ProductionSite
            {
                Id = details.Id,
                Name = details.Name,
                Description = details.Description,
                EquipmentCount = details.Equipment.Count
            };
        }
    }
}
