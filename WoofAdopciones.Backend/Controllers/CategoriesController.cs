using Microsoft.AspNetCore.Mvc;
using Sales.Shared.Entities;
using WoofAdopciones.Backend.Controllers;
using WoofAdopciones.Backend.Interfaces;

namespace Sales.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : GenericController<Category>
    {
        public CategoriesController(IGenericUnitOfWork<Category> unitOfWork) : base(unitOfWork)
        {
        }
    }
}