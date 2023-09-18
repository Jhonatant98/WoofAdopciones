using Microsoft.AspNetCore.Mvc;
using WoofAdopciones.Backend.Interfaces;
using Sales.Shared.Entities;

namespace WoofAdopciones.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : GenericController<Pet>
    {
        public PetsController(IGenericUnitOfWork<Pet> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
