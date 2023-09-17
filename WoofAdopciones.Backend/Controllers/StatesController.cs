using Microsoft.AspNetCore.Mvc;
using Sales.Shared.Entities;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sales.Backend.Data;

namespace WoofAdopciones.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : GenericController<Country>
    {
        private readonly DataContext _context;

        public StatesController(IGenericUnitOfWork<Country> unitOfWork, DataContext context) : base(unitOfWork)
        {
            _context = context;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            return Ok(await _context.States
                .Include(s => s.Cities)
                .ToListAsync());
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var country = await _context.States
                .Include(s => s.Cities!)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            return Ok(country);
        }
    }
}
