using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoofAdopciones.Backend.Helpers;
using WoofAdopciones.Backend.UnitsOfWork;
using WoofAdopciones.Shared.DTOs;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AdoptionCenterController : GenericController<AdoptionCenter>
    {
        private readonly IAdoptionCenterUnitOfWork _adoptionCenterUnitOfWork;
        private readonly IFileStorage _fileStorage;
        private readonly string _container;


        public AdoptionCenterController(IGenericUnitOfWork<AdoptionCenter> unit, IAdoptionCenterUnitOfWork AdoptionCenterUnitOfWork, IFileStorage fileStorage) : base(unit)
        {
            _adoptionCenterUnitOfWork = AdoptionCenterUnitOfWork;
            _fileStorage = fileStorage;
            _container = "users";

        }

        [AllowAnonymous]
        [HttpGet("combo")]
        public async Task<IActionResult> GetComboAsync()
        {
            return Ok(await _adoptionCenterUnitOfWork.GetComboAsync());
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response = await _adoptionCenterUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _adoptionCenterUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }

        [HttpPost]
        public override async Task<IActionResult> PostAsync(AdoptionCenter  model)
        {
            
            if (!string.IsNullOrEmpty(model.Photo))
            {
                var photoUser = Convert.FromBase64String(model.Photo);
                model.Photo = await _fileStorage.SaveFileAsync(photoUser, ".jpg", _container);
            }
            var action = await _adoptionCenterUnitOfWork.AddAsync(model);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest(action.Message);
        }

    }
}