using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PetsController : GenericController<Pet>
    {
        private readonly IPetsUnitOfWork _petsUnitOfWork;
        public PetsController(IGenericUnitOfWork<Pet> unitOfWork, IPetsUnitOfWork petsUnitOfWork) : base(unitOfWork)
        {
            _petsUnitOfWork = petsUnitOfWork;
        }

        [AllowAnonymous]
        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _petsUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _petsUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public override async Task<IActionResult> GetAsync(int id)
        {
            var action = await _petsUnitOfWork.GetAsync(id);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }

        [HttpPost("full")]
        public async Task<IActionResult> PostFullAsync(PetDTO petDTO)
        {
            var action = await _petsUnitOfWork.AddFullAsync(petDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }

        [HttpPut("full")]
        public async Task<IActionResult> PutFullAsync(PetDTO petDTO)
        {
            var action = await _petsUnitOfWork.UpdateFullAsync(petDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return NotFound(action.Message);
        }

        [HttpPost("addImages")]
        public async Task<IActionResult> PostAddImagesAsync(ImageDTO imageDTO)
        {
            var action = await _petsUnitOfWork.AddImageAsync(imageDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }

        [HttpPost("removeLastImage")]
        public async Task<IActionResult> PostRemoveLastImageAsync(ImageDTO imageDTO)
        {
            var action = await _petsUnitOfWork.RemoveLastImageAsync(imageDTO);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }
    }
}