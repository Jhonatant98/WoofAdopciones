using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;

namespace WoofAdopciones.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AdoptionsController : ControllerBase
    { 
        private readonly IAdoptionsUnitOfWork _adoptionsUnitOfWork;

        public AdoptionsController(IAdoptionsUnitOfWork adoptionsUnitOfWork)
        {
         _adoptionsUnitOfWork = adoptionsUnitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(AdoptionDTO adoptionDTO)
        {
            var response = await _adoptionsUnitOfWork.ProcessAdoptionAsync(User.Identity!.Name!, adoptionDTO.PetId);
            if (response.WasSuccess)
            {
                return NoContent();
            }

            return BadRequest(response.Message);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _adoptionsUnitOfWork.GetAsync(User.Identity!.Name!, pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _adoptionsUnitOfWork.GetTotalPagesAsync(User.Identity!.Name!, pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _adoptionsUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(AdoptionDTO adoptionDTO)
        {
            var response = await _adoptionsUnitOfWork.UpdateFullAsync(User.Identity!.Name!, adoptionDTO);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }
    }
}
