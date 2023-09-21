using Microsoft.AspNetCore.Mvc;
using Sales.Shared.Entities;
using WoofAdopciones.Backend.Interfaces;
using WoofAdopciones.Shared.Entities;

namespace WoofAdopciones.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderTypeController : GenericController<OrderType>
    {
        public OrderTypeController(IGenericUnitOfWork<OrderType> unitOfWork) : base(unitOfWork)
        {
        }
    }
}