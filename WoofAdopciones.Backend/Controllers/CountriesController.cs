using Microsoft.AspNetCore.Mvc;
using Sales.Shared.Entities;
using WoofAdopciones.Backend.Interfaces;

namespace WoofAdopciones.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : GenericController<Country>
    {
        public CountriesController(IGenericUnitOfWork<Country> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
