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
    public class OrderTypeController : GenericController<OrderType>
    {
        public OrderTypeController(IGenericUnitOfWork<OrderType> unitOfWork, DataContext context) : base(unitOfWork, context)
        {
        }
    }
}
