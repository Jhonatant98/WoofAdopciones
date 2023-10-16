using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WoofAdopciones.Backend.Data;
using WoofAdopciones.Backend.Interfaces;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PetsController : GenericController<Pet>
    {
        public PetsController(IGenericUnitOfWork<Pet> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
        }
    }
}
