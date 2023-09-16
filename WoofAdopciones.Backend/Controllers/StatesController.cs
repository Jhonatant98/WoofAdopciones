using Microsoft.AspNetCore.Mvc;
using Sales.Shared.Entities;
using WoofAdopciones.Backend.Interfaces;

namespace WoofAdopciones.Backend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : GenericController<State>
    {
        public StatesController(IGenericUnitOfWork<State> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
