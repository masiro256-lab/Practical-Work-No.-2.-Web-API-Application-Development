using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Api.Data;
using ProductCatalogApp.Api.Dtos;
using ProductCatalogApp.Api.Models;

namespace ProductCatalogApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class EquipmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EquipmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetAll(
            [FromQuery] int? productionSiteId,
            [FromQuery] string? search)
        {
            var query = _context.EquipmentItems.AsNoTracking();

            if (productionSiteId.HasValue)
            {
                query = query.Where(e => e.ProductionSiteId == productionSiteId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var pattern = $"%{search.Trim()}%";
                query = query.Where(e =>
                    EF.Functions.Like(e.Name, pattern) ||
                    EF.Functions.Like(e.Model, pattern) ||
                    EF.Functions.Like(e.InventoryNumber, pattern));
            }

            var equipment = await query
                .OrderBy(e => e.ProductionSite!.Name)
                .ThenBy(e => e.Name)
                .Select(e => new EquipmentDto(
                    e.Id,
                    e.Name,
                    e.Model,
                    e.InventoryNumber,
                    e.ProductionSiteId,
                    e.ProductionSite!.Name))
                .ToListAsync();

            return Ok(equipment);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EquipmentDto>> GetById(int id)
        {
            var equipment = await _context.EquipmentItems
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Select(e => new EquipmentDto(
                    e.Id,
                    e.Name,
                    e.Model,
                    e.InventoryNumber,
                    e.ProductionSiteId,
                    e.ProductionSite!.Name))
                .FirstOrDefaultAsync();

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }

        [HttpPost]
        [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EquipmentDto>> Create(EquipmentInput input)
        {
            var site = await _context.ProductionSites.FindAsync(input.ProductionSiteId);

            if (site == null)
            {
                return SiteNotFound();
            }

            var equipment = new Equipment
            {
                Name = input.Name.Trim(),
                Model = input.Model.Trim(),
                InventoryNumber = input.InventoryNumber.Trim(),
                ProductionSiteId = site.Id
            };

            _context.EquipmentItems.Add(equipment);
            await _context.SaveChangesAsync();

            var dto = new EquipmentDto(
                equipment.Id,
                equipment.Name,
                equipment.Model,
                equipment.InventoryNumber,
                site.Id,
                site.Name);

            return CreatedAtAction(nameof(GetById), new { id = equipment.Id }, dto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, EquipmentInput input)
        {
            var equipment = await _context.EquipmentItems.FindAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            if (!await _context.ProductionSites.AnyAsync(s => s.Id == input.ProductionSiteId))
            {
                return SiteNotFound();
            }

            equipment.Name = input.Name.Trim();
            equipment.Model = input.Model.Trim();
            equipment.InventoryNumber = input.InventoryNumber.Trim();
            equipment.ProductionSiteId = input.ProductionSiteId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var equipment = await _context.EquipmentItems.FindAsync(id);

            if (equipment == null)
            {
                return NotFound();
            }

            _context.EquipmentItems.Remove(equipment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private ActionResult SiteNotFound()
        {
            ModelState.AddModelError(
                nameof(EquipmentInput.ProductionSiteId),
                "Производственный участок с таким идентификатором не найден.");

            return ValidationProblem(ModelState);
        }
    }
}
