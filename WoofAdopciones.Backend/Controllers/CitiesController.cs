using Microsoft.AspNetCore.Mvc;
using Sales.Shared.Entities;
using WoofAdopciones.Backend.Interfaces;

namespace WoofAdopciones.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : GenericController<City>
    {
        public CitiesController(IGenericUnitOfWork<City> unitOfWork) : base(unitOfWork)
        {
        }
    }
}