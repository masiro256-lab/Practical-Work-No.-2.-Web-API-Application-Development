using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductCatalogApp.Client.Models;
using ProductCatalogApp.Client.Services;

namespace ProductCatalogApp.Client.Controllers
{
    public class EquipmentsController : Controller
    {
        private readonly IProductCatalogApiClient _api;

        public EquipmentsController(IProductCatalogApiClient api)
        {
            _api = api;
        }

        // GET: Equipments
        // Также выполняет фильтрацию оборудования по площадке.
        public async Task<IActionResult> Index(int? productionSiteId, string? search)
        {
            await LoadProductionSitesAsync(productionSiteId);
            ViewData["Search"] = search;

            return View(await _api.GetEquipmentAsync(productionSiteId, search));
        }

        // GET: Equipments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _api.GetEquipmentByIdAsync(id.Value);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // GET: Equipments/Create
        public async Task<IActionResult> Create()
        {
            await LoadProductionSitesAsync();

            return View(new Equipment());
        }

        // POST: Equipments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Equipment equipment)
        {
            if (ModelState.IsValid)
            {
                var result = await _api.CreateEquipmentAsync(equipment);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                result.CopyErrorsTo(ModelState);
            }

            await LoadProductionSitesAsync(equipment.ProductionSiteId);

            return View(equipment);
        }

        // GET: Equipments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _api.GetEquipmentByIdAsync(id.Value);

            if (equipment == null)
            {
                return NotFound();
            }

            await LoadProductionSitesAsync(equipment.ProductionSiteId);

            return View(equipment);
        }

        // POST: Equipments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Equipment equipment)
        {
            if (id != equipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _api.UpdateEquipmentAsync(equipment.Id, equipment);

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

            await LoadProductionSitesAsync(equipment.ProductionSiteId);

            return View(equipment);
        }

        // GET: Equipments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var equipment = await _api.GetEquipmentByIdAsync(id.Value);

            if (equipment == null)
            {
                return NotFound();
            }

            return View(equipment);
        }

        // POST: Equipments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _api.DeleteEquipmentAsync(id);

            return RedirectToAction(nameof(Index));
        }

        // Создаёт выпадающий список производственных площадок.
        private async Task LoadProductionSitesAsync(int? selectedId = null)
        {
            var productionSites = await _api.GetProductionSitesAsync();

            ViewData["ProductionSiteId"] = new SelectList(
                productionSites,
                nameof(ProductionSite.Id),
                nameof(ProductionSite.Name),
                selectedId);
        }
    }
}
