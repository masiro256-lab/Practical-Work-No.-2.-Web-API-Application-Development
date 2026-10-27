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
    public class ProductionSitesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductionSitesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductionSiteDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductionSiteDto>>> GetAll([FromQuery] string? search)
        {
            var query = _context.ProductionSites.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var pattern = $"%{search.Trim()}%";
                query = query.Where(site => EF.Functions.Like(site.Name, pattern));
            }

            var sites = await query
                .OrderBy(site => site.Name)
                .Select(site => new ProductionSiteDto(
                    site.Id,
                    site.Name,
                    site.Description,
                    site.EquipmentItems.Count))
                .ToListAsync();

            return Ok(sites);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductionSiteDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductionSiteDetailsDto>> GetById(int id)
        {
            var site = await _context.ProductionSites
                .AsNoTracking()
                .Include(s => s.EquipmentItems)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (site == null)
            {
                return NotFound();
            }

            var equipment = site.EquipmentItems
                .OrderBy(e => e.Name)
                .Select(e => new EquipmentDto(
                    e.Id,
                    e.Name,
                    e.Model,
                    e.InventoryNumber,
                    e.ProductionSiteId,
                    site.Name))
                .ToList();

            return Ok(new ProductionSiteDetailsDto(site.Id, site.Name, site.Description, equipment));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductionSiteDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductionSiteDto>> Create(ProductionSiteInput input)
        {
            var site = new ProductionSite
            {
                Name = input.Name.Trim(),
                Description = input.Description?.Trim() ?? string.Empty
            };

            _context.ProductionSites.Add(site);
            await _context.SaveChangesAsync();

            var dto = new ProductionSiteDto(site.Id, site.Name, site.Description, 0);

            return CreatedAtAction(nameof(GetById), new { id = site.Id }, dto);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, ProductionSiteInput input)
        {
            var site = await _context.ProductionSites.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            site.Name = input.Name.Trim();
            site.Description = input.Description?.Trim() ?? string.Empty;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var site = await _context.ProductionSites.FindAsync(id);

            if (site == null)
            {
                return NotFound();
            }

            _context.ProductionSites.Remove(site);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
