using Microsoft.AspNetCore.Mvc;
using WoofAdopciones.Backend.Interfaces;
using Sales.Shared.Entities;

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
